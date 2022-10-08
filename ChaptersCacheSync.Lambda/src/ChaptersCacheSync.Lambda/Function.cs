using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using StackExchange.Redis;

namespace ChaptersCacheSync.Lambda
{
    public class Function
    {
        private static IConnectionMultiplexer _connectionMultiplexer;
        private static string _redisConnectionString = Environment.GetEnvironmentVariable("RedisConnectionString");

        private static async Task Main(string[] args)
        {
            _connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(_redisConnectionString);
            Func<DynamoDBEvent, ILambdaContext, Task> handler = FunctionHandler;
            await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
                .Build()
                .RunAsync();
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        ///
        /// To use this handler to respond to an AWS event, reference the appropriate package from 
        /// https://github.com/aws/aws-lambda-dotnet#events
        /// and change the string input parameter to the desired event type.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task FunctionHandler(DynamoDBEvent input, ILambdaContext context)
        {
            //see https://www.youtube.com/watch?v=uass0C6NEpA
            context.Logger.LogLine($"Beginning handling DynamoDb updates, Doc count: {input.Records.Count}");

            //todo redis cache
            var database = _connectionMultiplexer.GetDatabase();
            foreach(var record in input.Records)
            {
                if(record.EventName == Amazon.DynamoDBv2.OperationType.REMOVE)
                {
                    var deletedId = record.Dynamodb.OldImage["pk"].S; //before it was modified/deleted
                    await database.KeyDeleteAsync(deletedId);
                }

                var recordAsDocument = Document.FromAttributeMap(record.Dynamodb.NewImage);
                var json = recordAsDocument.ToJson();
                var id = record.Dynamodb.NewImage["pk"].S;
                await database.StringSetAsync(id, json);

            }
            
            context.Logger.LogLine($"Finished handling DynamoDb updates, Doc count: {input.Records.Count}");
        }
    }
}