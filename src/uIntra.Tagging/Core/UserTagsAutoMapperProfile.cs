using System;
using AutoMapper;
using uIntra.Core;
using uIntra.Tagging.UserTags.Models;

namespace uIntra.Tagging
{

    public class UserTagsAutoMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<UserTag, LabeledIdentity<Guid>>()
                .ForMember(f => f.Text, d => d.MapFrom(i => i.Text));
        }
    }
}