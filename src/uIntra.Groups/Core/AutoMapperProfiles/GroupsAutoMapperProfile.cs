using AutoMapper;
using uIntra.CentralFeed;
using uIntra.Core.User;
using uIntra.Groups.Dashboard;
using uIntra.Groups.Sql;

namespace uIntra.Groups
{
    public class GroupsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Group, BackofficeGroupViewModel>()
                .ForMember(d => d.CreateDate, o => o.MapFrom(s => s.CreatedDate))
                .ForMember(d => d.UpdateDate, o => o.MapFrom(s => s.UpdatedDate))
                .ForMember(d => d.CreatorName, o => o.Ignore())
                .ForMember(d => d.Link, o => o.Ignore());

            Mapper.CreateMap<GroupCreateModel, Group>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreatedDate, o => o.Ignore())
                .ForMember(d => d.UpdatedDate, o => o.Ignore())
                .ForMember(d => d.IsHidden, o => o.Ignore())
                .ForMember(d => d.GroupTypeId, o => o.Ignore())
                .ForMember(d => d.ImageId, o => o.Ignore())
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.CreatorId, o => o.MapFrom(s => s.CreatorId));

            Mapper.CreateMap<Group, GroupCreateModel>()
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.CreatorId, o => o.MapFrom(s => s.CreatorId))
                .ForMember(d => d.Media, o => o.MapFrom(el => el.ImageId))
                .ForMember(d => d.AllowedMediaExtentions, o => o.Ignore())
                .ForMember(d => d.MediaRootId, o => o.Ignore())
                .ForMember(d => d.NewMedia, o => o.Ignore());

            Mapper.CreateMap<Group, GroupEditModel>()
                .IncludeBase<Group, GroupCreateModel>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id));

            Mapper.CreateMap<GroupEditModel, Group>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ImageId, o => o.Ignore())
                .ForMember(d => d.CreatedDate, o => o.Ignore())
                .ForMember(d => d.UpdatedDate, o => o.Ignore())
                .ForMember(d => d.IsHidden, o => o.Ignore())
                .ForMember(d => d.CreatorId, o => o.Ignore())
                .ForMember(d => d.GroupTypeId, o => o.Ignore())
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description));

            Mapper.CreateMap<Group, GroupInfoViewModel>()
                .ForMember(d => d.MembersCount, o => o.Ignore())
                .ForMember(d => d.Creator, o => o.Ignore())
                .ForMember(d => d.IsMember, o => o.Ignore())
                .ForMember(d => d.GroupImageUrl, o => o.Ignore())
                .ForMember(d => d.CanUnsubscribe, o => o.Ignore())
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title));

            Mapper.CreateMap<Group, GroupViewModel>()
               .ForMember(d => d.MembersCount, o => o.Ignore())
               .ForMember(d => d.Creator, o => o.Ignore())
               .ForMember(d => d.IsMember, o => o.Ignore())
               .ForMember(d => d.GroupImageUrl, o => o.Ignore())
               .ForMember(d => d.GroupUrl, o => o.Ignore())
               .ForMember(d => d.HasImage, o => o.MapFrom(s => s.ImageId.HasValue))
               .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
               .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
               .ForMember(d => d.Title, o => o.MapFrom(s => s.Title));

            Mapper.CreateMap<ActivityFeedTabModel, GroupNavigationTabViewModel>()
                .ForMember(d => d.Title, o => o.Ignore())
                .ForMember(d => d.AlignRight, o => o.Ignore());

            Mapper.CreateMap<IIntranetUser, GroupMemberViewModel>()
                .ForMember(d => d.IsGroupAdmin, o => o.Ignore())
                .ForMember(d => d.CanUnsubscribe, o => o.Ignore())
                .ForMember(d => d.Name, o => o.MapFrom(s => s.DisplayedName));
        }
    }
}