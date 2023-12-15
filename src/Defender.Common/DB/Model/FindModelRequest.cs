using Defender.Common.Entities;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Defender.Common.DB.Model
{
    public class FindModelRequest<T> where T : IBaseModel, new()
    {
        private FilterDefinition<T> _filterDefinition;

        public FindModelRequest()
        {
            _filterDefinition = FilterDefinition<T>.Empty;
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

        public FilterDefinition<T> BuildFilterDefinition()
        {
            return _filterDefinition;
        }

        private static FilterDefinition<T> BuildFilterDefinition<FType>(
            Expression<Func<T, FType>> field,
            FType value,
            FilterType type)
        {
#pragma warning disable CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
            return type switch
            {
                FilterType.Eq => Builders<T>.Filter.Eq(field, value),
                FilterType.Ne => Builders<T>.Filter.Ne(field, value),
                FilterType.Gt => Builders<T>.Filter.Gt(field, value),
                FilterType.Lt => Builders<T>.Filter.Lt(field, value),
            };
#pragma warning restore CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
        }
    }
}
