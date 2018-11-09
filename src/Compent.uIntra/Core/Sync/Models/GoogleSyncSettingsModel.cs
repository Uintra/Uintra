using System.ComponentModel.DataAnnotations;

namespace Compent.Uintra.Core.Sync.Models
{
    public class GoogleSyncSettingsModel
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public string ClientSecret { get; set; }

        [Required]
        public string Domain { get; set; }

        [Required]
        public string User { get; set; }
    }
}