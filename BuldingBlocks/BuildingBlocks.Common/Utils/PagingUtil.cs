using BuildingBlocks.Common.Interfaces;
using BuildingBlocks.Common.Models;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Common.Utils
{
    /// <summary>
    /// Service, which has some methods, for working with pagination
    /// </summary>
    public static class PagingUtil
    {
        public static Paging CreatePaging(this IPageQuery query, long numberOfRecords)
        {
            var numberOfPages = DetermineNumberOfPages(query.PageSize, numberOfRecords);

            return new Paging(numberOfRecords, numberOfPages);
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, IPageQuery dto) where T : class
        {
            if (dto is null)
            {
                return query;
            }

            if (dto.PageSize == 0)
            {
                return query;
            }

            return query
                .Skip(dto.PageSize * dto.PageIndex)
                .Take(dto.PageSize);
        }

        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> query, IPageQuery dto) where T : class
        {
            if (dto is null) return query;

            if (dto.PageSize == 0)
            {
                return query;
            }

            return query
                .Skip(dto.PageSize * dto.PageIndex)
                .Take(dto.PageSize);
        }

        private static long DetermineNumberOfPages(int pageSize, long numberOfRecords)
        {
            if (pageSize == 0) return 1;

            return numberOfRecords / pageSize + (numberOfRecords % pageSize > 0 ? 1 : 0);
        }
    }
}