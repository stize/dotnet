using System;
using System.Globalization;

namespace Stize.DotNet.Extensions.Common
{
    internal static class Error
    {
        /// <summary>
        /// Formats the specified resource string using <see cref="M:CultureInfo.CurrentCulture" />.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>The formatted string.</returns>
        internal static string Format(string format, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        /// Creates an <see cref="ArgumentException" /> with the provided properties.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="paramName">represents the name of the parameter that causes the error.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static ArgumentException Argument(string messageFormat, string paramName)
        {
            return new ArgumentException(messageFormat, paramName);
        }

        /// <summary>
        /// Creates an <see cref="ArgumentNullException" /> with the provided properties.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="parameterName">The name of the parameter that causes the current exception.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static ArgumentNullException ArgumentNull(string parameterName, string messageFormat = null)
        {
            return new ArgumentNullException(parameterName, messageFormat ?? string.Empty);
        }
    }
}
