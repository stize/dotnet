namespace Stize.DotNet.Search.Filter
{
    /// <summary>
    ///  FilterDescriptor class. Indicates by which properties the filtering should be performed and which conditions should be applied.
    ///  Several can be combined using the semicolon symbol (;)
    /// </summary>
    public class FilterDescriptor
    {
        /// <summary>
        /// Path or  property on which the filter will be applied. 
        /// It can also be used to indicate whether the Any function (with the | character) or the All function (with the & character) will be applied.
        /// </summary>
        public string Member { get; set; }

        /// <summary>
        /// String representing the value or, in case of an array, comma-separated values.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Operator that conditions filtering.
        /// </summary>
        public FilterOperator Operator { get; set; }
    }
}