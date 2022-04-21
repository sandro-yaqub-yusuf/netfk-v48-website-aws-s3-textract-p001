<%@ Page Title="OCR PDF" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OCRPdf.aspx.cs" Inherits="AWS_S3_TEXTRACT.OCRPdf" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-default floatLeft body-formulario">
        <div id="Titulo" class="title">
            <h2 class="tituloDefault">
                <asp:Label ID="lblTitulo" Text="Teste de OCR em um arquivo PDF salvo no AWS usando o TEXTRACT" runat="server" />
            </h2>
        </div>
        <div class="row default-padding" style="margin-top: 30px;">
            <div class="col-xs-12 floatLeft">
                <span style="font-weight: bold;">Retorno do OCR do arquivo PDF:</span>
            </div>
            <div class="col-xs-12 floatLeft" style="margin-top: 10px;">
                <asp:Label ID="lblMensagemOCR" Text="Aguardando OCR..." runat="server" />
            </div>
        </div>
        <div class="row default-padding" style="margin-top: 30px;">
            <div class="col-xs-12 floatLeft">
                <span style="font-weight: bold;">Análise do retorno do OCR do arquivo PDF para saber se os dados do Comprovante estão corretos:</span>
            </div>
            <div class="col-xs-12 floatLeft" style="margin-top: 10px;">
                <asp:Label ID="lblMensagemAnaliseOCR" Text="Aguardando análise..." runat="server" />
            </div>
        </div>
        <div id="divBotoesFinais" class="row default-padding" style="margin-top: 30px;">
            <div class="col-xs-12 floatLeft direita">
                <asp:Button ID="btnOCRPdf" Class="btn btn-primary" runat="server" Text="Aplicar OCR" Style="width: 120px" OnClick="btnOCRPdf_Click" />
                <asp:Button ID="btnAnalisarOCRPdf" Class="btn btn-primary" runat="server" Text="Aplicar Análise OCR" Style="width: 160px" OnClick="btnAnalisarOCRPdf_Click" />
            </div>
        </div>
        <div class="row default-padding" style="margin-top: 30px;">
            <div class="col-xs-12 floatLeft">
                <asp:Label ID="lblComprovante" Text="Comprovante" runat="server" />
            </div>
            <div id="divImagemAWS" class="col-xs-12 floatLeft" style="margin-top: 10px;">
                <iframe id="iframePDF" src="https://s3.amazonaws.com/sgm-usa/comprovantes_prestacao/2021-11/2021_11_05_19_09_19_607_WvdhS2K1MTA8exx_490318.pdf" runat="server" width="900" height="900"  />
            </div>
        </div>
    </div>
</asp:Content>
