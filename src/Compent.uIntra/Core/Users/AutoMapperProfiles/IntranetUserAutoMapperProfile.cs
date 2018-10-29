using System;
using AutoMapper;
using Compent.Uintra.Core.Search.Entities;
using Uintra.Core.User;
using Uintra.Users;
using Uintra.Users.UserList;

namespace Compent.Uintra.Core.Users
{
    public class IntranetUserAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IntranetUser, ProfileViewModel>()
                .ForMember(dst => dst.EditingUser, o => o.MapFrom(user => user));

            Mapper.CreateMap<IntranetUser, UserModel>()
                .ForMember(dst => dst.User, o => o.MapFrom(user => user))
                .ForMember(dst => dst.ProfileUrl, o => o.Ignore());

            Mapper.CreateMap<IntranetUser, ProfileEditModel>()
                .ForMember(dst => dst.MediaRootId, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.MemberNotifierSettings, o => o.Ignore());

            Mapper.CreateMap<ProfileEditModel, ExtendedProfileEditModel>()
                .ForMember(dst => dst.TagIdsData, o => o.MapFrom(i => string.Empty));

            Mapper.CreateMap<IntranetUser, IntranetUserDTO>()
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