using System;
using System.Collections.Generic;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
namespace WebApplicationClient
{
    public class DetectFace
    {
        public static List<BoundingBox> Example(string pic)
        {
            string accessKey = "AKIAXTVX5PM2WVUPY72A";
            string secretKey = "1R4RDv8j2vi7ZwWWtZY4zNm8A7f4qpyYjKqsj5Uu";


            String photo = pic;
            String bucket = "recig";

            AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient(
            accessKey,
            secretKey,
            Amazon.RegionEndpoint.EUWest2);
            DetectFacesRequest detectFacesRequest = new DetectFacesRequest()
            {
                Image = new Image()
                {
                    S3Object = new S3Object()
                    {
                        Name = photo,
                        Bucket = bucket
                    }
                }
            };
            List<BoundingBox> boundingBox = new List<BoundingBox>();
            try
            {
                DetectFacesResponse detectFacesResponse = rekognitionClient.DetectFacesAsync(detectFacesRequest).GetAwaiter().GetResult();
                bool hasAll = detectFacesRequest.Attributes.Contains("ALL");

                foreach (FaceDetail face in detectFacesResponse.FaceDetails)
                {
                    boundingBox.Add(face.BoundingBox);
                   
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return boundingBox;
        }
    }
}