using System.ComponentModel.DataAnnotations;

namespace Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps
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
