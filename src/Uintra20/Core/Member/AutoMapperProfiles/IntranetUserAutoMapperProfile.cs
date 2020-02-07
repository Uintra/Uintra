using AutoMapper;
using Uintra20.Core.Member.Commands;
using Uintra20.Core.Member.Models;

namespace Uintra20.Core.Member.AutoMapperProfiles
{
    public class IntranetUserAutoMapperProfile : AutoMapper.Profile
    {
	    public IntranetUserAutoMapperProfile()
        {
            //Mapper.CreateMap<ProfileEditViewModel, UpdateMemberDto>()
            //    .ForMember(dst => dst.DeleteMedia, o => o.Ignore())
            //    .ForMember(dst => dst.NewMedia, o => o.Ignore());

            //Mapper.CreateMap<IIntranetMember, MentionUserModel>()
            //    .ForMember(dst => dst.Id, o => o.MapFrom(u => u.Id))
            //    .ForMember(dst => dst.Value, o => o.MapFrom(u => u.DisplayedName))
            //    .ForMember(dst => dst.Url, o => o.Ignore());

            //Mapper.CreateMap<MembersListSearchModel, ActiveMemberSearchQuery>()
            //    .ForMember(dst => dst.MembersOfGroup, o => o.MapFrom(s => !s.IsInvite))
            //    .ForMember(dst => dst.GroupId, o => o.MapFrom(query => query.GroupId));

            CreateMap<MentionModel, MentionCommand>();
        }
    }
}