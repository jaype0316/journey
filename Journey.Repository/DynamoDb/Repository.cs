using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Journey.Core;
using Journey.Core.Query;
using Journey.Core.Repository;
using Journey.Models.DTO;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Journey.Repository.DynamoDb
{
    public class Repository : IRepository
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly IDynamoDBContext _dbContext;
        private readonly IOptions<DatabaseSettings> _databaseSettings;

        public Repository(IAmazonDynamoDB dynamoDb, IDynamoDBContext dbContext)
        {
            _dynamoDb = dynamoDb;
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync<T>(T entity) where T : IDTO, IEntity
        {
            var entityAsJson = JsonSerializer.Serialize(entity);
            var itemAsDocument = Document.FromJson(entityAsJson);
            var itemAsAttributes = itemAsDocument.ToAttributeMap();
            var createItemRequest = new PutItemRequest
            {
                TableName = entity.ItemName,
                Item = itemAsAttributes
            };
            try
            {
                var response = await _dynamoDb.PutItemAsync(createItemRequest);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                var i = ex;
                throw;
            }

        }

        public async Task<bool> DeleteAsync<T>(string id, string userId) where T : IDTO, IEntity
        {
            var deleteRequest = new DeleteItemRequest()
            {
                TableName = Activator.CreateInstance<T>().ItemName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "pk", new AttributeValue { S = id.ToString() } },
                    { "sk", new AttributeValue { S = userId.ToString() } }
                }
            };

            var response = await _dynamoDb.DeleteItemAsync(deleteRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<T> GetAsync<T>(string id, string userId) where T : IDTO, IEntity
        {
            var request = new GetItemRequest()
            {
                TableName = Activator.CreateInstance<T>().ItemName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "pk", new AttributeValue { S = id.ToString() } },
                    { "sk", new AttributeValue { S = userId.ToString() } }
                }
            };
            try
            {
                var response = await _dynamoDb.GetItemAsync(request);
                if (response.Item.Count == 0)
                {
                    return default(T);
                }

                var itemAsDocument = Document.FromAttributeMap(response.Item);
                return JsonSerializer.Deserialize<T>(itemAsDocument.ToJson());
            }
            catch (Exception ex)
            {
                var i = 0;
                throw ex;
            }


        }

        public async Task<bool> UpdateAsync<T>(T entity) where T : IDTO, IEntity
        {
            var entityAsJson = JsonSerializer.Serialize(entity);
            var itemAsDocument = Document.FromJson(entityAsJson);
            var itemAsAttributes = itemAsDocument.ToAttributeMap();
            var updateItemRequest = new PutItemRequest
            {
                TableName = entity.ItemName,
                Item = itemAsAttributes
            };

            var response = await _dynamoDb.PutItemAsync(updateItemRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }

    public class IndexedRepository : IIndexedRepository
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly IOptions<DatabaseSettings> _databaseSettings;

        public IndexedRepository(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(QueryOption query) where T : IIndexedEntity
        {
            try
            {
                var request = new QueryRequest
                {
                    TableName = Activator.CreateInstance<T>().ItemName,
                    IndexName = Activator.CreateInstance<T>().IndexName,
                    KeyConditionExpression = $"{query.Field} = :{query.Field}",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                        {$":{query.Field}", new AttributeValue { S =  query.Value }}
                    },
                    ScanIndexForward = true
                };

                var response = await _dynamoDb.QueryAsync(request);
                if (response.Items.Count == 0)
                {
                    return Enumerable.Empty<T>();
                }

                //todo: figure out if there's a simpler way
                var results = new List<T>();
                foreach (var item in response.Items)
                {
                    var itemAsDocument = Document.FromAttributeMap(item);
                    var result = JsonSerializer.Deserialize<T>(itemAsDocument.ToJson());
                    results.Add(result);
                }

                return results;
            }
            catch (Exception ex)
            {
                var t = ex;
                throw;
            }
           
        }
    }
}
