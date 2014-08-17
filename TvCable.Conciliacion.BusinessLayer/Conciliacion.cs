using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Transactions;
using TvCable.Conciliacion.DTO;
using TvCable.Conciliacion.DTO.Entities;
using TvCable.Conciliacion.Libs;

namespace TvCable.Conciliacion.BusinessLayer
{
    /// <summary>
    /// Secuencial para Intermix
    /// </summary>
    public class Conciliacion : IDisposable
    {
        internal readonly Nucleo Nucleo = new Nucleo();
        internal Base Base = new Base();

        public int EliminaTransaccionesTablaTrabajo(int idArchivo)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.EliminaTransaccionesTablaTrabajo(idArchivo);
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int ObtenerSecuencialIntermix()
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.ObtenerSecuencialIntermix();
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public EValidaRegistroArchivoResult ObtieneDatosArchivoConciliacionMdp(string nombreArchivo, DateTime fechaArchivo, string codigoMdp)
        {
            Boolean permiteRegistroArchivo = false;
            var resultCode = 0;
            var response = new EValidaRegistroArchivoResult();
            try
            {
                var detalleEstado = new StringBuilder();
                var responseDescription = new StringBuilder();
                var objConciliacion = new Data.Conciliacion();
                var obtieneDatosFile = objConciliacion.ObtieneDatosArchivoConciliacionMdp(nombreArchivo, fechaArchivo, codigoMdp);
                if (obtieneDatosFile != null)
                {
                    if (obtieneDatosFile.Tables.Count > 0)
                    {
                        if (obtieneDatosFile.Tables[0].Rows.Count > 0)
                        {
                            // Existen pendientes registros de archivos por procesare o en estado OK
                            var dtFiles = obtieneDatosFile.Tables[0];
                            foreach (DataRow row in dtFiles.Rows)
                            {
                                detalleEstado.Append("Id Archivo: " + row["DAC_ID"] + " - Estado actual: " +
                                                     row["ITC_NOMBRE"].ToString() + ", ");
                            }
                            permiteRegistroArchivo = false;
                            resultCode = 8;
                            responseDescription.Append(
                                "No se puede registrar este archivo ya que existen (" +
                                obtieneDatosFile.Tables[0].Rows.Count +
                                ") archivos registrados con la informacion enviada: " +
                                detalleEstado);
                        }
                        else
                        {
                            permiteRegistroArchivo = true;
                            resultCode = 0;
                            responseDescription.Append("Registro del archivo permitido.");
                        }
                    }
                    else
                    {
                        permiteRegistroArchivo = true;
                        resultCode = 0;
                        responseDescription.Append("Registro del archivo permitido.");
                    }
                }
                else
                {
                    permiteRegistroArchivo = true;
                    resultCode = 0;
                    responseDescription.Append("Registro del archivo permitido.");
                }

                response.ResultCode = resultCode;
                response.ResultDescription = responseDescription.ToString();
                response.PermiteRegistroArchivo = permiteRegistroArchivo;
                return response;
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.ResultDescription = "Se presento una excepcion: Mensaje: " + ex.Message + ". Exception: " + ex.ToString();
                response.PermiteRegistroArchivo = false;
                return response;
            }
        }

        public ERecaudadorData ObtieneDatosRecaudadorPorCodigo(string codigo)
        {
            var objResponse = new ERecaudadorData();
            try
            {
                var objConciliacion = new Data.Conciliacion();
                var dsRecaudador = objConciliacion.ObtieneDatosRecaudadorPorCodigo(codigo);
                if (dsRecaudador != null)
                {
                    if (dsRecaudador.Tables.Count > 0 && dsRecaudador.Tables[0].Rows.Count > 0)
                    {
                        var dtRecaudador = dsRecaudador.Tables[0];
                        objResponse.IdRec = (int)dtRecaudador.Rows[0]["REC_ID"];
                        objResponse.IdRecaudador = dtRecaudador.Rows[0]["REC_ID_RECAUDADOR"].ToString();
                        objResponse.Ruc = dtRecaudador.Rows[0]["REC_RUC"].ToString();
                        objResponse.IdTipoCliente = dtRecaudador.Rows[0]["REC_ID_TIPOCLIENTE"].ToString();
                        objResponse.Telefono = dtRecaudador.Rows[0]["REC_TELEFONO"].ToString();
                        objResponse.IdPersona = dtRecaudador.Rows[0]["REC_ID_PERSONA"].ToString();
                        objResponse.IdContribuyente = dtRecaudador.Rows[0]["REC_ID_CONTRIBUYENTE"].ToString();
                        objResponse.RazonSocial = dtRecaudador.Rows[0]["REC_RAZONSOCIAL"].ToString();
                        objResponse.Direccion = dtRecaudador.Rows[0]["REC_DIRECCION"].ToString();
                        objResponse.Correo = dtRecaudador.Rows[0]["REC_CORREO"].ToString();
                        objResponse.CodigoServicio = dtRecaudador.Rows[0]["REC_COD_SERVICIO"].ToString();
                        objResponse.Codigo = dtRecaudador.Rows[0]["REC_CODIGO"].ToString();
                        objResponse.CodigoTuves = dtRecaudador.Rows[0]["REC_CODIGO_TUVES"].ToString();
                    }
                    else
                    {
                        objResponse.IdRec = -1;
                        objResponse.IdRecaudador = string.Empty;
                        objResponse.Ruc = string.Empty;
                        objResponse.IdTipoCliente = string.Empty;
                        objResponse.Telefono = string.Empty;
                        objResponse.IdPersona = string.Empty;
                        objResponse.IdContribuyente = string.Empty;
                        objResponse.RazonSocial = string.Empty;
                        objResponse.Direccion = string.Empty;
                        objResponse.Correo = string.Empty;
                        objResponse.CodigoServicio = string.Empty;
                        objResponse.Codigo = string.Empty;
                        objResponse.CodigoTuves = string.Empty;
                    }
                }

                return objResponse;
            }
            catch (Exception ex)
            {
                objResponse.IdRec = -1;
                objResponse.IdRecaudador = string.Empty;
                objResponse.Ruc = string.Empty;
                objResponse.IdTipoCliente = string.Empty;
                objResponse.Telefono = string.Empty;
                objResponse.IdPersona = string.Empty;
                objResponse.IdContribuyente = string.Empty;
                objResponse.RazonSocial = string.Empty;
                objResponse.Direccion = string.Empty;
                objResponse.Correo = string.Empty;
                objResponse.CodigoServicio = string.Empty;
                objResponse.Codigo = string.Empty;
                objResponse.CodigoTuves = string.Empty;
                return objResponse;
            }
        }

