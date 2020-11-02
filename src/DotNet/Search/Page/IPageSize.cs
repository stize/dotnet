namespace Stize.DotNet.Search.Page
{
    /// <summary>
    /// PageSize interface
    /// </summary>
    public interface IPageSize
    {
        /// <summary>
        /// Number of elements to skip in a section
        /// </summary>
        int? Skip { get; set; }
        /// <summary>
        /// Number of elements to select
        /// </summary>
        int? Take { get; set; }
    }
}