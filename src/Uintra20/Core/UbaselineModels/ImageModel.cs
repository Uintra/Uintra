namespace Uintra20.Core.UbaselineModels
{
    public class ImageModel : UBaseline.Shared.Media.ImageModel, IGenericPropertiesComposition
    {
        public GenericPropertiesCompositionModel GenericProperties { get; set; }
    }
}