        public int RegistraXmlResultadoConciliacion(int idArchivo, string response)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.RegistraXmlResultadoConciliacion(idArchivo, response);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ActualizaEstadoArchivo(int idArchivo, int idEstado)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.ActualizaEstadoArchivo(idArchivo, idEstado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Mauricio - 20 julio 2014
        // obtiene todos los codigos de los recaudadores
        public DataSet ObtenerRecaudadoresSinArchivo()
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.ObtenerRecaudadoresSinArchivo();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public EConciliacionResult GeneraProcesoConciliacion(int idArchivo, string codigoMdp, DateTime fechaTrxProceso, DataTable dtMdp, DataTable dtTuves)
        {
            var objResponse = new EConciliacionResult();
            var stopWatch = new Stopwatch();
            var conciliacion = new Data.Conciliacion();
            var trxOk = 0;
            var trxErrorMonto = 0;
            var trxErrorCliente = 0;
            var trxSobTuves = 0;
            var trxFalTuves = 0;
            string tuvesFechaTrx = string.Empty;
            int idRecaudador = 1;
            try
            {
                // Obtiene los estados para el registro de la conciliacion
                var estadoResultConciliado = Nucleo.GetItemCatalogoPorCodigoItemCodigoCatalogo(Constants.CodeResultConciliado, Constants.CodeEstadoConciliacion);
                var estadoResultError = Nucleo.GetItemCatalogoPorCodigoItemCodigoCatalogo(Constants.CodeResultError, Constants.CodeEstadoConciliacion);

                // Estados internos
                var estadoIntConciliado = Nucleo.GetItemCatalogoPorCodigoItemCodigoCatalogo(Constants.CodeIntConciliadoTuves, Constants.CodeEstadoIntConciliacion);
                var estadoIntSobranteTuves = Nucleo.GetItemCatalogoPorCodigoItemCodigoCatalogo(Constants.CodeIntSobranteTuves, Constants.CodeEstadoIntConciliacion);
                var estadoIntFaltanteTuves = Nucleo.GetItemCatalogoPorCodigoItemCodigoCatalogo(Constants.CodeIntFaltanteTuves, Constants.CodeEstadoIntConciliacion);
                var estadoIntMontoInvalido = Nucleo.GetItemCatalogoPorCodigoItemCodigoCatalogo(Constants.CodeMontoInvalido, Constants.CodeEstadoIntConciliacion);
                var estadoIntClientesDiferentes = Nucleo.GetItemCatalogoPorCodigoItemCodigoCatalogo(Constants.CodeClientesDiferentes, Constants.CodeEstadoIntConciliacion);

                // Obtiene los datos del recaudador
                var dsRecaudador = conciliacion.ObtieneDatosRecaudadorPorCodigo(codigoMdp);
                if (dsRecaudador != null)
                {
                    if (dsRecaudador.Tables.Count > 0 && dsRecaudador.Tables[0].Rows.Count > 0)
                    {
                        idRecaudador = (int)dsRecaudador.Tables[0].Rows[0]["REC_ID"];
                    }
                }

                stopWatch.Start();

                #region Inicia el proceso de conciliacion

                using (var scope = new TransactionScope())
                {
                    foreach (DataRow rowMdp in dtMdp.Rows)
                    {
                        var idRegistroMdp = (int)rowMdp["MDP_ID"];
                        var mdpCodigo = rowMdp["MPD_CODIGO_MDP"].ToString();
                        var mdpFechaIni = rowMdp["MDP_FECHA_INI_TRX"].ToString();
                        var mdpHoraIni = rowMdp["MDP_HORA_INI_TRX"].ToString();
                        var mdpIdTrxMdp = rowMdp["MDP_ID_TRX_MDP"].ToString();
                        var mdpIdTrxTuves = rowMdp["MDP_ID_TRX_TVCABLE"].ToString();
                        var mdpClienteId = rowMdp["MDP_CLIENTE_ID"].ToString();
                        var mdpUsuarioVenta = rowMdp["MDP_USUARIO_VENTA"].ToString();
                        var mdpProducto = rowMdp["MDP_PRODUCTO"].ToString();
                        var mdpMonto = rowMdp["MDP_MONTO"].ToString();
                        var mdpFechaFin = rowMdp["MDP_FECHA_FIN_TRX"].ToString();
                        var mdpHoraFin = rowMdp["MDP_HORA_FIN_TRX"].ToString();
                        var mdpTrxProcesado = rowMdp["MDP_TRX_PROCESADO"].ToString();
                        var mdpIdArchivo = rowMdp["MDP_ID_ARCHIVO_MDP"].ToString();

                        // Obtiene los datos del recaudador
                        dsRecaudador = conciliacion.ObtieneDatosRecaudadorPorCodigoUsuarioVenta(codigoMdp, mdpUsuarioVenta);
                        if (dsRecaudador != null)
                        {
                            if (dsRecaudador.Tables.Count > 0 && dsRecaudador.Tables[0].Rows.Count > 0)
                            {
                                idRecaudador = (int)dsRecaudador.Tables[0].Rows[0]["REC_ID"];
                            }
                        }

                        // Busca si este ID del MDP existe en Tuves para verificar su monto
                        string expression = "TUV_TRX_MPD_ID = '" + mdpIdTrxMdp.ToString() + "'";
                        DataRow[] resultTuves = dtTuves.Select(expression);

                        if (resultTuves.Length > 0) // Verifica si existe esta TRX del MDP en Tuves
                        {
                            // Existe la transaccion de del MDP en TUVES.
                            // Valida el campo: MONTO

                            // Datos de pago de Tuves
                            var tuvesIdTrx = (int)resultTuves[0].ItemArray[0];
                            tuvesFechaTrx = resultTuves[0].ItemArray[3].ToString().Trim();
                            var tuvesHoraTrx = resultTuves[0].ItemArray[4].ToString().Trim();
                            var tuvesIdTrxTuves = resultTuves[0].ItemArray[5].ToString().Trim();
                            var tuvesIdTrxMdp = resultTuves[0].ItemArray[6].ToString().Trim();
                            var tuvesMonto = resultTuves[0].ItemArray[7].ToString().Trim();
                            var tuvesClienteId = resultTuves[0].ItemArray[10].ToString().Trim();
                            var tuvesUsuarioVenta = resultTuves[0].ItemArray[12].ToString().Trim();
                            var tuvesProducto = resultTuves[0].ItemArray[11].ToString().Trim();

                            if (Util.ConvertStringToDecimal(mdpMonto) == Util.ConvertStringToDecimal(tuvesMonto))  // Valida Monto
                            {
                                if (System.String.CompareOrdinal(mdpClienteId, tuvesClienteId) == 0) // Valida el Cliente
                                {
                                    // Transaccion OK, CONCILIADA
                                    var idResultadoConciliado = RegistraResultadoConciliacion(Util.ConvertStringToDateTimeMysql(tuvesFechaTrx),
                                        Util.ConvertStringToDateTimeMysql(tuvesHoraTrx), tuvesIdTrxTuves, tuvesIdTrxMdp, Util.ConvertStringToDecimal(tuvesMonto), string.Empty, tuvesClienteId, tuvesProducto,
                                        tuvesUsuarioVenta, idArchivo, Util.ConvertStringToDateTimeMysql(mdpFechaIni), Util.ConvertStringToDateTimeMysql(mdpHoraIni), mdpIdTrxMdp, mdpIdTrxTuves, mdpClienteId,
                                        mdpUsuarioVenta, mdpProducto, Util.ConvertStringToDecimal(mdpMonto), Util.ConvertStringToDateTimeMysql(mdpFechaFin), Util.ConvertStringToDateTimeMysql(mdpHoraFin),
                                        mdpCodigo, idArchivo, true, true, estadoResultConciliado.IdItemCatalogo, idRecaudador, idArchivo, false, estadoIntConciliado.IdItemCatalogo, fechaTrxProceso);

                                    if (idResultadoConciliado == -1)
                                        throw new Exception("Se presento un error al registrar el resultado de la conciliacion (conciliado).");

                                    trxOk++;
                                }
                                else
                                {
                                    // Los clientes no son validos, registra como inconsistente

                                    #region Registro de Transaccion como error: Los clientes de MDP y Tuves no son iguales

                                    var idResultadoErr = RegistraResultadoConciliacion(Util.ConvertStringToDateTimeMysql(tuvesFechaTrx),
                                        Util.ConvertStringToDateTimeMysql(tuvesHoraTrx), tuvesIdTrxTuves, tuvesIdTrxMdp, Util.ConvertStringToDecimal(tuvesMonto), string.Empty, tuvesClienteId, tuvesProducto,
                                        tuvesUsuarioVenta, idArchivo, Util.ConvertStringToDateTimeMysql(mdpFechaIni), Util.ConvertStringToDateTimeMysql(mdpHoraIni), mdpIdTrxMdp, mdpIdTrxTuves, mdpClienteId,
                                        mdpUsuarioVenta, mdpProducto, Util.ConvertStringToDecimal(mdpMonto), Util.ConvertStringToDateTimeMysql(mdpFechaFin), Util.ConvertStringToDateTimeMysql(mdpHoraFin),
                                        mdpCodigo, idArchivo, true, true, estadoResultError.IdItemCatalogo, idRecaudador, idArchivo, false, estadoIntClientesDiferentes.IdItemCatalogo, fechaTrxProceso);

                                    if (idResultadoErr == -1)
                                        throw new Exception("Se presento un error al registrar el resultado de la conciliacion (Error Clientes de recarga no son iguales).");

                                    trxErrorCliente++;

                                    #endregion Registro de Transaccion como error: Los clientes de MDP y Tuves no son iguales
                                }
                            }
                            else
                            {
                                // Los montos no son iguales se debe marcarla como Inconsistente

                                #region Registro de Transaccion como error: Montos no validos

                                var idResultadoErr = RegistraResultadoConciliacion(Util.ConvertStringToDateTimeMysql(tuvesFechaTrx),
                                    Util.ConvertStringToDateTimeMysql(tuvesHoraTrx), tuvesIdTrxTuves, tuvesIdTrxMdp, Util.ConvertStringToDecimal(tuvesMonto), string.Empty, tuvesClienteId, tuvesProducto,
                                    tuvesUsuarioVenta, idArchivo, Util.ConvertStringToDateTimeMysql(mdpFechaIni), Util.ConvertStringToDateTimeMysql(mdpHoraIni), mdpIdTrxMdp, mdpIdTrxTuves, mdpClienteId,
                                    mdpUsuarioVenta, mdpProducto, Util.ConvertStringToDecimal(mdpMonto), Util.ConvertStringToDateTimeMysql(mdpFechaFin), Util.ConvertStringToDateTimeMysql(mdpHoraFin),
                                    mdpCodigo, idArchivo, true, true, estadoResultError.IdItemCatalogo, idRecaudador, idArchivo, false, estadoIntMontoInvalido.IdItemCatalogo, fechaTrxProceso);

                                if (idResultadoErr == -1)
                                    throw new Exception("Se presento un error al registrar el resultado de la conciliacion (Error montos no validos).");

                                trxErrorMonto++;

                                #endregion Registro de Transaccion como error: Montos no validos
                            }

                            // Cambiar estado en Tuves y al MDP

                            // Actualiza la transaccion del MDP a procesado
                            var updEstadoProcesadoMdp = conciliacion.ActualizaEstadoTrxMdp(idRegistroMdp, true);

                            // Actualiza la TRX a Procesada.
                            var updEstadoProcesadoTuves = conciliacion.ActualizaEstadoTrxTuves(tuvesIdTrx, true);
                        }
                        else
                        {
                            #region Registro de Transaccion como error
                            var dateTimeNullValue = default(DateTime);
                            DateTime? dateNull = null;

                            // Esta transaccion no existe en TUVES, la registra en la tabla de resultado como error
                            var idResultadoErr = RegistraResultadoConciliacion(dateNull,
                                dateNull, null, null, 0, null, null, null, null, idArchivo, Util.ConvertStringToDateTimeMysql(mdpFechaIni),
                                Util.ConvertStringToDateTimeMysql(mdpHoraIni), mdpIdTrxMdp, mdpIdTrxTuves, mdpClienteId,
                                mdpUsuarioVenta, mdpProducto, Util.ConvertStringToDecimal(mdpMonto), Util.ConvertStringToDateTimeMysql(mdpFechaFin), Util.ConvertStringToDateTimeMysql(mdpHoraFin),
                                mdpCodigo, idArchivo, true, false, estadoResultError.IdItemCatalogo, idRecaudador, idArchivo, false, estadoIntFaltanteTuves.IdItemCatalogo, fechaTrxProceso);

                            // Actualiza la transaccion del MDP a procesado
                            var updEstadoProcesado = conciliacion.ActualizaEstadoTrxMdp(idRegistroMdp, true);

                            if (idResultadoErr == -1)
                                throw new Exception("Se presento un error al registrar el resultado de la conciliacion (no conciliado).");
                            trxFalTuves++;

                            #endregion Registro de Transaccion como error
                        }
                    }

                    // Verifico las transacciones que no han sido procesadas en la tabla de trabajo de Tuves
                    var pagosPorProcesar = conciliacion.ObtieneTrxTuvesPorEstado(false, fechaTrxProceso);
                    if (pagosPorProcesar != null)
                    {
                        if (pagosPorProcesar.Tables.Count > 0)
                        {
                            if (pagosPorProcesar.Tables[0].Rows.Count > 0)
                            {
                                // Existen Transacciones que no han sido procesadas del lado de Tuves por lo que se deben marcar como inconsistentes.
                                var dtSobrantes = pagosPorProcesar.Tables[0];
                                foreach (DataRow row in dtSobrantes.Rows)
                                {
                                    var idTrxTuves = (int)row["TUV_ID"];
                                    var idMdp = row["MPD_ID"];
                                    var codeMdp = row["MDP_CODE"];
                                    var tuvesFecha = row["TUV_FECHA"].ToString();
                                    var tuvesHora = row["TUV_HORA"].ToString();
                                    var tuvesIdTrx = row["TUV_TRX_ID"].ToString();
                                    var tuvesIdTrxMdp = row["TUV_TRX_MPD_ID"].ToString();
                                    var tuvesMonto = row["TUV_MONTO"].ToString();
                                    var tuvesCanal = row["TUV_CANAL"].ToString();
                                    var tuvesTrxProcesada = row["TUV_TRX_PROCESADO"];
                                    var tuvesCliente = row["TUV_CLIENTE"].ToString();
                                    var tuvesProducto = row["TUV_PRODUCTO"].ToString();
                                    var tuvesUsuarioVenta = row["TUV_USUARIO_VENTA"].ToString();
                                    var tuvesIdArchivoMdp = (int)row["TUV_ID_ARCHIVO_MDP"];
                                    var tuvesMdpCode = row["TUV_MDP_CODE"];

                                    var dateTimeNullValue = default(DateTime);

                                    DateTime? dateNull = null;
                                    //var dateNull = Nullable<DateTime>;

                                    // Registra esta transaccion en la tabla RESULTADO DE CONCILIACION
                                    var idSobranteTuves = RegistraResultadoConciliacion(Util.ConvertStringToDateTimeMysql(tuvesFecha),
                                        Util.ConvertStringToDateTimeMysql(tuvesHora), tuvesIdTrx, tuvesIdTrxMdp, Util.ConvertStringToDecimal(tuvesMonto), tuvesCanal, tuvesCliente, tuvesProducto, tuvesUsuarioVenta, idArchivo, dateNull,
                                        dateNull, null, null, null, null, null, 0, dateNull, dateNull,
                                        null, idArchivo, false, true, estadoResultError.IdItemCatalogo, idRecaudador, idArchivo, false, estadoIntSobranteTuves.IdItemCatalogo, fechaTrxProceso);

                                    // Actualiza la TRX a Procesada.
                                    var updEstadoProcesado = conciliacion.ActualizaEstadoTrxTuves(idTrxTuves, true);

                                    trxSobTuves++;
                                }
                            }
                        }
                    }

                    scope.Complete();
                }

                #endregion Inicia el proceso de conciliacion

                #region Elimina los registros usados en el proceso de Conciliacion (Tablas de trabajo)

                var eliminaTablaTrabajo = conciliacion.EliminaTransaccionesTablaTrabajo(idArchivo);

                #endregion Elimina los registros usados en el proceso de Conciliacion (Tablas de trabajo)

                stopWatch.Stop();
                objResponse.ResultCode = 0;
                objResponse.ResultDescription = "Proceso de conciliacion ejecutado correctamente.";
                objResponse.TiempoEjecucion = stopWatch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture);
                objResponse.TransaccionesConciliadas = trxOk;
                objResponse.TransaccionesErrorMonto = trxErrorMonto;
                objResponse.TransaccionesErrorCliente = trxErrorCliente;
                objResponse.TransaccionesFaltantesTuves = trxFalTuves;
                objResponse.TransaccionesSobrantesTuves = trxSobTuves;
                return objResponse;
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                objResponse.ResultCode = -1;
                objResponse.ResultDescription = "Se presento una excepcion: " + ex.Message + ". Detalle: " + ex.ToString();
                objResponse.TiempoEjecucion = stopWatch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture);
                objResponse.TransaccionesConciliadas = 0;
                objResponse.TransaccionesErrorMonto = 0;
                objResponse.TransaccionesErrorCliente = 0;
                objResponse.TransaccionesFaltantesTuves = 0;
                objResponse.TransaccionesSobrantesTuves = 0;
                return objResponse;
            }
        }

