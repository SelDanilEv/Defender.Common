namespace Defender.Common.Pagination;

public record PagedResult<T>
{
    public long TotalItemsCount { get; set; }
    public long CurrentPage { get; set; }
    public long PageSize { get; set; }
    public long TotalPagesCount => TotalItemsCount / PageSize + 1;

    public IList<T> Items { get; set; }
}
