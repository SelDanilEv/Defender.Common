using Defender.Common.Entities;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Defender.Common.Models
{
    public class FindModelRequest<T> where T : IBaseModel, new()
    {
        private FilterDefinition<T> _filterDefinition;
        private readonly FilterDefinitionBuilder<T> _filterDefinitionBuilder;

        public FindModelRequest()
        {
            _filterDefinition = FilterDefinition<T>.Empty;
            _filterDefinitionBuilder = Builders<T>.Filter;
        }

        public static FindModelRequest<T> Init()
        {
            return new FindModelRequest<T>();
        }

        public static FindModelRequest<T> Init<FType>(Expression<Func<T, FType>> field, FType value)
        {
            return new FindModelRequest<T>()
            {
                _filterDefinition = Builders<T>.Filter.Eq(field, value),
            };
        }

        public FindModelRequest<T> Clear()
        {
            _filterDefinition = FilterDefinition<T>.Empty;

            return this;
        }

        public FindModelRequest<T> And<FType>(Expression<Func<T, FType>> field, FType value)
        {
            _filterDefinition = Builders<T>.Filter.And(_filterDefinition, _filterDefinitionBuilder.Eq(field, value));

            return this;
        }

        public FindModelRequest<T> And<FType>(FindModelRequest<T> request)
        {
            _filterDefinition = Builders<T>.Filter.And(_filterDefinition, request.BuildFilterDefinition());

            return this;
        }

        public FindModelRequest<T> Or<FType>(Expression<Func<T, FType>> field, FType value)
        {
            _filterDefinition = Builders<T>.Filter.Or(_filterDefinition, _filterDefinitionBuilder.Eq(field, value));

            return this;
        }

        public FindModelRequest<T> Or<FType>(FindModelRequest<T> request)
        {
            _filterDefinition = Builders<T>.Filter.Or(_filterDefinition, request.BuildFilterDefinition());

            return this;
        }

        public FilterDefinition<T> BuildFilterDefinition()
        {
            return _filterDefinition;
        }
    }
}
