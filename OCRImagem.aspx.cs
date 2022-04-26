using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.UI;

namespace AWS_S3_TEXTRACT
{
    public partial class OCRImagem : Page
    {
        private readonly DateTime dataCompra = Convert.ToDateTime("02/02/2022");
        private readonly decimal valorCompra = 1051.55M;
        private readonly string cepCompra = "40275240";
        private readonly string cnpjDeposito = null; //"00000000000000";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string imageAWS = ConfigurationManager.AppSettings["AWSTextractImageTest"];
                
                ImageAWS.ImageUrl = imageAWS;
                ImageAWS.Visible = true;

                lblComprovante.Text = string.Format("Dados do Comprovante => Data: {0} - Valor: {1} - Cep: {2}", dataCompra.ToString("dd/MM/yyyy"), valorCompra, cepCompra);
            }
        }

        protected void btnOCRImage_Click(object sender, EventArgs e)
        {
            string imageAWS = ConfigurationManager.AppSettings["AWSTextractImageTest"];

            List<string> retorno;

            lblMensagemOCR.Text = "Aguarde... Processando o OCR da imagem...";

            try
            {
                retorno = Task.Run(async () => await FuncoesComuns.OCRAnexoS3Async(imageAWS)).Result;

                lblMensagemOCR.Text = String.Join(" | ", retorno);
            }
            catch (Exception ex)
            {
                lblMensagemOCR.Text = "ERRO => " + ex.Message + " => " + ex.InnerException.Message;
            }
        }

        protected void btnAnalisarOCRImagem_Click(object sender, EventArgs e)
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
