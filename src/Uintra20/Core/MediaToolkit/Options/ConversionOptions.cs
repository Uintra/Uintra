using System;

namespace Uintra20.Core.MediaToolkit.Options
{
    public class ConversionOptions
    {
        
        public void CutMedia(TimeSpan seekToPosition, TimeSpan length)
        {
            this.Seek = seekToPosition;
            this.MaxVideoDuration = length;
        }

    
        public int? AudioBitRate = null;
        
        public AudioSampleRate AudioSampleRate = AudioSampleRate.Default;

        public TimeSpan? MaxVideoDuration = null;

        public TimeSpan? Seek = null;
        
        public Target Target = Target.Default;
        
        public TargetStandard TargetStandard = TargetStandard.Default;

        public VideoAspectRatio VideoAspectRatio = VideoAspectRatio.Default;

        public int? VideoBitRate = null;

        public int? VideoFps = null;

        public VideoSize VideoSize = VideoSize.Default;
        
        public int? CustomWidth { get; set; }

        public int? CustomHeight { get; set; }

        public CropRectangle SourceCrop { get; set; }

        public bool BaselineProfile { get; set; }
    }

}