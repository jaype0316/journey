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
        private readonly IBlobUriResolver _blobUriResolver;

        public AwsBlobStorage(IAmazonS3 client, IBlobUriResolver blobUriResolver)
        {
            _client = client;
            _blobUriResolver = blobUriResolver;
        }

        public string BlobBaseUri => "https://journey-logos.s3.us-east-2.amazonaws.com/";

        public async Task<bool> Add(string key, IFormFile file)
        {
            var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(_client, BUCKET_NAME);
            if (!bucketExists)
            {
                var putBucketRequest = new PutBucketRequest()
                {
                    BucketName = BUCKET_NAME,
                    UseClientRegion = true,
                    CannedACL = S3CannedACL.PublicRead
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

        public async Task<IEnumerable<string>> GetList(string bucketName, string startAfter, int take)
        {
            var request = new ListObjectsV2Request()
            {
                BucketName = bucketName,
                MaxKeys = take,
                StartAfter = startAfter
            };
            var blobs = await _client.ListObjectsV2Async(request);
            
            var blobUris = new List<string>(blobs.KeyCount);
            foreach(var blob in blobs.S3Objects)
            {
                var blobUri = _blobUriResolver.Resolve(bucketName, _client.Config.RegionEndpoint.OriginalSystemName, blob.Key);
                blobUris.Add(blobUri);
            }

            return blobUris;
        }
    }
}
