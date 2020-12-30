using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stize.Persistence.QueryHandler;

namespace Stize.Persistence
{
    public class QueryHandlerFactoryOptions
    {
        private readonly IList<Type> queryHandlers = new Collection<Type>();

        public IEnumerable<Type> QueryHandlers => new ReadOnlyCollection<Type>(this.queryHandlers);

        public void AddHandler(Type handlerType)
        {
            if (handlerType == null) throw new ArgumentNullException(nameof(handlerType));

            var t = typeof(IQueryHandler);
            if (!t.IsAssignableFrom(handlerType))
            {
                throw new ArgumentException($"The type {handlerType.Name} does not implements {t.Name}");
            }
            this.queryHandlers.Insert(0, handlerType);
        }
    }
}