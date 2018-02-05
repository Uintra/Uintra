namespace uIntra.Core.Constants
{
    public static class UmbracoAliases
    {
        public const string GalleryPreviewImageCrop = "galleryPreview";
        public const string EnumDropdownList = "EnumDropdownList";

        public static class Media
        {
            public const string ImageTypeAlias = "Image";
            public const string FolderTypeAlias = "Folder";
            public const string FileTypeAlias = "File";
            public const string VideoTypeAlias = "video";

            public const string UmbracoFilePropertyAlias = "umbracoFile";

            public const string MediaHeight = "umbracoHeight";
            public const string MediaWidth = "umbracoWidth";
            public const string MediaExtension = "umbracoExtension";
            public const int DefaultActivityOverviewImagesCount = 3;
            public const string IsDeletedPropertyTypeAlias = "isDeleted";
            public const string IsDeletedDataTypeDefinitionName = "Media - Is deleted - TrueFalse";
            public const string UseInSearchPropertyAlias = "useInSearch";
        }

        public static class ImagePresets
        {
            public const string Thumbnail = "thumbnail";         
            public const string Preview = "preview";
            public const string GroupImageThumbnail = "groupImageThumbnail";
        }

        public static class QueryStringParameters
        {
            public const string ImagePreset = "preset";
        }
    }
}