using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TvCable.Conciliacion.DTO;
using TvCable.Conciliacion.Libs;
using TvCable.Conciliacion.BusinessLayer;
namespace TvCable.Conciliacion.Web
{
    public partial class MainConciliacionweb : System.Web.UI.Page
    {
        #region variables
        internal BusinessLayer.Conciliacion Conciliacion = new BusinessLayer.Conciliacion();
        internal BusinessLayer.Nucleo Nucleo = new BusinessLayer.Nucleo();
        internal DataSet dscbFiltroConciliacion, dscbDataRecaudador, dscbEstadoPago, dscbEstadoConciliacion, dscbTipoConciliacion;
        internal string reqtipoConciliacion;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                FillListEstadoPago();
                FillListRecaudador();
                FillListTipoConciliacion();
            }

        }
        /// <summary>
        /// Llenado de la grilla según los parámetros de filtro que se le envíen
        /// </summary>
        /// <param name="recaudador">id del recaudador</param>
        /// <param name="fechainicio">fecha inicio de busqueda</param>
        /// <param name="fechafin">fecha fin de busqueda</param>
        /// <param name="tipoconciliacion">id tipo de conciliacion</param>
        protected void FillGreadViewConciliacion(string recaudador, string fechainicio, string fechafin, string tipoconciliacion)
        {
            if (String.IsNullOrEmpty(reqtipoConciliacion))
            {
                dscbFiltroConciliacion = Conciliacion.GetDataConciliacion(null, null, null, tipoconciliacion);
                if (dscbFiltroConciliacion.Tables.Count > 0)
                {
                    this.GridViewConciliacion.DataSource = dscbFiltroConciliacion.Tables[0].DefaultView;
                    this.GridViewConciliacion.DataBind();
                }
            }
            else if (String.IsNullOrEmpty(recaudador) && String.IsNullOrEmpty(fechainicio) && String.IsNullOrEmpty(fechafin) && String.IsNullOrEmpty(tipoconciliacion))
            {
                Conciliacion.GetDataConciliacion(recaudador, fechainicio, fechafin, tipoconciliacion);
                if (dscbFiltroConciliacion.Tables.Count > 0)
                {
                    this.GridViewConciliacion.DataSource = dscbFiltroConciliacion.Tables[0].DefaultView;
                    this.GridViewConciliacion.DataBind();
                }
            }

        }


        /// <summary>
        /// Metodo que carga el listado de  pagos
        /// </summary>
        protected void FillListEstadoPago()
        {
            try
            {
                //llamo al metodo que me devuelve un data que tiene como variable de entrada el cat_id
                dscbEstadoPago = Conciliacion.GetDataEstadoPago(Constants.CodeEstadoPago);

                if (dscbEstadoPago.Tables.Count > 0)
                { //utilidad que me permite llenar el listado del formulario segun parametros de entrada 
                    Util.LoadingDropDownList(this.LisdrodowEstado, Constants.CodeItemEstadoPago, Constants.CodeIdEstadoPago, dscbEstadoPago.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("No sé ha podido llenar Estado pago error:" + ex);
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
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("No sé ha podido llenar Recaudador error:" + ex);
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
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("No sé ha podido llenar Conciliacion error:" + ex);
            }
        }

        /// <summary>
        /// Método propio del GridViewConciliacion   que permite cargar la entidad EDetallePagoConciliacion con datos seleccionados por el mismo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewConciliacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            var objSelec = Conciliacion.ObtieneDetallePagoConciliacion(
                GridViewConciliacion.SelectedRow.Cells[3].Text,
                GridViewConciliacion.SelectedRow.Cells[4].Text,
                GridViewConciliacion.SelectedRow.Cells[5].Text,
                GridViewConciliacion.SelectedRow.Cells[6].Text,
                GridViewConciliacion.SelectedRow.Cells[7].Text,
                GridViewConciliacion.SelectedRow.Cells[8].Text,
                GridViewConciliacion.SelectedRow.Cells[9].Text);
            //Llenado los label con información guardada en el entidad
            lblId.Text = objSelec.IdConciliacion;
            lblFecha.Text = objSelec.FechaConciliacion;
            lblMonto.Text = objSelec.MontoConciliacion;
            lblUsuario.Text = objSelec.UsuarioConciliacion;
            lblpdp.Text = objSelec.IdtranpdpConciliacion;
            lblTuves.Text = objSelec.IdtrantuvesConciliacion;
            lblEstado.Text = objSelec.EstadoConciliacion;

        }
        /// <summary>
        /// Método propio del botón que permite enviar la búsqueda al método que me devuelve un dataset y alimentar el  GridViewConciliacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtbuscarPagos_Click(object sender, EventArgs e)
        {
            try
            {
                dscbFiltroConciliacion = Conciliacion.GetDataConciliacion(this.LisdrodowRecaudador.SelectedValue,
                    this.TextBoxfechainicio.Text,
                    this.TextBoxfechafin.Text,
                    this.LisdrodowConciliacion.SelectedValue);
                if (dscbFiltroConciliacion.Tables.Count > 0)
                {
                    dscbFiltroConciliacion.Tables[0].Columns[0].ColumnName = "ID";
                    dscbFiltroConciliacion.Tables[0].Columns[1].ColumnName = "Fecha";
                    dscbFiltroConciliacion.Tables[0].Columns[2].ColumnName = "Monto";
                    dscbFiltroConciliacion.Tables[0].Columns[3].ColumnName = "Id Usuario";
                    dscbFiltroConciliacion.Tables[0].Columns[4].ColumnName = "Id transacción PDP";
                    dscbFiltroConciliacion.Tables[0].Columns[5].ColumnName = "Id transacción Tuves";
                    dscbFiltroConciliacion.Tables[0].Columns[6].ColumnName = "Estado";
                    this.GridViewConciliacion.DataSource = dscbFiltroConciliacion.Tables[0].DefaultView;
                    this.GridViewConciliacion.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("No sé ha podido consultar error:" + ex);
            }
        }
        /// <summary>
        /// Método que  me permite seleccionar todos los ítems por medio del check genral
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
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
        protected void GetSelectedRecords()
        {
            int i = 0;
            foreach (GridViewRow row in this.GridViewConciliacion.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    if (chkRow.Checked)
                    {
                        var objSelec = Conciliacion.ObtieneDetallePagoConciliacion(
                 GridViewConciliacion.SelectedRow.Cells[3].Text,
                 GridViewConciliacion.SelectedRow.Cells[4].Text,
                 GridViewConciliacion.SelectedRow.Cells[5].Text,
                 GridViewConciliacion.SelectedRow.Cells[6].Text,
                 GridViewConciliacion.SelectedRow.Cells[7].Text,
                 GridViewConciliacion.SelectedRow.Cells[8].Text,
                 GridViewConciliacion.SelectedRow.Cells[9].Text);


                      //  Conciliacion.UpdateRechazado(Convert.ToInt32(objSelec.IdConciliacion.ToString));

                        i++;
                    }
                }
            }
        }
    }


}