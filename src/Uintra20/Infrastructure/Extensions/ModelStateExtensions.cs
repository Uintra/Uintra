using System.Text;
using System.Web.Http.ModelBinding;

namespace Uintra20.Infrastructure.Extensions
{
    public static class ModelStateExtensions
    {
        private static StringBuilder builder = new StringBuilder();
        public static string CollectErrors(this ModelStateDictionary src)
        {
            foreach (var value in src.Values)
            {
                foreach (var error in value.Errors)
                {
                    builder.AppendLine(error.ErrorMessage);
                }
            }

            return builder.ToString();
        }
    }
}