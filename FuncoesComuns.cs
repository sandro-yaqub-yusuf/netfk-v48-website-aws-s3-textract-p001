using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AWS_S3_TEXTRACT
{
    public class FuncoesComuns
    {
        public static string AlfanumericoAleatorio(int tamanho)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijlmnopqrstuvxz";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, tamanho).Select(s => s[random.Next(s.Length)]).ToArray());

            return result;
        }

        public static List<string> AnalisarOCRComprovantePC(string[] ocrImagem, DateTime dataCompra, decimal valorCompra, string cepCompra, string cnpjDeposito = null)
        {
            List<string> retorno = new List<string>();

            bool achouDataCompra = false;
            bool achouValorCompra = false;
            bool achouCepCompra = false;
            bool achouCnpjDeposito = false;

            List<string> datas = new List<string> { 
                dataCompra.ToString("dd/MM/yy"), 
                dataCompra.ToString("dd/MM/yyyy"),
                dataCompra.ToString("dd/MMM/yyyy").ToUpper(),
                dataCompra.ToString("dd MMM yyyy").ToUpper(),
                dataCompra.ToString("dd ## MMMM ## yyyy").Replace("##", "de").ToUpper()
            };

            List<string> valores = new List<string> { 
                valorCompra.ToString(), 
                valorCompra.ToString().Replace(",", ".") 
            };

            List<string> ceps = new List<string> {
                cepCompra.Replace("-", ""),
                Convert.ToUInt64(cepCompra.Replace("-", "")).ToString("00000-000"),
                Convert.ToUInt64(cepCompra.Replace("-", "")).ToString("00.000-000")
            };

            List<string> cnpjsDeposito = new List<string>();

            if (cnpjDeposito != null)
            {
                cnpjsDeposito = new List<string> { 
                    cnpjDeposito.Replace(".", "").Replace("/", "").Replace("-", ""), 
                    Convert.ToUInt64(cnpjDeposito.Replace(".", "").Replace("/", "").Replace("-", "")).ToString("000.000.000/0000-00") 
                };
            }

            foreach (var item in ocrImagem)
            {
                if (!achouDataCompra) achouDataCompra = datas.Any(w => item.Contains(w));
                if (!achouValorCompra) achouValorCompra = valores.Any(w => item.Contains(w));

                if (cnpjDeposito != null)
                {
                    if (!achouCnpjDeposito) achouCnpjDeposito = cnpjDeposito.Any(w => item.Contains(w));
                }
                else
                {
                    if (!achouCepCompra) achouCepCompra = ceps.Any(w => item.Contains(w));
                }
            }

            if (achouDataCompra) retorno.Add("Data Compra: OK"); else retorno.Add("Data Compra: divergente");
            if (achouValorCompra) retorno.Add("Valor Compra: OK"); else retorno.Add("Valor Compra: divergente");
            if (cnpjDeposito != null) if (achouCnpjDeposito) retorno.Add("CNPJ DEPOSITO: OK"); else retorno.Add("CNPJ DEPOSITO: divergente");
            else if (achouCepCompra) retorno.Add("CEP Compra: OK"); else retorno.Add("CEP Compra: divergente");

            return retorno;
        }

        public static async Task<List<string>> OCRAnexoS3Async(string link)
        {
            var resposta = await AWSTextract.DetectSampleAsync(link);

            return resposta;
        }

        public static string SalvarAnexoS3(Stream stream, string fileName, long length, string extensao)
        {
            string link = "";
            string url = "";
            string hash = "";
            byte[] blob = new byte[stream.Length];

            stream.Read(blob, 0, Convert.ToInt32(blob.Length));

            hash = AlfanumericoAleatorio(15);

            url = DateTime.Now.Year.ToString("D4") + "_" + DateTime.Now.Month.ToString("D2") + "_" + DateTime.Now.Day.ToString("D2") + "_" +
                  DateTime.Now.Hour.ToString("D2") + "_" + DateTime.Now.Minute.ToString("D2") + "_" + DateTime.Now.Second.ToString("D2") + "_" + DateTime.Now.Millisecond.ToString("D3") + "_" +
                  hash + "_" + extensao;

            var resposta = AWSUploadS3.UploadFileAnexoS3(stream, url);

            if (resposta.Substring(0, 4) != "ERRO")
            {
                link = resposta;

                stream.Close();
            }

            return link;
        }
    }
}
