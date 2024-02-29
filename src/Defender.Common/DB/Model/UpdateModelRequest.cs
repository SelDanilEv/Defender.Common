using Defender.Common.Entities;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Defender.Common.DB.Model
{
    public class UpdateModelRequest<T> where T : IBaseModel, new()
    {
        private readonly List<UpdateDefinition<T>> _updateDefinitions;
        private readonly UpdateDefinitionBuilder<T> _updateDefinitionBuilder;

        private Guid _modelId;

        private UpdateModelRequest()
        {
            _updateDefinitions = new List<UpdateDefinition<T>>();
            _updateDefinitionBuilder = Builders<T>.Update;
        }

        public static UpdateModelRequest<T> Init(Guid modelId)
        {
            return new UpdateModelRequest<T>()
            {
                _modelId = modelId
            };
        }

        public static UpdateModelRequest<T> Init(T model)
        {
            return Init(model.Id);
        }

        public Guid ModelId => _modelId;

        public UpdateModelRequest<T> Clear()
        {
            _updateDefinitions.Clear();

            return this;
        }

        public UpdateModelRequest<T> SetIfNotNull<FType>(
            Expression<Func<T, FType>> field,
            FType value)
        {
            var condition = () => value != null;
            if (value is string)
            {
                condition = () =>
                value != null
                && (!string.IsNullOrWhiteSpace(value as string));
            }

            return Set(field, value, condition);
        }

        public UpdateModelRequest<T> Set<FType>(
            Expression<Func<T, FType>> field,
            FType value,
            Func<bool>? condition = null)
        {
            return AddUpdateDefinition(field, value, condition, UpdateFieldType.Set);
        }

        public UpdateModelRequest<T> AddToSet<FType, EType>(
            Expression<Func<T, FType>> field,
            EType value)
        {
            var condition = () => value != null;
            if (value is string)
            {
                condition = () =>
                value != null
                && (!string.IsNullOrWhiteSpace(value as string));
            }

            return AddUpdateDefinition(field, value, condition, UpdateFieldType.AddToSet);
        }


        public UpdateDefinition<T> BuildUpdateDefinition()
        {
            return _updateDefinitionBuilder.Combine(_updateDefinitions);
        }

        private UpdateModelRequest<T> AddUpdateDefinition<FType, VType>(
            Expression<Func<T, FType>> field,
            VType value,
            Func<bool>? condition,
            UpdateFieldType updateFieldType
            )
        {
            if (condition != null && !condition())
                return this;

            UpdateDefinition<T>? updateDefinition = default;

            switch (updateFieldType)
            {
                case UpdateFieldType.Set:

                    if (value is FType fValue)
                    {
                        updateDefinition = _updateDefinitionBuilder
                            .Set(field, fValue);
                    }
                    break;
                case UpdateFieldType.AddToSet:
                    updateDefinition = _updateDefinitionBuilder
                        .AddToSet(new ExpressionFieldDefinition<T>(field), value);
                    break;
            }

            if (updateDefinition != null)
                _updateDefinitions.Add(updateDefinition);

            return this;
        }
    }
}
