using Uintra20.Core.MediaToolkit.Model;
using Uintra20.Core.MediaToolkit.Options;
using Uintra20.Core.MediaToolkit.Util;

namespace Uintra20.Core.MediaToolkit
{
    internal class EngineParameters
    {
        internal bool HasCustomArguments => !this.CustomArguments.IsNullOrWhiteSpace();
        internal ConversionOptions ConversionOptions { get; set; }
        internal string CustomArguments { get; set; }
        internal MediaFile InputFile { get; set; }
        internal MediaFile OutputFile { get; set; }
        internal FFmpegTask Task { get; set; }
    }
}