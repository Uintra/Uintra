using System.Text;
using System.Web.Http.ModelBinding;

namespace Uintra20.Infrastructure.Extensions
{
    public static class ModelStateExtensions
    {
        private static StringBuilder _builder;

        public static string CollectErrors(this ModelStateDictionary src)
        {
            _builder = new StringBuilder();

            foreach (var value in src.Values)
            {
                foreach (var error in value.Errors)
                {
                    _builder.AppendLine(error.ErrorMessage);
                }
            }

            return _builder.ToString();
        }
    }
}