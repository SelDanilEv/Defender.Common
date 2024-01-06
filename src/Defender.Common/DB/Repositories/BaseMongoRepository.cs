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
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static MongoClient client;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private static IMongoDatabase database;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected IMongoCollection<Model> _mongoCollection;

    protected BaseMongoRepository(MongoDbOptions mongoOption)
    {
        client ??= new MongoClient(mongoOption.ConnectionString);

        database ??= client.GetDatabase($"{mongoOption.Environment}_{mongoOption.AppName}");

        _mongoCollection = database.GetCollection<Model>(typeof(Model).Name);
    }


    protected virtual async Task<Model> GetItemAsync(Guid id)
    {
#pragma warning disable CS0168 // Variable is declared but never used
        try
        {
            var filter = CreateIdFilter(id);

            return await _mongoCollection.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }
#pragma warning restore CS0168 // Variable is declared but never used
    }

    protected virtual async Task<Model> GetItemAsync(FindModelRequest<Model> request)
    {
#pragma warning disable CS0168 // Variable is declared but never used
        try
        {
            return await _mongoCollection.Find(request.BuildFilterDefinition()).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }
#pragma warning restore CS0168 // Variable is declared but never used
    }

    protected virtual async Task<IList<Model>> GetItemsAsync()
    {
#pragma warning disable CS0168 // Variable is declared but never used
        try
        {
            return await _mongoCollection.Find(new BsonDocument()).ToListAsync();
        }
        catch (Exception e)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }
#pragma warning restore CS0168 // Variable is declared but never used
    }

    protected virtual async Task<PagedResult<Model>> GetItemsAsync(
        PaginationSettings<Model> settings)
    {
        var result = new PagedResult<Model>()
        {
            CurrentPage = settings.Page,
            PageSize = settings.PageSize,
        };

#pragma warning disable CS0168 // Variable is declared but never used
        try
        {
            var query = _mongoCollection.Find(settings.Filter);
            var totalTask = query.CountDocumentsAsync();
            var itemsTask = query.Skip(settings.Offset).Limit(settings.PageSize).ToListAsync();
            await Task.WhenAll(totalTask, itemsTask);

            result.TotalItemsCount = totalTask.Result;
            result.Items = itemsTask.Result;
        }
        catch (Exception e)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }
#pragma warning restore CS0168 // Variable is declared but never used

        return result;
    }

    protected virtual async Task<Model> AddItemAsync(Model newModel)
    {
#pragma warning disable CS0168 // Variable is declared but never used
        try
        {
            await _mongoCollection.InsertOneAsync(newModel);
        }
        catch (Exception e)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }
#pragma warning restore CS0168 // Variable is declared but never used

        return newModel;
    }

    protected virtual async Task<Model> UpdateItemAsync(UpdateModelRequest<Model> request)
    {
#pragma warning disable CS0168 // Variable is declared but never used
        try
        {
            var filter = CreateIdFilter(request.ModelId);

            var options = new FindOneAndUpdateOptions<Model>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = false
            };

            return await _mongoCollection.FindOneAndUpdateAsync(
                filter,
                request.BuildUpdateDefinition(),
                options);
        }
        catch (Exception e)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }
#pragma warning restore CS0168 // Variable is declared but never used
    }

    protected virtual async Task<Model> ReplaceItemAsync(Model updatedModel)
    {
#pragma warning disable CS0168 // Variable is declared but never used
        try
        {
            var filter = CreateIdFilter(updatedModel.Id);

            await _mongoCollection.ReplaceOneAsync(filter, updatedModel);
        }
        catch (Exception e)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }
#pragma warning restore CS0168 // Variable is declared but never used

        return updatedModel;
    }

    protected virtual async Task RemoveItemAsync(Guid id)
    {
#pragma warning disable CS0168 // Variable is declared but never used
        try
        {
            var filter = CreateIdFilter(id);

            await _mongoCollection.DeleteOneAsync(filter);
        }
        catch (Exception e)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }
#pragma warning restore CS0168 // Variable is declared but never used
    }

    protected FilterDefinition<Model> CreateIdFilter(Guid id)
    {
        return Builders<Model>.Filter.Eq(s => s.Id, id);
    }
}
