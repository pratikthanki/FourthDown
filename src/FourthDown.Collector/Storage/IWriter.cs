using System.Collections.Generic;

namespace FourthDown.Collector.Storage
{
    public interface IWriter<in T>
    {
        int Write(IEnumerable<T> entity);
        int Write(T entity);
    }
}