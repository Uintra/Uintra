namespace Uintra20.Core.UbaselineModels
{
    public class FileModel : UBaseline.Shared.Media.FileModel, IGenericPropertiesComposition
    {
        public GenericPropertiesCompositionModel GenericProperties { get; set; }
    }
}