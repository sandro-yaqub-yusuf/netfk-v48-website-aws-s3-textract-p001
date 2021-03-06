using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static List<string> AnalisarOCRComprovantePC(string ocrArquivo, DateTime dataCompra, decimal valorCompra, string cepCompra, string cnpjDeposito = null)
        {
            List<string> retorno = new List<string>();

            CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-BR");

            bool achouDataCompra = false;
            bool achouValorCompra = false;
            bool achouCepCompra = false;
            bool achouCnpjDeposito = false;

            List<string> datas = new List<string> {
                dataCompra.ToString("dd M yy"),
                dataCompra.ToString("dd,M,yy"),
                dataCompra.ToString("dd.M.yy"),
                dataCompra.ToString("dd-M-yy"),
                dataCompra.ToString("dd/M/yy"),
                dataCompra.ToString("dd/M/yyyy"),
                dataCompra.ToString("dd MM yy"),
                dataCompra.ToString("dd,MM,yy"),
                dataCompra.ToString("dd.MM.yy"),
                dataCompra.ToString("dd-MM-yy"),
                dataCompra.ToString("dd/MM/yy"),
                dataCompra.ToString("dd MM yyyy"),
                dataCompra.ToString("dd,MM,yyyy"),
                dataCompra.ToString("dd.MM.yyyy"),
                dataCompra.ToString("dd-MM-yyyy"),
                dataCompra.ToString("dd/MM/yyyy"),
                dataCompra.ToString("dd/MMM/yyyy").ToUpper(),
                dataCompra.ToString("dd MMM yyyy").ToUpper(),
                dataCompra.ToString("dd / MM ## yyyy").Replace("##", "de").ToUpper(),
                dataCompra.ToString("dd / MMMM ## yyyy").Replace("##", "de").ToUpper(),
                dataCompra.ToString("dd ## MMMM ## yyyy").Replace("##", "de").ToUpper()
            };

            List<string> valores = new List<string> { 
                valorCompra.ToString(), 
                valorCompra.ToString().Replace(",", "."),
                valorCompra.ToString("N", culture),
                valorCompra.ToString("N", culture).Replace(",", ".")
            };

            List<string> ceps = new List<string>();
            List<string> cnpjsDeposito = new List<string>();

            if (cnpjDeposito != null)
            {
                cnpjsDeposito = new List<string> {
                    cnpjDeposito.Replace(".", "").Replace("/", "").Replace("-", ""),
                    string.Format(@"{0:00\.000\.000\/0000\-00}", long.Parse(cnpjDeposito.Replace(".", "").Replace("/", "").Replace("-", "")))
                };
            }
            else
            {
                cepCompra = cepCompra.Replace("N/A", "000");

                ceps = new List<string> {
                    cepCompra.Replace("-", ""),
                    string.Format(@"{0:00000\-000}", long.Parse(cepCompra.Replace("-", ""))),
                    string.Format(@"{0:00\.000\-000}", long.Parse(cepCompra.Replace("-", "")))
                };
            }

            if (!achouDataCompra) achouDataCompra = datas.Any(w => ocrArquivo.Contains(w));
            if (!achouValorCompra) achouValorCompra = valores.Any(w => ocrArquivo.Contains(w));

            if (cnpjDeposito != null)
            {
                if (!achouCnpjDeposito) achouCnpjDeposito = cnpjsDeposito.Any(w => ocrArquivo.Contains(w));
            }
            else
            {
                if (!achouCepCompra) achouCepCompra = ceps.Any(w => ocrArquivo.Contains(w));
            }

            if (achouDataCompra) retorno.Add("Data Compra: OK"); else retorno.Add("Data Compra: divergente");
            if (achouValorCompra) retorno.Add("Valor Compra: OK"); else retorno.Add("Valor Compra: divergente");
            if (cnpjDeposito != null) if (achouCnpjDeposito) retorno.Add("CNPJ DEPOSITO: OK"); else retorno.Add("CNPJ DEPOSITO: divergente");
            else if (achouCepCompra) retorno.Add("CEP Compra: OK"); else retorno.Add("CEP Compra: divergente");

            return retorno;
        }

        public static async Task<List<string>> OCRAnexoS3Async(string link)
        {
            List<string> resposta;

            string url = @link;
            string ext = Path.GetExtension(url).ToLower().Trim();

            if (ext == ".pdf") resposta = await AWSTextract.StartDetectSampleAsync(link);
            else resposta = await AWSTextract.DetectSampleAsync(link);

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
