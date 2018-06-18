namespace Uintra.Core.Constants
{
    public static class UmbracoAliases
    {
        public const string GalleryPreviewImageCrop = "galleryPreview";
        public const string EnumDropdownList = "EnumDropdownList";

        public static class Tags
        {
            public const string TagText = "text";
        }

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

        public static class Video
        {
            public const string ThumbnailUrlPropertyAlias = "thumbnailUrl";
            public const string VideoHeightPropertyAlias = "videoHeight";
            public const string VideoWidthPropertyAlias = "videoWidth";
        }

        public static class ImagePresets
        {
            public const string Thumbnail = "thumbnail";            
            public const string Preview = "preview";
            public const string PreviewTwo = "previewTwo";
            public const string GroupImageThumbnail = "groupImageThumbnail";
        }

        public static class ImageResize
        {
            public const string Thumbnail = "width=238&height=158&mode=crop";            
            public const string Preview = "width=720&height=478&mode=crop";
            public const string PreviewTwo = "width=359&height=239&mode=crop";
        }

        public static class QueryStringParameters
        {
            public const string ImagePreset = "preset";
        }
    }
}