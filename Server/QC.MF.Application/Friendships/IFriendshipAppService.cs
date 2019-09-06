using System.Threading.Tasks;
using Abp.Application.Services;
using QC.MF.Friendships.Dto;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using QC.MF.CommonDto;

namespace QC.MF.Friendships
{
    public interface IFriendshipAppService : IApplicationService
    {
        Task<FriendDto> CreateFriendshipRequest(CreateFriendshipRequestInput input);

        Task<List<FriendDto>> BatchCreateFriendshipRequest(List<CreateFriendshipRequestInput> input);

        Task<FriendDto> CreateFriendshipRequestByUserName(CreateFriendshipRequestByUserNameInput input);

        void BlockUser(BlockUserInput input);

        void UnblockUser(UnblockUserInput input);

        void AcceptFriendshipRequest(AcceptFriendshipRequestInput input);

        Task<PagedResultDto<FirendshipUserDto>> GetCreateFriendshipUserList(PagedAndFilteredInputDto input);
    }
}
