namespace Defender.Common.DB.Pagination;

public record PagedResult<T>
{
    public long TotalItemsCount { get; set; }
    public long CurrentPage { get; set; }
    public long PageSize { get; set; }
    public long TotalPagesCount => TotalItemsCount / PageSize + 1;
    public IList<T> Items { get; set; } = new List<T>();

    public static PagedResult<N> FromPagedResult<N>(
        PagedResult<T> pagedResult,
        Func<T,N> mapper
        )
    {
        return new PagedResult<N>
        {
            TotalItemsCount = pagedResult.TotalItemsCount,
            CurrentPage = pagedResult.CurrentPage,
            PageSize = pagedResult.PageSize,
            Items = pagedResult.Items.Select(mapper).ToList()
        };
    }
}
