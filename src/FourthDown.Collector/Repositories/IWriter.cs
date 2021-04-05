using System.Collections.Generic;

namespace FourthDown.Collector.Repositories
{
    public interface IWriter<in T>
    {
        int Write(IEnumerable<T> entity);
        int Write(T entity);
    }
}