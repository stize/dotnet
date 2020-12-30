using System.Collections.Generic;

namespace Stize.Domain.Model
{
    public interface IEnvelopedModel<T>
    {
        int? Take { get; set;}
        int? Skip { get; set;}
        int Total { get; set;}
        IEnumerable<T> Data { get; set;}
    }
}