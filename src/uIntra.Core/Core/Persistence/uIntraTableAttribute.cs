using System.ComponentModel.DataAnnotations.Schema;

namespace uIntra.Core.Persistence
{
    public class uIntraTableAttribute : TableAttribute
    {
        public uIntraTableAttribute(string name) : base("uIntra_" + name)
        {
            
        }
    }
}
