using System;
using System.ComponentModel.DataAnnotations;

namespace uCommunity.Core.App_Plugins.Core.Activity.Models
{
    public class IntranetActivitySaveModelBase
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}