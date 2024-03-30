using Defender.Common.Entities;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Defender.Common.DB.Model;

public class FindModelRequest<T> where T : IBaseModel, new()
{
    private FilterDefinition<T> _filterDefinition;
    private SortDefinition<T> _sortDefinition;

    public FindModelRequest()
    {
        _filterDefinition = FilterDefinition<T>.Empty;
        _sortDefinition = Builders<T>.Sort.Ascending(x=> x.Id);
    }

    public static FindModelRequest<T> Init()
    {
        return new FindModelRequest<T>();
    }

    public static FindModelRequest<T> Init<FType>(
        Expression<Func<T, FType>> field,
        FType value,
        FilterType type = FilterType.Eq)
    {
        return new FindModelRequest<T>()
        {
            _filterDefinition = BuildFilterDefinition(field, value, type),
        };
    }

    public FindModelRequest<T> Clear()
    {
        _filterDefinition = FilterDefinition<T>.Empty;

        return this;
    }

    public FindModelRequest<T> And<FType>(
        Expression<Func<T, FType>> field,
        FType value,
        FilterType type = FilterType.Eq)
    {
        _filterDefinition = Builders<T>.Filter.And(
            _filterDefinition,
            BuildFilterDefinition(field, value, type));

        return this;
    }

    public FindModelRequest<T> And<FType>(FindModelRequest<T> request)
    {
        _filterDefinition = Builders<T>.Filter.And(
            _filterDefinition,
            request.BuildFilterDefinition());

        return this;
    }

    public FindModelRequest<T> Or<FType>(
        Expression<Func<T, FType>> field,
        FType value,
        FilterType type = FilterType.Eq)
    {
        _filterDefinition = Builders<T>.Filter.Or(
            _filterDefinition,
            BuildFilterDefinition(field, value, type));

        return this;
    }

    public FindModelRequest<T> Or<FType>(FindModelRequest<T> request)
    {
        _filterDefinition = Builders<T>.Filter.Or(
            _filterDefinition,
            request.BuildFilterDefinition());

        return this;
    }

    public FindModelRequest<T> Sort<FType>(
        Expression<Func<T, FType>> field,
        SortType type = SortType.Asc)
    {
        _sortDefinition = BuildSortDefinition(field, type);

        return this;
    }

    public FilterDefinition<T> BuildFilterDefinition()
    {
        return _filterDefinition;
    }

    public SortDefinition<T> BuildSortDefinition()
    {
        return _sortDefinition;
    }

    private static FilterDefinition<T> BuildFilterDefinition<FType>(
        Expression<Func<T, FType>> field,
        FType value,
        FilterType type)
    {
        return type switch
        {
            FilterType.Eq => Builders<T>.Filter.Eq(field, value),
            FilterType.Ne => Builders<T>.Filter.Ne(field, value),
            FilterType.Gt => Builders<T>.Filter.Gt(field, value),
            FilterType.Lt => Builders<T>.Filter.Lt(field, value),
            _ => Builders<T>.Filter.Eq(field, value),
        };
    }

    private static SortDefinition<T> BuildSortDefinition<FType>(
        Expression<Func<T, FType>> field,
        SortType type)
    {
        var fieldDefinition = new ExpressionFieldDefinition<T>(field);

        return type switch
        {
            SortType.Asc => Builders<T>.Sort.Ascending(fieldDefinition),
            SortType.Desc => Builders<T>.Sort.Descending(fieldDefinition),
            _ => Builders<T>.Sort.Ascending(fieldDefinition),
        };
    }
}
