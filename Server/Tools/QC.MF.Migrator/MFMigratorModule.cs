using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using QC.MF.EntityFramework;

namespace QC.MF.Migrator
{
    [DependsOn(typeof(MFDataModule))]
    public class MFMigratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<MFDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
