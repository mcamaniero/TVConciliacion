<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainConciliacionweb.aspx.cs" Inherits="TvCable.Conciliacion.Web.MainConciliacionweb" EnableEventValidation="false"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">



<html xmlns="http://www.w3.org/1999/xhtml">


<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>TVCable Conciliación</title>



    <link rel="stylesheet" type="text/css" href="Styles/reset.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="Styles/text.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="Styles/grid.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="Styles/layout.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="Styles/nav.css" media="screen" />
    <link id="linkCss" rel="stylesheet" href="../Styles/layoutboxdiag.css" />





</head>
<body>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="smPageMainConciliacion" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ScriptManager>

        <div>
            <div class="container_12">
                <div class="grid_12 header-repeat">
                    <div id="branding">
                        <div class="floatleft">
                            <img src="img/logo.png" alt="Logo" />
                        </div>
                        <div class="floatright">
                            <div class="floatleft">
                                <img src="img/img-profile.jpg" alt="Profile Pic" />
                            </div>
                            <div class="floatleft marginleft10">
                                <ul class="inline-ul floatleft">
                                    <li>Hola Admin</li>
                                    <li><a href="#">Config</a></li>
                                    <li><a href="#">Cerrar sesión</a></li>
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
                        <li class="ic-dashboard"><a href="#"><span>Home</span></a> </li>
                    </ul>
                </div>
                <div class="clear">
                </div>

                <div class="grid_10">
                    <div class="box round first">
                        <h2>Conciliación</h2>
                        <div class="block">
                            <div id="chart1">
                                <table>
                                    <tr>
                                        <td>Recaudador:</td>
                                        <td>
                                            <asp:DropDownList ID="LisdrodowRecaudador" runat="server"></asp:DropDownList>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td class="auto-style10">

                                            <p class="MsoNormal">
                                                <span>Tipo Conciliación:</span>
                                            </p>

                                        </td>
                                        <td>
                                            <asp:DropDownList ID="LisdrodowConciliacion" runat="server"></asp:DropDownList></td>
                                        <td></td>

                                    </tr>
                                    <tr>
                                        <td>Fecha inicio:</td>
                                        <td>
                                            <asp:TextBox ID="TextBoxfechainicio" runat="server"></asp:TextBox>
                                            <asp:ImageButton ID="imgPopup1" ImageUrl="~/img/calendar.gif" ImageAlign="Bottom"
                                                runat="server" />
                                            <cc1:CalendarExtender ID="CalendarFechaini" PopupButtonID="imgPopup1" runat="server" TargetControlID="TextBoxfechainicio"
                                                Format="dd/MM/yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td></td>
                                        <td>Fecha fin:</td>
                                        <td>
                                            <asp:TextBox ID="TextBoxfechafin" runat="server"></asp:TextBox>
                                            <asp:ImageButton ID="imgPopup" ImageUrl="~/img/calendar.gif" ImageAlign="Bottom"
                                                runat="server" />
                                            <cc1:CalendarExtender ID="CalendarFechafin" PopupButtonID="imgPopup" runat="server" TargetControlID="TextBoxfechafin"
                                                Format="yyyy/MM/dd">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td></td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="BtbuscarPagos" runat="server" OnClick="BtbuscarPagos_Click" Text="Buscar pagos" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:UpdatePanel ID="UdpanelGrilla" runat="server">
                                    <ContentTemplate>


                                        <asp:GridView ID="GridViewConciliacion"
                                            runat="server"
                                            BackColor="White"
                                            CellPadding="3"
                                            BorderColor="#CCCCCC" BorderStyle="Solid"
                                            CssClass="gridview" AllowPaging="True" OnSelectedIndexChanged="GridViewConciliacion_SelectedIndexChanged">


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

                                                <asp:CommandField ShowSelectButton="True" />
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="lnkUpdate" CausesValidation="false" Visible="false" ItemStyle-ForeColor="White"></asp:LinkButton>
                                                    </ItemTemplate>

                                                    
                                                </asp:TemplateField>


                                            </Columns>
                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#11557F" ForeColor="#000066" HorizontalAlign="Left" />
                                            <RowStyle ForeColor="#141516" />
                                            <SelectedRowStyle BackColor="#2C5FA5" Font-Bold="True" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                            <SortedDescendingHeaderStyle BackColor="#00547E" />

                                        </asp:GridView>
                                        <asp:Button runat="server" ID="btnModalPopUp" Style="display: none" />
                                       
                                   
                                <asp:Panel ID="PanelDetalle" runat="server" CssClass="CajaDialogo"  >
                                    <div>
                                        <asp:Label ID="lblId1" runat="server" />
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="font-size: x-large">Detalle pago</td>

                                                <td align="right">
                                                    <asp:Button ID="btnCancel" runat="server" CssClass="button" Text="[ X ]" />
                                                </td>
                                            </tr>

                                        </table>
                                    </div>
                                    <div>
                                        <table style="width: 100%">
                                            <tr>
                                                <td class="auto-style6">
                                                    <asp:Label ID="Label2" runat="server" Text="ID:" />
                                                </td>
                                                <td class="auto-style1">
                                                    <asp:Label ID="lblId" runat="server" Text="Label"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style6">
                                                    <asp:Label ID="Label6" runat="server" Text="Fecha:" />
                                                </td>
                                                <td class="auto-style1">

                                                    <asp:Label ID="lblFecha" runat="server" Text="Label"></asp:Label>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style6">
                                                    <asp:Label ID="Label5" runat="server" Text="Monto:" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblMonto" runat="server" Text="Label"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style6">
                                                    <asp:Label ID="Label7" runat="server" Text="Usuario:" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblUsuario" runat="server" Text="Label"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style6">
                                                    <asp:Label ID="Label8" runat="server" Text="Id transaccion pdp  :" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblpdp" runat="server" Text="Label"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style6">
                                                    <asp:Label ID="Label9" runat="server" Text="Id transacción tuves:" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTuves" runat="server" Text="Label"></asp:Label>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td class="auto-style6">
                                                    <asp:Label ID="Label1" runat="server" Text="Estado:" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblEstado" runat="server" Text="Label"></asp:Label>
                                                </td>

                                            </tr>
                                        </table>
                                    </div>
                                    <div>
                                        <table style="width: 100%">
                                            <tr>
                                                <td class="auto-style5">Cambio estado</td>
                                                <td>
                                                    <asp:DropDownList ID="LisdrodowEstado" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Button ID="BtCambioestado" runat="server" Text="Cambiar estado" />
                                                </td>
                                            </tr>

                                        </table>
                                        <asp:Button ID="btnOK" runat="server" Text="OK Client Side" Style="display: none" />

                                    </div>
                                </asp:Panel>
                                <cc1:ModalPopupExtender BackgroundCssClass="modalBg"
                                    DropShadow="true" ID="ModalPopupExtender1"
                                    PopupControlID="UdpanelGrilla"
                                    runat="server"
                                    TargetControlID="btnModalPopUp"
                                    PopupDragHandleControlID="PanelDetalle"
                                    OkControlID="btnOK"
                                    OnOkScript="onOK()"
                                    CancelControlID="btnCancel">
                                </cc1:ModalPopupExtender>
                                 </ContentTemplate>
                                </asp:UpdatePanel>


                                <asp:Button ID="BtfileInternix" runat="server" Text="Generar archivo Intermix" /><asp:Button ID="Btrechpagos" runat="server" Text="Rechazar pagos seleccionados" />
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
                    Copyright <a href="#">TVCable</a>. Todos los derechos reservados.
                </p>
            </div>
        </div>

    </form>

</body>
</html>
