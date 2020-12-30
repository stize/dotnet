﻿using System;

namespace Stize.DotNet.Delta
{
    /// <summary>
    /// Represents a <see cref="Delta"/> that can be used when a backing CLR type exists for
    /// the object type and complex type whose changes are tracked.
    /// </summary>
    public abstract class TypedDelta : Delta
    {
        /// <summary>
        /// Gets the actual type of the structural object for which the changes are tracked.
        /// </summary>
        public abstract Type StructuredType { get; }

        /// <summary>
        /// Gets the expected type of the object for which the changes are tracked.
        /// </summary>
        public abstract Type ExpectedClrType { get; }

        /// <summary>
        /// Helper method to check whether the given type is Delta generic type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if it is a Delta generic type; false otherwise.</returns>
        internal static bool IsDeltaOfT(Type type)
        {
            return type != null && type.IsGenericType() && type.GetGenericTypeDefinition() == typeof(Delta<>);
        }


    }
}