using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using TvCable.Conciliacion.BusinessLayer;
using TvCable.Conciliacion.DTO;
using TvCable.Conciliacion.DTO.Entities;
using TvCable.Conciliacion.Libs;

namespace TvCable.Conciliacion.Web
{
    public partial class MainConciliacionWeb : System.Web.UI.Page
    {
        #region variables

        internal BusinessLayer.Conciliacion Conciliacion = new BusinessLayer.Conciliacion();
        internal BusinessLayer.Nucleo Nucleo = new BusinessLayer.Nucleo();
        internal TvCable.Conciliacion.Libs.Base Base = new Base();
        internal DataSet dscbFiltroConciliacion, dscbDataRecaudador, dscbEstadoConci, dtsEmpresaFac, dtsHitorial;
        internal int numeroItems;
        internal bool redirect;


        #endregion variables

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                try
                {
                    redirect = false;
                    var sessioCurrent = (EUsuario)Session["UserAutentication"];
                    if (sessioCurrent != null)
                    {
                        if (!string.IsNullOrEmpty(sessioCurrent.Usuario))
                        {
                            lblNombreUser.Text = Convert.ToString(sessioCurrent.Usuario);
                            RolFuncionalidad();
                            if (String.IsNullOrEmpty(TextBoxfechainicio.Text) == true || String.IsNullOrEmpty(TextBoxfechafin.Text) == true)
                            {
                                this.TextBoxfechainicio.Text = Convert.ToString(String.Format(Constants.formatoFechaText, DateTime.Now));
                                this.TextBoxfechafin.Text = Convert.ToString(String.Format(Constants.formatoFechaText, DateTime.Now));
                            }
                            else
                            {
                                this.TextBoxfechainicio.Text = Request.Form[TextBoxfechainicio.UniqueID];
                            }

                            this.CalendarFechaEmi.SelectedDate = DateTime.Now;
                            FillListRecaudador();
                            FillListTipoConciliacion();
                            FillListEmpresaFacturadora();

                            this.Btrechpagos.Visible = false;
                            if (LisdrodowConciliacion.Text.Equals(Constants.EstadoAprobado) && LblmensajeConsulta.Text.Equals(Resources.WebFormResource.msgNullDataGrill))
                            {
                                BtfileInternix.Visible = true;
                            }
                            else
                            {
                                BtfileInternix.Visible = false;
                            }
                        }
                        else
                        {
                            redirect = true;
                        }
                    }
                    else
                    {
                        redirect = true;

                    }
                }
                catch (Exception ex)
                {
                    Base.WriteLog(Base.ErrorTypeEnum.Error, 12, "Excepción en el  Load , mensaje: " + ex.Message + ". Excepción : " + ex.ToString());
                }


