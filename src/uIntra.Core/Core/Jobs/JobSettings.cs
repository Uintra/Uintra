using System;

namespace uIntra.Core.Jobs
{
    public class JobSettings
    {
        public JobTimeType TimeType { get; set; }

        public JobRunTypeEnum RunType { get; set; }

        public DateTime? Date { get; set; }

        public int? Time { get; set; }

        public bool IsEnabled { get; set; }
    }
}
