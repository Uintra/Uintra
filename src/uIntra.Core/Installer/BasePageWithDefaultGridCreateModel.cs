using System.ComponentModel.DataAnnotations;

namespace uIntra.Core.Installer
{
    public class BasePageWithDefaultGridCreateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Alias { get; set; }
        [Required]
        public string Icon { get; set; }
        public string ParentAlias { get; set; }
    }
}
