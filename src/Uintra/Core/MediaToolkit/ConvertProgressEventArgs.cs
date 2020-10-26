using System;

namespace Uintra.Core.MediaToolkit
{
    public class ConvertProgressEventArgs : EventArgs
    {
        public ConvertProgressEventArgs(TimeSpan processed, TimeSpan totalDuration, long? frame, double? fps, int? sizeKb,
            double? bitrate)
        {
            TotalDuration = totalDuration;
            ProcessedDuration = processed;
            Frame = frame;
            Fps = fps;
            SizeKb = sizeKb;
            Bitrate = bitrate;
        }

        public long? Frame { get; private set; }
        public double? Fps { get; private set; }
        public int? SizeKb { get; private set; }
        public TimeSpan ProcessedDuration { get; private set; }
        public double? Bitrate { get; private set; }
        public TimeSpan TotalDuration { get; internal set; }
    }
}