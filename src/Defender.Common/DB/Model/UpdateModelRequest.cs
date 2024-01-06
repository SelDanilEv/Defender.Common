using Defender.Common.Entities;
using Force.DeepCloner;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Defender.Common.DB.Model
{
    public class UpdateModelRequest<T> where T : IBaseModel, new()
    {
        private readonly List<UpdateDefinition<T>> _updateDefinitions;
        private readonly UpdateDefinitionBuilder<T> _updateDefinitionBuilder;

        private T _modelBackup;
        private T _model;

        private Guid _modelId;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private UpdateModelRequest()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _updateDefinitions = new List<UpdateDefinition<T>>();
            _updateDefinitionBuilder = Builders<T>.Update;
        }

        public static UpdateModelRequest<T> Init(Guid modelId)
        {
            return Init(new T() { Id = modelId });
        }

        public static UpdateModelRequest<T> Init(T model)
        {
            return new UpdateModelRequest<T>()
            {
                _modelId = model.Id,
                _model = model,
                _modelBackup = model.DeepClone()
            };
        }

        public Guid ModelId => _modelId;

        public UpdateModelRequest<T> CommitChanges()
        {
            _modelBackup = _model.DeepClone();

            return this;
        }

        public UpdateModelRequest<T> Clear()
        {
            _model = _modelBackup.DeepClone();
            _updateDefinitions.Clear();

            return this;
        }

        public UpdateModelRequest<T> UpdateFieldIfNotNull<FType>(Expression<Func<T, FType>> field, FType value)
        {
            var condition = () => value != null;
            if (value is string)
            {
                condition = () => value != null && (!string.IsNullOrWhiteSpace(value as string));
            }

            return UpdateField(field, value, condition);
        }

        public UpdateModelRequest<T> UpdateField<FType>(Expression<Func<T, FType>> field, FType value, Func<bool> condition)
        {
            if (condition())
                return UpdateField(field, value);

            return this;
        }

        public UpdateModelRequest<T> UpdateField<FType>(Expression<Func<T, FType>> field, FType value)
        {
            _updateDefinitions.Add(_updateDefinitionBuilder.Set(field, value));

#pragma warning disable CS0168 // Variable is declared but never used
            try
            {
                if (_model != null)
                {
                    var property = _model.GetType().GetProperty(((MemberExpression)field.Body).Member.Name);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    var val = Convert.ChangeType(value, type);
                    property.SetValue(_model, val, null);
                }
            }
            catch (Exception e) { }
#pragma warning restore CS0168 // Variable is declared but never used

            return this;
        }

        public UpdateDefinition<T> BuildUpdateDefinition()
        {
            return _updateDefinitionBuilder.Combine(_updateDefinitions);
        }

    }
}
