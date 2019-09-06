using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QC.MF.Roles.Dto;
using QC.MF.Users.Dto;
using DataExporting.Dto;
using QC.MF.CommonDto;

namespace QC.MF.Users
{
    /// <summary>
    /// �û�����
    /// </summary>
    public interface IUserAppService : IApplicationService
    {
        /// <summary>
        /// ��ȡȫ���Ľ�ɫ
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<RoleDto>> GetRoles();

        /// <summary>
        /// ��ȡ�û��б�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<UserListDto>> GetUsers(GetUsersInput input);

        /// <summary>
        /// �����û���Ϣ��Excel�ļ�
        /// </summary>
        /// <returns></returns>
        Task<DataExporting.Dto.FileDto> GetUsersToExcel(GetUsersInput input);

        /// <summary>
        /// ��ȡ�༭ʱ��Ҫ�ĵ����û���Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input);

        /// <summary>
        /// ��ȡ�༭ʱ��Ҫ���û�Ȩ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input);

        /// <summary>
        /// �����û�Ȩ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ResetUserSpecificPermissions(EntityDto<long> input);

        /// <summary>
        /// �޸��û�Ȩ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateUserPermissions(UpdateUserPermissionsInput input);

        /// <summary>
        /// ������༭�û���IdΪ��ʱ����������༭
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateUser(CreateOrUpdateUserInput input);

        /// <summary>
        /// ɾ���û�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteUser(EntityDto<long> input);

        /// <summary>
        /// ����ɾ���û�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BatchDeleteUser(ArrayDto<long> input);

        /// <summary>
        /// �����û�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UnlockUser(EntityDto<long> input);

        /// <summary>
        /// ���������û�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BatchUnlockUser(ArrayDto<long> input);

        /// <summary>
        /// ���������û�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BatchActiveUser(BatchActiveUserInput input);


        /// <summary>
        /// �û��޸��Լ�����Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateCurrentUser(UpdateCurrentUserInput input);

        /// <summary>
        /// �л��û����ý���״̬
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ToggleActiveStatus(EntityDto<long> input);
    }
}
