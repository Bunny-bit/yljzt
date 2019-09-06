using System.Linq;
using QC.MF.EntityFramework;
using QC.MF.MultiTenancy;

namespace QC.MF.Migrations.SeedData
{
    public class DefaultTenantCreator
    {
        private readonly MFDbContext _context;

        public DefaultTenantCreator(MFDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateUserAndRoles();
        }

        private void CreateUserAndRoles()
        {
            //Default tenant

            var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == Tenant.DefaultTenantName);
            if (defaultTenant == null)
            {
                _context.Tenants.Add(new Tenant {TenancyName = Tenant.DefaultTenantName, Name = Tenant.DefaultTenantName});
                _context.SaveChanges();
            }
        }
    }
}
