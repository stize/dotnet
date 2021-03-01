using Stize.Persistence.InquiryHandler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Stize.Persistence.InquiryDispatcher
{
    public class InquiryHandlerFactoryOptions
    {
        private readonly IList<Type> inquiryHandlers = new Collection<Type>();

        public IEnumerable<Type> InquiryHandlers => new ReadOnlyCollection<Type>(inquiryHandlers);

        public void AddHandler(Type handlerType)
        {
            if (handlerType == null) throw new ArgumentNullException(nameof(handlerType));

            var t = typeof(IInquiryHandler<,>);
            if (!handlerType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == t))
            {
                throw new ArgumentException($"The type {handlerType.Name} does not implements {t.Name}");
            }
            inquiryHandlers.Insert(0, handlerType);
        }
    }
}