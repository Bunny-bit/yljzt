using System.Linq;
using Abp.Configuration;
using Abp.Localization;
using Abp.Net.Mail;
using QC.MF.EntityFramework;

namespace QC.MF.Migrations.SeedData
{
    public class DefaultSettingsCreator
    {
        private readonly MFDbContext _context;

        public DefaultSettingsCreator(MFDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            //Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "");
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "");
            AddSettingIfNotExists(EmailSettingNames.Smtp.UserName, "");
            AddSettingIfNotExists(EmailSettingNames.Smtp.Password, "");
            AddSettingIfNotExists(EmailSettingNames.Smtp.Host, "");
            AddSettingIfNotExists(EmailSettingNames.Smtp.Port, "");
            AddSettingIfNotExists(EmailSettingNames.Smtp.Domain, "");
            AddSettingIfNotExists(EmailSettingNames.Smtp.EnableSsl, "");
            AddSettingIfNotExists(EmailSettingNames.Smtp.UseDefaultCredentials, "");

            //Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "zh-CN");
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}
