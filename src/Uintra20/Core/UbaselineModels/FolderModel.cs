using UBaseline.Shared.Property;

namespace Uintra20.Core.UbaselineModels
{
    public class FolderModel : UBaseline.Shared.Media.FolderModel
    {
        public PropertyModel<string> FolderType { get; set; }
        public PropertyModel<string> AllowedMediaExtensions { get; set; }
    }
}