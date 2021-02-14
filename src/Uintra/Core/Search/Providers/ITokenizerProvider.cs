using Nest;

namespace Uintra.Core.Search.Providers
{
    public interface ITokenizerProvider
    {
        AnalysisDescriptor Apply(AnalysisDescriptor analysis);
    }
}
