namespace BuildingBlocks.Common.Interfaces
{
    public interface IPageQuery
    {
        int PageIndex { get; set; }

        int PageSize { get; set; }
    }
}