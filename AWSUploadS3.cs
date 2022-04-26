using System;
using System.Configuration;
using System.IO;
using System.Net;
using Amazon.S3;
using Amazon.S3.Model;

namespace AWS_S3_TEXTRACT
{
    public class AWSUploadS3
    {
        public static string UploadFileAnexoS3(Stream stream, string url)
        {
            string retorno = "";

            try
            {
                string bucketName = ConfigurationManager.AppSettings["BucketName"];
                string destino = "comprovantes_prestacao_homologa/" + DateTime.Now.Year.ToString("D4") + "-" + DateTime.Now.Month.ToString("D2");
                string s3ServiceUrl = ConfigurationManager.AppSettings["AWSServiceUrl"];
                string accessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
                string secretAccessKey = ConfigurationManager.AppSettings["AWSSecretKey"];

                PutObjectResponse response = new PutObjectResponse();

                var s3Config = new AmazonS3Config() { ServiceURL = s3ServiceUrl };

                using (var s3Client = new AmazonS3Client(accessKey, secretAccessKey, s3Config))
                {
                    PutObjectRequest request = new PutObjectRequest();
                    request.BucketName = bucketName;
                    request.CannedACL = S3CannedACL.PublicRead;
                    request.Key = destino + "/" + url;
                    request.InputStream = stream;
                    response = s3Client.PutObject(request);
                }

                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    retorno = (s3ServiceUrl + "/" + bucketName + "/" + destino + "/" + url);
                }
                else
                {
                    retorno = "ERRO - " + response.HttpStatusCode.ToString();
                }
            }
            catch (AmazonS3Exception s3Exception)
            {
                retorno = "ERRO - " + s3Exception.Message;
            }

            return retorno;
        }
    }
}
