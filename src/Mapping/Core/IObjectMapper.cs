using System.Collections.Generic;

namespace Stize.Mapping
{
    /// <summary>
    /// Object mapper interface
    /// </summary>
    public interface IObjectMapper
    {
        /// <summary>
        /// Maps the given source to new target
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TTarget">Target type</typeparam>
        /// <param name="source">Object to map from</param>
        /// <returns>New object instance mapped</returns>
        TTarget Map<TSource, TTarget>(TSource source);

        /// <summary>
        /// Maps the given source to existing target instance
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TTarget">Target type</typeparam>
        /// <param name="source">Object to map from</param>
        /// <param name="target">Object to map to</param>
        /// <returns>target object mapped</returns>
        TTarget Map<TSource, TTarget>(TSource source, TTarget target);

        /// <summary>
        /// Maps a collection of source items to new targets
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TTarget">Target type</typeparam>
        /// <param name="source">Source collection of objects</param>
        /// <returns>A mapped collection</returns>
        IEnumerable<TTarget> Map<TSource, TTarget>(IEnumerable<TSource> source);
    }
}