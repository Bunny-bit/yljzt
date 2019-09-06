using QC.MF.EntityFramework;
using EntityFramework.DynamicFilters;

namespace QC.MF.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly MFDbContext _context;

        public InitialHostDbBuilder(MFDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new DefaultEditionsCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            new LanguageTextOverrider(_context).Create();
            new DefaultMenuCreator(_context).Create();
        }
    }
}
