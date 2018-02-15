using System;
using AutoMapper;
using Uintra.Core;
using Uintra.Tagging.UserTags.Models;

namespace Uintra.Tagging
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