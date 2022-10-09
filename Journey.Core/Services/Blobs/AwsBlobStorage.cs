using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Blobs
{
    public class AwsBlobStorage : IBlobStorageService
    {
        private const string BUCKET_NAME = "journey-logos"; //todo: move this to config
        readonly IAmazonS3 _client;
        public AwsBlobStorage(IAmazonS3 client)
        {
            _client = client;
        }
        public async Task<bool> Add(string key, IFormFile file)
        {
            var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(_client, BUCKET_NAME);
            if (!bucketExists)
            {
                var putBucketRequest = new PutBucketRequest()
                {
                    BucketName = BUCKET_NAME,
                    UseClientRegion = true
                };
                await _client.PutBucketAsync(putBucketRequest);
            }
            var putObjectRequest = new PutObjectRequest()
            {
                BucketName = BUCKET_NAME,
                Key = key,
                InputStream = file.OpenReadStream(),
                CannedACL = S3CannedACL.PublicRead
            };
            var response = await _client.PutObjectAsync(putObjectRequest);

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<(Stream, string)> Get(string key)
        {
            var response = await _client.GetObjectAsync(BUCKET_NAME, key);
            return (response.ResponseStream, response.Headers.ContentType);

        }
    }
}
