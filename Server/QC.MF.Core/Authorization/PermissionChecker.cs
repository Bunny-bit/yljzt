using Abp.Authorization;
using QC.MF.Authorization.Roles;
using QC.MF.Authorization.Users;

namespace QC.MF.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
