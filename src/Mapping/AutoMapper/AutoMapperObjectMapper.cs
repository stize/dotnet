using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Stize.Mapping.AutoMapper
{
    /// <summary>
    /// AutoMapper IObjectMapper implementation
    /// </summary>
    public class AutoMapperObjectMapper : IObjectMapper
    {

        private readonly IMapper mapper;

        /// <summary>
        /// Creates a new instance of <see cref="AutoMapperObjectMapper"/>
        /// </summary>
        /// <param name="mapper">AutoMapper IMapper</param>
        public AutoMapperObjectMapper(IMapper mapper)
        {
            this.mapper = mapper;
        }

        ///<inheritdoc/>
        public TTarget Map<TSource, TTarget>(TSource source, TTarget target)
        {
            return this.mapper.Map(source, target);
        }

        ///<inheritdoc/>
        public TTarget Map<TSource, TTarget>(TSource source)
        {
            return this.mapper.Map<TSource, TTarget>(source);
        }

        ///<inheritdoc/>
        public IEnumerable<TTarget> Map<TSource, TTarget>(IEnumerable<TSource> source)
        {
            return source.Select(x => this.Map<TSource, TTarget>(x));
        }

        /// <summary>
        /// Maps properties from a <paramref name="source"/> object of type <paramref name="sourceType"/> to a <paramref name="target"/> object of type <paramref name="targetType"/>
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="target">Target object</param>
        /// <param name="sourceType">Source type</param>
        /// <param name="targetType">Target type</param>
        /// <returns><paramref name="source"/> mapped into <paramref name="target"/></returns>
        public object Map(object source, object target, Type sourceType, Type targetType)
        {
            return this.mapper.Map(source, target, sourceType, targetType);
        }
    }
}