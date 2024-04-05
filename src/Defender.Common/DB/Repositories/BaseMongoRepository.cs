using Defender.Common.Configuration.Options;
using Defender.Common.DB.Model;
using Defender.Common.DB.Pagination;
using Defender.Common.Entities;
using Defender.Common.Errors;
using Defender.Common.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Defender.Common.DB.Repositories;

public abstract class BaseMongoRepository<Model> where Model : IBaseModel, new()
{
    protected readonly IMongoClient? _client;
    protected readonly IMongoDatabase? _database;
    protected readonly IMongoCollection<Model> _mongoCollection;

    protected BaseMongoRepository(
        MongoDbOptions mongoOption,
        string? collectionName = null)
    {
        _client ??= new MongoClient(mongoOption.ConnectionString);

        _database ??= _client.GetDatabase(mongoOption.GetDatabaseName());

        collectionName ??= typeof(Model).Name;

        _mongoCollection = _database.GetCollection<Model>(collectionName);
    }


    protected virtual async Task<Model> GetItemAsync(Guid id)
    {
        try
        {
            var filter = CreateIdFilter(id);

            return await _mongoCollection
                .Find(filter)
                .FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }
    }

    protected virtual async Task<Model> GetItemAsync(FindModelRequest<Model> request)
    {
        try
        {
            return await _mongoCollection
                .Find(request.BuildFilterDefinition())
                .Sort(request.BuildSortDefinition())
                .FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }
    }

    protected virtual async Task<long> CountItemsAsync(FindModelRequest<Model> request)
    {
        try
        {
            return await _mongoCollection
                .Find(request.BuildFilterDefinition())
                .CountDocumentsAsync();
        }
        catch (Exception)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }
    }

    protected virtual async Task<IList<Model>> GetItemsAsync()
    {
        try
        {
            return await _mongoCollection
                .Find(new BsonDocument())
                .ToListAsync();
        }
        catch (Exception)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }
    }

    protected virtual async Task<PagedResult<Model>> GetItemsAsync(
        PaginationSettings<Model> settings)
    {
        var result = new PagedResult<Model>()
        {
            CurrentPage = settings.Page,
            PageSize = settings.PageSize,
        };

        try
        {
            var query = _mongoCollection
                .Find(settings.Filter);
            var totalTask = query.CountDocumentsAsync();
            var itemsTask = query
                .Sort(settings.Sort)
                .Skip(settings.Offset)
                .Limit(settings.PageSize)
                .ToListAsync();
            await Task.WhenAll(totalTask, itemsTask);

            result.TotalItemsCount = totalTask.Result;
            result.Items = itemsTask.Result;
        }
        catch (Exception)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }

        return result;
    }

    protected virtual async Task<Model> AddItemAsync(
        Model newModel,
        IClientSessionHandle? session = null)
    {
        try
        {
            if (session != null)
                await _mongoCollection.InsertOneAsync(session, newModel);
            else
                await _mongoCollection.InsertOneAsync(newModel);
        }
        catch (Exception)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }

        return newModel;
    }

    protected virtual async Task<Model> UpdateItemAsync(
        UpdateModelRequest<Model> request,
        IClientSessionHandle? session = null)
    {
        try
        {
            var filter = CreateIdFilter(request.ModelId);

            var options = new FindOneAndUpdateOptions<Model>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = false
            };

            if (session != null)
                return await _mongoCollection.FindOneAndUpdateAsync(
                    session,
                    filter,
                    request.BuildUpdateDefinition(),
                    options);
            else
                return await _mongoCollection.FindOneAndUpdateAsync(
                    filter,
                    request.BuildUpdateDefinition(),
                    options);

        }
        catch (Exception)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }
    }

    protected virtual async Task<Model> ReplaceItemAsync(
        Model updatedModel,
        IClientSessionHandle? session = null)
    {
        try
        {
            var filter = CreateIdFilter(updatedModel.Id);

            if (session != null)
                await _mongoCollection.ReplaceOneAsync(session, filter, updatedModel);
            else
                await _mongoCollection.ReplaceOneAsync(filter, updatedModel);
        }
        catch (Exception)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }

        return updatedModel;
    }

    protected virtual async Task RemoveItemAsync(
        Guid id,
        IClientSessionHandle? session = null)
    {
        try
        {
            var filter = CreateIdFilter(id);

            if (session != null)
                await _mongoCollection.DeleteOneAsync(session, filter);
            else
                await _mongoCollection.DeleteOneAsync(filter);
        }
        catch (Exception)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }
    }

    protected FilterDefinition<Model> CreateIdFilter(Guid id)
    {
        return Builders<Model>.Filter.Eq(s => s.Id, id);
    }
}
