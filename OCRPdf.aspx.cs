using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.UI;

namespace AWS_S3_TEXTRACT
{
    public partial class OCRPdf : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string pdfAWS = ConfigurationManager.AppSettings["AWSTextractPDFTest"];

                iframePDF.Attributes.Add("src", pdfAWS);
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

        protected void btnAnalisarOCRPdf_Click(object sender, EventArgs e)
        {
            //lblMensagemOCR.Text = "03/11/2021 16:51 | MINHAS VIAGENS - USUÁRIOS DA UBER | 16 DE OUTUBRO DE 2021 | OBRIGADO POR VIAJAR, GESSE | TOTAL | R$ 8,92 | PREÇO DA VIAGEM | R$ 8,17 | SUBTOTAL | R$ 8,17 | CUSTO FIXO | ? | R$ 0,75 | VALOR COBRADO | 5871 | R$ 8,92 | REALIZAMOS UMA PRÉ-AUTORIZAÇÃO DE R$ 8,92 NA FORMA DE PAGAMENTO 5871. ESSA TRANSAÇÃO NÃO | É UMA COBRANÇA E SERÁ REMOVIDA. EM BREVE, ELA SERÁ EXCLUÍDA DO SEU EXTRATO BANCÁRIO. SAIBA MAIS | PARA MAIS INFORMAÇÕES, ACESSE A PÁGINA DA SUA VIAGEM | REPORTAR ITEM PERDIDO | > | ENTRAR EM CONTATO COM O | MINHAS VIAGENS > | SUPORTE > | HTTPS://RIDERS.UBER.COM/TRIPS/09638613-20B3-4345-B44D-54B4A50E8DT | 1/1 | 03/11/2021 | 16:51 | MINHAS | VIAGENS | - | USUÁRIOS | DA | UBER | 16 | DE | OUTUBRO | DE | 2021 | OBRIGADO | POR | VIAJAR, | GESSE | TOTAL | R$ | 8,92 | PREÇO | DA | VIAGEM | R$ 8,17 | SUBTOTAL | R$ 8,17 | CUSTO | FIXO | ? | R$ 0,75 | VALOR | COBRADO | 5871 | R$ | 8,92 | REALIZAMOS | UMA | PRÉ-AUTORIZAÇÃO | DE | R$ | 8,92 | NA | FORMA | DE | PAGAMENTO | 5871. | ESSA | TRANSAÇÃO | NÃO | É | UMA | COBRANÇA | E | SERÁ | REMOVIDA. | EM | BREVE, | ELA | SERÁ | EXCLUÍDA | DO | SEU | EXTRATO | BANCÁRIO. | SAIBA | MAIS | PARA | MAIS | INFORMAÇÕES, | ACESSE | A | PÁGINA | DA | SUA | VIAGEM | REPORTAR | ITEM | PERDIDO | > | ENTRAR | EM | CONTATO | COM | O | MINHAS | VIAGENS | > | SUPORTE | > | HTTPS://RIDERS.UBER.COM/TRIPS/09638613-20B3-4345-B44D-54B4A50E8DT | 1/1 |";

            string[] ocrImagem = lblMensagemOCR.Text.Split('|');

            List<string> retorno;
            DateTime dataCompra = Convert.ToDateTime("16/10/2021");
            decimal valorCompra = 8.92M;
            string cepCompra = "05426100";
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
