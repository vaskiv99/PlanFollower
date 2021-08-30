using BuildingBlocks.Common.Interfaces;

namespace BuildingBlocks.Common.Models
{
    public class PageQuery : IPageQuery
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}