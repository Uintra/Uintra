using UBaseline.Shared.Property;

namespace Uintra.Core.UbaselineModels
{
    public class FolderModel : UBaseline.Shared.Media.FolderModel
    {
        public PropertyModel<string> FolderType { get; set; }
        public PropertyModel<string> AllowedMediaExtensions { get; set; }
    }
}