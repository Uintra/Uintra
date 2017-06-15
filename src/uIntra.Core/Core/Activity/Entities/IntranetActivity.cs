using System;
using Newtonsoft.Json;

namespace uIntra.Core.Activity
{
    public interface IIntranetActivity
    {
        Guid Id { get; set; }
        IntranetActivityTypeEnum Type { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime ModifyDate { get; set; }
        bool IsPinActual { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        bool IsHidden { get; set; }
        bool IsPinned { get; set; }
        DateTime? EndPinDate { get; set; }
    }

    public abstract class IntranetActivity : IIntranetActivity
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public IntranetActivityTypeEnum Type { get; set; }

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