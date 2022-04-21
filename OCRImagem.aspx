<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OCRImagem.aspx.cs" Inherits="AWS_S3_TEXTRACT.OCRImagem" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-default floatLeft body-formulario">
        <div id="Titulo" class="title">
            <h2 class="tituloDefault">
                <asp:Label ID="lblTitulo" Text="Teste de OCR em uma imagem salva no AWS usando o TEXTRACT" runat="server" />
            </h2>
        </div>
        <div class="row default-padding" style="margin-top: 30px;">
            <div class="col-xs-12 floatLeft">
                <span style="font-weight: bold;">Retorno do OCR da imagem:</span>
            </div>
            <div class="col-xs-12 floatLeft" style="margin-top: 10px;">
                <asp:Label ID="lblMensagemOCR" Text="Aguardando OCR..." runat="server" />
            </div>
        </div>
        <div class="row default-padding" style="margin-top: 30px;">
            <div class="col-xs-12 floatLeft">
                <span style="font-weight: bold;">Análise do retorno do OCR da imagem para saber se os dados do Comprovante estão corretos:</span>
            </div>
            <div class="col-xs-12 floatLeft" style="margin-top: 10px;">
                <asp:Label ID="lblMensagemAnaliseOCR" Text="Aguardando análise..." runat="server" />
            </div>
        </div>
        <div id="divBotoesFinais" class="row default-padding" style="margin-top: 30px;">
            <div class="col-xs-12 floatLeft direita">
                <asp:Button ID="btnOCRImage" Class="btn btn-primary" runat="server" Text="Aplicar OCR" Style="width: 120px" OnClick="btnOCRImage_Click" />
                <asp:Button ID="btnAnalisarOCRImagem" Class="btn btn-primary" runat="server" Text="Aplicar Análise OCR" Style="width: 160px" OnClick="btnAnalisarOCRImagem_Click" />
            </div>
        </div>
        <div class="row default-padding" style="margin-top: 30px;">
            <div class="col-xs-12 floatLeft">
                <asp:Label ID="lblComprovante" Text="Comprovante" runat="server" />
            </div>
            <div id="divImagemAWS" class="col-xs-12 floatLeft" style="margin-top: 10px;">
                <asp:Image ID="ImageAWS" Visible = "false" runat="server" Height = "500" Width = "500" />
            </div>
        </div>
    </div>
</asp:Content>
