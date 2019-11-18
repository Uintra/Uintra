using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uintra20.Core.Extensions
{
    public static class ContextExtensions
    {
        public static bool HasFlagScalar(Enum a, Enum b) => HasFlagScalar(a.ToInt(), b.ToInt());

        public static bool HasFlagScalar(Enum a, int b) => HasFlagScalar(a.ToInt(), b);

        public static bool HasFlagScalar(int a, Enum b) => HasFlagScalar(a, b.ToInt());

        public static bool HasFlagScalar(int a, int b) => (a & b) != 0;

        public static bool ExactScalar(Enum a, Enum b) => a.ToInt() == b.ToInt();
    }
}