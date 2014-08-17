using System;
using System.Data;
using System.Data.SqlClient;
using TvCable.Conciliacion.DTO;
using TvCable.Conciliacion.Libs;

namespace TvCable.Conciliacion.Data
{
    public class Conciliacion : IDisposable
    {
        
        public int EliminaTransaccionesTablaTrabajo(int idArchivo)
        {
            try
            {
                int ret = -1;
                var parametros = new SqlParameter[2];

                parametros[0] = new SqlParameter("@i_idArchivo", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = idArchivo;

                parametros[1] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[1].Direction = ParameterDirection.Output;

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpEliminaTransaccionesTablaTrabajo, parametros, string.Empty);
                if (parametros[1].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[1].Value.ToString());

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ObtenerSecuencialIntermix()
        {
            try
            {
                int ret = -1;
                var parametros = new SqlParameter[2];

                parametros[0] = new SqlParameter("@nextSeq", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Output;

                parametros[1] = new SqlParameter("@intervalSeq", SqlDbType.Int);
                parametros[1].Direction = ParameterDirection.Output;

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpObtieneSecuencialIntermix, parametros, string.Empty);
                if (parametros[0].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[0].Value.ToString());

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ObtieneDatosArchivoConciliacionMdp(string nombreArchivo, DateTime fechaArchivo, string codigoMdp)
        {
            try
            {
                var parametros = new SqlParameter[3];

                #region Parametros

                parametros[0] = new SqlParameter("@i_nombreArchivo", SqlDbType.VarChar, 50);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = nombreArchivo;

                parametros[1] = new SqlParameter("@i_fechaTrx", SqlDbType.Date);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = fechaArchivo;

                parametros[2] = new SqlParameter("@i_codigoMdp", SqlDbType.VarChar, 50);
                parametros[2].Direction = ParameterDirection.Input;
                parametros[2].Value = codigoMdp;

                #endregion Parametros

                var objDatos = new ClsSqlClientHelper();
                return objDatos.DSExecuteQueryStoredProcedure(Constants.SpObtieneDatosArchivoConciliacionMdp, parametros, string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ObtieneDatosRecaudadorPorCodigo(string codigo)
        {
            try
            {
                var parametros = new SqlParameter[1];

                #region Parametros

                parametros[0] = new SqlParameter("@i_codigoRecaudador", SqlDbType.VarChar, 50);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = codigo;

                #endregion Parametros

                var objDatos = new ClsSqlClientHelper();
                return objDatos.DSExecuteQueryStoredProcedure(Constants.SpObtieneDatosRecaudadorPorCodigo, parametros, string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ObtieneDatosRecaudadorPorCodigoUsuarioVenta(string codigo, string usuarioVenta)
        {
            try
            {
                var parametros = new SqlParameter[2];

                #region Parametros

                parametros[0] = new SqlParameter("@i_codigoRecaudador", SqlDbType.VarChar, 50);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = codigo;

                parametros[1] = new SqlParameter("@i_codigoUsuarioVenta", SqlDbType.VarChar, 50);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = usuarioVenta;

                #endregion Parametros

                var objDatos = new ClsSqlClientHelper();
                return objDatos.DSExecuteQueryStoredProcedure(Constants.SpObtieneDatosRecaudadorPorCodigoUsuarioVentas, parametros, string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RegistraXmlResultadoConciliacion(int idArchivo, string response)
        {
            try
            {
                int ret = -1;
                var parametros = new SqlParameter[3];

                parametros[0] = new SqlParameter("@i_idArchivo", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = idArchivo;

                parametros[1] = new SqlParameter("@i_response", SqlDbType.Xml);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = response;

                parametros[2] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[2].Direction = ParameterDirection.Output;

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpRegistraXmlResultadoConciliacion, parametros, string.Empty);
                if (parametros[2].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[2].Value.ToString());

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Mauricio - 20 julio 2014 
        // obtiene los códigos de todos los recaudadores
        public DataSet ObtenerRecaudadoresSinArchivo()
        {
            try
            {
                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpGetAllRecaudador);

                if (dsResult != null)
                {
                    return dsResult;
                }
                return null;
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
                int ret = -1;
                var parametros = new SqlParameter[3];

                parametros[0] = new SqlParameter("@i_idArchivo", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = idArchivo;

                parametros[1] = new SqlParameter("@i_estado", SqlDbType.Int);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = idEstado;

                parametros[2] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[2].Direction = ParameterDirection.Output;

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpActualizaEstadoArchivoMdp, parametros, string.Empty);
                if (parametros[2].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[2].Value.ToString());

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ObtieneTrxTuvesPorEstado(Boolean estado, DateTime fechaTrx)
        {
            try
            {
                var parametros = new SqlParameter[2];

                #region Parametros

                parametros[0] = new SqlParameter("@i_estado", SqlDbType.Bit);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = estado;

                parametros[1] = new SqlParameter("@i_fechaTrx", SqlDbType.Date);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = fechaTrx;

                #endregion Parametros

                var objDatos = new ClsSqlClientHelper();
                return objDatos.DSExecuteQueryStoredProcedure(Constants.SpObtieneTrxTuvesPorEstado, parametros, string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ActualizaEstadoTrxTuves(int idTrxTuves, Boolean estado)
        {
            try
            {
                int ret = -1;
                var parametros = new SqlParameter[3];

                #region Parametros

                parametros[0] = new SqlParameter("@i_idTrxTuves", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = idTrxTuves;

                parametros[1] = new SqlParameter("@i_procesado", SqlDbType.Bit);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = estado;

                parametros[2] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[2].Direction = ParameterDirection.Output;

                #endregion Parametros

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpActualizaEstadoTrxTuves, parametros, string.Empty);
                if (parametros[2].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[2].Value.ToString());

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ActualizaEstadoTrxMdp(int idTrxMdp, Boolean estado)
        {
            try
            {
                int ret = -1;
                var parametros = new SqlParameter[3];

                #region Parametros

                parametros[0] = new SqlParameter("@i_idTrxMdp", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = idTrxMdp;

                parametros[1] = new SqlParameter("@i_procesado", SqlDbType.Bit);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = estado;

                parametros[2] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[2].Direction = ParameterDirection.Output;

                #endregion Parametros

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpActualizaEstadoTrxMdp, parametros, string.Empty);
                if (parametros[2].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[2].Value.ToString());

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int EliminaTrxTuves(string codeMdp, DateTime fechaTrxMdp)
        {
            try
            {
                int ret = -1;
                var parametros = new SqlParameter[3];

                #region Parametros

                parametros[0] = new SqlParameter("@i_fechaTrxMdp", SqlDbType.Date);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = fechaTrxMdp;

                parametros[1] = new SqlParameter("@i_codeMdp", SqlDbType.VarChar, 50);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = codeMdp;

                parametros[2] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[2].Direction = ParameterDirection.Output;

                #endregion Parametros

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpEliminaTrxTuves, parametros, string.Empty);
                if (parametros[2].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[2].Value.ToString());

                return ret;
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
                int ret = -1;
                var parametros = new SqlParameter[3];

                #region Parametros

                parametros[0] = new SqlParameter("@i_fechaTrxMdp", SqlDbType.DateTime);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = fechaTrxMdp;

                parametros[1] = new SqlParameter("@i_codigoMdp", SqlDbType.VarChar, 50);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = codigoMdp;

                parametros[2] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[2].Direction = ParameterDirection.Output;

                #endregion Parametros

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpEliminaTrxMdp, parametros, string.Empty);
                if (parametros[2].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[2].Value.ToString());

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RegistraResultadoConciliacion(DateTime? tuvesFechaTrx, DateTime? tuvesHoraTrx, string tuvesIdTrx, string tuvesIdTrxMdp, decimal tuvesMonto, string tuvesCanal,
            string tuvesCliente, string tuvesProducto, string tuvesUsuarioVenta, int tuvesIdArchivo, DateTime? mdpFechaTrxIni, DateTime? mdpHoraTrxIni, string mdpIdTrxMdp,
            string mdpIdTrxTuves, string mdpIdCliente, string mdpUsuarioVenta, string mdpProducto, decimal mdpMonto, DateTime? mdpFechaTrxFin, DateTime? mdpHoraTrxFin,
            string mdpCodigoMdp, int mdpIdArchivo, bool existeTrxMdp, bool existeTrxTuves, int estadoGeneralTrx, int idRecaudador, int idArchivo, bool registroIntermix, int idEstadoConciliacion, DateTime fechaTrxProceso)
        {
            try
            {
                int ret = -1;
                var parametros = new SqlParameter[31];

                
                #region Parametros
                if (!tuvesFechaTrx.HasValue)
                {
                    parametros[0] = new SqlParameter("@i_tuvFecha", SqlDbType.DateTime);
                    parametros[0].Direction = ParameterDirection.Input;
                    parametros[0].Value = DBNull.Value;
                }
                else
                {
                    parametros[0] = new SqlParameter("@i_tuvFecha", SqlDbType.DateTime);
                    parametros[0].Direction = ParameterDirection.Input;
                    parametros[0].Value = tuvesFechaTrx;
                }

                //parametros[0] = new SqlParameter("@i_tuvFecha", SqlDbType.DateTime);
                //parametros[0].Direction = ParameterDirection.Input;
                //parametros[0].Value = tuvesFechaTrx;

                if (!tuvesHoraTrx.HasValue)
                {
                    parametros[1] = new SqlParameter("@i_tuvHota", SqlDbType.DateTime);
                    parametros[1].Direction = ParameterDirection.Input;
                    parametros[1].Value = DBNull.Value;
                }
                else
                {
                    parametros[1] = new SqlParameter("@i_tuvHota", SqlDbType.DateTime);
                    parametros[1].Direction = ParameterDirection.Input;
                    parametros[1].Value = tuvesHoraTrx;
                }

                //parametros[1] = new SqlParameter("@i_tuvHota", SqlDbType.DateTime);
                //parametros[1].Direction = ParameterDirection.Input;
                //parametros[1].Value = tuvesHoraTrx;

                parametros[2] = new SqlParameter("@i_tuvIdTrx", SqlDbType.VarChar, 50);
                parametros[2].Direction = ParameterDirection.Input;
                if (tuvesIdTrx == null)
                    parametros[2].Value = System.Data.SqlTypes.SqlString.Null;
                else
                    parametros[2].Value = tuvesIdTrx;

                parametros[3] = new SqlParameter("@i_tuvIdTrxMdp", SqlDbType.VarChar, 50);
                parametros[3].Direction = ParameterDirection.Input;
                if (tuvesIdTrxMdp == null)
                    parametros[3].Value = System.Data.SqlTypes.SqlString.Null;
                else
                    parametros[3].Value = tuvesIdTrxMdp;

                parametros[4] = new SqlParameter("@i_tuvMonto", SqlDbType.Decimal);
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].Value = tuvesMonto;

                parametros[5] = new SqlParameter("@i_tuvCanal", SqlDbType.VarChar, 50);
                parametros[5].Direction = ParameterDirection.Input;
                //parametros[5].Value = tuvesCanal;
                if (tuvesCanal == null)
                    parametros[5].Value = System.Data.SqlTypes.SqlString.Null;
                else
                    parametros[5].Value = tuvesCanal;

                parametros[6] = new SqlParameter("@i_tuvCliente", SqlDbType.VarChar, 50);
                parametros[6].Direction = ParameterDirection.Input;
                //parametros[6].Value = tuvesCliente;
                if (tuvesCliente == null)
                    parametros[6].Value = System.Data.SqlTypes.SqlString.Null;
                else
                    parametros[6].Value = tuvesCliente;

                parametros[7] = new SqlParameter("@i_tuvProducto", SqlDbType.VarChar, 50);
                parametros[7].Direction = ParameterDirection.Input;
                //parametros[7].Value = tuvesProducto;
                if (tuvesProducto == null)
                    parametros[7].Value = System.Data.SqlTypes.SqlString.Null;
                else
                    parametros[7].Value = tuvesProducto;

                parametros[8] = new SqlParameter("@i_tuvUsuarioVenta", SqlDbType.VarChar, 50);
                parametros[8].Direction = ParameterDirection.Input;
                //parametros[8].Value = tuvesUsuarioVenta;
                if (tuvesUsuarioVenta == null)
                    parametros[8].Value = System.Data.SqlTypes.SqlString.Null;
                else
                    parametros[8].Value = tuvesUsuarioVenta;

                parametros[9] = new SqlParameter("@i_tuvIdArchivo", SqlDbType.Int);
                parametros[9].Direction = ParameterDirection.Input;
                parametros[9].Value = tuvesIdArchivo;

                if (!mdpFechaTrxIni.HasValue)
                {
                    parametros[10] = new SqlParameter("@i_mdpFechaIni", SqlDbType.DateTime);
                    parametros[10].Direction = ParameterDirection.Input;
                    parametros[10].Value = DBNull.Value;
                }
                else
                {
                    parametros[10] = new SqlParameter("@i_mdpFechaIni", SqlDbType.DateTime);
                    parametros[10].Direction = ParameterDirection.Input;
                    parametros[10].Value = mdpFechaTrxIni;
                }

                if (!mdpHoraTrxIni.HasValue)
                {
                    parametros[11] = new SqlParameter("@i_mdpHoraIni", SqlDbType.DateTime);
                    parametros[11].Direction = ParameterDirection.Input;
                    parametros[11].Value = DBNull.Value;
                }
                else
                {
                    parametros[11] = new SqlParameter("@i_mdpHoraIni", SqlDbType.DateTime);
                    parametros[11].Direction = ParameterDirection.Input;
                    parametros[11].Value = mdpHoraTrxIni;
                }

                parametros[12] = new SqlParameter("@i_mdpIdTrxMdp", SqlDbType.VarChar, 50);
                parametros[12].Direction = ParameterDirection.Input;
                //parametros[12].Value = mdpIdTrxMdp;
                if (mdpIdTrxMdp == null)
                    parametros[12].Value = System.Data.SqlTypes.SqlString.Null;
                else
                    parametros[12].Value = mdpIdTrxMdp;

                parametros[13] = new SqlParameter("@i_mdpIdTrxTuves", SqlDbType.VarChar, 50);
                parametros[13].Direction = ParameterDirection.Input;
                //parametros[13].Value = mdpIdTrxTuves;
                if (mdpIdTrxTuves == null)
                    parametros[13].Value = System.Data.SqlTypes.SqlString.Null;
                else
                    parametros[13].Value = mdpIdTrxTuves;

                parametros[14] = new SqlParameter("@i_mdpClienteId", SqlDbType.VarChar, 50);
                parametros[14].Direction = ParameterDirection.Input;
                //parametros[14].Value = mdpIdCliente;
                if (mdpIdCliente == null)
                    parametros[14].Value = System.Data.SqlTypes.SqlString.Null;
                else
                    parametros[14].Value = mdpIdCliente;

                parametros[15] = new SqlParameter("@i_mdpUsuarioVenta", SqlDbType.VarChar, 50);
                parametros[15].Direction = ParameterDirection.Input;
                //parametros[15].Value = mdpUsuarioVenta;
                if (mdpUsuarioVenta == null)
                    parametros[15].Value = System.Data.SqlTypes.SqlString.Null;
                else
                    parametros[15].Value = mdpUsuarioVenta;

                parametros[16] = new SqlParameter("@i_mdpProducto", SqlDbType.VarChar, 50);
                parametros[16].Direction = ParameterDirection.Input;
                //parametros[16].Value = mdpProducto;
                if (mdpProducto == null)
                    parametros[16].Value = System.Data.SqlTypes.SqlString.Null;
                else
                    parametros[16].Value = mdpProducto;

                parametros[17] = new SqlParameter("@i_mdpMonto", SqlDbType.Decimal);
                parametros[17].Direction = ParameterDirection.Input;
                parametros[17].Value = mdpMonto;

                if (!mdpFechaTrxFin.HasValue)
                {
                    parametros[18] = new SqlParameter("@i_mdpFechaFin", SqlDbType.DateTime);
                    parametros[18].Direction = ParameterDirection.Input;
                    parametros[18].Value = DBNull.Value;
                }
                else
                {
                    parametros[18] = new SqlParameter("@i_mdpFechaFin", SqlDbType.DateTime);
                    parametros[18].Direction = ParameterDirection.Input;
                    parametros[18].Value = mdpFechaTrxFin;
                }

                if (!mdpHoraTrxFin.HasValue)
                {
                    parametros[19] = new SqlParameter("@i_mdpHoraFin", SqlDbType.DateTime);
                    parametros[19].Direction = ParameterDirection.Input;
                    parametros[19].Value = DBNull.Value;
                }
                else
                {
                    parametros[19] = new SqlParameter("@i_mdpHoraFin", SqlDbType.DateTime);
                    parametros[19].Direction = ParameterDirection.Input;
                    parametros[19].Value = mdpHoraTrxFin;
                }
                
                parametros[20] = new SqlParameter("@i_mdpCodigoMdp", SqlDbType.VarChar, 50);
                parametros[20].Direction = ParameterDirection.Input;
                //parametros[20].Value = mdpCodigoMdp;
                if (mdpCodigoMdp == null)
                    parametros[20].Value = System.Data.SqlTypes.SqlString.Null;
                else
                    parametros[20].Value = mdpCodigoMdp;

                parametros[21] = new SqlParameter("@i_mdpIdArchivo", SqlDbType.Int);
                parametros[21].Direction = ParameterDirection.Input;
                parametros[21].Value = mdpIdArchivo;

                parametros[22] = new SqlParameter("@i_resExisteMdp", SqlDbType.Bit);
                parametros[22].Direction = ParameterDirection.Input;
                parametros[22].Value = existeTrxMdp;

                parametros[23] = new SqlParameter("@i_resExisteTuves", SqlDbType.Bit);
                parametros[23].Direction = ParameterDirection.Input;
                parametros[23].Value = existeTrxTuves;

                parametros[24] = new SqlParameter("@i_resEstadoGen", SqlDbType.Int);
                parametros[24].Direction = ParameterDirection.Input;
                parametros[24].Value = estadoGeneralTrx;

                parametros[25] = new SqlParameter("@i_IdRecaudador", SqlDbType.Int);
                parametros[25].Direction = ParameterDirection.Input;
                parametros[25].Value = idRecaudador;

                parametros[26] = new SqlParameter("@i_idArchivo", SqlDbType.Int);
                parametros[26].Direction = ParameterDirection.Input;
                parametros[26].Value = idArchivo;

                parametros[27] = new SqlParameter("@i_registraIntermix", SqlDbType.Bit);
                parametros[27].Direction = ParameterDirection.Input;
                parametros[27].Value = registroIntermix;

                parametros[28] = new SqlParameter("@i_estadoConciliacion", SqlDbType.Int);
                parametros[28].Direction = ParameterDirection.Input;
                parametros[28].Value = idEstadoConciliacion;

                parametros[29] = new SqlParameter("@i_fechaTrxProceso", SqlDbType.DateTime);
                parametros[29].Direction = ParameterDirection.Input;
                parametros[29].Value = fechaTrxProceso;

                parametros[30] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[30].Direction = ParameterDirection.Output;

                #endregion Parametros

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpRegistraResultadoConciliacion, parametros, string.Empty);
                if (parametros[30].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[30].Value.ToString());

                return ret;
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
                var parametros = new SqlParameter[1];
                parametros[0] = new SqlParameter("@i_idArchivo", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = idArchivo;

                var objDatos = new ClsSqlClientHelper();
                return objDatos.DSExecuteQueryStoredProcedure(Constants.SpObtienePagosTuvesPorIdArchivo, parametros, Constants.DatasetArchivos);
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
                var parametros = new SqlParameter[1];
                parametros[0] = new SqlParameter("@i_idArchivo", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = idArchivo;

                var objDatos = new ClsSqlClientHelper();
                return objDatos.DSExecuteQueryStoredProcedure(Constants.SpObtienePagosMdpPorIdArchivo, parametros, Constants.DatasetArchivos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RegistraPagoTuvez(string idMdp, string codigoMdp, DateTime fecha, DateTime hora, string idTransaccionTuves,
            string idTrxMdp, decimal monto, string canal, int trxProcesado, string idCliente, string producto, string usuarioVenta,
            int idArchivoMdp, string tuvCodigoMdp)
        {
            try
            {
                int ret = -1;
                var parametros = new SqlParameter[15];

                parametros[0] = new SqlParameter("@i_idMdp", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = idMdp;

                parametros[1] = new SqlParameter("@i_codigoMdp", SqlDbType.VarChar, 50);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = codigoMdp;

                parametros[2] = new SqlParameter("@i_fecha", SqlDbType.DateTime);
                parametros[2].Direction = ParameterDirection.Input;
                parametros[2].Value = fecha;

                parametros[3] = new SqlParameter("@i_hora", SqlDbType.DateTime);
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].Value = hora;

                parametros[4] = new SqlParameter("@i_idTrxTvcable", SqlDbType.VarChar, 50);
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].Value = idTransaccionTuves;

                parametros[5] = new SqlParameter("@i_idTrxMdp", SqlDbType.VarChar, 50);
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].Value = idTrxMdp;

                parametros[6] = new SqlParameter("@i_monto", SqlDbType.Decimal);
                parametros[6].Direction = ParameterDirection.Input;
                parametros[6].Value = monto;

                parametros[7] = new SqlParameter("@i_canal", SqlDbType.VarChar, 10);
                parametros[7].Direction = ParameterDirection.Input;
                parametros[7].Value = canal;

                parametros[8] = new SqlParameter("@i_trxProcesado", SqlDbType.Int);
                parametros[8].Direction = ParameterDirection.Input;
                parametros[8].Value = trxProcesado;

                parametros[9] = new SqlParameter("@i_cliente", SqlDbType.VarChar,50);
                parametros[9].Direction = ParameterDirection.Input;
                parametros[9].Value = idCliente;

                parametros[10] = new SqlParameter("@i_producto", SqlDbType.VarChar, 50);
                parametros[10].Direction = ParameterDirection.Input;
                parametros[10].Value = producto;

                parametros[11] = new SqlParameter("@i_usuarioVenta", SqlDbType.VarChar, 50);
                parametros[11].Direction = ParameterDirection.Input;
                parametros[11].Value = usuarioVenta;

                parametros[12] = new SqlParameter("@i_idArchivoMdp", SqlDbType.Int);
                parametros[12].Direction = ParameterDirection.Input;
                parametros[12].Value = idArchivoMdp;

                parametros[13] = new SqlParameter("@i_tuvesCodigoMdp", SqlDbType.VarChar, 50);
                parametros[13].Direction = ParameterDirection.Input;
                parametros[13].Value = tuvCodigoMdp;

                parametros[14] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[14].Direction = ParameterDirection.Output;

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpRegistraDetallePagoTuves, parametros, string.Empty);
                if (parametros[14].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[14].Value.ToString());

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RegistraPagoMdp(string codigoMdp, DateTime fechaInicio, DateTime horaInicio, string idTransaccionMdp,
            string idTrxTvcable, string idCliente, string usuarioVenta, string producto, decimal monto, DateTime fechaFin, DateTime horaFin, int idEstadoPago, int idArchivoMdp)
        {
            try
            {
                int ret = -1;

                var parametros = new SqlParameter[14];

                parametros[0] = new SqlParameter("@i_codigoMdp", SqlDbType.VarChar, 50);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = codigoMdp;

                parametros[1] = new SqlParameter("@i_fechaInicio", SqlDbType.DateTime);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = fechaInicio;

                parametros[2] = new SqlParameter("@i_horaInicio", SqlDbType.DateTime);
                parametros[2].Direction = ParameterDirection.Input;
                parametros[2].Value = horaInicio;

                parametros[3] = new SqlParameter("@i_idTrxMdp", SqlDbType.VarChar, 50);
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].Value = idTransaccionMdp;

                parametros[4] = new SqlParameter("@i_idTrxTvcable", SqlDbType.VarChar, 50);
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].Value = idTrxTvcable;

                parametros[5] = new SqlParameter("@i_idCliente", SqlDbType.VarChar, 50);
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].Value = idCliente;

                parametros[6] = new SqlParameter("@i_usuarioVenta", SqlDbType.VarChar, 50);
                parametros[6].Direction = ParameterDirection.Input;
                parametros[6].Value = usuarioVenta;

                parametros[7] = new SqlParameter("@i_producto", SqlDbType.VarChar, 50);
                parametros[7].Direction = ParameterDirection.Input;
                parametros[7].Value = producto;

                parametros[8] = new SqlParameter("@i_monto", SqlDbType.Decimal);
                parametros[8].Direction = ParameterDirection.Input;
                parametros[8].Value = monto;

                parametros[9] = new SqlParameter("@i_fechaFin", SqlDbType.DateTime);
                parametros[9].Direction = ParameterDirection.Input;
                parametros[9].Value = fechaFin;

                parametros[10] = new SqlParameter("@i_horaFin", SqlDbType.DateTime);
                parametros[10].Direction = ParameterDirection.Input;
                parametros[10].Value = horaFin;

                parametros[11] = new SqlParameter("@i_idEstadoPago", SqlDbType.Int);
                parametros[11].Direction = ParameterDirection.Input;
                parametros[11].Value = idEstadoPago;

                parametros[12] = new SqlParameter("@i_idArchivoMdp", SqlDbType.Int);
                parametros[12].Direction = ParameterDirection.Input;
                parametros[12].Value = idArchivoMdp;

                parametros[13] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[13].Direction = ParameterDirection.Output;

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpRegistraDetallePago, parametros, string.Empty);
                if (parametros[13].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[13].Value.ToString());

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene el detalle de un archivo del MDP por ID del archivo
        /// </summary>
        /// <param name="idArchivo"></param>
        /// <returns></returns>
        public DataSet ObtieneDetalleArchivoMdpPorId(int idArchivo)
        {
            try
            {
                var parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@i_idArchivo", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = idArchivo;

                var objDatos = new ClsSqlClientHelper();
                return objDatos.DSExecuteQueryStoredProcedure(Constants.SpObtieneDetalleArchivoPorIdArchivo, parametros, Constants.DatasetArchivos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza los datos del archivo de conciliacion: Estado y el archivo.
        /// </summary>
        /// <param name="idArchivo">idArchivo</param>
        /// <param name="idEstado">idEstado</param>
        /// <param name="archivo">archivo</param>
        /// <returns></returns>
        public int ActualizaDatosArchivo(int idArchivo, int idEstado, byte[] archivo)
        {
            try
            {
                int ret = -1;
                var parametros = new SqlParameter[4];

                parametros[0] = new SqlParameter("@i_idArchivo", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = idArchivo;

                parametros[1] = new SqlParameter("@i_estado", SqlDbType.Int);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = idEstado;

                parametros[2] = new SqlParameter("@i_file", SqlDbType.Binary);
                parametros[2].Direction = ParameterDirection.Input;
                parametros[2].Value = archivo;

                parametros[3] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[3].Direction = ParameterDirection.Output;

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpActualizaEstadoArchivo, parametros, string.Empty);
                if (parametros[3].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[3].Value.ToString());

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene la lista de archivos de conciliacion por estado
        /// </summary>
        /// <param name="idEstado">Id del estado del archivo (catalogo)</param>
        /// <returns>Dataset</returns>
        public DataSet ObtenerArchivoConciliacionPorEstado(int idEstado)
        {
            try
            {
                DataSet dsArchivos = null;
                var parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@i_estadoId", SqlDbType.Int, 8);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = idEstado;

                var objDatos = new ClsSqlClientHelper();
                dsArchivos = objDatos.DSExecuteQueryStoredProcedure(Constants.SpObtieneArchivoConciliacionPorEstado, parametros, Constants.DatasetArchivos);
                return dsArchivos;
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
        public int RegistraEntregaArchivoConciliacionMdp(string nombreArchivo, DateTime fechaTransacciones, string codigoMdp,
            int numTransacciones, decimal montoTotal, int estadoArchivo, string mailNotificacion, string observaciones, DateTime fechaRegistro)
        {
            try
            {
                int ret = -1;

                var parametros = new SqlParameter[10];

                parametros[0] = new SqlParameter("@i_nombreFile", SqlDbType.VarChar, 80);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = nombreArchivo;

                parametros[1] = new SqlParameter("@i_fechaTransacciones", SqlDbType.DateTime);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = fechaTransacciones;

                parametros[2] = new SqlParameter("@i_codigoMdp", SqlDbType.VarChar);
                parametros[2].Direction = ParameterDirection.Input;
                parametros[2].Value = codigoMdp;

                parametros[3] = new SqlParameter("@i_numTransacciones", SqlDbType.Int);
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].Value = numTransacciones;

                parametros[4] = new SqlParameter("@i_montoTotal", SqlDbType.Decimal);
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].Value = montoTotal;

                parametros[5] = new SqlParameter("@i_estadoId", SqlDbType.Int);
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].Value = estadoArchivo;

                parametros[6] = new SqlParameter("@i_mailNotificacion", SqlDbType.VarChar);
                parametros[6].Direction = ParameterDirection.Input;
                parametros[6].Value = mailNotificacion;

                parametros[7] = new SqlParameter("@i_observacion", SqlDbType.VarChar);
                parametros[7].Direction = ParameterDirection.Input;
                parametros[7].Value = observaciones;

                parametros[8] = new SqlParameter("@i_fechaRegistro", SqlDbType.DateTime);
                parametros[8].Direction = ParameterDirection.Input;
                parametros[8].Value = fechaRegistro;

                parametros[9] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[9].Direction = ParameterDirection.Output;

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpInsertaArchivoConciliacionMdp, parametros, string.Empty);
                if (parametros[9].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[9].Value.ToString());

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Consulta a la tabla de conciliación y según sus parámetros de entrada devuelve un dataset
        /// </summary>
        /// <param name="recaudador">parametro tipo recaudador</param>
        /// <param name="fechaInicio">parametro fecha inicio</param>
        /// <param name="fechaFin">parametro fecha fin</param>
        /// <param name="tipoConciliacion">parametro tipo de conciliacion</param>
        /// <returns>retorna un dataset con la consulta realizada</returns>
        public DataSet GetDataConciliacion(int recaudador, DateTime fechaInicio, DateTime fechaFin, int tipoConciliacion)
        {
            try
            {
                var parametros = new SqlParameter[4];

                parametros[0] = new SqlParameter("@i_recaudador", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = recaudador;

                parametros[1] = new SqlParameter("@i_fecha_inicio", SqlDbType.DateTime);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = fechaInicio;

                parametros[2] = new SqlParameter("@i_fecha_fin", SqlDbType.DateTime);
                parametros[2].Direction = ParameterDirection.Input;
                parametros[2].Value = fechaFin;

                parametros[3] = new SqlParameter("@i_tipo_conciliacion", SqlDbType.Int);
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].Value = tipoConciliacion;

                /*
                var prmParametros = new SqlParameter[]{
                    string.IsNullOrEmpty(recaudador) ? new SqlParameter("@i_recaudador",  DBNull.Value) : new SqlParameter("@i_recaudador",  recaudador),
                    string.IsNullOrEmpty(fechaInicio) ? new SqlParameter("@i_fecha_inicio	",  DBNull.Value) : new SqlParameter("@i_fecha_inicio",  fechaInicio),
                    string.IsNullOrEmpty(fechaFin) ? new SqlParameter("@i_fecha_fin",  DBNull.Value) : new SqlParameter("@i_fecha_fin",  fechaFin) ,
                    string.IsNullOrEmpty(tipoConciliacion) ? new SqlParameter("@i_tipo_conciliacion",  DBNull.Value) : new SqlParameter("@i_tipo_conciliacion",  tipoConciliacion)
                };
                */

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpGetDataConciliacion, parametros, string.Empty);
                return dsResult;
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
                var prmParametros = new SqlParameter[]{
                    string.IsNullOrEmpty(usuario) ? new SqlParameter("@i_usuario",  DBNull.Value) : new SqlParameter("@i_usuario",  usuario),
                    string.IsNullOrEmpty(rol) ? new SqlParameter("@i_rol	",  DBNull.Value) : new SqlParameter("@i_rol",  rol),
                    };
                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpUsuarioFuncionalidades, prmParametros, string.Empty);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Trae el la empresa facturadora a partir de su id
        /// </summary>
        /// <param name="idfacturadora"></param>
        /// <returns></returns>
        public DataSet GetDataEmpresaFacturadoraId(string idfacturadora)
        {
            try
            {
                var prmParametros = new SqlParameter[]{
                    string.IsNullOrEmpty(idfacturadora) ? new SqlParameter("@i_idfacturadora",  DBNull.Value) : new SqlParameter("@i_idfacturadora",  idfacturadora),
                     };
                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpGetDataEmpresaFacturadoraId, prmParametros, string.Empty);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Se obtiene el recaudador por ID
        /// </summary>
        /// <param name="idrecaudado">Id Recaudador</param>
        /// <returns></returns>
        public DataSet GetDataReacaudadorId(string idrecaudado)
        {
            try
            {
                var parametros = new SqlParameter[1];

                parametros[0] = new SqlParameter("@i_idrecaudador", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = idrecaudado;

                var objDatos = new ClsSqlClientHelper();
                return objDatos.DSExecuteQueryStoredProcedure(Constants.SpGetDataReacaudadorId, parametros, Constants.DatasetArchivos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Devuelve  toda la lista de recaudadores
        /// </summary>
        /// <returns>Retorna un data set con la lista de recaudadores</returns>
        public DataSet GetDataRecaudador()
        {
            try
            {
                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpGetDataRecaudador);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Devuelve  toda la lista de empresa facturadora
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataEmpresaFacturadora()
        {
            try
            {
                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpGetDataEmpresaFacturadora);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Devuelve listado de todos los tipos de conciliacion en el item catalogo
        /// </summary>
        /// <param name="estconciliacion">id del catalogo que corresponde a catalogo</param>
        /// <returns>Retorna un data con el listado de todos los item de tipo catalogo conciliado</returns>
        public DataSet GetDatListadoConciliacion(string estconciliacion)
        {
            try
            {
                var prmParametros = new SqlParameter[]{
                    string.IsNullOrEmpty(estconciliacion) ? new SqlParameter("@i_estconciliaciono",  DBNull.Value) : new SqlParameter("@i_estconciliacion",estconciliacion),
                    };
                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpGetDataListadoConciliacion, prmParametros, string.Empty);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Devuelve listado de los estados de pago
        /// </summary>
        /// <param name="catid">id del catalogo que corresponde a estados de pago</param>
        /// <returns>Devuelve un dataset listando de los estados de pago</returns>
        public DataSet GetDataEstadoConciliacion(string idcon)
        {
            try
            {
                var prmParametros = new SqlParameter[]{
                    string.IsNullOrEmpty(idcon) ? new SqlParameter("@i_codestadopadre",  DBNull.Value) : new SqlParameter("@i_codestadopadre",idcon),
                    };
                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpGetDataEstadoConciliacion, prmParametros, string.Empty);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Devuelve la consulta de conciliación por tipo de conciliacion
        /// </summary>
        /// <param name="tipo_conciliacion">id de tipo de catalogo</param>
        /// <returns>Retorna un data con la conciliacion por tipo</returns>
        public DataSet GetDataConciliacionPorTipo(string tipo_conciliacion)
        {
            try
            {
                var prmParametros = new SqlParameter[]{
                    string.IsNullOrEmpty(tipo_conciliacion) ? new SqlParameter("@i_tipoconciliacion",  DBNull.Value) : new SqlParameter("@i_tipoconciliacion",  tipo_conciliacion)
                };

                var objDatos = new ClsSqlClientHelper();

                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpGetDataConciliacionPorTipo, prmParametros, string.Empty);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insertar  el historial de Intermix una vez que se ha insertado en FACTURA_TVC correspondiente a TVCable
        /// </summary>
        /// <param name="resultid"></param>
        /// <param name="resultseq"></param>
        /// <param name="tifechainter"></param>
        /// <param name="usuario"></param>
        /// <param name="monto"></param>
        /// <param name="rolusu"></param>
        /// <param name="observacion"></param>
        /// <returns></returns>
        public int InsertHistorialIntermix(int resultid, int resultseq, DateTime tifechainter, string usuario, string rolusu, string observacion)
        {
            try
            {
                int ret = -1;
                var parametros = new SqlParameter[7];

                parametros[0] = new SqlParameter("@i_resultid", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = resultid;

                parametros[1] = new SqlParameter("@i_resultseq", SqlDbType.Int);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = resultseq;

                parametros[2] = new SqlParameter("@i_fechainter", SqlDbType.DateTime);
                parametros[2].Direction = ParameterDirection.Input;
                parametros[2].Value = tifechainter;

                parametros[3] = new SqlParameter("@i_usuario", SqlDbType.VarChar, 50);
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].Value = usuario;

                parametros[4] = new SqlParameter("@i_rolusu", SqlDbType.VarChar, 20);
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].Value = rolusu;

                parametros[5] = new SqlParameter("@i_observacion", SqlDbType.Text);
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].Value = observacion;

                parametros[6] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[6].Direction = ParameterDirection.Output;

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpInsertHistorialIntermix, parametros, string.Empty);
                if (parametros[6].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[6].Value.ToString());

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Aactualiza el estado de los pagos
        /// </summary>
        /// <param name="tipoconciliacion">parametro tipo de conciliacion</param>
        /// <param name="estadopago">parametro estado de pago a ser actualizado</param>
        /// <returns>retorna el numero de campos afectados</returns>
        public int UpdateEstado(int tipoconciliacion, int estadonuevo)
        {
            try
            {
                int ret = -1;
                var parametros = new SqlParameter[3];

                parametros[0] = new SqlParameter("@i_tipoconciliacion", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = tipoconciliacion;

                parametros[1] = new SqlParameter("@i_estadonuevoconciliacion", SqlDbType.Int);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = estadonuevo;

                parametros[2] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[2].Direction = ParameterDirection.Output;

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpUpdateEstado, parametros, string.Empty);
                if (parametros[2].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[2].Value.ToString());

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza los conciliados a rechazados
        /// </summary>
        /// <param name="idrescon">id del campo que va a ser actualizado</param>
        ///  <param name="idtipoco">id correspondiente a rechazado</param>
        /// <returns>retorna el numero de campos afectados</returns>
        public int UpdateRechazado(string idcatalogo, int idupdate)
        {
            try
            {
                int ret = -1;
                var parametros = new SqlParameter[3];

                parametros[0] = new SqlParameter("@i_idnomcatalogo", SqlDbType.VarChar, 20);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = idcatalogo;

                parametros[1] = new SqlParameter("@i_idupdate", SqlDbType.Int);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = idupdate;

                parametros[2] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[2].Direction = ParameterDirection.Output;

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpUpdateRechazado, parametros, string.Empty);
                if (parametros[2].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[2].Value.ToString());

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza el estado de resultado conciliado una vez que los registros ya han sido calculados, verificados y  pasados al Intermix
        /// </summary>
        /// <param name="idconciliacion">id conciliacion</param>
        /// <param name="estadoconci">Estado de la conciliación si ya ha sido o no insertado en Itermix</param>
        /// <returns></returns>
        public int UpdateConciliacionPorIntermix(int idconciliacion, string estadoconci)
        {
            try
            {
                int ret = -1;
                var parametros = new SqlParameter[3];

                parametros[0] = new SqlParameter("@i_idconcilicacion", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = idconciliacion;

                parametros[1] = new SqlParameter("@i_estadointerx", SqlDbType.NVarChar, 18);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = estadoconci;

                parametros[2] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[2].Direction = ParameterDirection.Output;

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpUpdateConciliacionPorIntermix, parametros, string.Empty);
                if (parametros[2].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[2].Value.ToString());

                return ret;
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
                int ret = -1;
                var parametros = new SqlParameter[7];

                parametros[0] = new SqlParameter("@i_idconciliacion", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = idconciliacion;

                parametros[1] = new SqlParameter("@i_idresultadocon", SqlDbType.Int);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = idresultadocon;

                parametros[2] = new SqlParameter("@i_fecha", SqlDbType.DateTime);
                parametros[2].Direction = ParameterDirection.Input;
                parametros[2].Value = fecha;

                parametros[3] = new SqlParameter("@i_idusuario", SqlDbType.VarChar, 20);
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].Value = dusuari;

                parametros[4] = new SqlParameter("@i_usuario", SqlDbType.VarChar, 20);
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].Value = usuario;

                parametros[5] = new SqlParameter("@i_observacion", SqlDbType.Text);
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].Value = observacion;

                parametros[6] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[6].Direction = ParameterDirection.Output;

                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpInsertHistorialEstado, parametros, string.Empty);
                if (parametros[6].Value != System.DBNull.Value)
                    return Convert.ToInt32(parametros[6].Value.ToString());

                return ret;
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
                var prmParametros = new SqlParameter[]{
                    string.IsNullOrEmpty(idconciliacion) ? new SqlParameter("@i_idconciliacion",  DBNull.Value) : new SqlParameter("@i_idconciliacion",  idconciliacion),
                    string.IsNullOrEmpty(idestado) ? new SqlParameter("@i_idestado	",  DBNull.Value) : new SqlParameter("@i_idestado",  idestado),
                   };
                var objDatos = new ClsSqlClientHelper();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpGetDataHistorialPorId, prmParametros, string.Empty);
                return dsResult;
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
                int ret = -1;
                var parametros = new SqlParameter[25];

                parametros[0] = new SqlParameter("@i_id", SqlDbType.Int);
                parametros[0].Direction = ParameterDirection.Input;
                parametros[0].Value = idconcilrec;

                parametros[1] = new SqlParameter("@i_codempresa", SqlDbType.Int);
                parametros[1].Direction = ParameterDirection.Input;
                parametros[1].Value = codempresa;

                parametros[2] = new SqlParameter("@i_codpuntoemision", SqlDbType.NVarChar, 10);
                parametros[2].Direction = ParameterDirection.Input;
                parametros[2].Value = codpuntoemision;

                parametros[3] = new SqlParameter("@i_ruc", SqlDbType.NVarChar, 15);
                parametros[3].Direction = ParameterDirection.Input;
                parametros[3].Value = ruc;

                parametros[4] = new SqlParameter("@i_tipocliente", SqlDbType.Int);
                parametros[4].Direction = ParameterDirection.Input;
                parametros[4].Value = tipocliente;

                parametros[5] = new SqlParameter("@i_telefono", SqlDbType.NVarChar, 20);
                parametros[5].Direction = ParameterDirection.Input;
                parametros[5].Value = telefono;

                parametros[6] = new SqlParameter("@i_tipopersona", SqlDbType.Int);
                parametros[6].Direction = ParameterDirection.Input;
                parametros[6].Value = tipopersona;

                parametros[7] = new SqlParameter("@i_tipocontribuyente", SqlDbType.Int);
                parametros[7].Direction = ParameterDirection.Input;
                parametros[7].Value = tipocontribuyente;

                parametros[8] = new SqlParameter("@i_nombre", SqlDbType.NVarChar, 75);
                parametros[8].Direction = ParameterDirection.Input;
                parametros[8].Value = nombre;

                parametros[9] = new SqlParameter("@i_direccion", SqlDbType.NVarChar, 75);
                parametros[9].Direction = ParameterDirection.Input;
                parametros[9].Value = direccion;

                parametros[10] = new SqlParameter("@i_email", SqlDbType.NVarChar, 20);
                parametros[10].Direction = ParameterDirection.Input;
                parametros[10].Value = email;

                parametros[11] = new SqlParameter("@i_coditem", SqlDbType.NVarChar, 15);
                parametros[11].Direction = ParameterDirection.Input;
                parametros[11].Value = coditem;

                parametros[12] = new SqlParameter("@i_valorneto", SqlDbType.Decimal);
                parametros[12].Direction = ParameterDirection.Input;
                parametros[12].Value = valorneto;

                parametros[13] = new SqlParameter("@i_base_ice", SqlDbType.Decimal);
                parametros[13].Direction = ParameterDirection.Input;
                parametros[13].Value = base_ice;

                parametros[14] = new SqlParameter("@i_valorice", SqlDbType.Decimal);
                parametros[14].Direction = ParameterDirection.Input;
                parametros[14].Value = valorice;

                parametros[15] = new SqlParameter("@i_baseiva", SqlDbType.Decimal);
                parametros[15].Direction = ParameterDirection.Input;
                parametros[15].Value = baseiva;

                parametros[16] = new SqlParameter("@i_valoriva", SqlDbType.Decimal);
                parametros[16].Direction = ParameterDirection.Input;
                parametros[16].Value = valoriva;

                parametros[17] = new SqlParameter("@i_valortotal", SqlDbType.Decimal);
                parametros[17].Direction = ParameterDirection.Input;
                parametros[17].Value = valortotal;

                parametros[18] = new SqlParameter("@i_fechaproceso", SqlDbType.SmallDateTime);
                parametros[18].Direction = ParameterDirection.Input;
                parametros[18].Value = fechaproceso;

                parametros[19] = new SqlParameter("@i_fechaemision", SqlDbType.SmallDateTime);
                parametros[19].Direction = ParameterDirection.Input;
                parametros[19].Value = fechaemision;

                parametros[20] = new SqlParameter("@i_fechadesde", SqlDbType.SmallDateTime);
                parametros[20].Direction = ParameterDirection.Input;
                parametros[20].Value = fechadesde;

                parametros[21] = new SqlParameter("@i_fechahasta", SqlDbType.SmallDateTime);
                parametros[21].Direction = ParameterDirection.Input;
                parametros[21].Value = fechahasta;

                parametros[22] = new SqlParameter("@i_comentario", SqlDbType.NVarChar, 255);
                parametros[22].Direction = ParameterDirection.Input;
                parametros[22].Value = comentario;

                parametros[23] = new SqlParameter("@i_factura", SqlDbType.Int);
                parametros[23].Direction = ParameterDirection.Input;
                parametros[23].Value = factura;

                parametros[24] = new SqlParameter("@o_return", SqlDbType.Int);
                parametros[24].Direction = ParameterDirection.Output;

                var objDatos = new ClsSqlClientHelperIntermix();
                var dsResult = objDatos.DSExecuteQueryStoredProcedure(Constants.SpInsertIntermix, parametros, string.Empty);
                if (parametros[24].Value != System.DBNull.Value)

                    return Convert.ToInt32(parametros[24].Value.ToString());

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion IDisposable Members

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion IDisposable Members
    }
}