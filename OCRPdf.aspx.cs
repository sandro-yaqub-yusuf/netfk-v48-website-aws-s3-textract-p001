using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.UI;

namespace AWS_S3_TEXTRACT
{
    public partial class OCRPdf : Page
    {
        private readonly DateTime dataCompra = Convert.ToDateTime("16/10/2021");
        private readonly decimal valorCompra = 8.92M;
        private readonly string cepCompra = "05426100";
        private readonly string cnpjDeposito = null; //"00000000000000";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string pdfAWS = ConfigurationManager.AppSettings["AWSTextractPDFTest"];

                iframePDF.Attributes.Add("src", pdfAWS);

                lblComprovante.Text = string.Format("Dados do Comprovante => Data: {0} - Valor: {1} - Cep: {2}", dataCompra.ToString("dd/MM/yyyy"), valorCompra, cepCompra);
            }
        }

        protected void btnOCRPdf_Click(object sender, EventArgs e)
        {
            string pdfAWS = ConfigurationManager.AppSettings["AWSTextractPDFTest"];

            List<string> retorno;

            lblMensagemOCR.Text = "Aguarde... Processando o OCR do arquivo PDF...";

            try
            {
                retorno = Task.Run(async () => await FuncoesComuns.OCRAnexoS3Async(pdfAWS)).Result;

                lblMensagemOCR.Text = String.Join(" | ", retorno);
            }
            catch (Exception ex)
            {
                lblMensagemOCR.Text = "ERRO => " + ex.Message + " => " + ex.InnerException.Message;
            }
        }

        protected void btnAnalisarOCRPdf_Click(object sender, EventArgs e)
        {
            //lblMensagemOCR.Text = "";

            string ocrArquivo = lblMensagemOCR.Text.Replace(" | ", " ");

            List<string> retorno;

            lblMensagemAnaliseOCR.Text = "Aguarde... Processando a análise do OCR da imagem...";

            retorno = FuncoesComuns.AnalisarOCRComprovantePC(ocrArquivo, dataCompra, valorCompra, cepCompra, cnpjDeposito);

            string msg = "";

            foreach (var item in retorno)
            {
                if (msg.Length <= 0) msg = item.ToString();
                else msg += " - " + item.ToString();
            }

            lblMensagemAnaliseOCR.Text = msg;
        }
    }
}
