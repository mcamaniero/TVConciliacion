<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainConciliacionWeb.aspx.cs" Inherits="TvCable.Conciliacion.Web.MainConciliacionWeb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>TVCable Conciliación</title>

    <link rel="stylesheet" type="text/css" href="CSS/reset.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="CSS/text.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="CSS/grid.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="CSS/layout.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="CSS/nav.css" media="screen" />
    <script type="text/javascript" src="jscrit/Utilis.js"></script>
    <link href="CSS/Stylepopup.css" rel="stylesheet" />
</head>
<body>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="smPageMainConciliacion" runat="server"></asp:ScriptManager>

        <div class="container_12">
            <div class="grid_12 header-repeat">
                <div id="branding">
                    <div class="floatleft">
                        <img src="imag/logo.png" alt="Logo" />
                    </div>
                    <div class="floatright">
                        <div class="floatleft">
                            <img src="imag/img-profile.jpg" alt="Profile Pic" />
                        </div>
                        <div class="floatleft marginleft10">
                            <ul class="inline-ul floatleft">
                                <li>
                                    <asp:Label ID="lbltitSaludo" runat="server" Text="<%$ Resources:WebFormResource, titSaludo %>"></asp:Label><asp:Label ID="lblNombreUser" runat="server" Text=""></asp:Label></li>

                                <li><a href="#">
                                    <asp:LinkButton ID="linkBCerarSesion" runat="server" Text="<%$ Resources:WebFormResource, titCerraSesion %>" OnClick="linkBCerarSesion_Click"></asp:LinkButton>
                                </a></li>
                            </ul>
                            <br />
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
            <div class="clear">
            </div>
            <div class="grid_12">
                <ul class="nav main">
                    <li class="ic-dashboard">&nbsp;</li>
                </ul>
            </div>
            <div class="clear">
            </div>

            <div class="grid_10">
                <div class="box round first">
                    <h2>Conciliación</h2>
                    <div class="block">
                        <div id="chart1">
                            <asp:Panel ID="GropuBoxSelect" runat="server">

                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblfechaini" runat="server" Text="<%$ Resources:WebFormResource, titfechaini %>"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="TextBoxfechainicio" runat="server"></asp:TextBox>
                                            <asp:ImageButton ID="imgPopup1" runat="server" ImageAlign="Bottom" ImageUrl="~/imag/calendar.gif" />
                                            <cc1:CalendarExtender ID="CalendarFechainicio" runat="server"
                                                Format="dd/MM/yyyy" PopupButtonID="imgPopup1"
                                                TargetControlID="TextBoxfechainicio" DaysModeTitleFormat="dd/MM/yyyy"
                                                TodaysDateFormat="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td></td>
                                        <td>

                                            <asp:Label ID="lblFechafin" runat="server" Text="<%$ Resources:WebFormResource, tipFechafin %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBoxfechafin" runat="server"></asp:TextBox>
                                            <asp:ImageButton ID="imgPopup" runat="server" ImageAlign="Bottom" ImageUrl="~/imag/calendar.gif" />
                                            <cc1:CalendarExtender ID="CalendarFechafin" runat="server" Format="dd/MM/yyyy"
                                                PopupButtonID="imgPopup" TargetControlID="TextBoxfechafin"
                                                DaysModeTitleFormat="dd/MM/yyyy" TodaysDateFormat="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbltitrecaudador" runat="server" Text="<%$ Resources:WebFormResource, titRecaudador %>"></asp:Label></td>
                                        <td>
                                            <asp:DropDownList ID="LisdrodowRecaudador" runat="server" Width="300px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td class="auto-style10">
                                            <p class="MsoNormal">
                                                <span>
                                                    <asp:Label ID="lblConcil" runat="server" Text="<%$ Resources:WebFormResource, tipConci %>"></asp:Label>
                                                </span>
                                            </p>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="LisdrodowConciliacion" runat="server" Width="300px">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table style="width: 100%;">
                                <tr>
                                    <td class="auto-style18">
                                        <table>
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="BtbuscarPagos" runat="server" OnClick="BtbuscarPagos_Click" Text="<%$ Resources:WebFormResource, titBtbuscarPagos %>" /></td>
                                            </tr>
                                        </table>
                                    </td>

                                    <td></br>

                                        <asp:Panel ID="GrupBoxInsertaIntermix" runat="server" Visible="False">

                                            <table align="left">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Button ID="BtfileInternix" runat="server" Text="<%$ Resources:WebFormResource, titBtfileInternixs %>" OnClick="BtfileInternix_Click" />
                                                    </td>
                                                    <td align="left">
                                                        <asp:Button ID="Btrechpagos" runat="server" Text="<%$ Resources:WebFormResource, titBtrechpagos %>" OnClick="Btrechpagos_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>

                            <div>
                                <h1>
                                    <asp:Label ID="LblmensajeConsulta" runat="server" Text=""></asp:Label>
                                </h1>
                            </div>

                            <div>
                                <asp:Label ID="lblNumeroItmeslist" runat="server" Text="<%$ Resources:WebFormResource, msglblnumeroitems %>"></asp:Label>
                                <asp:DropDownList ID="LisdrodowItems" runat="server" OnSelectedIndexChanged="LisdrodowItems_SelectedIndexChanged" AutoPostBack="True">
                                    <asp:ListItem>50</asp:ListItem>
                                    <asp:ListItem>100</asp:ListItem>
                                    <asp:ListItem>500</asp:ListItem>
                                    <asp:ListItem>1000</asp:ListItem>
                                    <asp:ListItem>2000</asp:ListItem>
                                    <asp:ListItem Value="10000">Todos</asp:ListItem>
                                </asp:DropDownList>

                                &nbsp;
                            </div>

                            <asp:GridView ID="GridViewConciliacion"
                                runat="server"
                                CellPadding="4"
                                CssClass="gridview"
                                OnSelectedIndexChanged="GridViewConciliacion_SelectedIndexChanged"
                                Width="100%" AutoGenerateColumns="False"
                                DataKeyNames="RCO_ID" AllowPaging="True"
                                OnPageIndexChanging="OnPaging" PageSize="5" ForeColor="#333333" GridLines="None">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField HeaderText="" SortExpression="check">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkboxSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkboxSelectAll_CheckedChanged" />
                                        </HeaderTemplate>

                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkRow" runat="server" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRow" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select" Text="
                                                    &lt;img src=&quot;imag/application_double.png&quot; /&gt;
                                            "></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkUpdate" OnClick="GridViewConciliacion_SelectedIndexChanged" Text="">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RCO_ID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                        <HeaderStyle CssClass="hideGridColumn "></HeaderStyle>

                                        <ItemStyle CssClass="hideGridColumn"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TUV_MONTO" HeaderText="MONTO TUVES" DataFormatString="{0:C}" />
                                    <asp:BoundField DataField="MDP_MONTO" HeaderText="MONTO MDP" DataFormatString="{0:C}" />
                                    <asp:BoundField DataField="TUV_FECHA" DataFormatString="{0:d}" HeaderText="FECHA TUVES" />
                                    <asp:BoundField DataField="MDP_FECHA_INI_TRX" DataFormatString="{0:d}" HeaderText="FECHA MDP" />
                                    <asp:BoundField DataField="TUV_CLIENTE" HeaderText="CLIENTE TUVES" />
                                    <asp:BoundField DataField="MDP_CLIENTE_ID" HeaderText=" CLIENTE MDP" />
                                    <asp:BoundField DataField="TUV_TRX_ID" HeaderText="ID TUVES" />
                                    <asp:BoundField DataField="MDP_ID_TRX_MDP" HeaderText="ID MDP" />
                                </Columns>
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            </asp:GridView>
                            <i>
                                <asp:Label ID="lblnumpag" runat="server" Text="<%$ Resources:WebFormResource, msglblnumpag %>"></asp:Label>
                                <%=GridViewConciliacion.PageIndex + 1%>de
                                <%=GridViewConciliacion.PageCount%>
                            </i>

                            <asp:Button runat="server" ID="btnModalPopUp" Style="display: none" />
                            <cc1:ModalPopupExtender ID="modalPoExtenderDetalle" runat="server"
                                TargetControlID="btnModalPopUp"
                                PopupControlID="PanelDetalle"
                                BackgroundCssClass="modalBackground"
                                OkControlID="btnCancel"
                                DropShadow="true"
                                PopupDragHandleControlID="PanelDetalle">
                            </cc1:ModalPopupExtender>
                            <asp:Panel ID="PanelDetalle" runat="server" CssClass="CajaDialogo" Width="394px" Style="display: none;">
                                <div>
                                    <p align="right">
                                        <asp:LinkButton ID="btnCancel" runat="server" ForeColor="White">Cerrar</asp:LinkButton>
                                    </p>
                                </div>
                                <div>

                                    <asp:Label ID="lblId1" runat="server" />
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="font-size: x-large">

                                                <asp:Label ID="lbldetalleConci" runat="server" Text="<%$ Resources:WebFormResource, tidetalleConci %>"></asp:Label>
                                                <asp:Label ID="lblEstadodetalle" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>

                                    <table style="width: 100%" align="left">
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblTituloMonto" runat="server" Text="<%$ Resources:WebFormResource, tiTituloTuv %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTituloMdp" runat="server" Text="<%$ Resources:WebFormResource, tiTituloMdp %>"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTiMonto" runat="server" Text="<%$ Resources:WebFormResource, titTituloMonto %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMontoTUV" runat="server" Text="lblMontoTUV"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMontoMdp" runat="server" Text="lblMontoMdp"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFecha" runat="server" Text="<%$ Resources:WebFormResource, titTituloFecha %>" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFechaTuv" runat="server" Text="lblFechaTuv"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFechaMdp" runat="server" Text="lblFechaMdp"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTituloUsuario" runat="server" Text="<%$ Resources:WebFormResource, titTituloUsuario %>" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblUsuarioTUV" runat="server" Text="lblUsuarioTUV"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblUsuarioMpd" runat="server" Text="lblUsuarioMpd"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTituloID" runat="server" Text="<%$ Resources:WebFormResource, titID %>" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIdTuv" runat="server" Text="lblIdTuv"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIdMdp" runat="server" Text="lblIdMdp"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbltitobservacion" runat="server" Text="<%$ Resources:WebFormResource, titObservacion %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblObservacion" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <table style="width: 100%" align="left">

                                        <asp:Panel ID="PanelEsado" runat="server">
                                            <tr>

                                                <td>
                                                    <asp:Label ID="LblCambioestado" runat="server" Text="<%$ Resources:WebFormResource, titCambioestado %>"></asp:Label>
                                                </td>
                                                <td></td>
                                                <td>
                                                    <asp:DropDownList ID="LisdrodowEstado" runat="server" CssClass="ValorListado" Width="300px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td>
                                                    <asp:Label ID="Lblobser" runat="server" Text="<%$ Resources:WebFormResource, titObservacion%>"></asp:Label>
                                                </td>
                                                <td></td>
                                                <td>
                                                    <asp:TextBox ID="txtOservacion" runat="server" TextMode="MultiLine" CssClass="ValorTxtArea" Width="294px" Height="76px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td></td>
                                                <td></td>
                                                <td>
                                                    <asp:Button ID="BtCambioestado" runat="server" OnClick="BtCambioestado_Click" Text="<%$ Resources:WebFormResource, titBtCambioestado%>" CssClass="ValorBooton" Width="300px" OnClientClick="Confirm()" />
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                    </table>
                                </div>
                            </asp:Panel>
                        </div>

                        <asp:Button runat="server" ID="Btrechpagosvarios" Style="display: none" />

                        <cc1:ModalPopupExtender ID="ModalPopupExtendervarios" runat="server"
                            TargetControlID="Btrechpagosvarios"
                            PopupControlID="PanelCambiovarios"
                            BackgroundCssClass="modalBackground"
                            OkControlID="btnCancelVarios"
                            PopupDragHandleControlID="PanelCambiovarios">
                        </cc1:ModalPopupExtender>

                        <div>
                            <asp:Panel ID="PanelCambiovarios" runat="server" CssClass="CajaDialogo" Width="425px" Style="display: none;">
                                <div>
                                    <p align="right">
                                        <asp:LinkButton ID="btnCancelVarios" runat="server" ForeColor="White">Cerrar</asp:LinkButton>
                                    </p>
                                </div>
                                <div>
                                    <asp:Label ID="Label3" runat="server" />
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="font-size: x-large">
                                                <asp:Label ID="lbltitItemasVarios" runat="server" Text="<%$ Resources:WebFormResource, titItemasVarios%>"></asp:Label>

                                                <asp:Label ID="lblEstadoVarios" runat="server" Text="Label"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <asp:Label ID="lblNumItems" runat="server" Text=""></asp:Label>
                                </div>
                                <div>
                                    <table style="width: 406px">
                                        <tr>
                                            <td>

                                                <asp:Label ID="lbltitActuEstado" runat="server" Text="<%$ Resources:WebFormResource, titActuEstado%>"></asp:Label>
                                            </td>

                                            <td class="auto-style5">

                                                <asp:DropDownList ID="LisdrodowEstadovarios" runat="server" CssClass="ValorListado" Width="300px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbltitActuObservacion" runat="server" Text="<%$ Resources:WebFormResource, ltitActuObservacion%>"></asp:Label>
                                            </td>

                                            <td>
                                                <asp:TextBox ID="txtOservacionvarios" runat="server" TextMode="MultiLine" CssClass="ValorTxtArea" Width="294px" Height="76px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td align="right">
                                                <asp:Button ID="BtCambioestadovarios" runat="server" OnClick="BtCambioestadovarios_Click" Text="<%$ Resources:WebFormResource, titBtCambioestadovarios%>" CssClass="ValorBooton" Width="300px" OnClientClick="Confirm()" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </div>
                        <div>

                            <asp:Button runat="server" ID="BtfileInternixvarios" Style="display: none" />
                            <cc1:ModalPopupExtender ID="ModalPopupExtenderIntermix" runat="server"
                                TargetControlID="BtfileInternixvarios"
                                PopupControlID="PanelInermix"
                                BackgroundCssClass="modalBackground"
                                OkControlID="btnCancelIntermix"
                                PopupDragHandleControlID="PanelInermix">
                            </cc1:ModalPopupExtender>
                            <asp:Panel ID="PanelInermix" runat="server" Width="424px" CssClass="CajaDialogoitermixPop" BackImageUrl="~/imag/nav-repeat3.jpg" BorderColor="#6E6E6E" BorderWidth="4px" Style="display: none;">

                                <div>
                                    <p align="right">
                                        <asp:LinkButton ID="btnCancelIntermix" runat="server" ForeColor="White">Cerrar</asp:LinkButton>
                                    </p>
                                </div>
                                <div>
                                    <asp:Label ID="Label9" runat="server" />
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="font-size: x-large">
                                                <asp:Label ID="lbltitInternix" runat="server" Text="<%$ Resources:WebFormResource, titInternix%>"></asp:Label></td>

                                            <td align="right"></td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <asp:Label ID="numItemsIterx" runat="server" Text=""></asp:Label>
                                </div>
                                <div>
                                    <table>

                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbltitFechaEmision" runat="server" Text="<%$ Resources:WebFormResource, titFechaEmision%>" /></td>
                                            <td>
                                                <asp:TextBox ID="txtFechaEmision" runat="server"></asp:TextBox>

                                                <asp:ImageButton ID="imgPopupEmi" runat="server" ImageAlign="Bottom" ImageUrl="~/imag/calendar.gif" />

                                                <cc1:CalendarExtender
                                                    ID="CalendarFechaEmi"
                                                    PopupButtonID="imgPopupEmi"
                                                    runat="server"
                                                    TargetControlID="txtFechaEmision"
                                                    Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:MaskedEditExtender
                                                    TargetControlID="txtFechaEmision"
                                                    Mask="99/99/9999" MaskType="Date"
                                                    ID="maskedEditExtender3"
                                                    OnFocusCssClass="MaskedEditFocus"
                                                    runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>

                                                <asp:Label ID="lbltitEmpresaFactura" runat="server" Text="<%$ Resources:WebFormResource, titEmpresaFactura%>" />
                                            </td>
                                            <td align="right">

                                                <asp:DropDownList ID="drdEmpresaFac" runat="server" CssClass="ValorListado" Style="margin-left: 0px" Width="300px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="lbltitInterObservacion" runat="server" Text="<%$ Resources:WebFormResource, titInterObservacion%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtObervacionIntermix" runat="server" TextMode="MultiLine" CssClass="ValorTxtArea" Width="294px" Height="76px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Button ID="btInsertarIntermix" runat="server" OnClick="btInsertarIntermix_Click" Text="<%$ Resources:WebFormResource, titbtInsertarIntermix%>" CssClass="ValorBooton" Width="300px" OnClientClick="ConfirmIntermix()" /></td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
        <div class="clear">
        </div>
        <div id="site_info">
            <p>
                <asp:Label ID="lbltitCopyright" runat="server" Text="<%$ Resources:WebFormResource, titCopyright%>"></asp:Label>
                <a href="#">TVCable</a>.<asp:Label ID="lbltitderechos" runat="server" Text="<%$ Resources:WebFormResource, titderechos%>"></asp:Label>
            </p>
        </div>
        </div>
    </form>
</body>
</html>