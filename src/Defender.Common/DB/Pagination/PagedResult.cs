namespace Defender.Common.DB.Pagination;

public record PagedResult<T>
{
    public long TotalItemsCount { get; set; }
    public long CurrentPage { get; set; }
    public long PageSize { get; set; }
    public long TotalPagesCount => TotalItemsCount / PageSize + 1;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public IList<T> Items { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