                if (redirect)
                    this.Response.Redirect(Constants.UrlLogin);
                
            }
        }

        /// <summary>
        /// Deshabilita los componentes web
        /// </summary>
        /// <param name="panel"></param>
        protected void DeshabilitarPorRol(string panel)
        {
            try
            {
                if (panel.Equals(Constants.GroSelc))
                {
                    BtCambioestado.Visible = false;
                    PanelEsado.Visible = false;
                    PanelDetalle.Height = Constants.formatoPopup;
                }
                else if (panel.Equals(Constants.GroInsert))
                {
                    GrupBoxInsertaIntermix.Visible = true;
                    BtCambioestado.Visible = true;
                    PanelEsado.Visible = true;

                    //  if (!String.IsNullOrEmpty(LisdrodowConciliacion.))                 {
                    if (LisdrodowConciliacion.SelectedItem.Text.Equals(Constants.EstadoAprobado))
                    { PanelDetalle.Height = Constants.formatoPopup; }
                    else
                    {
                        PanelDetalle.Height = Constants.formatoPopUp;
                    }
                    if (LisdrodowConciliacion.SelectedItem.Text.Equals(Constants.CodeIdRechazado))
                    {
                        PanelDetalle.Height = Constants.formatoPopup;
                    }

                    // }
                }
            }
            catch (Exception ex)
            {
                //Base.WriteLog(Base.ErrorTypeEnum.Information, 20, "Excepción al deshabilitar los componentes web, mensaje: " + ex.Message + ". Excepción:" + ex.ToString());
            }
        }

        /// <summary>
        /// Se verifica los roles del usuario para habilitar y deshabilitar las opciones del mismo
        /// </summary>
        protected EUsuario RolFuncionalidad()
        {
            EUsuario objCatalogo = new EUsuario();
            try
            {
                objCatalogo = (EUsuario)Session["UserAutentication"];
                var arrayconObj = objCatalogo.ItemFuncionalidad;
                if (arrayconObj.Length > 0)
                { //utilidad que me permite llenar el listado del formulario segun parametros de entrada
                    foreach (var array in arrayconObj)
                    {
                        DeshabilitarPorRol(array.Elemento);
                    }

                    return objCatalogo;
                }
                else
                {
                    Base.WriteLog(Base.ErrorTypeEnum.Information, 32, "El usuario:" + User + " no tiene privilegio alguno.");
                }
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 33, "Excepción al cargar las funcionalidades del usuario , mensaje: " + ex.Message + ". Excepción; " + ex.ToString());
                this.Response.Redirect(Constants.UrlLogin);
            }
            return objCatalogo;
        }

        /// <summary>
        /// Cerrar sesión
        /// </summary>
        public void Logout()
        {
            try
            {

                var usuario = RolFuncionalidad();
                Base.WriteLog(Base.ErrorTypeEnum.Stop, 3, "El usuario: " + usuario.Usuario + ", con el rol: " + usuario.Rol + " ha cerrado cesión");

                Session.Abandon();
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 270, "Excepción al cerrar la sesión . mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
            }


            Response.Cookies.Add(new HttpCookie("UserAuthentication", ""));
            HttpContext.Current.Response.Redirect(Constants.UrlLogin, true);


        }

        /// <summary>
        /// Metodo que carga el listado de  pagos
        /// </summary>
        protected void FillListEstadosConciliacion(string idconci)
        {
            try
            {
                //llamo al metodo que me devuelve un data que tiene como variable de entrada el cat_id
                dscbEstadoConci = Conciliacion.GetDataEstadoConciliacion(idconci);
                if (dscbEstadoConci.Tables.Count > 0)
                { //utilidad que me permite llenar el listado del formulario segun parametros de entrada
                    Util.LoadingDropDownList(this.LisdrodowEstado, Constants.CodeItemEstadoConci, Constants.CodeIdEstadoConci, dscbEstadoConci.Tables[0]);

                    Util.LoadingDropDownList(this.LisdrodowEstadovarios, Constants.CodeItemEstadoConci, Constants.CodeIdEstadoConci, dscbEstadoConci.Tables[0]);

                    if (dscbEstadoConci.Tables[0].Rows.Count == 0)
                    {
                        LisdrodowEstadovarios.Visible = false;
                        LisdrodowEstado.Visible = false;
                        BtCambioestado.Visible = false;
                        LblCambioestado.Visible = false;
                        Lblobser.Visible = false;
                        txtOservacion.Visible = false;
                    }
                    else
                    {
                        LisdrodowEstadovarios.Visible = true;
                        LisdrodowEstado.Visible = true;
                        BtCambioestado.Visible = true;
                        LblCambioestado.Visible = true;
                        Lblobser.Visible = true;
                        txtOservacion.Visible = true;
                    }
                }
                else
                {
                    LisdrodowEstadovarios.Visible = false;
                    LisdrodowEstado.Visible = false;
                    BtCambioestado.Visible = false;
                    LblCambioestado.Visible = false;
                    txtOservacion.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 46, "Excepción  de carga Combobox CU y CV. mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
        }

        /// <summary>
        /// Trae el historial como máxima fecha del registro correspondiente a id de conciliación
        /// </summary>
        /// <param name="idconciliacion">id conciliacion</param>
        /// <param name="idestado">id estado</param>
        protected void HistorialPorId(string idconciliacion, string idestado)
        {
            try
            {
                //llamo al metodo que me devuelve un data que tiene como variable de entrada el idconciliacion,idestado
                dtsHitorial = Conciliacion.GetDataHistorialPorId(idconciliacion, idestado);
                if (dtsHitorial.Tables.Count > -1)
                {
                    foreach (DataRow row in dtsHitorial.Tables[0].Rows)
                    {
                        this.txtOservacion.Text = Convert.ToString(row[Constants.hObservacion]);
                        this.lblObservacion.Text = Convert.ToString(row[Constants.hObservacion]);
                    }
                }
                else
                {
                    MensajeScript(Resources.WebFormResource.msgScriptErrorH, Constants.popupScript);
                }
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 53, "Excepción  al carga el historial:" + ex.Message + ". Excepción: " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo que carga el listado de recaudadores
        /// </summary>
        protected void FillListRecaudador()
        {
            try
            { //llamo al metodo que me devuelve un data que tiene la lista de recaudadores
                dscbDataRecaudador = Conciliacion.GetDataRecaudador();
                if (dscbDataRecaudador.Tables.Count > 0)
                {//utilidad que me permite llenar el listado del formulario segun parametros de entrada
                    Util.LoadingDropDownList(this.LisdrodowRecaudador, Constants.CodeItemRecaudador, Constants.CodeIdRecaudador, dscbDataRecaudador.Tables[0]);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 65, "Excepción al carga Combobox CR, mensaje de error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
        }

        /// <summary>
        /// Listar empresa facturadora
        /// </summary>
        protected void FillListEmpresaFacturadora()
        {
            try
            { //llamo al metodo que me devuelve un data que tiene la lista de recaudadores
                dtsEmpresaFac = Conciliacion.GetDataEmpresaFacturadora();

                if (dtsEmpresaFac.Tables.Count > 0)
                {
                    //utilidad que me permite llenar el listado del formulario segun parametros de entrada

                    Util.LoadingDropDownList(this.drdEmpresaFac, Constants.CodeItemEmpresaFacturadora, Constants.CodeIdEmpresaFacturadora, dtsEmpresaFac.Tables[0]);
                }
                else
                {
                    Base.WriteLog(Base.ErrorTypeEnum.Information, 74, "No existe Empresas Facturadoras,Combobox CEF.");
                }
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 75, "Excepción al carga Combobox CEF, mensaje de error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo que carga el listado tipo de conciliacion
        /// </summary>
        protected void FillListTipoConciliacion()
        {
            try
            {
                var objCatalogo = Nucleo.GetCatalogoPorCodigoCatalogo(Constants.CodeEstadoConciliacion);
                var arrayconObj = objCatalogo.ItemCatalogo;
                if (arrayconObj.Length > 0)
                { //utilidad que me permite llenar el listado del formulario segun parametros de entrada
                    Util.LoadingDropDownList(this.LisdrodowConciliacion, Constants.CodeItemCatalogo, Constants.CodeIdCatalogo, arrayconObj);
                }
                else
                {
                    Base.WriteLog(Base.ErrorTypeEnum.Information, 84, "No existe Tipos de Conciliación ,Combobox CTC.");
                }
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 85, "Excepción al carga Combobox CTC, mensaje de error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
        }

        /// <summary>
        /// Verifica si por lo menos esta seleccionado algún ítem
        /// </summary>
        /// <returns>Retorna verdadero o falso</returns>
        protected bool verificacionGread()
        {
            numeroItems = 0;
            bool estadoselect = false;
            try
            {
                foreach (GridViewRow row in this.GridViewConciliacion.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                        if (chkRow.Checked)
                        {
                            numeroItems++;
                            estadoselect = true;
                            //return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 90, "Excepción al seleccionar ítem de la grilla, mensaje de error:" + ex.Message + ". Excepción: " + ex.ToString());
            }

            return estadoselect;
        }

        /// <summary>
        /// Método que permite llenar la entidad EDetallePagoConciliacion con datos del dataset de consulta.
        /// </summary>
        /// <returns>Entidad Pago Conciliación</returns>
        protected EDetallePagoConciliacion ReferenciaObjGrid()
        {
            EDetallePagoConciliacion objDetalle = new EDetallePagoConciliacion();
            GridViewRow rowGr = GridViewConciliacion.SelectedRow;
            try
            {
                var dataConsult = FillGridView(false);
                foreach (DataRow renglon in dataConsult.Tables[0].Rows)
                {
                    if (Convert.ToString(renglon["RCO_ID"]) == rowGr.Cells[3].Text)
                    {
                        objDetalle.Id = Convert.ToString(renglon["RCO_ID"]);
                        objDetalle.MontoTUV = Convert.ToString(renglon["TUV_MONTO"]);
                        objDetalle.MontoMdp = Convert.ToString(renglon["MDP_MONTO"]);
                        objDetalle.FechaTuv = Convert.ToString(renglon["TUV_FECHA"]);
                        objDetalle.FechainiMdp = Convert.ToString(renglon["MDP_FECHA_INI_TRX"]);
                        objDetalle.UsuarioVentaTuv = Convert.ToString(renglon["TUV_CLIENTE"]);
                        objDetalle.UsuarioVentaMpd = Convert.ToString(renglon["MDP_CLIENTE_ID"]);
                        objDetalle.IdTuv = Convert.ToString(renglon["TUV_TRX_ID"]);
                        objDetalle.IdMdp = Convert.ToString(renglon["MDP_ID_TRX_MDP"]);
                        break;
                    }
                    else
                    {
                        txtOservacion.Text = "";
                        lblObservacion.Text = "";
                    }
                }

                //Llenado los label con información guardada en el entidad

                if (String.IsNullOrEmpty(Constants.signodolar))
                {
                    this.lblMontoTUV.Text = "-";
                    this.lblMontoTUV.Text = Constants.signodolar + objDetalle.MontoTUV;
                }
                else
                {
                    this.lblMontoTUV.Text = Constants.signodolar + objDetalle.MontoTUV;
                }
                if (String.IsNullOrEmpty(Constants.signodolar))
                {
                    this.lblMontoMdp.Text = "-";
                    this.lblMontoMdp.Text = Constants.signodolar + objDetalle.MontoMdp;
                }
                else
                {
                    this.lblMontoMdp.Text = Constants.signodolar + objDetalle.MontoMdp;
                }

                string fechaTuv = objDetalle.FechaTuv.Remove(10, 8);
                string fechaMdp = objDetalle.FechainiMdp.Remove(10, 8);

                if (String.IsNullOrEmpty(fechaTuv))
                {
                    fechaTuv = "-";
                    this.lblFechaTuv.Text = fechaTuv;
                }
                else
                {
                    this.lblFechaTuv.Text = fechaTuv;
                }
                if (String.IsNullOrEmpty(fechaMdp))
                {
                    fechaMdp = "-";
                    this.lblFechaMdp.Text = fechaMdp;
                }
                else
                {
                    this.lblFechaMdp.Text = fechaMdp;
                }
                if (String.IsNullOrEmpty(objDetalle.UsuarioVentaTuv))
                {
                    objDetalle.UsuarioVentaTuv = "-";
                    this.lblUsuarioTUV.Text = objDetalle.UsuarioVentaTuv;
                }
                else
                {
                    this.lblUsuarioTUV.Text = objDetalle.UsuarioVentaTuv;
                }
                if (String.IsNullOrEmpty(objDetalle.UsuarioVentaMpd))
                {
                    objDetalle.UsuarioVentaMpd = "-";
                    this.lblUsuarioMpd.Text = objDetalle.UsuarioVentaMpd;
                }
                else
                {
                    this.lblUsuarioMpd.Text = objDetalle.UsuarioVentaMpd;
                }
                if (String.IsNullOrEmpty(objDetalle.IdTuv))
                {
                    objDetalle.IdTuv = "-";
                    this.lblIdTuv.Text = objDetalle.IdTuv;
                }
                else
                {
                    this.lblIdTuv.Text = objDetalle.IdTuv;
                }
                if (String.IsNullOrEmpty(objDetalle.IdMdp))
                {
                    objDetalle.IdMdp = "-";
                    this.lblIdMdp.Text = objDetalle.IdMdp;
                }
                else
                {
                    this.lblIdMdp.Text = objDetalle.IdMdp;
                }

                FillListEstadosConciliacion(LisdrodowConciliacion.SelectedValue);
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 102, "Excepción al carga de los del ítem  PDE , mensaje de error:" + ex.Message + ". Excepción: " + ex.ToString());
                MensajeScript(Resources.WebFormResource.msgNullItem, Constants.popupScript);
            }
            return objDetalle;
        }

        /// <summary>
        /// Método propio del GridViewConciliacion   que permite cargar la entidad EDetallePagoConciliacion con datos seleccionados por el mismo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewConciliacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtOservacion.Text = "";
                lblObservacion.Text = "";
                ReferenciaObjGrid();
                HistorialPorId(GridViewConciliacion.SelectedRow.Cells[3].Text, LisdrodowConciliacion.SelectedValue);
                lblEstadodetalle.Text = LisdrodowConciliacion.SelectedItem.Text;
                modalPoExtenderDetalle.Show();
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 110, "Excepción al seleccionar ítem de la grilla,mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
        }

        /// <summary>
        /// paginacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridViewConciliacion.PageIndex = e.NewPageIndex;
                FillGridView(false);
                GridViewConciliacion.DataBind();
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 270, "Excepción al paginar,mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
        }

        /// <summary>
        /// Método propio del botón que permite traer el método llenado del Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtbuscarPagos_Click(object sender, EventArgs e)
        {
            try
            {
                //Se valida si no tiene activo el botón de rechazado para que no concuerde con el listado de tipo de conciliación
                if (LisdrodowConciliacion.SelectedItem.Text.Equals(Constants.CodeIdRechazado))
                {
                    PanelDetalle.Height = Constants.formatoPopup;
                    BtfileInternix.Visible = false;
                    Btrechpagos.Visible = false;
                    FillGridView(true);
                }
                else
                {
                    Btrechpagos.Visible = true;
                    FillGridView(true);
                }
                if (LisdrodowConciliacion.SelectedItem.Text.Equals(Constants.EstadoAprobado))
                {
                    PanelDetalle.Height = Constants.formatoPopUp;
                    var refer = FillGridView(false);
                    if (dscbFiltroConciliacion.Tables.Count < 0)
                    {
                        Btrechpagos.Visible = false;
                        BtfileInternix.Visible = true;
                    }
                    else
                    {
                        Btrechpagos.Visible = false;
                        BtfileInternix.Visible = true;
                    }
                    if (refer.Tables[0].Rows.Count == 0)
                    {
                        BtfileInternix.Visible = false;
                    }
                }
                else
                {
                    BtfileInternix.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 121, "Excepción con el botón de búsqueda BBP, mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
        }

        /// <summary>
        /// Método que permite enviar la búsqueda al método que me devuelve un dataset y alimentar el  GridViewConciliacion
        /// </summary>
        protected DataSet FillGridView(bool consulta)
        {
            try
            {
                var dateIni = Util.ConvertStringToDateTime(this.TextBoxfechainicio.Text.Trim());
                var dateFin = Util.ConvertStringToDateTime(this.TextBoxfechafin.Text.Trim());
                Base.WriteLog(Base.ErrorTypeEnum.Information, 122, "Obteniendo el detalle de pagos: Recaudador: " + Convert.ToInt32(LisdrodowRecaudador.SelectedValue) + ". Fecha Inicio: " + dateIni + ". Fecha Fin: " + dateFin + ". Tipo de conciliacion: " + Convert.ToInt32(LisdrodowConciliacion.SelectedValue));

                dscbFiltroConciliacion = Conciliacion.GetDataConciliacion(Convert.ToInt32(LisdrodowRecaudador.SelectedValue),
                    dateIni,
                    dateFin,
                   Convert.ToInt32(LisdrodowConciliacion.SelectedValue));

                if (dscbFiltroConciliacion.Tables.Count > 0)
                {
                    
                    this.GridViewConciliacion.DataSource = dscbFiltroConciliacion.Tables[0].DefaultView;
                    this.GridViewConciliacion.DataBind();
                    var usuario = RolFuncionalidad();

                    if (consulta == true)
                    {
                        Base.WriteLog(Base.ErrorTypeEnum.Information, 132, "El usuario :" + usuario.Usuario + " con rol :" + usuario.Rol + " ha realizado la siguiente consulta: Empresa Facturadora: " + this.LisdrodowRecaudador.SelectedItem + ", Fecha Inicio: " + this.TextBoxfechainicio.Text + ", Fecha Fin: " + this.TextBoxfechafin.Text + ", Estado de Pago:" + LisdrodowConciliacion.SelectedItem);
                    }

                    if (dscbFiltroConciliacion.Tables[0].Rows.Count == 0)
                    {
                        LblmensajeConsulta.Text = Resources.WebFormResource.msgNullDataGrill;
                        this.Btrechpagos.Visible = false;
                        this.BtfileInternix.Visible = false;
                    }
                    else
                    {
                        LblmensajeConsulta.Text = null;
                    }
                }
                else
                {
                    Base.WriteLog(Base.ErrorTypeEnum.Information, 133, "No se ha encontrado resultado alguno para la siguiente búsqueda: Fecha Inicio: " + dateIni + ". Fecha fin: " + dateFin + ". Recaudador:" + LisdrodowRecaudador.SelectedItem + ". Estado pago: " + LisdrodowConciliacion.SelectedItem + " resultado :" + dscbEstadoConci.GetXml());
                    this.Btrechpagos.Visible = false;
                    this.BtfileInternix.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MensajeScript(Resources.WebFormResource.msgNullGrilla, Constants.popupScript);
                Base.WriteLog(Base.ErrorTypeEnum.Error, 134, "Excepción  Al realizar la búsqueda, mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
            return dscbFiltroConciliacion;
        }

        /// <summary>
        /// Método que  me permite seleccionar todos los ítems por medio del check genral
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox ChkBoxHeader = (CheckBox)GridViewConciliacion.HeaderRow.FindControl("chkboxSelectAll");
                foreach (GridViewRow row in GridViewConciliacion.Rows)
                {
                    CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");
                    if (ChkBoxHeader.Checked == true)
                    {
                        ChkBoxRows.Checked = true;
                    }
                    else
                    {
                        ChkBoxRows.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MensajeScript(Resources.WebFormResource.msgNullItemError, Constants.popupScript);
                Base.WriteLog(Base.ErrorTypeEnum.Error, 140, "Problemas al seleccionar todos los ítems a la vez, mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
        }

        /// <summary>
        /// Método que actualiza el estado pagos
        /// </summary>
        protected void CambioEstado(bool valorar)
        {
            if (valorar.Equals(false))
            {
                try
                {
                    Base.WriteLog(Base.ErrorTypeEnum.Start, 150, "Inicia el proceso de actualización estado conciliación con un solo ítem.");
                    string idcon = GridViewConciliacion.SelectedRow.Cells[3].Text;
                    int i = Conciliacion.UpdateEstado(Convert.ToInt32(GridViewConciliacion.SelectedRow.Cells[3].Text), Convert.ToInt32(LisdrodowEstado.SelectedValue));

                    if (i == -1)
                    {
                        MensajeScript(Resources.WebFormResource.msgScriptOk, Constants.popupScript);
                        var usuario = RolFuncionalidad();
                        Base.WriteLog(Base.ErrorTypeEnum.Stop, 151, "El usuario:" + usuario.Usuario + " ,con rol:" + usuario.Rol + " a modificado la siguiente conciliación ,ID RESULTADO CONCILIACION:" + idcon + ", Estado Anterior: " + LisdrodowConciliacion.SelectedItem + ", Estado Actual: " + LisdrodowEstado.SelectedItem + ", OBSERVACION:" + this.txtOservacion.Text);
                        int j = InsertHistorial(this.GridViewConciliacion.SelectedRow.Cells[3].Text, (this.LisdrodowEstado.SelectedValue), this.txtOservacion.Text);
                        if (j < 0)
                        {
                            MensajeScript(Resources.WebFormResource.msgInsertHistorialKo, Constants.popupScript);
                        }
                        else
                        {
                            Base.WriteLog(Base.ErrorTypeEnum.Information, 171, "Se realizó el registro de este cambio de estado en la tabla de: HISTORIAL_ESTADO, ID generado del registro al historial: ID:" + j);
                            FillGridView(false);
                            this.txtOservacion.Text = "";
                            this.lblObservacion.Text = "";
                        }

                    }
                    else
                    {
                        MensajeScript(Resources.WebFormResource.msgScriptError, Constants.popupScript);

                        this.txtOservacion.Text = "";
                        lblObservacion.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    Base.WriteLog(Base.ErrorTypeEnum.Error, 152, "Excepción  al actualizar  el estado de conciliación. mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
                }
            }
            else
            {
                try
                {
                    int update = 0;
                    Base.WriteLog(Base.ErrorTypeEnum.Start, 160, "Inicia el proceso de actualización de estados de conciliación con varios ítems.");
                    var usuario = RolFuncionalidad();
                    foreach (GridViewRow row in this.GridViewConciliacion.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                            if (chkRow.Checked)
                            {
                                int i = Conciliacion.UpdateEstado(Convert.ToInt32(row.Cells[3].Text), Convert.ToInt32(LisdrodowEstadovarios.SelectedValue));
                                if (i == -1)
                                {
                                    Base.WriteLog(Base.ErrorTypeEnum.Stop, 161, "El usuario :" + usuario.Usuario + " con rol :" + usuario.Rol + " a modificado la siguiente conciliación :" + row.Cells[3].Text + ", Estado Anterior: " + LisdrodowConciliacion.SelectedItem + ", Estado Actual: " + LisdrodowEstadovarios.SelectedItem);
                                    int j = InsertHistorial(row.Cells[3].Text, (this.LisdrodowEstadovarios.SelectedValue), this.txtOservacionvarios.Text);


                                    if (j < 0)
                                    {
                                        MensajeScript(Resources.WebFormResource.msgInsertHistorialKo, Constants.popupScript);
                                    }
                                    else
                                    {
                                        Base.WriteLog(Base.ErrorTypeEnum.Information, 171, "Se realizó el registro de este cambio de estado en la tabla de: HISTORIAL_ESTADO, ID generado del registro al historial: ID:" + j);
                                        FillGridView(false);
                                        this.txtOservacion.Text = "";
                                        this.lblObservacion.Text = "";
                                        update++;
                                    }
                                }
                                else
                                {
                                    this.txtOservacion.Text = "";
                                    this.lblObservacion.Text = "";
                                }

                                FillGridView(false);
                            }
                        }
                        else
                        {
                            MensajeScript(Resources.WebFormResource.msgNullItem, Constants.popupScript);
                        }
                    }

                    if (update > 0)
                    {
                        MensajeScript(update + Resources.WebFormResource.msgUpdateconci, Constants.popupScript);
                    }
                }
                catch (Exception ex)
                {
                    Base.WriteLog(Base.ErrorTypeEnum.Error, 162, "Excepción  al actualizar  el estado de conciliación con varios ítems. mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// Método que inserta al historial
        /// </summary>
        protected int InsertHistorial(string idconciliacion, string idactualizado, string observacion)
        {
            int reghisto = -1;
            try
            {
                Base.WriteLog(Base.ErrorTypeEnum.Start, 170, "Inicio de inserción de historial ");
                var usuario = RolFuncionalidad();

                reghisto = Conciliacion.InsertHistorialEstado(Convert.ToInt32(idconciliacion), Convert.ToInt32(idactualizado), DateTime.Now, usuario.Rol, usuario.Usuario, observacion);
                if (reghisto > -1)
                {
                    return reghisto;
                }
                else
                {

                    Base.WriteLog(Base.ErrorTypeEnum.Information, 172, "No se ha insertado ningún dato al historial ");

                }

                FillGridView(false);
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 173, "Excepción  al insertar en historial  . mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
            return reghisto;
        }

        /// <summary>
        /// El botón hace la llama al método que actualiza el estado de los pagos
        /// </summary>
        /// <param name="sender">evento boton</param>
        /// <param name="e"></param>
        protected void BtCambioestado_Click(object sender, EventArgs e)
        {
            try
            {
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    CambioEstado(false);
                }
                else
                {
                    MensajeScript(Resources.WebFormResource.mdgNoafectado, Constants.popupScript);
                }
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 180, "Excepción en el botón cambio de estado , BtCambioestado , mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
        }

        /// <summary>
        /// El botón hace la llama la método que actualiza a rechazados
        /// </summary>
        /// <param name="sender">evento boton</param>
        /// <param name="e"></param>
        protected void Btrechpagos_Click(object sender, EventArgs e)
        {
            try
            {
                if (verificacionGread().Equals(true))
                {
                    this.txtOservacionvarios.Text = "";
                    this.lblObservacion.Text = "";
                    this.lblEstadoVarios.Text = LisdrodowConciliacion.SelectedItem.Text;
                    this.lblNumItems.Text = Resources.WebFormResource.msgnumItems + Convert.ToString(numeroItems);
                    FillListEstadosConciliacion(LisdrodowConciliacion.SelectedValue);
                    this.ModalPopupExtendervarios.Show();
                }
                else
                {
                    MensajeScript(Resources.WebFormResource.msgNullItem, Constants.popupScript);
                }
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 190, "Excepción en el botón cambio de estado varios ítems, Btrechpagos , mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
        }

        /// <summary>
        /// Método del botón que me trae la lista de empresas facturadoras
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtfileInternix_Click(object sender, EventArgs e)
        {
            try
            {
                this.TxtObervacionIntermix.Text = null;
                if (verificacionGread().Equals(true))
                {
                    numItemsIterx.Text = Resources.WebFormResource.msgnumItems + Convert.ToString(numeroItems);
                    this.ModalPopupExtenderIntermix.Show();
                }
                else
                {
                    MensajeScript(Resources.WebFormResource.msgNullItem, Constants.popupScript);
                }
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 200, "Excepción en el botón Inserción  Intermix, BtfileInternix , mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
        }

        /// <summary>
        /// Mensaje Script
        /// </summary>
        /// <param name="mensaje"></param>
        /// <param name="tipo"></param>
        protected void MensajeScript(string mensaje, string tipo)
        {
            try
            {
                if (!IsClientScriptBlockRegistered(tipo))
                {
                    String cstext1 = "<script type=\"text/javascript\">" +
                "alert('" + mensaje + "'); </" + "script>";
                    RegisterStartupScript(tipo, cstext1);
                }
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 210, "Excepción en la ventana java script tipo mensaje , mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
        }

        /// <summary>
        /// Método que Inserta a intermix
        /// </summary>
        protected void InsertIntermix()
        {
            int registros = 0;
            //Desde este try se procede a realizar los caculos de iva ice
            decimal valorTotal = 0, valorIva = 0, baseIva = 0, valorIce = 0, baseIce = 0, valorNeto = 0;
            List<int> items = new List<int>();
            try
            {
                Base.WriteLog(Base.ErrorTypeEnum.Start, 220, "Inicio de inserción a Intermix una vez aprobado.");
                int i = 0;

                Util calculos = new Util();
                foreach (GridViewRow row in this.GridViewConciliacion.Rows)
                {
                    //Base.WriteLog(Base.ErrorTypeEnum.Trace, 231, "-Foreach");
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                       //Base.WriteLog(Base.ErrorTypeEnum.Trace, 232, "-chkRow: " + row.Cells[0]);

                        CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                        if (chkRow.Checked)
                        {
                            //Base.WriteLog(Base.ErrorTypeEnum.Trace, 224, "Calculando el valor Total: " + row.Cells[4].Text);
                            valorTotal = valorTotal + Util.ConvertStringToDecimal(row.Cells[4].Text);

                            //Base.WriteLog(Base.ErrorTypeEnum.Trace, 230, "Valor Item: " + row.Cells[3].Text);
                            items.Add(Convert.ToInt32(row.Cells[3].Text));
                            i++;
                            registros++;
                        }
                    }
                    else
                    {
                        MensajeScript(Resources.WebFormResource.msgSumaError, Constants.popupScript);
                    }
                }
                
                baseIva = calculos.BaseIva(valorTotal);
                //Base.WriteLog(Base.ErrorTypeEnum.Trace, 225, "Calculando BaseIva: " + baseIva);
                
                valorIva = calculos.ValorIva(baseIva);
                //Base.WriteLog(Base.ErrorTypeEnum.Trace, 226, "Calculando ValorIva: " + valorIva);

                valorNeto = calculos.ValorNeto(baseIva);
                //Base.WriteLog(Base.ErrorTypeEnum.Trace, 227, "Calculando ValorNeto: " + valorNeto);

                baseIce = calculos.BaseIce(baseIva);
                //Base.WriteLog(Base.ErrorTypeEnum.Trace, 228, "Calculando BaceIce: " + baseIce);

                valorIce = calculos.ValorIce(baseIce);
                //Base.WriteLog(Base.ErrorTypeEnum.Trace, 229, "Calculando ValorIce: " + valorIce);
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 221, "Excepción  no sé a podido hacer los cálculos, mensaje error:" + ex.Message + ". Excepcion: " + ex.ToString());
            }

            try
            {
                EIntermix objintermix = new EIntermix();
                //Se carga a las entidades con los valores de Empresa Facturadora y Recaudador
                var empresaFacId = Conciliacion.GetDataEmpresaFacturadoraId(drdEmpresaFac.SelectedValue);
                var recaudador = Conciliacion.GetDataReacaudadorId(LisdrodowRecaudador.SelectedValue);
                if (empresaFacId.Tables.Count > 0 && recaudador.Tables.Count > 0)
                {
                    var idconcilrec = Conciliacion.ObtenerSecuencialIntermix();
                    objintermix.IDCONCILREC = idconcilrec;
                    foreach (DataRow row in empresaFacId.Tables[0].Rows)
                    {
                        objintermix.COD_EMPRESA = (int)(row[Constants.codempresa]);
                        objintermix.COD_PUNTOEMISION = Convert.ToString(row[Constants.codpuntoemision]);
                    }
                    foreach (DataRow row in recaudador.Tables[0].Rows)
                    {
                        objintermix.RUC = Convert.ToString(row[Constants.ruc]);
                        objintermix.TIPO_CLIENTE = (int)(row[Constants.tipocliente]);
                        objintermix.TELEFONO = Convert.ToString(row[Constants.telefono]);
                        objintermix.TIPO_PERSONA = (int)(row[Constants.tipopersona]);
                        objintermix.TIPO_CONTRIBUYENTE = (int)(row[Constants.tipocontribuyente]);
                        objintermix.NOMBRE = Convert.ToString(row[Constants.nombre]);
                        objintermix.DIRECCION = Convert.ToString(row[Constants.direccion]);
                        objintermix.EMAIL = Convert.ToString(row[Constants.email]);
                        objintermix.COD_ITEM = Convert.ToString(row[Constants.codservicio]);
                    }

                    int i = Conciliacion.InsertIntermix(
                        objintermix.IDCONCILREC,
                        objintermix.COD_EMPRESA,
                            objintermix.COD_PUNTOEMISION,
                        objintermix.RUC,
                        // 0,
                        objintermix.TIPO_CLIENTE,
                        objintermix.TELEFONO,
                        objintermix.TIPO_PERSONA,
                        objintermix.TIPO_CONTRIBUYENTE,
                        objintermix.NOMBRE,
                        objintermix.DIRECCION,
                        objintermix.EMAIL,
                        objintermix.COD_ITEM,
                        valorNeto,
                        baseIce,
                        valorIce,
                        baseIva,
                        valorIva,
                        valorTotal,
                        DateTime.Now,
                        Util.ConvertStringToDateTime(txtFechaEmision.Text),
                        Util.ConvertStringToDateTime(TextBoxfechainicio.Text),
                        Util.ConvertStringToDateTime(TextBoxfechafin.Text),
                        //"0",
                        this.TxtObervacionIntermix.Text,
                        Constants.facturado
                        );
                    var usuario = RolFuncionalidad();
                    Base.WriteLog(Base.ErrorTypeEnum.Information, 222, "El usuario:" + usuario.Usuario + " con rol:" + usuario.Rol + " ha realizado el envío e inserción a Intermix del siguiente registro: ( Se enviaron " + registros + " registros por un total de:" + valorTotal +
                            " Codigo de Empresa:" + objintermix.COD_EMPRESA + " ," +
                            " RUC Proveedor:" + objintermix.RUC + " ," +
                            " Telefono Proveedor:" + objintermix.TELEFONO + " ," +
                            " Nombre Proveedor:" + objintermix.NOMBRE + " ," +
                            " Direccion Proveedor:" + objintermix.DIRECCION + " ," +
                            " Email Proveedor:" + objintermix.EMAIL + " " +
                            " Valor Neto:" + valorNeto + " ," +
                            " Bace ICE:" + baseIce + " ," +
                           " Valor ICE:" + valorIce + " ," +
                            " Base IVA:" + baseIva + " ," +
                            " Valor IVA:" + valorIva + " ," +
                            " Valor Total:" + valorTotal + " ," +
                            " Fecha Emision:" + txtFechaEmision.Text + ", " +
                            " Fecha Inicio:" + TextBoxfechainicio.Text + " ," +
                           " Fecha Fin:" + TextBoxfechafin.Text + ", " +
                           TxtObervacionIntermix.Text + ")");
                    if (i == -1)
                    {
                        //Se procede a actualizar los ítems seleccionados una vez que se ha verificado que se ha insertado en Internix
                        items.ForEach(delegate(int idcon)
                        {
                            int result = Conciliacion.UpdateConciliacionPorIntermix(idcon, Convert.ToString(objintermix.IDCONCILREC));
                            int resultHisto = Conciliacion.InsertHistorialIntermix(idcon, objintermix.IDCONCILREC, DateTime.Now, usuario.Usuario, usuario.Rol, TxtObervacionIntermix.Text);

                            if (result == -1)
                            {

                                FillGridView(false);
                            }
                            else
                            {
                                MensajeScript(Resources.WebFormResource.msgUpdateConciliacionKo, Constants.popupScript);
                            }
                            if (resultHisto > -1)
                            {
                                FillGridView(false);
                            }
                            else
                            {
                                MensajeScript(Resources.WebFormResource.msgInsertHisIntermixKo, Constants.popupScript);
                            }
                        });
                        FillGridView(false);
                        MensajeScript(Resources.WebFormResource.msgInsertInterxOk, Constants.popupScript);
                    }
                    else
                    {
                        MensajeScript(Resources.WebFormResource.msginsertInterxKo, Constants.popupScript);
                    }
                }
                else
                {
                    MensajeScript(Resources.WebFormResource.msgInsertInterxNull, Constants.popupScript);
                }
            }
            catch (Exception ex)
            {
                MensajeScript(Resources.WebFormResource.msgIntermixKo, Constants.popupScript);
                Base.WriteLog(Base.ErrorTypeEnum.Error, 223, "Excepción  al insertar en Intermix . mensaje error:" + ex.Message + ". Excepcion: " + ex.ToString());
            }
        }

        /// <summary>
        /// Método que me permite capturar todos los datos de Internix prepararlo e instar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btInsertarIntermix_Click(object sender, EventArgs e)
        {
            try
            {
                string confirmValue = Request.Form["confirm_valueitermix"];
                if (confirmValue == "Yes")
                {
                    InsertIntermix();
                }
                else
                {
                    MensajeScript(Resources.WebFormResource.mdgNoafectado, Constants.popupScript);
                }
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 230, "Excepción en el botón Inserción  Intermix, btInsertarIntermix , mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
        }

        /// <summary>
        /// Botón que hace la llamada para cambiar el estado varios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtCambioestadovarios_Click(object sender, EventArgs e)
        {
            try
            {
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    CambioEstado(true);
                }
                else
                {
                    MensajeScript(Resources.WebFormResource.mdgNoafectado, Constants.popupScript);
                }
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 240, "Excepción en el botón cambiar estado varios, BtCambioestadovarios , mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
        }

        /// <summary>
        /// Link que cierra la sesión del usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void linkBCerarSesion_Click(object sender, EventArgs e)
        {
            Logout();
        }

        /// <summary>
        /// Método  que permite confirma o no si desea hacer cambios se utiliza para cada botón que se quiera hacer cambios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnConfirm(object sender, EventArgs e)
        {
            try
            {
                string confirmValue = Request.Form["confirm_value"];
                string confirmValueIntermix = Request.Form["confirm_valueitermix"];
                if (confirmValue == "Yes")
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
                }

                if (confirmValueIntermix == "Yes")
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
                }


            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 260, "Excepción en el método que confirmación de actualización , linkBCerarSesion , mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
        }

        /// <summary>
        /// Numero de ítems a mostrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LisdrodowItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewConciliacion.PageSize = int.Parse(((DropDownList)sender).SelectedValue);
                GridViewConciliacion.DataSource = FillGridView(false).Tables[0];
                GridViewConciliacion.DataBind();
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 280, "Excepción al  cambiar el número de ítems  a mostrar , LisdrodowItems , mensaje error:" + ex.Message + ". Excepción: " + ex.ToString());
            }
        }
    }
}