﻿namespace Merchello.Core
{
    using System;
    using System.Runtime.Remoting.Messaging;

    /// <summary>
    /// Extension methods for <see cref="DateTime"/>
    /// </summary>
    internal static class DateTimeExtensions
    {
        /// <summary>
        /// The SQL date time min value string.
        /// </summary>
        private const string SqlDateTimeMinValueString = "1753-1-1";

        /// <summary>
        /// The SQL date time max value string.
        /// </summary>
        private const string SqlDateTimeMaxValueString = "9999-12-31";

        /// <summary>
        /// Converts a date time min value to null.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime? ConverDateTimeMinValueToNull(this DateTime value)
        {
            return !value.Equals(DateTime.MinValue) ? value : (DateTime?)null;
        }

        public static DateTime? ConvertDateTimeMaxValueToNull(this DateTime value)
        {
            return !value.Equals(DateTime.MaxValue) ? value : (DateTime?)null;
        }

        public static DateTime ConvertDateTimeNullToMinValue(this DateTime? dt)
        {
            return dt == null ? DateTime.MinValue : dt.Value;
        }

        public static DateTime ConvertDateTimeNullToMaxValue(this DateTime? dt)
        {
            return dt == null ? DateTime.MaxValue : dt.Value;
        }

        /// <summary>
        /// Checks if parameter is a min SQL DateTime value and return the .NET DateTime Min.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime SqlDateTimeMinValueAsDateTimeMinValue(this DateTime value)
        {
            return !value.Equals(SqlDateTimeMinValue()) ? value : DateTime.MinValue;
        }

        /// <summary>
        /// Checks if parameter is a max SQL DateTime value and return the .NET DateTime Max.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime SqlDateTimeMaxValueAsSqlDateTimeMaxValue(this DateTime value)
        {
            return !value.Equals(SqlDateTimeMaxValue()) ? value : DateTime.MaxValue;
        }

        /// <summary>
        /// If value parameter passes is DateTime min returns the SQL Server min date time value
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime AsSqlDateTimeMinValue(this DateTime value)
        {
            return !value.Equals(DateTime.MinValue) ? value : SqlDateTimeMinValue();
        }

        /// <summary>
        /// If value parameter passes is DateTime max returns the SQL Server max date time value
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime AsSqlDateTimeMaxValue(this DateTime value)
        {
            return !value.Equals(DateTime.MinValue) ? value : SqlDateTimeMaxValue();
        }

        /// <summary>
        /// Parses the SQL DateTime min value string
        /// </summary>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        private static DateTime SqlDateTimeMinValue()
        {
            return DateTime.Parse(SqlDateTimeMinValueString);
        }

        /// <summary>
        /// Parses the SQL DateTime max value string
        /// </summary>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        private static DateTime SqlDateTimeMaxValue()
        {
            return DateTime.Parse(SqlDateTimeMaxValueString);
        }
    }
}