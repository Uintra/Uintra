using System;
using Uintra20.Core.Jobs.Enums;

namespace Uintra20.Core.Jobs
{
    public class JobConfiguration
    {
        public JobTimeTypeEnum TimeType { get; set; }

        public JobRunTypeEnum RunType { get; set; }

        public DateTime? Date { get; set; }

        public int? AtHour { get; set; }

        public int? AtMinutes { get; set; }

        public int? Time { get; set; }

        public bool Enabled { get; set; }
    }
}
