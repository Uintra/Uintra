using System;
using System.Linq;
using AutoMapper;
using Compent.Uintra.Core.Search.Entities;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Uintra.Core.User;
using Uintra.Core.User.DTO;
using Uintra.Users;
using Uintra.Users.UserList;

namespace Compent.Uintra.Core.Users
{
    public class IntranetUserAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IntranetUser, ProfileViewModel>()
                .ForMember(dst => dst.EditingMember, o => o.MapFrom(user => user));

            Mapper.CreateMap<IIntranetMember, MemberViewModel>()
                .ForMember(dst => dst.Id, o => o.MapFrom(user => user.Id))
                .ForMember(dst => dst.DisplayedName, o => o.MapFrom(user => user.DisplayedName))
                .ForMember(dst => dst.Email, o => o.MapFrom(user => user.Email))
                .ForMember(dst => dst.LoginName, o => o.MapFrom(user => user.LoginName))
                .ForMember(dst => dst.Photo, o => o.MapFrom(user => user.Photo))
                .ForMember(dst => dst.Inactive, o => o.MapFrom(user => user.Inactive));

            Mapper.CreateMap<IntranetUser, MemberModel>()
                .ForMember(dst => dst.Member, o => o.MapFrom(user => user))
                .ForMember(dst => dst.ProfileUrl, o => o.Ignore())
                .ForMember(dst => dst.IsGroupAdmin, o => o.Ignore());
            Mapper.CreateMap<IntranetUser, ProfileEditModel>()
                .ForMember(dst => dst.MediaRootId, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.MemberNotifierSettings, o => o.Ignore())
                .ForMember(dst => dst.ProfileUrl, o => o.Ignore());

            Mapper.CreateMap<User, CreateMemberDto>()
                .ForMember(dst => dst.FirstName, o => o.Ignore())
                .ForMember(dst => dst.LastName, o => o.Ignore())
                .ForMember(dst => dst.Phone, o => o.MapFrom(s => s.Phones.Any() ? s.Phones.First().Value : string.Empty))
                .ForMember(dst => dst.Department, o => o.Ignore())
                .ForMember(dst => dst.Email, o => o.MapFrom(s => s.Emails.First().Address))
                .ForMember(dst => dst.Role, o => o.MapFrom(s => IntranetRolesEnum.UiPublisher))
                .ForMember(dst => dst.MediaId, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    if (src.Name.FullName.Contains(' '))
                    {
                        var names = src.Name.FullName.Split(' ');

                        dst.FirstName = names[0];
                        dst.LastName = names[1];
                    }
                });

            Mapper.CreateMap<User, UpdateMemberDto>()
                .ForMember(dst => dst.FirstName, o => o.Ignore())
                .ForMember(dst => dst.LastName, o => o.Ignore())
                .ForMember(dst => dst.Phone, o => o.MapFrom(s => s.Phones.Any() ? s.Phones.First().Value : string.Empty))
                .ForMember(dst => dst.Department, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.DeleteMedia, o => o.Ignore())
                .ForMember(dst => dst.Id, o => o.Ignore())
                .AfterMap((src, dst) =>
                {
                    if (src.Name.FullName.Contains(' '))
                    {
                        var names = src.Name.FullName.Split(' ');

                        dst.FirstName = names[0];
                        dst.LastName = names[1];
                    }
                });


            Mapper.CreateMap<ProfileEditModel, ExtendedProfileEditModel>()
                .ForMember(dst => dst.TagIdsData, o => o.MapFrom(i => string.Empty));

            Mapper.CreateMap<IntranetUser, UpdateMemberDto>()
                .ForMember(dst => dst.DeleteMedia, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore());

            Mapper.CreateMap<SearchableUser, MentionUserModel>()
                .ForMember(dst => dst.Id, o => o.MapFrom(u => Guid.Parse(u.Id.ToString())))
                .ForMember(dst => dst.Value, o => o.MapFrom(u => u.FullName))
                .ForMember(dst => dst.Url, o => o.Ignore());

            base.Configure();
        }
    }
}