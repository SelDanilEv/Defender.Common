using Defender.Common.Entities;
using Force.DeepCloner;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Defender.Common.Models
{
    public class UpdateModelRequest<T> where T : IBaseModel, new()
    {
        private readonly List<UpdateDefinition<T>> _updateDefinitions;
        private readonly UpdateDefinitionBuilder<T> _updateDefinitionBuilder;

        private T _modelBackup;
        private T _model;

        public UpdateModelRequest()
        {
            _updateDefinitions = new List<UpdateDefinition<T>>();
            _updateDefinitionBuilder = Builders<T>.Update;
        }

        public static UpdateModelRequest<T> Init()
        {
            return new UpdateModelRequest<T>()
            {
                _model = new T(),
                _modelBackup = new T(),
            };
        }

        public static UpdateModelRequest<T> Init(T model)
        {
            return new UpdateModelRequest<T>()
            {
                _model = model,
                _modelBackup = model.DeepClone()
            };
        }

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

        public UpdateModelRequest<T> UpdateField<FType>(Expression<Func<T, FType>> field, FType value)
        {
            _updateDefinitions.Add(_updateDefinitionBuilder.Set(field, value));

            try
            {
                if (_model != null)
                {
                    var property = _model.GetType().GetProperty(((MemberExpression)field.Body).Member.Name);
                    var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    var val = Convert.ChangeType(value, type);
                    property.SetValue(_model, val, null);
                }
            }
            catch (Exception e) { }

            return this;
        }

        public UpdateDefinition<T> BuildUpdateDefinition()
        {
            return _updateDefinitionBuilder.Combine(_updateDefinitions);
        }

    }
}
