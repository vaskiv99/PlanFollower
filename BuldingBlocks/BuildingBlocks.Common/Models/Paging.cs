namespace BuildingBlocks.Common.Models
{
    /// <summary>
    /// Represent paging response
    /// </summary>
    public class Paging
    {
        public Paging(long numberOfRecords, long numberOfPages)
        {
            NumberOfRecords = numberOfRecords;
            NumberOfPages = numberOfPages;
        }

        public long NumberOfPages { get; }
        public long NumberOfRecords { get; }
    }
}