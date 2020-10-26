using System.ComponentModel.DataAnnotations.Schema;

namespace Uintra20.Persistence
{
    public class UintraTableAttribute : TableAttribute
    {
        public UintraTableAttribute(string name) : base("Uintra_" + name)
        {
        }
    }
}