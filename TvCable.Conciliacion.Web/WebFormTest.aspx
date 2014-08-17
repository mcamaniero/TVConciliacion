<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFormTest.aspx.cs" Inherits="TvCable.Conciliacion.Web.WebFormTest" %>

<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="Atk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

  <style type="text/css">
.tabla {
 width: 333px;
 border-top:1px solid #ccc;
 border-left:1px solid #ccc;
 background-color:#eee;
 color: gray;
 text-align:center;
 font-family:arial,verdana,times;
 font-size:12px;
 }
.tabla p {
 clear:both;
 width: 100%;
 margin: 0;
}

.tabla .titulo {
 padding: 5px;
 background-color: #ddd;
 font-family:arial,verdana,times;
 float:left;width:100px;
 border-right: 1px solid #ccc;
 font-weight:bold;
 }

.tabla .columna {
 padding: 5px;
 float:left;width:100px;
 border-right: 1px solid #ccc;
 border-bottom: 1px solid #ccc;
 }


      .auto-style1
      {
          height: 23px;
      }


  </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <asp:TextBox ID="txtFechaInspeccion" runat="server" Width="240px"></asp:TextBox>

            <Atk:CalendarExtender ID="defaultCalendarExtender" runat="server" TargetControlID="txtFechaInspeccion"
                Format="dd/MM/yyyy" />
            

            <div class="tabla">


<div class="columna">Monto </div><div class="columna">TUV MONTO</div><div class="columna">MDP MONTO</div>
<p><div class="columna"> </div><div class="columna">25</div><div class="columna">26</div></p>

<div class="columna">TUV FECHA </div><div class="columna">Fecha</div>
<p><div class="columna">MDP FECHA INICIO</div><div class="columna">datos2</div></p>
<p><div class="columna">MDP FECHA FIN</div><div class="columna">datos2</div></p>
 <p><div class="columna">ESTADO</div><div class="columna">datos2</div></p>
</div>







            <table style="width: 100%">
                                            <tr>
                                                <td class="auto-style1">
                                                    </td>
                                                <td class="auto-style1">TUV MONTO</td>
                                                    
                                                <td class="auto-style1">

                                                    MDP MONTO</td>
                                                    
                                            </tr>
                 <tr>
                                                <td class="auto-style1">
                                                    MONTO</td>
                                                <td class="auto-style1">25</td>
                                                    
                                                <td class="auto-style1">

                                                    23</td>
                                                    
                                            </tr>
                                            <tr>
                                                <td class="auto-style6">
                                                    <asp:Label ID="Label6" runat="server" Text="TUV FECHA" />
                                                </td>
                                                <td class="auto-style1">

                                                    <asp:Label ID="lblFecha" runat="server" Text="Label"></asp:Label>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style6">
                                                    <asp:Label ID="Label5" runat="server" Text="MDP FECHA INICIO" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblMonto" runat="server" Text="Label"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style6">
                                                    <asp:Label ID="Label7" runat="server" Text="MDP FECHA FIN:" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblUsuario" runat="server" Text="Label"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style8">
                                                    <asp:Label ID="Label8" runat="server" Text="USUARIO VENTA" />
                                                </td>
                                                <td class="auto-style9">
                                                    <asp:Label ID="lblpdp" runat="server" Text="Label"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style6">
                                                    <asp:Label ID="Label9" runat="server" Text="ESTADO" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTuves" runat="server" Text="Label"></asp:Label>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td class="auto-style6">
                                                   <asp:Label ID="Label1" runat="server" Text="ID TUV" /> </td>
                                                <td>
                                                    <asp:Label ID="lblEstado" runat="server" Text="Label"></asp:Label>
                                                </td>

                                            </tr>
                <tr>
                                                <td class="auto-style6">
                                                   <asp:Label ID="Label2" runat="server" Text="MDP ID" /> </td>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
                                                </td>

                                            </tr>
                                        </table>
    </form>
</body>
</html>
