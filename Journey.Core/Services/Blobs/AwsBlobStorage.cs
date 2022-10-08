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
        //todo: move to secrets manager.s
        private BasicAWSCredentials credentials = new BasicAWSCredentials("AKIAT75RS3OC2YEMZB7A", "fds5dTXcIMcDF7zupbcvZL6pQbSG1+w81+eZnK8A");
        private const string BUCKET_NAME = "journey-logos";

        public AwsBlobStorage()
        {

        }
        public async Task<bool> Add(string key, IFormFile file)
        {
            var client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USEast2);
            var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(client, BUCKET_NAME);
            if (!bucketExists)
            {
                var putBucketRequest = new PutBucketRequest()
                {
                    BucketName = BUCKET_NAME,
                    UseClientRegion = true
                };
                await client.PutBucketAsync(putBucketRequest);
            }
            var putObjectRequest = new PutObjectRequest()
            {
                BucketName = BUCKET_NAME,
                Key = key,
                InputStream = file.OpenReadStream(),
                CannedACL = S3CannedACL.PublicRead
            };
            var response = await client.PutObjectAsync(putObjectRequest);

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<(Stream, string)> Get(string key)
        {
            var client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USEast2);
            var response = await client.GetObjectAsync(BUCKET_NAME, key);
            return (response.ResponseStream, response.Headers.ContentType);

        }
    }
}