        public int EliminaTrxTuves(string codeMdp, DateTime fechaTrxMdp)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.EliminaTrxTuves(codeMdp, fechaTrxMdp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int EliminaTrxMdp(DateTime fechaTrxMdp, string codigoMdp)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.EliminaTrxMdp(fechaTrxMdp, codigoMdp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RegistraResultadoConciliacion(DateTime? tuvesFechaTrx, DateTime? tuvesHoraTrx, string tuvesIdTrx,
            string tuvesIdTrxMdp, decimal tuvesMonto, string tuvesCanal,
            string tuvesCliente, string tuvesProducto, string tuvesUsuarioVenta, int tuvesIdArchivo,
            DateTime? mdpFechaTrxIni, DateTime? mdpHoraTrxIni, string mdpIdTrxMdp,
            string mdpIdTrxTuves, string mdpIdCliente, string mdpUsuarioVenta, string mdpProducto, decimal mdpMonto,
            DateTime? mdpFechaTrxFin, DateTime? mdpHoraTrxFin,
            string mdpCodigoMdp, int mdpIdArchivo, bool existeTrxMdp, bool existeTrxTuves, int estadoGeneralTrx,
            int idRecaudador, int idArchivo, bool registroIntermix, int idEstadoConciliacion, DateTime fechaTrxProceso)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.RegistraResultadoConciliacion(tuvesFechaTrx, tuvesHoraTrx, tuvesIdTrx,
                        tuvesIdTrxMdp, tuvesMonto, tuvesCanal,
                        tuvesCliente, tuvesProducto, tuvesUsuarioVenta, tuvesIdArchivo,
                        mdpFechaTrxIni, mdpHoraTrxIni, mdpIdTrxMdp,
                        mdpIdTrxTuves, mdpIdCliente, mdpUsuarioVenta, mdpProducto, mdpMonto,
                        mdpFechaTrxFin, mdpHoraTrxFin,
                        mdpCodigoMdp, mdpIdArchivo, existeTrxMdp, existeTrxTuves, estadoGeneralTrx,
                        idRecaudador, idArchivo, registroIntermix, idEstadoConciliacion, fechaTrxProceso);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ObtieneDetallePagosMdpPorIdArchivo(int idArchivo)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.ObtieneDetallePagosMdpPorIdArchivo(idArchivo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ObtieneDetallePagosTuvesPorIdArchivo(int idArchivo)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.ObtieneDetallePagosTuvesPorIdArchivo(idArchivo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RegistraPagoTuvez(string idMdp, string codigoMdp, DateTime fecha, DateTime hora,
            string idTransaccionTuves,
            string idTrxMdp, decimal monto, string canal, int trxProcesado, string idCliente, string producto,
            string usuarioVenta, int idArchivoMdp, string tuvCodigoMdp)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.RegistraPagoTuvez(idMdp, codigoMdp, fecha, hora, idTransaccionTuves, idTrxMdp,
                        monto, canal, trxProcesado, idCliente, producto, usuarioVenta, idArchivoMdp, tuvCodigoMdp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RegistraPagoMdp(string codigoMdp, DateTime fechaInicio, DateTime horaInicio, string idTransaccionMdp,
            string idTrxTvcable, string idCliente, string usuarioVenta, string producto, decimal monto,
            DateTime fechaFin, DateTime horaFin, int idEstadoPago, int idArchivoMdp)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.RegistraPagoMdp(codigoMdp, fechaInicio, horaInicio, idTransaccionMdp,
                        idTrxTvcable, idCliente, usuarioVenta, producto, monto, fechaFin, horaFin, idEstadoPago, idArchivoMdp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EDetallePagoData ObtieneDetalleTransaccionArchivo(string datosPago)
        {
            var response = new EDetallePagoData();
            try
            {
                var separadorDataFile = ConfigurationManager.AppSettings["SeparadorDatosPago"] ?? ";";
                var formatoFechaArchivoMdp = ConfigurationManager.AppSettings["FormatoFechaArchivoMDP"] ?? "yyyy-MM-dd";
                var formatoHoraArchivoMdp = ConfigurationManager.AppSettings["FormatoHoraArchivoMDP"] ?? "hh:mm:ss";
                var inicioDatosDetallePago = ConfigurationManager.AppSettings["InicioDatosDetallePago"] ?? "2";

                if (!string.IsNullOrEmpty(datosPago) && datosPago.StartsWith(inicioDatosDetallePago))
                {
                    var detallePagos = datosPago.Split(Convert.ToChar(separadorDataFile));
                    response.FechaInicio = Util.ConvertStringToDateTime(detallePagos[0], formatoFechaArchivoMdp);
                    response.HoraInicio = Util.ConvertStringToDateTime(detallePagos[1], formatoHoraArchivoMdp);
                    response.IdTransactionMdp = detallePagos[2];
                    response.IdTransactionTvCable = detallePagos[3];
                    response.Cedula = detallePagos[4];
                    response.IdMdp = detallePagos[5];
                    response.UsuarioVentas = detallePagos[6];
                    response.Producto = detallePagos[7];
                    response.Monto = Util.ConvertStringToDecimal(detallePagos[8]);
                    response.FechaFin = Util.ConvertStringToDateTime(detallePagos[9], formatoFechaArchivoMdp);
                    response.HoraFin = Util.ConvertStringToDateTime(detallePagos[10], formatoHoraArchivoMdp);
                    response.ResultCode = "0";
                }
                else
                {
                    response.ResultCode = "-1";
                    return response;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.ResultCode = "-2";
                return response;
            }
        }

        /// <summary>
        /// Obtiene el detalle del archivo por ID del Archivo
        /// </summary>
        /// <param name="idArchivo"></param>
        /// <returns></returns>
        public DataSet ObtieneDetalleArchivoMdpPorId(int idArchivo)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.ObtieneDetalleArchivoMdpPorId(idArchivo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza el estado y el contenido del archivo
        /// </summary>
        /// <param name="idArchivo">Id del archivo del MDP</param>
        /// <param name="idEstado">Id del nuevo estado (ItemCatalogo)</param>
        /// <param name="archivo">archivo</param>
        /// <returns></returns>
        public int ActualizaDatosArchivo(int idArchivo, int idEstado, byte[] archivo)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.ActualizaDatosArchivo(idArchivo, idEstado, archivo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene el detalle del archivo en un arreglo de byte[]
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        public byte[] ObtieneArchivoConciliacion(string nombreArchivo)
        {
            #region Variables

            byte[] archivoMdp = null;

            #endregion Variables

            try
            {
                var pathFileTemp = ConfigurationManager.AppSettings["ConciliacionTemp"] ?? "";
                if (string.IsNullOrEmpty(pathFileTemp))
                    throw new Exception("Debe crear el directorio temporal de trabajo / agregarlo en el archivo de configuracion: C:/ConciliacionTemp/");

                var pathTempoFileMdp = pathFileTemp + nombreArchivo;
                byte[] bytesFile = System.IO.File.ReadAllBytes(pathTempoFileMdp);
                return bytesFile;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Valida el formato del archivo entregado al directorio FTP por el MDP
        /// </summary>
        /// <param name="codigoMdp">Codigo del MDP</param>
        /// <param name="nombreArchivo">Nombre del archivo</param>
        /// <param name="fechaTransacciones">Fecha de las transacciones</param>
        /// <param name="numTransacciones">Numero de transacciones</param>
        /// <returns></returns>
        public EValidaArchivoData ValidaFormatoArchivoConciliacion(string codigoMdp, string nombreArchivo, DateTime fechaTransacciones, int numTransacciones, out byte[] fileMdp)
        {
            #region Variables

            var pathTempoFileMdp = string.Empty;
            var tipoLineHeadFile = string.Empty;
            var fechaTrxHeadFile = string.Empty;
            var numTrxHeadFile = string.Empty;
            var idMdpHeadFile = string.Empty;

            var resultCode = 8;
            var resultDescription = string.Empty;
            var statusFile = false;
            var response = new EValidaArchivoData();
            var sb = new StringBuilder();
            var getHead = false;
            var numeroTrxFile = 0;
            decimal valorTotalFile = 0;
            var stopWatch = new Stopwatch();

            int bytesRead = 0;
            byte[] buffer = new byte[4096];

            var destination = new MemoryStream();
            byte[] archivoMdp = null;

            #endregion Variables

            try
            {
                var pathFileMdp = ConfigurationManager.AppSettings["DireccionFTP"];
                var userFtp = ConfigurationManager.AppSettings["UsuarioAccesoFTP"];
                var passwordFtp = ConfigurationManager.AppSettings["ClaveAccesoFTP"];
                var puertoAccesoFtp = ConfigurationManager.AppSettings["PuertoAccesoFTP"];

                var separadorDataFile = ConfigurationManager.AppSettings["SeparadorDatosPago"] ?? ";";
                var inicioDatoHead = ConfigurationManager.AppSettings["InicioDatosHead"] ?? "1";
                var inicioDatoDetallePago = ConfigurationManager.AppSettings["InicioDatosDetallePago"] ?? "2";

                var pathFileTemp = ConfigurationManager.AppSettings["ConciliacionTemp"] ?? "C:/ConciliacionTemp/";
                if (string.IsNullOrEmpty(pathFileTemp))
                    throw new Exception("Debe crear el directorio temporal de trabajo: C:/ConciliacionTemp/");

                stopWatch.Start();

                pathTempoFileMdp = pathFileTemp + nombreArchivo;

                var util = new Util();

                var request = new WebClient();
                string url = pathFileMdp + nombreArchivo;
                // Mauricio - 20 julio 2014
                // modificar el manejo de usuario y password
                Base.WriteLog(Base.ErrorTypeEnum.Information, 301, "Intentando obtener credenciales para CodigoMDP: " + codigoMdp.ToLower() + " url: " + url);
                request.Credentials = new NetworkCredential(codigoMdp, obtenerCredencialesFTP(userFtp.ToLower(), passwordFtp, codigoMdp.ToLower()));
                //request.Credentials = new NetworkCredential(userFtp, passwordFtp);

                #region Obtiene la informacion del Archivo

                try
                {
                    var requestFile = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(url);
                    // Mauricio - 20 julio 2014
                    // modificar el manejo de usuario y password
                    //
                    //requestFile.Credentials = new System.Net.NetworkCredential(userFtp, passwordFtp);
                    requestFile.Credentials = new System.Net.NetworkCredential(codigoMdp, obtenerCredencialesFTP(userFtp.ToLower(), passwordFtp, codigoMdp.ToLower()));

                    try
                    {
                        // Registra el archivo en un directorio temporal
                        using (System.Net.WebResponse responseFile = requestFile.GetResponse())
                        {
                            using (System.IO.Stream streamFile = responseFile.GetResponseStream())
                            {
                                if (streamFile != null)
                                {
                                    var fileStream = new FileStream(pathTempoFileMdp, FileMode.Create);
                                    while (true)
                                    {
                                        bytesRead = streamFile.Read(buffer, 0, buffer.Length);

                                        if (bytesRead == 0)
                                            break;

                                        fileStream.Write(buffer, 0, bytesRead);
                                        fileStream.Close();
                                    }
                                }
                            }
                        }
                    }
                    catch (WebException e)
                    {
                        Base.WriteLog(Base.ErrorTypeEnum.Information, 304, "Error: " + ((FtpWebResponse)e.Response).StatusDescription) ;
                    }

                    // Obtiene el archivo para validar su formato
                    var filestream = new FileStream(pathTempoFileMdp,
                            FileMode.Open,
                            FileAccess.Read,
                            FileShare.ReadWrite);
                    var file = new StreamReader(filestream, Encoding.UTF8, true, 128);
                    string lineFile;
                    while ((lineFile = file.ReadLine()) != null)
                    {
                        // Obtiene el valor de la cabecera del archivo
                        if (!getHead)
                        {
                            tipoLineHeadFile = util.ObtieneCampoConciliacion(lineFile, 1, 1);
                            fechaTrxHeadFile = util.ObtieneCampoConciliacion(lineFile, 2, 8);
                            numTrxHeadFile = util.ObtieneCampoConciliacion(lineFile, 10, 8);
                            idMdpHeadFile = util.ObtieneCampoConciliacion(lineFile, 18, 8);
                            getHead = true;
                            archivoMdp = Util.ReadToEnd(filestream);
                        }
                        else
                        {
                            // Obtiene el detalle de las transacciones.
                            if (!string.IsNullOrEmpty(lineFile.Trim()))
                            {
                                var detallePagos = lineFile.Split(Convert.ToChar(separadorDataFile));
                                valorTotalFile = valorTotalFile + Util.ConvertStringToDecimal(detallePagos[8]);
                                numeroTrxFile++;
                            }
                        }
                    }
                }
                catch (WebException exLoadFtp)
                {
                    stopWatch.Stop();
                    response.ResultCode = -100;
                    //response.ResultDescription = "Url FTP: " + url + ". Local File Path: " + pathTempoFileMdp + ". Mensaje Ex: " + exLoadFtp.Message + ". Ex Detalle: " + exLoadFtp.ToString();
                    response.ResultDescription = " Local File Path: " + pathTempoFileMdp + ". Mensaje Ex: " + exLoadFtp.Message;
                    response.StatusFile = false;
                    response.TiempoValidacion = stopWatch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture);
                    fileMdp = null;
                    return response;
                }

                #endregion Obtiene la informacion del Archivo

                #region Valida la informacion registrada en el archivo

                // Valida que la cabecera
                if (string.Compare(tipoLineHeadFile, inicioDatoHead, System.StringComparison.Ordinal) != 0)
                {
                    stopWatch.Stop();
                    response.ResultCode = 1;
                    response.ResultDescription = "La cabecera (resumen) del archivo no está correcta, debe iniciar con el dato: " + inicioDatoHead;
                    response.StatusFile = false;
                    response.TiempoValidacion = stopWatch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture);
                    fileMdp = null;
                    return response;
                }

                // Valida que el numero de transacciones que se registro en el servicio web sea igual al que se encuentran en el archivo
                if (numeroTrxFile != numTransacciones)
                {
                    //stopWatch.Stop();
                    //response.ResultCode = 2;
                    response.ResultDescription = "El número de transacciones no son iguales: Registrada en el servicio web: " + numTransacciones + ". Existente en el resumen del archivo: " + numeroTrxFile + " - ";
                    //response.StatusFile = false;
                    //response.TiempoValidacion = stopWatch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture);
                    //fileMdp = null;
                    //return response;
                }

                // Valida las fechas registradas_ WS y Archivo
                DateTime dtFile = DateTime.ParseExact(fechaTrxHeadFile, "yyyyMMdd", CultureInfo.InvariantCulture);
                if (dtFile != fechaTransacciones)
                {
                    stopWatch.Stop();
                    response.ResultCode = 3;
                    response.ResultDescription = "Las fechas de las transacciones no son iguales: Registrada en el servicio web: " + fechaTransacciones.ToString("dd-MM-yyyy") + ". Existente en el resumen del archivo: " + dtFile.ToString("dd-MM-yyyy");
                    response.StatusFile = false;
                    response.TiempoValidacion = stopWatch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture);
                    fileMdp = null;
                    return response;
                }

                //throw new System.ArgumentException("La cabecera (resumen) del archivo no está correcta, debe iniciar con el dato: " + inicioDatoHead);

                #endregion Valida la informacion registrada en el archivo

                stopWatch.Stop();
                response.ResultCode = 0;
                //MC - Modificacion temporal para no validar el numero de transacciones
                response.ResultDescription = response.ResultDescription + "Formato correcto";
                response.StatusFile = true;
                response.TiempoValidacion = stopWatch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture);
                //fileMdp = archivoMdp;
                fileMdp = archivoMdp;
                return response;
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                response.ResultCode = -101;
                response.ResultDescription = "Mensaje: " + ex.Message;
                response.StatusFile = false;
                response.TiempoValidacion = stopWatch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture);
                fileMdp = null;
                return response;
            }
        }

        // Mauricio - 20 julio 2014
        // Obtiene el usuario y password para el servidor FTP
        private string obtenerCredencialesFTP(string usuario, string password, string medioPago)
        {
            var separadorDataFile = ConfigurationManager.AppSettings["SeparadorDatosPago"] ?? ";";
            int i;
            string tmpPassword = null;

            string[] usuarios = usuario.Split(Convert.ToChar(separadorDataFile));
            string[] passwords = password.Split(Convert.ToChar(separadorDataFile));

            for (i = 0; i < usuarios.Length; i++)
            {
                if (usuarios[i].ToLower() == medioPago.ToLower() )
                {
                    tmpPassword = passwords[i];
                }
            }
            Base.WriteLog(Base.ErrorTypeEnum.Information, 302, "credenciales para CodigoMDP: " + tmpPassword);
            return tmpPassword;
        }

        /// <summary>
        /// Obtiene la lista de archivos de conciliacion por estado
        /// </summary>
        /// <param name="idEstado">Estado del archivo</param>
        /// <returns>Dataset</returns>
        public DataSet ObtenerArchivoConciliacionPorEstado(int idEstado)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.ObtenerArchivoConciliacionPorEstado(idEstado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Registra la entrega de un archivo enviado por el MDP
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo del MDP</param>
        /// <param name="fechaTransacciones">Fecha de las transacciones que estan en el archivo</param>
        /// <param name="codigoMdp">Codigo del medio de pago</param>
        /// <param name="numTransacciones">Numero de transacciones que vienen en el archivo del medio de pago</param>
        /// <param name="montoTotal">Valor recaudado</param>
        /// <param name="estadoArchivo">Estado inicial del archivo</param>
        /// <param name="mailNotificacion">Cuentas de correo a quienes se notificara de alguna observacion</param>
        /// <param name="observaciones">Registro de observaciones</param>
        /// <param name="fechaRegistro">Fecha y hora en la que se registra esta peticion</param>
        /// <returns>ID del nuevo registro</returns>
        public int RegistraEntregaArchivoConciliacionMdp(string nombreArchivo, DateTime fechaTransacciones,
            string codigoMdp,
            int numTransacciones, decimal montoTotal, int estadoArchivo, string mailNotificacion, string observaciones,
            DateTime fechaRegistro)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.RegistraEntregaArchivoConciliacionMdp(nombreArchivo, fechaTransacciones,
                        codigoMdp, numTransacciones, montoTotal, estadoArchivo, mailNotificacion, observaciones,
                        fechaRegistro);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene con filtros la conciliación
        /// </summary>
        /// <param name="recaudador">Variable de filtro recaudador </param>
        /// <param name="fecha_inicio">Variable de filtro fecha inicio  </param>
        /// <param name="fecha_fin">Variable de filtro fecha fin entrada  </param>
        /// <param name="tipo_conciliacion">Variable de filtro conciliacion</param>
        /// <returns>Un data tipo RESULTADO_CONCILIACION con la consulta devuelta segun el filtro enviado</returns>

        public DataSet GetDataConciliacion(int recaudador, DateTime fechainicio, DateTime fechafin, int tipoconciliacion)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.GetDataConciliacion(recaudador, fechainicio, fechafin, tipoconciliacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateConciliacionPorIntermix(int idconciliacion, string estadoconci)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.UpdateConciliacionPorIntermix(idconciliacion, estadoconci);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene con un solo filtro la conciliación
        /// </summary>
        /// <param name="tipo_conciliacion">Variable de filtro conciliacio</param>
        /// <returns>Un data tipo RESULTADO_CONCILIACION con la consulta devuelta segun el filtro enviado</returns>
        public DataSet GetDataConciliacionPorTipo(String tipo_conciliacion)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.GetDataConciliacionPorTipo(tipo_conciliacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene sin filtro todos  la los estados de pago
        /// </summary>
        /// <param name="catid">id del catalogo que corresponde a estados de pago</param>
        /// <returns>Devuelve un dataset listando de los estados de pago</returns>
        public DataSet GetDataEstadoConciliacion(string catid)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.GetDataEstadoConciliacion(catid);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  Obtiene sin filtro todos  la los recaudadores
        /// </summary>
        /// <returns>Toda la lista de los recaudadores</returns>
        public DataSet GetDataRecaudador()
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.GetDataRecaudador();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene con filtros todos la lsita de tipos de conciliacion
        /// </summary>
        /// <param name="estconciliacion">parametro tipo item catalogo</param>
        /// <returns>Todo el listado que se tipo item catalogo</returns>
        public DataSet GetDatListadoConciliacion(string idcon)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.GetDatListadoConciliacion(idcon);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Trae el la empresa facturadora
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataEmpresaFacturadora()
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.GetDataEmpresaFacturadora();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Trae el la empresa facturadora a partir de su id
        /// </summary>
        /// <param name="idfacturadora">id facturadora</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataEmpresaFacturadoraId(string idfacturadora)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.GetDataEmpresaFacturadoraId(idfacturadora);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Trae un recaudador a partir de su ID
        /// </summary>
        /// <param name="idrecaudado">id recaudador</param>
        /// <returns>datset</returns>
        public DataSet GetDataReacaudadorId(string idrecaudado)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.GetDataReacaudadorId(idrecaudado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //DRO: Parámetros de actualización estado pago
        /// <summary>
        /// Se actualiza el estado de pago
        /// </summary>
        /// <param name="tipoconciliacion">tipo de conciliacion</param>
        /// <param name="estadopago">tipo de pago</param>
        /// <returns>Me retorna cuanto registros han sido afectados </returns>
        public int UpdateEstado(int tipoconciliacion, int estadonuevo)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.UpdateEstado(tipoconciliacion, estadonuevo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Se actualiza a estado rechazado la conciliacion
        /// </summary>
        /// <param name="idrescon">id del campo que va a ser actualizado</param>
        /// <param name="idtipoco">id correspondiente a rechazado</param>
        /// <returns>retorna el numero de campos afectados</returns>
        public int UpdateRechazado(string idcatalogo, int idupdate)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.UpdateRechazado(idcatalogo, idupdate);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inserta en la tabla HISTORIAL_ESTADO todos los datos necesarios para llevar un registro de los cambios de estado.
        /// </summary>
        /// <param name="iestadoid">id estado</param>
        /// <param name="iresultadoid">id resultado</param>
        /// <param name="ifecha">fecha sistema</param>
        /// <param name="iidusuario">id usuario</param>
        /// <param name="iusuario">usuario</param>
        /// <param name="iobservacion">observacion</param>
        /// <returns></returns>
        public int InsertHistorialEstado(int idconciliacion, int idresultadocon, DateTime fecha, string dusuari, string usuario, string observacion)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.InsertHistorialEstado(idconciliacion, idresultadocon, fecha, dusuari, usuario, observacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Trae el historial como máxima fecha del registro correspondiente a id de conciliación
        /// </summary>
        /// <param name="idconciliacion">id conciliacion</param>
        /// <param name="idestado">id estado</param>
        /// <returns>dataset</returns>
        public DataSet GetDataHistorialPorId(string idconciliacion, string idestado)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.GetDataHistorialPorId(idconciliacion, idestado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertHistorialIntermix(int resultid, int resultseq, DateTime tifechainter, string usuario, string rolusu, string observacion)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.InsertHistorialIntermix(resultid, resultseq, tifechainter, usuario, rolusu, observacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Trae los usuarios con sus funcionalidades
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="rol"></param>
        /// <returns></returns>
        public DataSet GetUsuarioFuncionalidades(string usuario, string rol)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.GetUsuarioFuncionalidades(usuario, rol);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inserción a la tabla de Intermix
        /// </summary>
        /// <param name="codempresa"></param>
        /// <param name="codagencia"></param>
        /// <param name="ruc"></param>
        /// <param name="codcliente"></param>
        /// <param name="tipocliente"></param>
        /// <param name="telefono"></param>
        /// <param name="tipopersona"></param>
        /// <param name="tipocontribuyente"></param>
        /// <param name="nombre"></param>
        /// <param name="direccion"></param>
        /// <param name="email"></param>
        /// <param name="codservicio"></param>
        /// <param name="valorneto"></param>
        /// <param name="base_ice"></param>
        /// <param name="valorice"></param>
        /// <param name="baseiva"></param>
        /// <param name="valoriva"></param>
        /// <param name="valortotal"></param>
        /// <param name="fechaproceso"></param>
        /// <param name="fechaemision"></param>
        /// <param name="fechadesde"></param>
        /// <param name="fechahasta"></param>
        /// <param name="referencia"></param>
        /// <param name="comentario"></param>
        /// <param name="factura"></param>
        /// <returns></returns>
        public int InsertIntermix(
                        int idconcilrec,
                        int codempresa,
                         string codpuntoemision,
                        string ruc,

                        int tipocliente,
                        string telefono,
                        int tipopersona,
                        int tipocontribuyente,
                        string nombre,
                        string direccion,
                        string email,
                         string coditem,
                        decimal valorneto,
                        decimal base_ice,
                        decimal valorice,
                        decimal baseiva,
                        decimal valoriva,
                        decimal valortotal,
                        DateTime fechaproceso,
                        DateTime fechaemision,
                        DateTime fechadesde,
                        DateTime fechahasta,

                        string comentario,
                        int factura)
        {
            try
            {
                using (var objConciliacion = new Data.Conciliacion())
                {
                    return objConciliacion.InsertIntermix(
                        idconcilrec,
                        codempresa,
                         codpuntoemision,
                         ruc,

                         tipocliente,
                         telefono,
                        tipopersona,
                        tipocontribuyente,
                         nombre,
                         direccion,
                         email,
                            coditem,
                         valorneto,
                         base_ice,
                         valorice,
                         baseiva,
                         valoriva,
                         valortotal,
                         fechaproceso,
                         fechaemision,
                        fechadesde,
                        fechahasta,

                         comentario,
                         factura);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Se carga todos los datos del pequeño formulario que se selecciona del item del Gread en una entidad
        /// </summary>
        /// <param name="idCatalogo">id catalogo</param>
        /// <param name="fechaCatalogo"> fecha del catalogo</param>
        /// <param name="montoCatalogo"></param>
        /// <param name="usuarioCatalogo"></param>
        /// <param name="idtranpdpCatalogo"></param>
        /// <param name="idtrantuvesCatalogo"></param>
        /// <param name="estadoCatalogo"></param>
        /// <returns></returns>
        public EDetallePagoConciliacion ObtieneDetallePagoConciliacion(string Id, string FechaTuv, string IdTuv, string MontoTUV, string IdMdp, string FechainiMdp, string UsuarioVenta, string MontoMdp)
        {
            var response = new EDetallePagoConciliacion();
            try
            {
                response.Id = Id;
                response.MontoTUV = MontoTUV;
                response.MontoMdp = MontoMdp;
                response.FechaTuv = FechaTuv;
                response.FechainiMdp = FechainiMdp;
                // response.UsuarioVenta = UsuarioVenta;
                response.IdTuv = IdTuv;
                response.IdMdp = IdMdp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }



        #region IDisposable Members

        void IDisposable.Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion IDisposable Members
    }
}