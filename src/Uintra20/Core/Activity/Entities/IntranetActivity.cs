using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Uintra20.Features.Location.Models;

namespace Uintra20.Core.Activity.Entities
{
    public abstract class IntranetActivity : IIntranetActivity
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Enum Type { get; set; }

        [JsonIgnore]
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public DateTime ModifyDate { get; set; }

        [JsonIgnore]
        public bool IsPinActual { get; set; }

        [JsonIgnore]
        public IEnumerable<int> MediaIds { get; set; } = Enumerable.Empty<int>();

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsHidden { get; set; }

        public bool IsPinned { get; set; }

        public DateTime? EndPinDate { get; set; }

        [JsonIgnore]
        public ActivityLocation Location { get; set; }
    }
}