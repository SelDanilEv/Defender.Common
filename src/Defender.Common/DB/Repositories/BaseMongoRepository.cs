﻿using Defender.Common.Configuration.Options;
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


    protected virtual async Task<long> CountItemsAsync(
        FindModelRequest<Model>? request = null)
    {
        try
        {
            return await _mongoCollection
                .Find(request?.BuildFilterDefinition() ?? new BsonDocument())
                .CountDocumentsAsync();
        }
        catch (Exception ex)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue, ex);
        }
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
        catch (Exception ex)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue, ex);
        }
    }

    protected virtual async Task<Model> GetItemAsync(FindModelRequest<Model> request)
    {
        try
        {
            return await _mongoCollection
                .Find(request.BuildFilterDefinition())
                .Sort(request.BuildSortDefinition())
                .Limit(1)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue, ex);
        }
    }

    protected virtual async Task<IList<Model>> GetItemsAsync(
        FindModelRequest<Model>? request = null)
    {
        try
        {
            return await _mongoCollection
                .Find(request?.BuildFilterDefinition() ?? new BsonDocument())
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue, ex);
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
        catch (Exception ex)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue, ex);
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
        catch (Exception ex)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue, ex);
        }

        return newModel;
    }

    #region Update

    protected virtual async Task<Model> UpdateItemAsync(
        UpdateModelRequest<Model> request,
        IClientSessionHandle? session = null)
    {
        return await UpdateItemAsync(request, CreateIdFilter(request.ModelId), session);
    }

    protected virtual async Task<Model> UpdateItemAsync(
        UpdateModelRequest<Model> request,
        FindModelRequest<Model> filter,
        IClientSessionHandle? session = null)
    {
        return await UpdateItemAsync(request, filter.BuildFilterDefinition(), session);
    }

    protected virtual async Task<Model> UpdateItemAsync(
        UpdateModelRequest<Model> request,
        FilterDefinition<Model> filter,
        IClientSessionHandle? session = null)
    {
        try
        {
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
        catch (Exception ex)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue, ex);
        }
    }

    #endregion Update

    #region Replace

    protected virtual async Task<Model> ReplaceItemAsync(
        Model updatedModel,
        IClientSessionHandle? session = null)
    {
        return await ReplaceItemAsync(updatedModel, CreateIdFilter(updatedModel.Id), session);
    }

    protected virtual async Task<Model> ReplaceItemAsync(
        Model updatedModel,
        FindModelRequest<Model> replaceFilter,
        IClientSessionHandle? session = null)
    {
        if (updatedModel.Id == Guid.Empty)
        {
            var existingModel = await GetItemAsync(replaceFilter);
            updatedModel.Id = existingModel?.Id ?? Guid.NewGuid();
        }

        return await ReplaceItemAsync(updatedModel, replaceFilter.BuildFilterDefinition(), session);
    }

    protected virtual async Task<Model> ReplaceItemAsync(
        Model updatedModel,
        FilterDefinition<Model> filter,
        IClientSessionHandle? session = null)
    {
        var options = new ReplaceOptions { IsUpsert = true, };

        try
        {
            if (session != null)
                await _mongoCollection.ReplaceOneAsync(session, filter, updatedModel, options);
            else
                await _mongoCollection.ReplaceOneAsync(filter, updatedModel, options);
        }
        catch (Exception ex)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue, ex);
        }

        return updatedModel;
    }

    #endregion

    #region Remove


    protected virtual async Task RemoveItemAsync(
        Guid id,
        IClientSessionHandle? session = null)
    {
        await RemoveItemAsync(id, CreateIdFilter(id), session);
    }

    protected virtual async Task RemoveItemAsync(
        Guid id,
        FindModelRequest<Model> removeFilter,
        IClientSessionHandle? session = null)
    {
        await RemoveItemAsync(id, removeFilter.BuildFilterDefinition(), session);
    }

    protected virtual async Task RemoveItemAsync(
        Guid id,
        FilterDefinition<Model> filter,
        IClientSessionHandle? session = null)
    {
        try
        {
            if (session != null)
                await _mongoCollection.DeleteOneAsync(session, filter);
            else
                await _mongoCollection.DeleteOneAsync(filter);
        }
        catch (Exception ex)
        {
            throw new ServiceException(ErrorCode.CM_DatabaseIssue, ex);
        }
    }

    #endregion

    protected static FilterDefinition<Model> CreateIdFilter(Guid id)
    {
        return Builders<Model>.Filter.Eq(s => s.Id, id);
    }
}
