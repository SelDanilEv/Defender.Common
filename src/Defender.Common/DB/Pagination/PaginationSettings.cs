﻿using Defender.Common.Consts;
using Defender.Common.DB.Model;
using Defender.Common.Entities;
using MongoDB.Driver;

namespace Defender.Common.DB.Pagination;

public record PaginationSettings<T> : PaginationRequest where T : IBaseModel, new()
{
    public FilterDefinition<T> Filter { get; set; } = FilterDefinition<T>.Empty;
    public SortDefinition<T> Sort { get; set; } = Builders<T>.Sort.Ascending(x => x.Id);

    public int Offset => Page * PageSize;

    private static PaginationSettings<T> DefaultRequest()
    {
        return new PaginationSettings<T>();
    }

    public static PaginationSettings<T> WithoutPagination() => DefaultRequest();

    public static PaginationSettings<T> FromPaginationRequest(PaginationRequest request)
    {
        return new PaginationSettings<T>
        {
            Page = request.Page >= 0 ? request.Page : ConstValues.DefaultPaginationStartPage,
            PageSize = request.PageSize > 0 ? request.PageSize : ConstValues.DefaultPaginationPageSize,
        };
    }

    public PaginationSettings<T> SetupFindOptions(FindModelRequest<T> request)
    {
        Filter = request.BuildFilterDefinition();
        Sort = request.BuildSortDefinition();

        return this;
    }
}
