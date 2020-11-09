using System;
using System.Collections.Generic;

namespace Stize.DotNet.Search.Filter
{
    public class FilterDescriptorFactory
    {
        public static string[] Tokens =
        {
            "<=","<",
            ">=",">",
            "!=", "=",
            "$startswith$", "$endswith$",
            "$contains$", "$notContains$",
            "$in$", "$notIn$"
        };

        public static IDictionary<FilterOperator, string> OperatorToToken = new Dictionary<FilterOperator, string>
        {
            {FilterOperator.IsLessThan, "<"},
            {FilterOperator.IsLessThanOrEqualTo, "<="},
            {FilterOperator.IsEqualTo, "="},
            {FilterOperator.IsNotEqualTo, "!="},
            {FilterOperator.IsGreaterThan, ">"},
            {FilterOperator.IsGreaterThanOrEqualTo, ">="},
            {FilterOperator.StartsWith, "$startswith$"},
            {FilterOperator.EndsWith, "$endswith$"},
            {FilterOperator.Contains, "$contains$"},
            {FilterOperator.DoesNotContain, "$notContains$"},
            {FilterOperator.IsContainedIn, "$in$"},
            {FilterOperator.IsNotContainedIn, "$notIn$"}
        };


        public static IDictionary<string, FilterOperator> TokenToOperator = new Dictionary<string, FilterOperator>
        {
            {"<", FilterOperator.IsLessThan},
            {"<=", FilterOperator.IsLessThanOrEqualTo},
            {"=", FilterOperator.IsEqualTo},
            {"!=", FilterOperator.IsNotEqualTo},
            {">", FilterOperator.IsGreaterThan},
            {">=", FilterOperator.IsGreaterThanOrEqualTo},
            {"$startswith$", FilterOperator.StartsWith},
            {"$endswith$", FilterOperator.EndsWith},
            {"$contains$", FilterOperator.Contains},
            {"$notContains$", FilterOperator.DoesNotContain},
            {"$in$", FilterOperator.IsContainedIn},
            {"$notIn$", FilterOperator.IsNotContainedIn}
        };

        public static FilterDescriptor Create(string filterValue)
        {
            var splitted = filterValue.Split(Tokens, StringSplitOptions.RemoveEmptyEntries);
            if (splitted.Length == 0)
            {
                return null;
            }

            var member = splitted[0];
            if (string.IsNullOrEmpty(member))
            {
                return null;
            }

            var value = (splitted.Length == 2) ? splitted[1] : "";

            var key = filterValue.TrimStart(member.ToCharArray()).TrimEnd(value.ToCharArray());
            if (!TokenToOperator.TryGetValue(key, out var filterOperatorValue))
            {
                return null;
            }

            return new FilterDescriptor
            {
                Member = member,
                Operator = filterOperatorValue,
                Value = value
            };
        }
    }
}