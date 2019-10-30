using AutoMapper;
using LanguageExt;
using Uintra.Core.User;
using Uintra.Users.Commands;
using Uintra.Core.User.DTO;
using Uintra.Users.UserList;

namespace Uintra.Users
{
    public class IntranetUserAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<ProfileEditModel, UpdateMemberDto>()
                .ForMember(dst => dst.DeleteMedia, o => o.Ignore())
                .ForMember(dst => dst.NewMedia, o => o.Ignore());

            Mapper.CreateMap<IIntranetMember, MentionUserModel>()
                .ForMember(dst => dst.Id, o => o.MapFrom(u => u.Id))
                .ForMember(dst => dst.Value, o => o.MapFrom(u => u.DisplayedName))
                .ForMember(dst => dst.Url, o => o.Ignore());

            Mapper.CreateMap<MembersListSearchModel, ActiveMemberSearchQuery>()
	            .ForMember(dst=>dst.MembersOfGroup,o=>o.MapFrom(s=>!s.IsInvite))
                .ForMember(dst => dst.GroupId, o => o.MapFrom(query => query.GroupId));

            Mapper.CreateMap<MentionModel, MentionCommand>();                

            base.Configure();
        }
    }
}