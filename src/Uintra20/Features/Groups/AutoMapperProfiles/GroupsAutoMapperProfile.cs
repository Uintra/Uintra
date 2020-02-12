﻿using AutoMapper;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Sql;

namespace Uintra20.Features.Groups.AutoMapperProfiles
{
    public class GroupsAutoMapperProfile : Profile
    {
        public GroupsAutoMapperProfile()
        {
            
            CreateMap<Group, GroupModel>().ReverseMap();
            

            //Mapper.CreateMap<GroupModel, BackofficeGroupViewModel>()
            //    .ForMember(d => d.CreateDate, o => o.MapFrom(s => s.CreatedDate))
            //    .ForMember(d => d.UpdateDate, o => o.MapFrom(s => s.UpdatedDate))
            //    .ForMember(d => d.CreatorName, o => o.Ignore())
            //    .ForMember(d => d.Link, o => o.Ignore());

            //Mapper.CreateMap<GroupCreateModel, GroupModel>()
            //    .ForMember(d => d.Id, o => o.Ignore())
            //    .ForMember(d => d.CreatedDate, o => o.Ignore())
            //    .ForMember(d => d.UpdatedDate, o => o.Ignore())
            //    .ForMember(d => d.IsHidden, o => o.Ignore())
            //    .ForMember(d => d.GroupTypeId, o => o.Ignore())
            //    .ForMember(d => d.ImageId, o => o.Ignore())
            //    .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
            //    .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
            //    .ForMember(d => d.CreatorId, o => o.Ignore())
            //    .AfterMap((s, d) => d.CreatorId = s.Creator.MemberId);

            //Mapper.CreateMap<GroupModel, GroupCreateModel>()
            //    .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
            //    .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
            //    .ForMember(d => d.Media, o => o.MapFrom(el => el.ImageId))
            //    .ForMember(d => d.AllowedMediaExtensions, o => o.Ignore())
            //    .ForMember(d => d.MediaRootId, o => o.Ignore())
            //    .ForMember(d => d.NewMedia, o => o.Ignore())
            //    .ForMember(d => d.Creator, o => o.Ignore())
            //    .AfterMap((s, d) =>
            //    {
            //        d.Creator = new GroupMemberSubscriptionModel
            //        {
            //            MemberId = s.CreatorId,
            //            IsAdmin = true
            //        };
            //    });

            //Mapper.CreateMap<GroupModel, GroupEditModel>()
            //    .IncludeBase<GroupModel, GroupCreateModel>()
            //    .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            //    .ForMember(d => d.CanHide, o => o.Ignore());

            //Mapper.CreateMap<GroupEditModel, GroupModel>()
            //    .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            //    .ForMember(d => d.ImageId, o => o.Ignore())
            //    .ForMember(d => d.CreatedDate, o => o.Ignore())
            //    .ForMember(d => d.UpdatedDate, o => o.Ignore())
            //    .ForMember(d => d.IsHidden, o => o.Ignore())
            //    .ForMember(d => d.CreatorId, o => o.Ignore())
            //    .ForMember(d => d.GroupTypeId, o => o.Ignore())
            //    .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
            //    .ForMember(d => d.Description, o => o.MapFrom(s => s.Description));

            //Mapper.CreateMap<GroupModel, GroupInfoViewModel>()
            //    .ForMember(d => d.MembersCount, o => o.Ignore())
            //    .ForMember(d => d.Creator, o => o.Ignore())
            //    .ForMember(d => d.IsMember, o => o.Ignore())
            //    .ForMember(d => d.GroupImageUrl, o => o.Ignore())
            //    .ForMember(d => d.CanUnsubscribe, o => o.Ignore())
            //    .ForMember(d => d.CreatorProfileUrl, o => o.Ignore())
            //    .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            //    .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
            //    .ForMember(d => d.Title, o => o.MapFrom(s => s.Title));

            //Mapper.CreateMap<GroupModel, GroupViewModel>()
            //   .ForMember(d => d.MembersCount, o => o.Ignore())
            //   .ForMember(d => d.Creator, o => o.Ignore())
            //   .ForMember(d => d.IsMember, o => o.Ignore())
            //   .ForMember(d => d.GroupImageUrl, o => o.Ignore())
            //   .ForMember(d => d.GroupUrl, o => o.Ignore())
            //   .ForMember(d => d.HasImage, o => o.MapFrom(s => s.ImageId.HasValue))
            //   .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            //   .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
            //   .ForMember(d => d.Title, o => o.MapFrom(s => s.Title));

            //Mapper.CreateMap<ActivityFeedTabModel, GroupNavigationActivityTabViewModel>();

            //Mapper.CreateMap<PageTabModel, GroupNavigationPageTabViewModel>()
            //    .ForMember(d => d.AlignRight, o => o.Ignore());

            //Mapper.CreateMap<IGroupMember, GroupMemberViewModel>()
            //    .ForMember(d => d.IsGroupAdmin, o => o.Ignore())
            //    .ForMember(d => d.CanUnsubscribe, o => o.Ignore())
            //    .ForMember(d => d.GroupMember, o => o.MapFrom(member => member));
        }
    }
}