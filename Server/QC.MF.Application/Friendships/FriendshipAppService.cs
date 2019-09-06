using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.MultiTenancy;
using Abp.RealTime;
using Abp.Runtime.Session;
using Abp.UI;
using QC.MF.Authorization.Users;
using QC.MF.Chat;
using QC.MF.Friendships.Dto;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using System.Linq;
using Abp.Linq.Extensions;
using System.Data.Entity;
using System.Collections.Generic;
using QC.MF.CommonDto;

namespace QC.MF.Friendships
{
    [AbpAuthorize]
    public class FriendshipAppService : MFAppServiceBase, IFriendshipAppService
    {
        private readonly IFriendshipManager _friendshipManager;
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly IChatCommunicator _chatCommunicator;
        private readonly ITenantCache _tenantCache;
        private readonly IChatFeatureChecker _chatFeatureChecker;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Friendship, long> _friendshipRepository;

        public FriendshipAppService(
            IFriendshipManager friendshipManager,
            IOnlineClientManager onlineClientManager,
            IChatCommunicator chatCommunicator,
            ITenantCache tenantCache,
            IChatFeatureChecker chatFeatureChecker,
            IRepository<User, long> userRepository,
            IRepository<Friendship, long> friendshipRepository)
        {
            _friendshipManager = friendshipManager;
            _onlineClientManager = onlineClientManager;
            _chatCommunicator = chatCommunicator;
            _tenantCache = tenantCache;
            _chatFeatureChecker = chatFeatureChecker;
            _userRepository = userRepository;
            _friendshipRepository = friendshipRepository;
        }

        public async Task<FriendDto> CreateFriendshipRequest(CreateFriendshipRequestInput input)
        {
            var userIdentifier = AbpSession.ToUserIdentifier();
            var probableFriend = new UserIdentifier(input.TenantId, input.UserId);

            _chatFeatureChecker.CheckChatFeatures(userIdentifier.TenantId, probableFriend.TenantId);

            if (_friendshipManager.GetFriendshipOrNull(userIdentifier, probableFriend) != null)
            {
                throw new UserFriendlyException(L("YouAlreadySentAFriendshipRequestToThisUser"));
            }

            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId());

            User probableFriendUser;
            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                probableFriendUser = (await UserManager.FindByIdAsync(input.UserId));
            }

            var friendTenancyName = probableFriend.TenantId.HasValue ? _tenantCache.Get(probableFriend.TenantId.Value).TenancyName : null;
            var sourceFriendship = new Friendship(userIdentifier, probableFriend, friendTenancyName, probableFriendUser.UserName, probableFriendUser.ProfilePictureId, FriendshipState.Accepted);
            _friendshipManager.CreateFriendship(sourceFriendship);

            var userTenancyName = user.TenantId.HasValue ? _tenantCache.Get(user.TenantId.Value).TenancyName : null;
            var targetFriendship = new Friendship(probableFriend, userIdentifier, userTenancyName, user.UserName, user.ProfilePictureId, FriendshipState.Accepted);
            _friendshipManager.CreateFriendship(targetFriendship);

            var clients = _onlineClientManager.GetAllByUserId(probableFriend);
            if (clients.Any())
            {
                var isFriendOnline = _onlineClientManager.IsOnline(sourceFriendship.ToUserIdentifier());
                _chatCommunicator.SendFriendshipRequestToClient(clients, targetFriendship, false, isFriendOnline);
            }

            var senderClients = _onlineClientManager.GetAllByUserId(userIdentifier);
            if (senderClients.Any())
            {
                var isFriendOnline = _onlineClientManager.IsOnline(targetFriendship.ToUserIdentifier());
                _chatCommunicator.SendFriendshipRequestToClient(senderClients, sourceFriendship, true, isFriendOnline);
            }

            var sourceFriendshipRequest = sourceFriendship.MapTo<FriendDto>();
            sourceFriendshipRequest.IsOnline = _onlineClientManager.GetAllByUserId(probableFriend).Any();

