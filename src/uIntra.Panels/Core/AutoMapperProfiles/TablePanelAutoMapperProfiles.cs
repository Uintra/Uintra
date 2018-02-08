using AutoMapper;
using uIntra.Panels.Core.Models;
using uIntra.Panels.Core.Models.Table;

namespace uIntra.Panels.Core.AutoMapperProfiles
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
