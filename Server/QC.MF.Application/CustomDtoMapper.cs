using Abp.Authorization;
using Abp.Authorization.Roles;
using AutoMapper;
using QC.MF.AppEditions;
using QC.MF.AppEditions.Dto;
using QC.MF.Authorization.Roles;
using QC.MF.Authorization.Users;
using QC.MF.Roles.Dto;
using QC.MF.Sessions.Dto;
using QC.MF.Users.Dto;
using System;

namespace QC.MF
{
    internal static class CustomDtoMapper
    {
        private static volatile bool _mappedBefore;
        private static readonly object SyncObj = new object();

        public static void CreateMappings(IMapperConfigurationExpression mapper)
        {
            lock (SyncObj)
            {
                if (_mappedBefore)
                {
                    return;
                }

                CreateMappingsInternal(mapper);

                _mappedBefore = true;
            }
        }

        private static void CreateMappingsInternal(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());

            // Role and permission
            mapper.CreateMap<Permission, string>().ConvertUsing(r => r.Name);
            mapper.CreateMap<RolePermissionSetting, string>().ConvertUsing(r => r.Name);

            mapper.CreateMap<CreateRoleDto, Role>().ForMember(x => x.Permissions, opt => opt.Ignore());
            mapper.CreateMap<RoleDto, Role>().ForMember(x => x.Permissions, opt => opt.Ignore());

            mapper.CreateMap<UserDto, User>();
            mapper.CreateMap<UserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());

            mapper.CreateMap<CreateUserDto, User>();
            mapper.CreateMap<CreateUserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());

            mapper.CreateMap<CreateUserDto, User>();
            mapper.CreateMap<User, UserLoginInfoDto>().ForMember(x => x.Profile, opt => opt.ResolveUsing(n => $"/Profile/GetProfilePictureById/{n.ProfilePictureId}"));

            mapper.CreateMap<DateTime, string>().ConvertUsing(n => n.ToString("yyyy/MM/dd HH:mm"));
            mapper.CreateMap<DateTime?, string>().ConvertUsing(n => n?.ToString("yyyy/MM/dd HH:mm") ?? "");

            mapper.CreateMap<AppEdition, AppEditionDto>()
                .ForMember(x => x.AppType, opt => opt.ResolveUsing(n => n is IOSAppEdition ? "IOS" : n is AndroidAppEdition ? "Android" : ""))
                .ForMember(x => x.ItunesUrl, opt => opt.ResolveUsing(n => n is IOSAppEdition ? ((IOSAppEdition)n).ItunesUrl : ""));
        }
    }
}
