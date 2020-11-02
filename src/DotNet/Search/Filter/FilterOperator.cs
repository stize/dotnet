namespace Stize.DotNet.Search.Filter
{
    public enum FilterOperator
    {
        /// <summary>
        ///     Left operand must be smaller than the right one.
        /// </summary>
        IsLessThan = 0,

        /// <summary>
        ///     Left operand must be smaller than or equal to the right one.
        /// </summary>
        IsLessThanOrEqualTo = 1,

        /// <summary>
        ///     Left operand must be equal to the right one.
        /// </summary>
        IsEqualTo = 2,

        /// <summary>
        ///     Left operand must be different from the right one.
        /// </summary>
        IsNotEqualTo = 3,

        /// <summary>
        ///     Left operand must be larger than or equal to the right one.
        /// </summary>
        IsGreaterThan = 4,

        /// <summary>
        ///     Left operand must be larger than the right one.
        /// </summary>
        IsGreaterThanOrEqualTo = 5,

        /// <summary>
        ///     Left operand must start with the right one.
        /// </summary>
        StartsWith = 6,

        /// <summary>
        ///     Left operand must end with the right one.
        /// </summary>
        EndsWith = 7,

        /// <summary>
        ///     Left operand must contain the right one.
        /// </summary>
        Contains = 8,

        /// <summary>
        ///     Left operand must not contain the right one.
        /// </summary>
        DoesNotContain = 9,

        /// <summary>
        ///     Left operand must be contained in the right one.
        /// </summary>
        IsContainedIn = 10,

        /// <summary>
        ///     Left operand must not be contained in the right one.
        /// </summary>
        IsNotContainedIn = 11
    }
}