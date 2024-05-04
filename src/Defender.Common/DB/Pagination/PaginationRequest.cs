﻿using Defender.Common.Consts;

namespace Defender.Common.DB.Pagination;

public record PaginationRequest
{
    public int Page { get; set; } = ConstValues.DefaultPaginationStartPage;
    public int PageSize { get; set; } = ConstValues.DefaultPaginationPageSize;
}
