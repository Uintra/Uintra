﻿using System;
using System.Linq;
using System.Web;
using Uintra20.Infrastructure.ApplicationSettings;
using Uintra20.Infrastructure.Providers;

namespace Uintra20.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToIsoUtcString(this DateTime date)
        {
            return date.ToUniversalTime().ToString("o");
        }

        public static string ToDayFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.WithUserOffset();
            return date.ToString("dd");
        }
        public static string ToMonthFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.WithUserOffset();
            return date.ToString("MMM");
        }

        public static string ToDateFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.WithUserOffset();
            return date.ToString(dateTimeFormatProvider.DateFormat);
        }

        public static string ToTimeFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.WithUserOffset();
            return date.ToString(dateTimeFormatProvider.TimeFormat);
        }
        
        public static string ToDateTimeFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.WithUserOffset();
            return date.ToString(dateTimeFormatProvider.DateTimeFormat);
        }
        
        public static string ToDateTimeFormat(this DateTime? date)
        {
            if (!date.HasValue)
            {
                return string.Empty;
            }
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.Value.WithUserOffset();
            return date.Value.ToString(dateTimeFormatProvider.DateTimeFormat);
        }

        public static string ToDateTimeValuePickerFormat(this DateTime date)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            date = date.WithUserOffset();
            return date.ToString(dateTimeFormatProvider.DateTimeValuePickerFormat);
        }

        public static DateTime WithUserOffset(this DateTime utc)
        {
            var clientTimezoneProvider = HttpContext.Current.GetService<IClientTimezoneProvider>();

            return TimeZoneInfo.ConvertTimeFromUtc(utc, clientTimezoneProvider.ClientTimezone);
        }

        public static DateTime WithCorrectedDaylightSavingTime(this DateTime utc, DateTime source)
        {
            var appSettings = HttpContext.Current.GetService<IApplicationSettings>();

            if (!appSettings.DaytimeSavingOffset && source.IsDaylightSavingTime())
            {
                var currentYear = DateTime.Now.Year;

                var corrected = TimeZoneInfo.Local.GetAdjustmentRules()
                    .Where(adjustmentRule => adjustmentRule.DateStart.Year <= currentYear && adjustmentRule.DateEnd.Year >= currentYear)
                    .Aggregate(utc, (current, adjustmentRule) => current.Add(adjustmentRule.DaylightDelta));
                return corrected;
            }
            else
            {
                return utc;
            }
        }
    }
}