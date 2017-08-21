using System;
using Newtonsoft.Json;
using uIntra.Core.TypeProviders;

namespace uIntra.Core.Activity
{
    public abstract class IntranetActivity : IIntranetActivity
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public IIntranetType Type { get; set; }

        [JsonIgnore]
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public DateTime ModifyDate { get; set; }

        [JsonIgnore]
        public bool IsPinActual { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsHidden { get; set; }

        public bool IsPinned { get; set; }

        public DateTime? EndPinDate { get; set; }
    }
}