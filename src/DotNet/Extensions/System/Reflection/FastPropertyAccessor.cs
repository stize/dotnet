
using Stize.DotNet.Extensions.Common;

namespace System.Reflection
{
    /// <summary>
    /// FastPropertyAccessor is a <see cref="PropertyAccessor{TStructuralType}"/> that speeds up (compares to reflection)
    /// a Getter and Setter for the PropertyInfo of TEntityType provided via the constructor.
    /// </summary>
    /// <typeparam name="T">The type on which the PropertyInfo exists</typeparam>
    public class FastPropertyAccessor<T> : PropertyAccessor<T> where T : class
    {
        private readonly bool isCollection;
        private readonly PropertyInfo property;
        private readonly Action<T, object> setter;
        private readonly Func<object, object> getter;

        public FastPropertyAccessor(PropertyInfo property)
            : base(property)
        {
            this.property = property;
            this.isCollection = property.PropertyType.IsCollection();

            if (!this.isCollection)
            {
                this.setter = PropertyHelper.MakeFastPropertySetter<T>(property);
            }
            this.getter = PropertyHelper.MakeFastPropertyGetter(property);
        }

        public override object GetValue(T instance)
        {
            if (instance == null)
            {
                throw Error.ArgumentNull(nameof(instance));
            }
            return this.getter(instance);
        }

        public override void SetValue(T instance, object value)
        {
            if (instance == null)
            {
                throw Error.ArgumentNull(nameof(instance));
            }

            if (this.isCollection)
            {
                Deserialization.SetCollectionProperty(instance, this.property, value, true);
            }
            else
            {
                this.setter(instance, value);
            }
        }
    }
}