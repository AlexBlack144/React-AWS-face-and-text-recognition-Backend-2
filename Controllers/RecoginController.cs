using Amazon.Rekognition.Model;
using Amazon.S3.Model;
using Amazon.S3;
using DataAccessEF.Data;
using DataAccessEF.Repositories;
using DataAccessEF.UnitOfWork;
using Domain.Interfaces;
using Domain.Models;
using Domain.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationClient.Controllers
{
    
    [Route("[controller]")]
    [ApiController]
    public class RecoginController : ControllerBase
    {
        static string accessKey = "AKIAXTVX5PM2WVUPY72A";
        static string secretKey = "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";
        static string bucket = "recig";
        string UserName = "";

        AmazonS3Client s3Client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.EUWest2);
        static async Task<bool> UploadFileAsync(IAmazonS3 client, string bucketName, string objectName, Stream source)
        {
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = objectName,
                InputStream = source,
            };
            var response = await client.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"Successfully uploaded {objectName} to {bucketName}.");
                return true;
            }
            else
            {
                Console.WriteLine($"Could not upload {objectName} to {bucketName}.");
                return false;
            }
        }

        [HttpPost]
        [Route("UploadImg")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<List<BoundingBox>> AddFile([FromForm]IFormFile file)
        {
            if (file != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    memoryStream.ToArray();
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await UploadFileAsync(s3Client, bucket, file.FileName, memoryStream);
                }

                return DetectFace.Example(file.FileName);
            }
            else { return null; }
        }

        [HttpPost]
        [Route("UploadImgText")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<string> AddFileText([FromForm] IFormFile file)
        {
            if (file != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    memoryStream.ToArray();
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await UploadFileAsync(s3Client, bucket, file.FileName, memoryStream);
                }

                return DetectText.Example(file.FileName);
            }
            else { return null; }
        }

        [HttpPost]
        [Route("UserName")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<string> HowThis([FromBody]string name)
        {
            UserName = name;
            return UserName;
        }
    }
}