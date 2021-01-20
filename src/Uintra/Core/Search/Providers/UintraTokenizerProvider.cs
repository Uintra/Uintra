using Nest;

namespace Uintra.Core.Search.Providers
{
    public class UintraTokenizerProvider : ITokenizerProvider
    {
        public AnalysisDescriptor Apply(AnalysisDescriptor analysis)
        {
            analysis.Tokenizers(t => t
                .NGram("phoneNgram", d => d.MinGram(2).MaxGram(10).TokenChars(TokenChar.Digit))
                .NGram("ngram", d => d.MinGram(2).MaxGram(30))
                .Pattern("comma", p => p.Pattern(",")));

            return analysis;
        }
    }
}