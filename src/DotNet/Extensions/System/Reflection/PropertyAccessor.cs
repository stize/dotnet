using Stize.DotNet.Extensions.Common;

namespace System.Reflection
{
    /// <summary>
    /// Represents a strategy for Getting and Setting a PropertyInfo on <typeparamref name="T" />
    /// </summary>
    /// <typeparam name="T">The type that contains the PropertyInfo</typeparam>
    public abstract class PropertyAccessor<T> where T : class
    {
        protected PropertyAccessor(PropertyInfo property)
        {
            if (property == null)
            {
                throw Error.ArgumentNull(nameof(property));
            }

            this.Property = property;
            if (this.Property.GetGetMethod() == null ||
                !property.PropertyType.IsCollection() && this.Property.GetSetMethod() == null)
            {
                throw Error.Argument(SRResources.PropertyMustHavePublicGetterAndSetter, nameof(property));
            }
        }

        public PropertyInfo Property { get; }

        public void Copy(T from, T to)
        {
            if (from == null)
            {
                throw Error.ArgumentNull(nameof(from));
            }

            if (to == null)
            {
                throw Error.ArgumentNull(nameof(to));
            }

            this.SetValue(to, this.GetValue(from));
        }

        public abstract object GetValue(T instance);

        public abstract void SetValue(T instance, object value);
    }
}