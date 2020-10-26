using System.ComponentModel.DataAnnotations.Schema;

namespace Uintra.Persistence
{
    public class UintraTableAttribute : TableAttribute
    {
        public UintraTableAttribute(string name) : base("Uintra_" + name)
        {
        }
    }
}