using System.Linq.Expressions;
using Defender.Common.Configuration.Options;
using Defender.Common.Entities;
using Defender.Common.Enums;
using Defender.Common.Helpers;
using Defender.Common.Models;
using Defender.Common.Pagination;
using MongoDB.Driver;

namespace Defender.Common.Repositories;

public abstract class BaseMongoRepository<Model> where Model : IBaseModel, new()
{
    private static MongoClient client;
    private static IMongoDatabase database;
    protected IMongoCollection<Model> _mongoCollection;

    protected BaseMongoRepository(MongoDbOptions mongoOption)
    {
        mongoOption.ConnectionString =
            string.Format(
                mongoOption.ConnectionString,
                SecretsHelper.GetSecret(Secret.MongoDBPassword));

        client ??= new MongoClient(mongoOption.ConnectionString);

        database ??= client.GetDatabase($"{mongoOption.Environment}_{mongoOption.AppName}");

        _mongoCollection = database.GetCollection<Model>(typeof(Model).Name);
    }


    protected virtual Task<Model> GetItemAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    protected virtual Task<Model> GetItemAsync(FindModelRequest<Model> request)
    {
        throw new NotImplementedException();
    }

    protected virtual Task<IList<Model>> GetItemsAsync()
    {
        throw new NotImplementedException();
    }

    protected virtual Task<PagedResult<Model>> GetItemsAsync(PaginationSettings<Model> pagination)
    {
        throw new NotImplementedException();
    }

    protected virtual Task<Model> AddItemAsync(Model newModel)
    {
        throw new NotImplementedException();
    }

    protected virtual Task UpdateItemAsync(Guid id, UpdateDefinition<Model> updateDefinition)
    {
        throw new NotImplementedException();
    }

    protected virtual Task<Model> ReplaceItemAsync(Model model)
    {
        throw new NotImplementedException();
    }

    protected virtual Task RemoveItemAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    protected FilterDefinition<Model> CreateIdFilter(Guid id)
    {
        return Builders<Model>.Filter.Eq(s => s.Id, id);
    }
}
