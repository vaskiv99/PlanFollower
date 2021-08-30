using System.Collections.Generic;

namespace BuildingBlocks.Common.Models
{
    /// <summary>
    /// Represent, query result with paging
    /// </summary>
    /// <typeparam name="T">Type of data</typeparam>
    public class QueryResult<T>
    {
        public QueryResult(ICollection<T> data, long numberOfPages, long numberOfRecords)
        {
            Result = data;
            Paging = new Paging(numberOfRecords, numberOfPages);
        }

        public QueryResult(ICollection<T> data, Paging paging)
        {
            Result = data;
            Paging = paging;
        }

        public QueryResult()
        {
            Result = new List<T>();
            Paging = new Paging(0, 0);
        }

        public ICollection<T> Result { get; }
        public Paging Paging { get; }
    }
}