using AutoMapper;

namespace Stize.Mapping.AutoMapper
{
    public abstract class Profile<TSource, TTarget> : Profile
    {
        protected Profile()
        {
            Configure(this);
        }

        protected virtual IMappingExpression<TSource, TTarget> MapSourceToTarget()
        {
            return this.CreateMap<TSource, TTarget>();
        }

        protected virtual IMappingExpression<TTarget, TSource> MapTargetToSource()
        {
            return this.CreateMap<TTarget, TSource>();
        }

        private static void Configure(Profile<TSource, TTarget> profile)
        {
            profile.MapSourceToTarget();
            profile.MapTargetToSource();
        }
    }


}
