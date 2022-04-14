using System;
using System.IO;
using System.Web.UI;

namespace AWS_S3_TEXTRACT
{
    public partial class EnviarImagem : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {}

        protected void btnUploadAWS_Click(object sender, EventArgs e)
        {
            string extensaoArquivo = "";
            string link = "";

            Stream fs = FileUpload.PostedFile.InputStream;

            //Transformar arquivo de base 64 para subir pro AWS 
            if (fs.Length > 0)
            {
                BinaryReader br = new BinaryReader(fs);
                Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                string[] strBase64Dividida = base64String.Split(',');
                string strBase64Fim = strBase64Dividida[0];
                byte[] byteArray = Convert.FromBase64String(strBase64Fim);

                ImageUploaded.ImageUrl = "data:image/jpg;base64," + base64String;
                ImageUploaded.Visible = true;

                Stream stream = new MemoryStream(byteArray);

                extensaoArquivo = Path.GetExtension(FileUpload.FileName).ToLowerInvariant();

                link = FuncoesComuns.SalvarAnexoS3(stream, FileUpload.FileName, stream.Length, extensaoArquivo);

                lblMensagem.Text = "Imagem enviada para AWS... Link de acesso => " + link;

                stream = null;
            }
        }
    }
}
