using Compent.Extensions;
using Uintra.Core.Member.Abstractions;
using Uintra.Core.Member.Models.Dto;
using Uintra.Core.Member.Profile.Edit.Models;

namespace Uintra.Core.Member.Profile.Edit.Mappers
{
    public class ProfileEditProfile : AutoMapper.Profile
    {
        public ProfileEditProfile()
        {
            CreateMap<IIntranetMember, ProfileEditViewModel>()
                .ForMember(dst => dst.NewMedia, o => o.Ignore())
                .ForMember(dst => dst.MemberNotifierSettings, o => o.Ignore())
                .ForMember(dst => dst.ProfileUrl, o => o.Ignore())
                .ForMember(dst => dst.Photo, o => o.MapFrom(user => user.Photo.HasValue() ? user.Photo : string.Empty))
                .ForMember(dst => dst.PhotoId, o => o.MapFrom(user => user.PhotoId))
                .ForMember(dst => dst.TagIdsData, o => o.Ignore());

            CreateMap<ProfileEditViewModel, UpdateMemberDto>()
                .ForMember(dst => dst.DeleteMedia, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore());

            CreateMap<ProfileEditModel, UpdateMemberDto>()
                .ForMember(dst => dst.DeleteMedia, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore());
        }
    }
}