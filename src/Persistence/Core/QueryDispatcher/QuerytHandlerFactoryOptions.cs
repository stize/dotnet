using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Stize.Persistence.QueryDispatcher
{
    public class QuerytHandlerFactoryOptions
    {
        private readonly IList<Type> queryHandlers = new Collection<Type>();

        public IEnumerable<Type> QueryHandlers => new ReadOnlyCollection<Type>(queryHandlers);

        public void AddHandler(Type handlerType)
        {
            if (handlerType == null) throw new ArgumentNullException(nameof(handlerType));

            var t = typeof(IQueryRequestHandler<,>);
            if (!handlerType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == t))
            {
                throw new ArgumentException($"The type {handlerType.Name} does not implements {t.Name}");
            }
            queryHandlers.Insert(0, handlerType);
        }
    }
}