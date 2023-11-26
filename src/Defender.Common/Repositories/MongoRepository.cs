using MongoDB.Driver;
using MongoDB.Bson;
using Defender.Common.Entities;
using Defender.Common.Configuration.Options;
using Defender.Common.Exceptions;
using Defender.Common.Errors;
using Defender.Common.Pagination;
using Defender.Common.Models;

namespace Defender.Common.Repositories;

public class MongoRepository<T> : BaseMongoRepository<T> where T : IBaseModel, new()
{
    public MongoRepository(MongoDbOptions mongoOption) : base(mongoOption)
    {
    }

    protected override async Task<T> GetItemAsync(Guid id)
    {
        var result = new T();

        try
        {
            var filter = CreateIdFilter(id);

            result = await _mongoCollection.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }

        return result;
    }

    protected override async Task<T> GetItemAsync(FindModelRequest<T> request)
    {
        var result = new T();

        try
        {
            result = await _mongoCollection.Find(request.BuildFilterDefinition()).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }

        return result;
    }

    protected override async Task<IList<T>> GetItemsAsync()
    {
        var result = new List<T>();

        try
        {
            result = await _mongoCollection.Find(new BsonDocument()).ToListAsync();
        }
        catch (Exception e)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }

        return result;
    }

    protected override async Task<PagedResult<T>> GetItemsAsync(PaginationSettings<T> settings)
    {
        var result = new PagedResult<T>()
        {
            CurrentPage = settings.Page,
            PageSize = settings.PageSize,
        };

        try
        {
            var query = _mongoCollection.Find(settings.Filter);
            var totalTask = query.CountAsync();
            var itemsTask = query.Skip(settings.Offset).Limit(settings.PageSize).ToListAsync();
            await Task.WhenAll(totalTask, itemsTask);

            result.TotalItemsCount = totalTask.Result;
            result.Items = itemsTask.Result;
        }
        catch (Exception e)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }

        return result;
    }


    protected override async Task<T> AddItemAsync(T newModel)
    {
        try
        {
            await _mongoCollection.InsertOneAsync(newModel);
        }
        catch (Exception e)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }

        return newModel;
    }

    protected override async Task<T> UpdateItemAsync(UpdateModelRequest<T> request)
    {
        try
        {
            var filter = CreateIdFilter(request.ModelId);

            return await _mongoCollection.FindOneAndUpdateAsync(filter, request.BuildUpdateDefinition());
        }
        catch (Exception e)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }
    }

    protected override async Task<T> ReplaceItemAsync(T updatedModel)
    {
        try
        {
            var filter = CreateIdFilter(updatedModel.Id);

            await _mongoCollection.ReplaceOneAsync(filter, updatedModel);
        }
        catch (Exception e)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }

        return updatedModel;
    }

    protected override async Task RemoveItemAsync(Guid id)
    {
        try
        {
            var filter = CreateIdFilter(id);

            await _mongoCollection.DeleteOneAsync(filter);
        }
        catch (Exception e)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue);
        }
    }

}
