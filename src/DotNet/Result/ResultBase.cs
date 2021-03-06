﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stize.DotNet.Result.Reasons;

namespace Stize.DotNet.Result
{
    public abstract class ResultBase
    {
        public bool IsSuccess { get; protected set; }

        public IEnumerable<Reason> Reasons => new ReadOnlyCollection<Reason>(this.InternalReasons);

        protected readonly List<Reason> InternalReasons = new List<Reason>();
    }

    public abstract class ResultBase<T> :ResultBase
        where T : ResultBase<T>
    {
        public T WithReason(Reason reason)
        {
            this.InternalReasons.Add(reason);
            return (T)this;
        }

        public T WithReasons(IEnumerable<Reason> reasons)
        {
            this.InternalReasons.AddRange(reasons);
            return (T)this;
        }
    }
}