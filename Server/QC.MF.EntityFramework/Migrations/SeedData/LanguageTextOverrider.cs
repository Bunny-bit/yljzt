using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Localization;
using QC.MF.EntityFramework;

namespace QC.MF.Migrations.SeedData
{
    public class LanguageTextOverrider
    {
        public static List<ApplicationLanguageText> InitialLanguageTexts { get; private set; }
        private readonly MFDbContext _context;

        public LanguageTextOverrider(MFDbContext context)
        {
            _context = context;
        }
        static LanguageTextOverrider()
        {
            InitialLanguageTexts = new List<ApplicationLanguageText>
            {
                new ApplicationLanguageText()
                {
                    TenantId = 1,
                    LanguageName = "zh-CN",
                    Source = "AbpZero",
                    Key = "OrganizationUnitDuplicateDisplayNameWarning",
                    Value = "当前组织机构层级下已存在名称为{0}的组织机构"
                },
                new ApplicationLanguageText()
                {
                    TenantId = 1,
                    LanguageName = "zh-CN",
                    Source = "AbpZero",
                    Key = "UserEmailIsNotConfirmedAndCanNotLogin",
                    Value = "你的邮箱地址未确认.不能登录"
                }
            };
        }

        public void Create()
        {
            AddLanguageTextIfNotExists();
        }

        private void AddLanguageTextIfNotExists()
        {
            foreach (var text in InitialLanguageTexts)
            {
                if (_context.LanguageTexts.Any(l => l.TenantId == text.TenantId && l.Source == text.Source && l.Key == text.Key))
                {
                    continue;
                }
                _context.LanguageTexts.Add(text);
            }
            _context.SaveChanges();
        }
    }
}
