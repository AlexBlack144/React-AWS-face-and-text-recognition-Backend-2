using System;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
namespace WebApplicationClient
{
    public class DetectText
    {
        public static string Example(string pic)
        {
            string accessKey = "AKIAXTVX5PM2WVUPY72A";
            string secretKey = "1R4RDv8j2vi7ZwWWtZY4zNm8A7f4qpyYjKqsj5Uu";
            string fileInfo = "";
            string text = "";
            //AmazonS3Config config = new AmazonS3Config();
            //config.ServiceURL = "";

            String photo = pic;
            String bucket = "recig";

            AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient(
            accessKey,
                    secretKey,
                    Amazon.RegionEndpoint.EUWest2
                    );
            DetectTextRequest detectTextRequest = new DetectTextRequest()
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

            try
            {
                DetectTextResponse detectTextResponse = rekognitionClient.DetectTextAsync(detectTextRequest).GetAwaiter().GetResult();
                Console.WriteLine("Detected lines and words for " + photo);
                foreach (TextDetection item in detectTextResponse.TextDetections)
                {
                    text += item.DetectedText;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return text;
        }
    }

}
