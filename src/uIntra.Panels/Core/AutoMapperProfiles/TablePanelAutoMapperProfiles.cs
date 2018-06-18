using AutoMapper;
using Uintra.Panels.Core.Models.Table;

namespace Uintra.Panels.Core.AutoMapperProfiles
{
    public class TablePanelAutoMapperProfiles : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<TableEditorModel, TableEditorViewModel>()
                .ForMember(dst => dst.Cells, o => o.Ignore());

        }
    }
}
