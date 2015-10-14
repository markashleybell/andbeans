using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace andbeans.Models
{
    public enum CacheExpiry : int
    {
        OneMinute = 60,
        TwoMinutes = 120,
        ThreeMinutes = 180,
        FourMinutes = 240,
        FiveMinutes = 300,
        TenMinutes = 600,
        FifteenMinutes = 900,
        TwentyMinutes = 1200,
        ThirtyMinutes = 1800,
        OneHour = 3600,
        ThreeHours = 10800,
        TwelveHours = 43200
    }
}