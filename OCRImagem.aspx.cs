using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.UI;

namespace AWS_S3_TEXTRACT
{
    public partial class OCRImagem : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string bucketName = ConfigurationManager.AppSettings["BucketName"];
                string serviceUrlAWS = ConfigurationManager.AppSettings["AWSServiceUrl"];
                string imageAWS = ConfigurationManager.AppSettings["AWSTextractImageTest"];

                ImageAWS.ImageUrl = (serviceUrlAWS + "/" + bucketName + "/" + imageAWS);
                ImageAWS.Visible = true;
            }
        }

        protected void btnOCRImage_Click(object sender, EventArgs e)
        {
            string imageAWS = ConfigurationManager.AppSettings["AWSTextractImageTest"];

            List<string> retorno;

            lblMensagemOCR.Text = "Aguarde... Processando o OCR da imagem...";

            try
            {
                retorno = Task.Run(async () => await FuncoesComuns.OCRImagemAnexoS3Async(imageAWS)).Result;

                string msg = "";

                foreach (var item in retorno)
                {
                    msg += item + " | ";
                }

                lblMensagemOCR.Text = msg;
            }
            catch (Exception ex)
            {
                lblMensagemOCR.Text = "ERRO => " + ex.Message + " => " + ex.InnerException.Message;
            }
        }

        protected void btnAnalisarOCRImagem_Click(object sender, EventArgs e)
        {
            //lblMensagemOCR.Text = "MONTEIRO BRAGA CONSULTORIA EMPRESARIAL LTDA | DEALERNET | R. ANDRÉ L. R. DAFONTE 25/26 - SALA 601 | 42.700-000 L. DE FREITAS - BA | CNPJ:63.358.000/0001-49 | IE:66994360-N0 | UF:BA | M:ISENTO | 25/06/2012 14:36:29 CCF:000002 | COO:000005 | CNPJ/CPF CONSUMIDOR:LL1.111.111/11 | NOME:URSO DA BATUCADA | END:RUA ALMIRANTE BARROSO. N°40. VITORIA, CEP:4 | 0275240. SALVADOR-BA | CUPOM FISCAL | ITEM CÓDIGO DESCRIÇÃO QTD. UN. VL_UNIT( R$) ST VL_ | ITEM( R$) | 001 13435708 | ABRACADEIRA 90.5 | 1UN X 9.28 | F1 | 9.28G | TOTAL R$ | 9.28 | DINHEIRO | 9.28 | D-5:CBF73CC09FFFC7EE5FABACB4D9F3901D | PV:0000005390 I NS :0093976-1 | MINAS LEGAL 14552558000194 25062012 928 | BEMATECH | MP-2100 TH FI | ECF-IF | VERSÃO:01.00.01 | ECF:001 LJ:0001 | 25/06/2012 14:37:05 | FAB:EMULADOR | BR | MONTEIRO | BRAGA CONSULTORIA | EMPRESARIAL | LTDA | DEALERNET | R. | ANDRÉ | L. | R. | DAFONTE | 25/26 | - | SALA | 601 | 42.700-000 | L. | DE | FREITAS | - | BA | CNPJ:63.358.000/0001-49 | IE:66994360-N0 | UF:BA | M:ISENTO | 25/06/2012 | 14:36:29 | CCF:000002 | COO:000005 | CNPJ/CPF | CONSUMIDOR:LL1.111.111/11 | NOME:URSO | DA | BATUCADA | END:RUA | ALMIRANTE | BARROSO. | N°40. | VITORIA, | CEP:4 | 0275240. | SALVADOR-BA | CUPOM | FISCAL | ITEM | CÓDIGO | DESCRIÇÃO | QTD. | UN. | VL_UNIT( | R$) | ST | VL_ | ITEM( | R$) | 001 | 13435708 | ABRACADEIRA | 90.5 | 1UN | X | 9.28 | F1 | 9.28G | TOTAL | R$ | 9.28 | DINHEIRO | 9.28 | D-5:CBF73CC09FFFC7EE5FABACB4D9F3901D | PV:0000005390 | I | NS | :0093976-1 | MINAS | LEGAL | 14552558000194 | 25062012 | 928 | BEMATECH | MP-2100 | TH | FI | ECF-IF | VERSÃO:01.00.01 | ECF:001 | LJ:0001 | 25/06/2012 | 14:37:05 | FAB:EMULADOR | BR | ";

            string[] ocrImagem = lblMensagemOCR.Text.Split('|');

            List<string> retorno;
            DateTime dataCompra = Convert.ToDateTime("25/06/2012");
            decimal valorCompra = 9.28M;
            string cepCompra = "40275240";
            string cnpjDeposito = null; //"63.358.000/0001-49";

            lblMensagemAnaliseOCR.Text = "Aguarde... Processando a análise do OCR da imagem...";

            retorno = FuncoesComuns.AnalisarOCRComprovantePC(ocrImagem, dataCompra, valorCompra, cepCompra, cnpjDeposito);

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