            return sourceFriendshipRequest;
        }
        public async Task<List<FriendDto>> BatchCreateFriendshipRequest(List<CreateFriendshipRequestInput> input)
        {
            var result = new List<FriendDto>();
            foreach (var item in input)
            {
                result.Add(await CreateFriendshipRequest(item));
            }
            return result;
        }

        public async Task<FriendDto> CreateFriendshipRequestByUserName(CreateFriendshipRequestByUserNameInput input)
        {
            var probableFriend = await GetUserIdentifier(input.TenancyName, input.UserName);
            return await CreateFriendshipRequest(new CreateFriendshipRequestInput
            {
                TenantId = probableFriend.TenantId,
                UserId = probableFriend.UserId
            });
        }

        public void BlockUser(BlockUserInput input)
        {
            var userIdentifier = AbpSession.ToUserIdentifier();
            var friendIdentifier = new UserIdentifier(input.TenantId, input.UserId);
            _friendshipManager.BanFriend(userIdentifier, friendIdentifier);

            var clients = _onlineClientManager.GetAllByUserId(userIdentifier);
            if (clients.Any())
            {
                _chatCommunicator.SendUserStateChangeToClients(clients, friendIdentifier, FriendshipState.Blocked);
            }
        }

        public void UnblockUser(UnblockUserInput input)
        {
            var userIdentifier = AbpSession.ToUserIdentifier();
            var friendIdentifier = new UserIdentifier(input.TenantId, input.UserId);
            _friendshipManager.AcceptFriendshipRequest(userIdentifier, friendIdentifier);

            var clients = _onlineClientManager.GetAllByUserId(userIdentifier);
            if (clients.Any())
            {
                _chatCommunicator.SendUserStateChangeToClients(clients, friendIdentifier, FriendshipState.Accepted);
            }
        }

        public void AcceptFriendshipRequest(AcceptFriendshipRequestInput input)
        {
            var userIdentifier = AbpSession.ToUserIdentifier();
            var friendIdentifier = new UserIdentifier(input.TenantId, input.UserId);
            _friendshipManager.AcceptFriendshipRequest(userIdentifier, friendIdentifier);

            var clients = _onlineClientManager.GetAllByUserId(userIdentifier);
            if (clients.Any())
            {
                _chatCommunicator.SendUserStateChangeToClients(clients, friendIdentifier, FriendshipState.Blocked);
            }
        }

        private async Task<UserIdentifier> GetUserIdentifier(string tenancyName, string userName)
        {
            int? tenantId = null;
            if (!tenancyName.Equals("."))
            {
                using (CurrentUnitOfWork.SetTenantId(null))
                {
                    var tenant = await TenantManager.FindByTenancyNameAsync(tenancyName);
                    if (tenant == null)
                    {
                        throw new UserFriendlyException("There is no such tenant !");
                    }

                    tenantId = tenant.Id;
                }
            }

            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var user = await UserManager.FindByNameOrEmailAsync(userName);
                if (user == null)
                {
                    throw new UserFriendlyException("There is no such user !");
                }

                return user.ToUserIdentifier();
            }
        }

        public async Task<PagedResultDto<FirendshipUserDto>> GetCreateFriendshipUserList(PagedAndFilteredInputDto input)
        {
            var query =
                from u in _userRepository.GetAll().Where(n => n.Id != AbpSession.UserId.Value)
                join fu in _friendshipRepository.GetAll().Where(n => n.UserId == AbpSession.UserId.Value)
                on u.Id equals fu.FriendUserId
                into all
                from o in all.DefaultIfEmpty()
                where o == null
                select u;
            query = query.WhereIf(
                !string.IsNullOrEmpty(input.Filter),
                u =>
                    u.Name.Contains(input.Filter) ||
                    u.UserName.Contains(input.Filter) ||
                    u.PhoneNumber.Contains(input.Filter) ||
                    u.EmailAddress.Contains(input.Filter)
            );
            var userCount = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.Name)
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<FirendshipUserDto>(
                userCount,
                users.MapTo<List<FirendshipUserDto>>()
                );
        }
    }
}
