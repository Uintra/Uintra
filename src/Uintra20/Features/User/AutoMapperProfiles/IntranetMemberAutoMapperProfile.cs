using AutoMapper;
using System.Linq;
using Uintra20.Features.User.DTO;
using Uintra20.Features.User.Models;

namespace Uintra20.Features.User.AutoMapperProfiles
{
    public class IntranetMemberAutoMapperProfile : Profile
    {
	    public IntranetMemberAutoMapperProfile()
        {
            //Mapper.CreateMap<IIntranetMember, ProfileViewModel>()
            //    .ForMember(dst => dst.EditingMember, o => o.MapFrom(user => user))
            //    .ForMember(dst => dst.Photo, o => o.MapFrom(user => user.Photo.IfNone(() => string.Empty)))
            //    .ForMember(dst => dst.PhotoId, o => o.MapFrom(user => user.PhotoId.ToNullable()));

            CreateMap<IIntranetMember, MemberViewModel>()
                .ForMember(dst => dst.Id, o => o.MapFrom(user => user.Id))
                .ForMember(dst => dst.DisplayedName, o => o.MapFrom(user => user.DisplayedName))
                .ForMember(dst => dst.Email, o => o.MapFrom(user => user.Email))
                .ForMember(dst => dst.LoginName, o => o.MapFrom(user => user.LoginName))
                .ForMember(dst => dst.Photo, o => o.MapFrom(user => user.Photo.IfNone(() => string.Empty)))
                .ForMember(dst => dst.PhotoId, o => o.MapFrom(user => user.PhotoId.ToNullable()));

            //Mapper.CreateMap<IIntranetMember, MemberModel>()
            //    .ForMember(dst => dst.Member, o => o.MapFrom(user => user))
            //    .ForMember(dst => dst.ProfileUrl, o => o.Ignore())
            //    .ForMember(dst => dst.IsGroupAdmin, o => o.Ignore())
            //    .ForMember(dst => dst.IsCreator, o => o.Ignore());

            //Mapper.CreateMap<IIntranetMember, ProfileEditModel>()
            //    .ForMember(dst => dst.MediaRootId, o => o.Ignore())
            //    .ForMember(dst => dst.NewMedia, o => o.Ignore())
            //    .ForMember(dst => dst.MemberNotifierSettings, o => o.Ignore())
            //    .ForMember(dst => dst.ProfileUrl, o => o.Ignore())
            //    .ForMember(dst => dst.Photo, o => o.MapFrom(user => user.Photo.IfNone(() => string.Empty)))
            //    .ForMember(dst => dst.PhotoId, o => o.MapFrom(user => user.PhotoId.ToNullable()));

            CreateMap<Google.Apis.Admin.Directory.directory_v1.Data.User, CreateMemberDto>()
                .ForMember(dst => dst.FirstName, o => o.Ignore())
                .ForMember(dst => dst.LastName, o => o.Ignore())
                .ForMember(dst => dst.Phone, o => o.MapFrom(s => s.Phones.Any() ? s.Phones.First().Value : string.Empty))
                .ForMember(dst => dst.Department, o => o.Ignore())
                .ForMember(dst => dst.Email, o => o.MapFrom(s => s.Emails.First().Address))
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

            CreateMap<Google.Apis.Admin.Directory.directory_v1.Data.User, UpdateMemberDto>()
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


            //Mapper.CreateMap<ProfileEditModel, ExtendedProfileEditModel>()
            //    .ForMember(dst => dst.TagIdsData, o => o.MapFrom(i => string.Empty));

            CreateMap<IIntranetMember, UpdateMemberDto>()
                .ForMember(dst => dst.DeleteMedia, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore());

            //Mapper.CreateMap<SearchableMember, MentionUserModel>()
            //    .ForMember(dst => dst.Id, o => o.MapFrom(u => Guid.Parse(u.Id.ToString())))
            //    .ForMember(dst => dst.Value, o => o.MapFrom(u => u.FullName))
            //    .ForMember(dst => dst.Url, o => o.Ignore());

        }
    }
}