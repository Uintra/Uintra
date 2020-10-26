﻿using Uintra.Core.Member.Commands;
using Uintra.Core.Member.Models;
using Uintra.Features.UserList.Models;

namespace Uintra.Core.Member.AutoMapperProfiles
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

            CreateMap<MembersListSearchModel, ActiveMemberSearchQuery>()
                .ForMember(dst => dst.MembersOfGroup, o => o.MapFrom(s => !s.IsInvite))
                .ForMember(dst => dst.GroupId, o => o.MapFrom(query => query.GroupId));

            CreateMap<MentionModel, MentionCommand>();
        }
    }
}