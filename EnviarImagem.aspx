<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EnviarImagem.aspx.cs" Inherits="AWS_S3_TEXTRACT.EnviarImagem" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<script>
    $(document).ready(function () {
        document.getElementById('MainContent_FileUpload').addEventListener('change', carregarImagem, false);

        function carregarImagem() {
            console.log("Imagem", $('#MainContent_FileUpload')[0].files[0]);

            return false;
        }
    });
</script>
<div class="panel panel-default floatLeft body-formulario">
    <div id="Titulo" class="title">
        <h2 class="tituloDefault">
            <asp:Label ID="lblTitulo" Text="Teste de envio de imagem para AWS" runat="server" />
        </h2>
    </div>
    <div class="row default-padding" style="margin-top: 30px;">
        <div class="col-xs-1 floatLeft">
            <span style="font-weight: bold;">Comprovante:</span>
        </div>
        <div id="divFileUploadNota" class="col-xs-3 floatLeft">
            <asp:FileUpload ID="FileUpload" CssClass="fuSaqueArquivo" runat="server" AllowMultiple="false" ToolTip="formato .jpg /.png /.pdf" />
        </div>
        <div class="col-xs-8 floatLeft">
            <span style="font-weight: bold;">Os formatos aceitos são: .jpg /.png /.pdf</span>
        </div>
    </div>
    <div class="row default-padding" style="margin-top: 30px;">
        <div class="col-xs-12 floatLeft">
            <asp:Label ID="lblMensagem" Text="Aguardando envio..." runat="server" />
        </div>
        <div class="col-xs-12 floatLeft" style="margin-top: 20px;">
            <asp:Image ID="ImageUploaded" Visible = "false" runat="server" Height = "500" Width = "500" />
        </div>
    </div>
    <div id="divBotoesFinais" class="row default-padding" style="margin-top: 30px;">
        <div class="col-xs-12 floatLeft direita">
            <asp:Button ID="btnUploadAWS" Class="btn btn-primary" runat="server" Text="Salvar na AWS" Style="width: 120px" OnClick="btnUploadAWS_Click" />
        </div>
    </div>
</div>
</asp:Content>
