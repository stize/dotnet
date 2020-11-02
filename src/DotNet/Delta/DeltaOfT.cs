using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Stize.DotNet.Delta.Common;

namespace Stize.DotNet.Delta
{
    /// <summary>
    ///     A class the tracks changes (i.e. the Delta) for a particular <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">TStructuralType is the type of the instance this delta tracks changes for.</typeparam>
    public class Delta<T> : TypedDelta where T : class
    {
        
        private readonly PropertyInfo dynamicDictionaryPropertyInfo;

        private IReadOnlyDictionary<string, PropertyAccessor<T>> allProperties;
        private HashSet<string> changedDynamicProperties;

        private HashSet<string> changedProperties;

        // Nested resources or structures changed at this level.
        private IDictionary<string, object> deltaNestedResources;
        private IDictionary<string, object> dynamicDictionaryCache;

        private T instance;
        private Type structuredType;
        private HashSet<string> updateableProperties;

        /// <summary>
        ///     Initializes a new instance of <see cref="Delta{TStructuralType}" />.
        /// </summary>
        public Delta() : this(typeof(T))
        {
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Delta{TStructuralType}" />.
        /// </summary>
        /// <param name="structuralType">
        ///     The derived entity type or complex type for which the changes would be tracked.
        ///     <paramref name="structuralType" /> should be assignable to instances of <typeparamref name="T" />.
        /// </param>
        public Delta(Type structuralType)
            : this(structuralType, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Delta{TStructuralType}" />.
        /// </summary>
        /// <param name="structuralType">
        ///     The derived entity type or complex type for which the changes would be tracked.
        ///     <paramref name="structuralType" /> should be assignable to instances of <typeparamref name="T" />.
        /// </param>
        /// <param name="updateableProperties">
        ///     The set of properties that can be updated or reset. Unknown property
        ///     names, including those of dynamic properties, are ignored.
        /// </param>
        public Delta(Type structuralType, IEnumerable<string> updateableProperties)
            : this(structuralType, updateableProperties, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="Delta{TStructuralType}" />.
        /// </summary>
        /// <param name="structuralType">
        ///     The derived entity type or complex type for which the changes would be tracked.
        ///     <paramref name="structuralType" /> should be assignable to instances of <typeparamref name="T" />.
        /// </param>
        /// <param name="updateableProperties">
        ///     The set of properties that can be updated or reset. Unknown property
        ///     names, including those of dynamic properties, are ignored.
        /// </param>
        /// <param name="dynamicDictionaryPropertyInfo">
        ///     The property info that is used as dictionary of dynamic
        ///     properties. <c>null</c> means this entity type is not open.
        /// </param>
        public Delta(Type structuralType, IEnumerable<string> updateableProperties, PropertyInfo dynamicDictionaryPropertyInfo)
        {
            this.dynamicDictionaryPropertyInfo = dynamicDictionaryPropertyInfo;
            this.Reset(structuralType);
            this.InitializeProperties(updateableProperties);
        }

        /// <inheritdoc />
        public override Type StructuredType => this.structuredType;

        /// <inheritdoc />
        public override Type ExpectedClrType => typeof(T);

        /// <inheritdoc />
        public override void Clear()
        {
            this.Reset(this.structuredType);
        }

        /// <inheritdoc />
        public override bool TrySetPropertyValue(string name, object value)
        {
            var type = value?.GetType();
            if (IsDelta(type) || IsDeltaCollection(type))
            {
                return this.TrySetNestedResourceInternal(name, value);
            }

            return this.TrySetPropertyValueInternal(name, value);
        }

        /// <inheritdoc />
        public override bool TryGetPropertyValue(string name, out object value)
        {
            if (name == null)
            {
                throw Error.ArgumentNull(nameof(name));
            }

            if (this.dynamicDictionaryPropertyInfo != null)
            {
                if (this.dynamicDictionaryCache == null)
                {
                    this.dynamicDictionaryCache =
                        GetDynamicPropertyDictionary(this.dynamicDictionaryPropertyInfo, this.instance, false);
                }

                if (this.dynamicDictionaryCache != null && this.dynamicDictionaryCache.TryGetValue(name, out value))
                {
                    return true;
                }
            }

            if (this.deltaNestedResources.ContainsKey(name))
            {
                // If this is a nested resource, get the value from the dictionary of nested resources.
                var deltaNestedResource = this.deltaNestedResources[name];

                Contract.Assert(deltaNestedResource != null, "deltaNestedResource != null");
                Contract.Assert(IsDeltaOfT(deltaNestedResource.GetType()));

                // Get the Delta<{NestedResourceType}>._instance using Reflection.
                var field = deltaNestedResource.GetType().GetField("_instance", BindingFlags.NonPublic | BindingFlags.Instance);
                Contract.Assert(field != null, "field != null");
                value = field.GetValue(deltaNestedResource);
                return true;
            }

            // try to retrieve the value of property.
            if (this.allProperties.TryGetValue(name, out var cacheHit))
            {
                value = cacheHit.GetValue(this.instance);
                return true;
            }

            value = null;
            return false;
        }

        /// <inheritdoc />
        public override bool TryGetPropertyType(string name, out Type type)
        {
            if (name == null)
            {
                throw Error.ArgumentNull(nameof(name));
            }

            if (this.dynamicDictionaryPropertyInfo != null)
            {
                if (this.dynamicDictionaryCache == null)
                {
                    this.dynamicDictionaryCache =
                        GetDynamicPropertyDictionary(this.dynamicDictionaryPropertyInfo, this.instance, false);
                }

                if (this.dynamicDictionaryCache != null &&
                    this.dynamicDictionaryCache.TryGetValue(name, out var dynamicValue))
                {
                    if (dynamicValue == null)
                    {
                        type = null;
                        return false;
                    }

                    type = dynamicValue.GetType();
                    return true;
                }
            }

            if (this.allProperties.TryGetValue(name, out var value))
            {
                type = value.Property.PropertyType;
                return true;
            }

            type = null;
            return false;
        }

        /// <summary>
        ///     Returns the known properties that have been modified through this <see cref="Delta" /> as an
        ///     <see cref="IEnumerable{T}" /> of property Names.
        ///     Includes the structural properties at current level.
        ///     Does not include the names of the changed dynamic properties.
        /// </summary>
        public override IEnumerable<string> GetChangedPropertyNames()
        {
            return this.changedProperties.Concat(this.deltaNestedResources.Keys);
        }

        /// <summary>
        ///     Returns the known properties that have not been modified through this <see cref="Delta" /> as an
        ///     <see cref="IEnumerable{T}" /> of property Names. Does not include the names of the changed dynamic
        ///     properties.
        /// </summary>
        public override IEnumerable<string> GetUnchangedPropertyNames()
        {
            return this.updateableProperties.Except(this.GetChangedPropertyNames());
        }

        /// <summary>
        ///     Returns the instance that holds all the changes (and original values) being tracked by this Delta.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification =
            "Not appropriate to be a property")]
        public T GetInstance()
        {
            return this.instance;
        }

        /// <summary>
        ///     Copies the changed property values from the underlying entity (accessible via <see cref="GetInstance()" />)
        ///     to the <paramref name="original" /> entity recursively.
        /// </summary>
        /// <param name="original">The entity to be updated.</param>
        public void CopyChangedValues(T original)
        {
            if (original == null)
            {
                throw Error.ArgumentNull(nameof(original));
            }

            // Delta parameter type cannot be derived type of original
            // to prevent unrecognizable information from being applied to original resource.
            if (!this.structuredType.IsAssignableFrom(original.GetType()))
            {
                //throw Error.Argument("original", SRResources.DeltaTypeMismatch, _structuredType, original.GetType());
                throw new ArgumentException(
                    $"Cannot use Delta of type '{this.structuredType.FullName}' on an entity of type '{original.GetType().FullName}'");
            }

            RuntimeHelpers.EnsureSufficientExecutionStack();

            // For regular non-structural properties at current level.
            var propertiesToCopy = this.changedProperties.Select(s => this.allProperties[s]).ToArray();
            foreach (var propertyToCopy in propertiesToCopy) propertyToCopy.Copy(this.instance, original);

            this.CopyChangedDynamicValues(original);

            // For nested resources.
            foreach (var nestedResourceName in this.deltaNestedResources.Keys)
            {
                // Patch for each nested resource changed under this TStructuralType.
                dynamic deltaNestedResource = this.deltaNestedResources[nestedResourceName];
                if (!TryGetPropertyRef(original, nestedResourceName, out var originalNestedResource))
                {
                    throw Error.Argument(nestedResourceName, SRResources.DeltaNestedResourceNameNotFound, nestedResourceName, original.GetType());
                }

                if (originalNestedResource == null)
                {
                    // When patching original target of null value, directly set nested resource.
                    dynamic deltaObject = this.deltaNestedResources[nestedResourceName];
                    var deltaInstance = deltaObject.GetInstance();

                    // Recursively patch up the instance with the nested resources.
                    deltaObject.CopyChangedValues(deltaInstance);

                    this.allProperties[nestedResourceName].SetValue(original, deltaInstance);
                }
                else if (IsDeltaCollection(deltaNestedResource.GetType()))
                {
                    if (deltaNestedResource is IEnumerable deltaEnumerableNestedResource &&
                        originalNestedResource is IEnumerable originalEnumerableNestedResource)
                    {
                        var originalComparable = originalEnumerableNestedResource.OfType<IComparable>().ToArray();
                        foreach (dynamic delta in deltaEnumerableNestedResource)
                        {
                            var target = originalComparable.FirstOrDefault(c => c.CompareTo(delta) == 0);
                            if (target != null)
                            {
                                delta.CopyChangedValues(target);
                            }
                        }
                    }
                }
                else
                {
                    // Recursively patch the subtree.
                    bool isDeltaType = IsDeltaOfT(deltaNestedResource.GetType());
                    Contract.Assert(isDeltaType,
                        nestedResourceName + "'s corresponding value should be Delta<T> type but is not.");

                    deltaNestedResource.CopyChangedValues(originalNestedResource);
                }
            }
        }

        /// <summary>
        ///     Copies the unchanged property values from the underlying entity (accessible via <see cref="GetInstance()" />)
        ///     to the <paramref name="original" /> entity.
        /// </summary>
        /// <param name="original">The entity to be updated.</param>
        public void CopyUnchangedValues(T original)
        {
            if (original == null)
            {
                throw Error.ArgumentNull(nameof(original));
            }

            if (!this.structuredType.IsInstanceOfType(original))
            {
                throw Error.Argument("original", SRResources.DeltaTypeMismatch, this.structuredType,
                    original.GetType());
            }

            var propertiesToCopy = this.GetUnchangedPropertyNames().Select(s => this.allProperties[s]);
            foreach (var propertyToCopy in propertiesToCopy) propertyToCopy.Copy(this.instance, original);

            this.CopyUnchangedDynamicValues(original);
        }

        /// <summary>
        ///     Overwrites the <paramref name="original" /> entity with the changes tracked by this Delta.
        ///     <remarks>The semantics of this operation are equivalent to a HTTP PATCH operation, hence the name.</remarks>
        /// </summary>
        /// <param name="original">The entity to be updated.</param>
        public void Patch(T original)
        {
            this.CopyChangedValues(original);
        }

        /// <summary>
        ///     Overwrites the <paramref name="original" /> entity with the values stored in this Delta.
        ///     <remarks>The semantics of this operation are equivalent to a HTTP PUT operation, hence the name.</remarks>
        /// </summary>
        /// <param name="original">The entity to be updated.</param>
        public void Put(T original)
        {
            this.CopyChangedValues(original);
            this.CopyUnchangedValues(original);
        }

        private static void CopyDynamicPropertyDictionary(IDictionary<string, object> source,
            IDictionary<string, object> dest, PropertyInfo dynamicPropertyInfo, T targetEntity)
        {
            Contract.Assert(source != null);
            Contract.Assert(dynamicPropertyInfo != null);
            Contract.Assert(targetEntity != null);

            if (source.Count == 0)
            {
                dest?.Clear();
            }
            else
            {
                if (dest == null)
                {
                    dest = GetDynamicPropertyDictionary(dynamicPropertyInfo, targetEntity, true);
                }
                else
                {
                    dest.Clear();
                }

                foreach (var item in source) dest.Add(item);
            }
        }

        private static IDictionary<string, object> GetDynamicPropertyDictionary(PropertyInfo propertyInfo,
            T entity, bool create)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var propertyValue = propertyInfo.GetValue(entity);
            if (propertyValue != null)
            {
                return (IDictionary<string, object>)propertyValue;
            }

            if (create)
            {
                if (!propertyInfo.CanWrite)
                {
                    //throw Error.InvalidOperation(SRResources.CannotSetDynamicPropertyDictionary, propertyInfo.Name, entity.GetType().FullName);
                    throw new InvalidOperationException(
                        $"The dynamic dictionary property '{propertyInfo.Name}' of type '{entity.GetType().FullName}' cannot be set. The dynamic property dictionary must have a setter.");
                }

                IDictionary<string, object> newPropertyValue = new Dictionary<string, object>();

                propertyInfo.SetValue(entity, newPropertyValue);
                return newPropertyValue;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to get the property by the specified name.
        /// </summary>
        /// <param name="structural">The structural object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyRef">Output for property value.</param>
        /// <returns>true if the property is found; false otherwise.</returns>
        private static bool TryGetPropertyRef(T structural, string propertyName,
            out dynamic propertyRef)
        {
            propertyRef = null;
            var propertyInfo = structural.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                propertyRef = propertyInfo.GetValue(structural, null);
                return true;
            }

            return false;
        }

        private void Reset(Type structuralType)
        {
            if (structuralType == null)
            {
                throw new ArgumentNullException(nameof(structuralType));
            }

            if (!typeof(T).IsAssignableFrom(structuralType))
            {
                //                throw Error.InvalidOperation(SRResources.DeltaEntityTypeNotAssignable, structuralType, typeof(TStructuralType));
                throw new InvalidOperationException(
                    $"The actual entity type '{structuralType}' is not assignable to the expected type '{typeof(T)}'.");
            }

            this.instance = Activator.CreateInstance(structuralType) as T;
            this.changedProperties = new HashSet<string>();
            this.deltaNestedResources = new Dictionary<string, object>();
            this.structuredType = structuralType;

            this.changedDynamicProperties = new HashSet<string>();
            this.dynamicDictionaryCache = null;
        }

        private void InitializeProperties(IEnumerable<string> propertyNames)
        {
            this.allProperties = PropertyCache<T>.Get();

            if (propertyNames != null)
            {
                this.updateableProperties = new HashSet<string>(propertyNames);
                this.updateableProperties.IntersectWith(this.allProperties.Keys);
            }
            else
            {
                this.updateableProperties = new HashSet<string>(this.allProperties.Keys);
            }

            if (this.dynamicDictionaryPropertyInfo != null)
            {
                this.updateableProperties.Remove(this.dynamicDictionaryPropertyInfo.Name);
            }
        }

        // Copy changed dynamic properties and leave the unchanged dynamic properties
        private void CopyChangedDynamicValues(T targetEntity)
        {
            if (this.dynamicDictionaryPropertyInfo == null)
            {
                return;
            }

            if (this.dynamicDictionaryCache == null)
            {
                this.dynamicDictionaryCache =
                    GetDynamicPropertyDictionary(this.dynamicDictionaryPropertyInfo, this.instance, false);
            }

            var fromDictionary = this.dynamicDictionaryCache;
            if (fromDictionary == null)
            {
                return;
            }

            var toDictionary =
                GetDynamicPropertyDictionary(this.dynamicDictionaryPropertyInfo, targetEntity, false);

            IDictionary<string, object> tempDictionary = toDictionary != null
                ? new Dictionary<string, object>(toDictionary)
                : new Dictionary<string, object>();

            foreach (var dynamicPropertyName in this.changedDynamicProperties)
            {
                var dynamicPropertyValue = fromDictionary[dynamicPropertyName];

                // a dynamic property value equal to null, it means to remove this dynamic property
                if (dynamicPropertyValue == null)
                {
                    tempDictionary.Remove(dynamicPropertyName);
                }
                else
                {
                    tempDictionary[dynamicPropertyName] = dynamicPropertyValue;
                }
            }

            CopyDynamicPropertyDictionary(tempDictionary, toDictionary, this.dynamicDictionaryPropertyInfo,
                targetEntity);
        }

        // Missing dynamic structural properties MUST be removed or set to null in *Put*
        private void CopyUnchangedDynamicValues(T targetEntity)
        {
            if (this.dynamicDictionaryPropertyInfo == null)
            {
                return;
            }

            if (this.dynamicDictionaryCache == null)
            {
                this.dynamicDictionaryCache = GetDynamicPropertyDictionary(this.dynamicDictionaryPropertyInfo, this.instance, false);
            }

            var toDictionary = GetDynamicPropertyDictionary(this.dynamicDictionaryPropertyInfo, targetEntity, false);

            if (this.dynamicDictionaryCache == null)
            {
                toDictionary?.Clear();
            }
            else
            {
                IDictionary<string, object> tempDictionary = toDictionary != null
                    ? new Dictionary<string, object>(toDictionary)
                    : new Dictionary<string, object>();

                var removedSet = tempDictionary.Keys.Except(this.changedDynamicProperties).ToList();

                foreach (var name in removedSet) tempDictionary.Remove(name);

                CopyDynamicPropertyDictionary(tempDictionary, toDictionary, this.dynamicDictionaryPropertyInfo,
                    targetEntity);
            }
        }

        private bool TrySetPropertyValueInternal(string name, object value)
        {
            if (name == null)
            {
                throw Error.ArgumentNull(nameof(name));
            }

            if (this.dynamicDictionaryPropertyInfo != null)
            {
                // Dynamic property can have the same name as the dynamic property dictionary.
                if (name == this.dynamicDictionaryPropertyInfo.Name ||
                    !this.allProperties.ContainsKey(name))
                {
                    if (this.dynamicDictionaryCache == null)
                    {
                        this.dynamicDictionaryCache =
                            GetDynamicPropertyDictionary(this.dynamicDictionaryPropertyInfo, this.instance, true);
                    }

                    this.dynamicDictionaryCache[name] = value;
                    this.changedDynamicProperties.Add(name);
                    return true;
                }
            }

            if (!this.updateableProperties.Contains(name))
            {
                return false;
            }

            var cacheHit = this.allProperties[name];
            if (value == null && !cacheHit.Property.PropertyType.IsNullable())
            {
                return false;
            }

            var effectivePropertyType = cacheHit.Property.PropertyType.GetUnderlyingTypeOrSelf();
            var isGuid = effectivePropertyType == typeof(Guid) && value is string;
            var isInt32 = effectivePropertyType == typeof(int) && value is long l && l <= int.MaxValue;
            if (value != null && isGuid)
            {
                value = new Guid((string)value);
            }

            if (value != null && isInt32)
            {
                value = (int)(long)value;
            }

            // enum type
            if (value != null && effectivePropertyType.IsEnum)
            {
                try
                {
                    value = Enum.Parse(effectivePropertyType, value.ToString(), true);
                }
                catch (ArgumentException)
                {
                    // value is not a member of enum
                    return false;
                }
            }

            if (value != null && !effectivePropertyType.IsCollection() && !isGuid && !effectivePropertyType.IsInstanceOfType(value))
            {
                return false;
            }


            cacheHit.SetValue(this.instance, value);
            this.changedProperties.Add(name);
            return true;
        }

        private bool TrySetNestedResourceInternal(string name, object deltaNestedResource)
        {
            if (name == null)
            {
                throw Error.ArgumentNull(nameof(name));
            }

            if (!this.updateableProperties.Contains(name))
            {
                return false;
            }

            if (this.deltaNestedResources.ContainsKey(name))
            {
                // Ignore duplicated nested resource.
                return false;
            }

            // Add the nested resource in the hierarchy.
            // Note: We shouldn't add the structural properties to the <code>changedProperties</code>, which
            // is used for keeping track of changed non-structural properties at current level.
            this.deltaNestedResources[name] = deltaNestedResource;

            return true;
        }
    }
}