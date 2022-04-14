using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Textract;
using Amazon.Textract.Model;

namespace AWS_S3_TEXTRACT
{
    public class AWSTextract
    {
        public static async Task<List<string>> DetectSampleAsync(string nomeArquivo)
        {
            MemoryStream imagem;

            string bucketName = ConfigurationManager.AppSettings["BucketName"];
            string s3ServiceUrl = ConfigurationManager.AppSettings["AWSServiceUrl"];
            string accessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
            string secretAccessKey = ConfigurationManager.AppSettings["AWSSecretKey"];

            var s3Config = new AmazonS3Config() { ServiceURL = s3ServiceUrl };

            using (var s3Client = new AmazonS3Client(accessKey, secretAccessKey, s3Config))
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest();
                getObjectRequest.BucketName = bucketName;
                getObjectRequest.Key = nomeArquivo;

                using (var getObjectResponse = s3Client.GetObject(getObjectRequest))
                {
                    using (var ms = new MemoryStream())
                    {
                        await getObjectResponse.ResponseStream.CopyToAsync(ms);

                        imagem = ms;
                    }
                }
            }

            List<string> retorno = new List<string>();

            using (var textractClient = new AmazonTextractClient(RegionEndpoint.USEast1))
            {
                var bytes = imagem.ToArray();

                var detectResponse = await textractClient.DetectDocumentTextAsync(new DetectDocumentTextRequest
                {
                    Document = new Document
                    {
                        Bytes = new MemoryStream(bytes)
                    }
                });

                foreach (var block in detectResponse.Blocks)
                {
                    if (block.Text != null) if (block.Text.Trim().Length > 0) retorno.Add(block.Text.Trim().ToUpper());
                }
            }

            return retorno;
        }
    }
}
