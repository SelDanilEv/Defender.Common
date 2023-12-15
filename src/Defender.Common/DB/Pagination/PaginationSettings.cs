using Defender.Common.Contst;
using Defender.Common.DB.Model;
using Defender.Common.Entities;
using MongoDB.Driver;

namespace Defender.Common.DB.Pagination;

public record PaginationSettings<T> where T : IBaseModel, new()
{
    public int Page { get; set; } = ConstValues.DefaultPaginationStartPage;
    public int PageSize { get; set; } = ConstValues.DefaultPaginationPageSize;
    public FilterDefinition<T> Filter { get; set; } = FilterDefinition<T>.Empty;

    public int Offset => (Page - 1) * PageSize;

    public static PaginationSettings<T> DefaultRequest()
    {
        return new PaginationSettings<T>();
    }

    public static PaginationSettings<T> FromPaginationRequest(PaginationRequest request)
    {
        return new PaginationSettings<T>
        {
            Page = request.Page > 0 ? request.Page : ConstValues.DefaultPaginationStartPage,
            PageSize = request.PageSize > 0 ? request.PageSize : ConstValues.DefaultPaginationPageSize,
        };
    }

    public PaginationSettings<T> AddFilter(FindModelRequest<T> request)
    {
        Filter = request.BuildFilterDefinition();

        return this;
    }
}
