using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
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
        public static async Task<List<string>> DetectSampleAsync(string link)
        {
            List<string> retorno = new List<string>();

            try
            {
                MemoryStream imagem;

                string bucketName = ConfigurationManager.AppSettings["BucketName"];
                string s3ServiceUrl = ConfigurationManager.AppSettings["AWSServiceUrl"];
                string accessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
                string secretAccessKey = ConfigurationManager.AppSettings["AWSSecretKey"];
                string nomeArquivo = link.Replace((bucketName + "/"), "").Replace((s3ServiceUrl + "/"), "");

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
                        //if (block.BlockType.Value == "LINE")
                        //{
                        //    if (block.Text != null) if (block.Text.Trim().Length > 0) retorno.Add("LINE: " + block.Text.Trim().ToUpper());
                        //}
                        if (block.BlockType.Value == "WORD")
                        {
                            if (block.Text != null) if (block.Text.Trim().Length > 0) retorno.Add(block.Text.Replace("\"", "").Trim().ToUpper());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.Add("ERRO => " + ex.Message);
            }

            return retorno;
        }

        public static async Task<List<string>> StartDetectSampleAsync(string link)
        {
            List<string> retorno = new List<string>();

            try
            {
                string bucketName = ConfigurationManager.AppSettings["BucketName"];
                string s3ServiceUrl = ConfigurationManager.AppSettings["AWSServiceUrl"];
                string accessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
                string secretAccessKey = ConfigurationManager.AppSettings["AWSSecretKey"];
                string nomeArquivo = link.Replace((bucketName + "/"), "").Replace((s3ServiceUrl + "/"), "");

                using (var textractClient = new AmazonTextractClient(RegionEndpoint.USEast1))
                {
                    var startResponse = await textractClient.StartDocumentTextDetectionAsync(new StartDocumentTextDetectionRequest
                    {
                        DocumentLocation = new DocumentLocation
                        {
                            S3Object = new Amazon.Textract.Model.S3Object
                            {
                                Bucket = bucketName,
                                Name = nomeArquivo
                            }
                        }
                    });

                    var getDetectionRequest = new GetDocumentTextDetectionRequest
                    {
                        JobId = startResponse.JobId
                    };

                    GetDocumentTextDetectionResponse getDetectionResponse = null;

                    do
                    {
                        Thread.Sleep(1000);

                        getDetectionResponse = await textractClient.GetDocumentTextDetectionAsync(getDetectionRequest);
                    } 
                    while (getDetectionResponse.JobStatus == JobStatus.IN_PROGRESS);

                    if (getDetectionResponse.JobStatus == JobStatus.SUCCEEDED)
                    {
                        do
                        {
                            foreach (var block in getDetectionResponse.Blocks)
                            {

                                //if (block.BlockType.Value == "LINE")
                                //{
                                //    if (block.Text != null) if (block.Text.Trim().Length > 0) retorno.Add("LINE: " + block.Text.Trim().ToUpper());
                                //}
                                if (block.BlockType.Value == "WORD")
                                {
                                    if (block.Text != null) if (block.Text.Trim().Length > 0) retorno.Add(block.Text.Replace("\"", "").Trim().ToUpper());
                                }
                            }

                            if (string.IsNullOrEmpty(getDetectionResponse.NextToken)) break;

                            getDetectionRequest.NextToken = getDetectionResponse.NextToken;
                            getDetectionResponse = await textractClient.GetDocumentTextDetectionAsync(getDetectionRequest);

                        } 
                        while (!string.IsNullOrEmpty(getDetectionResponse.NextToken));
                    }
                    else
                    {
                        retorno.Add("ERRO => " + getDetectionResponse.StatusMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.Add("ERRO => " + ex.Message);
            }

            return retorno;
        }
    }
}
