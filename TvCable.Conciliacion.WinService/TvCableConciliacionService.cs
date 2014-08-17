using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using TvCable.Conciliacion.DTO;
using TvCable.Conciliacion.Libs;
using TvCable.Conciliacion.WinService.TvCableConciliacionWebService;

namespace TvCable.Conciliacion.WinService
{
    public partial class TvCableConciliacionService : ServiceBase
    {
        #region Variables

        //Mauricio - 4 de agosto de 2014
        //private const double DefaultInterval = 30000;
        private string DefaultInterval = ConfigurationManager.AppSettings["intervaloEjecucionServicioMiliSegundos"] != null ? "30000" : "30000";
        private const string saltoLinea = "<br />";
        private const string negrilla = "<B>";
        private const string cierreNegrilla = "</B>";
        private const string linea = "<HR>";
        private const string lineaAzul = "<hr color='blue' size=3>";
        
        internal string StrHoraInicio = ConfigurationManager.AppSettings["horaInicioServicio"];
        internal string StrHoraHasta = ConfigurationManager.AppSettings["horaFinServicio"];
        //Mauricio - 20 de julio de 2014
        internal string StrHoraInicioFiles = ConfigurationManager.AppSettings["horaInicioServicioFiles"];
        internal string StrHoraHastaFiles = ConfigurationManager.AppSettings["horaFinServicioFiles"];

        internal string SeparadorCamposFile = ConfigurationManager.AppSettings["SeparadorCamposFile"] ?? ";";

        internal string StrDireccionFtp = ConfigurationManager.AppSettings["DireccionFTP"] != null ? (ConfigurationManager.AppSettings["DireccionFTP"]) : "";
        internal string UsuarioAccesoFtp = ConfigurationManager.AppSettings["UsuarioAccesoFTP"] != null ? (ConfigurationManager.AppSettings["UsuarioAccesoFTP"]) : "";
        internal string ClaveAccesoFtp = ConfigurationManager.AppSettings["ClaveAccesoFTP"] != null ? (ConfigurationManager.AppSettings["ClaveAccesoFTP"]) : "";
        internal string PuertoFtp = ConfigurationManager.AppSettings["PuertoAccesoFTP"] != null ? (ConfigurationManager.AppSettings["PuertoAccesoFTP"]) : "";

        internal char SeparatorMails = ConfigurationManager.AppSettings["SeparadorMails"] != null ? Convert.ToChar(ConfigurationManager.AppSettings["SeparadorMails"]) : Convert.ToChar(";");
        internal string FromMailNotification = ConfigurationManager.AppSettings["CuentaCorreoOrigen"].ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        internal string AsuntoMailNotificacion = ConfigurationManager.AppSettings["subjectNotificacionMail"].ToString(CultureInfo.InvariantCulture) ?? "Notificacion TvCable";
        internal string SubjectMailResultConciliacion = ConfigurationManager.AppSettings["bodyNotificacionMail1"].ToString(CultureInfo.InvariantCulture) ?? "";
        internal string MailNotificacionAdmin = ConfigurationManager.AppSettings["cuentaDestinoMail"].ToString(CultureInfo.InvariantCulture) ?? "";

        internal byte[] fileMdp = null;

        internal string StrNameFile = string.Empty;
        internal string StrNameFileErr = string.Empty;
        internal string StateSentFtp = string.Empty;
        internal int IdProcess = 0;
        internal int NumTransacciones = 0;
        internal string StrCabeceraFile = string.Empty;
        internal string StrHeadFile = string.Empty;
        internal string StrBadHeadFile = string.Empty;
        internal string StrErrorTrxFile = string.Empty;

        internal bool ExistTrxErr = false;
        Thread _workerThread;

        internal TvCable.Conciliacion.Libs.Base Base = new Base();
        internal Conciliacion.BusinessLayer.Conciliacion Conciliacion = new BusinessLayer.Conciliacion();
        internal Conciliacion.BusinessLayer.Tuves Tuves = new BusinessLayer.Tuves();
        bool _shouldStop = false;

        #endregion

        public TvCableConciliacionService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        //public void OnStart()
        {
            _workerThread = new Thread(this.DoWork);
            _workerThread.Start();
        }

        public void DoWork()
        {
            while (!_shouldStop)
            {
                try
                {
                    var nucleo = new BusinessLayer.Nucleo();
                    DateTime fechaTransacciones;
                    
                    var catalogoTest = nucleo.GetCatalogoPorCodigoCatalogo("EST_CONCILIACION");
                    
                    // Verifico si el servicio esta dentro de parametro permitido para ejecutar el proceso
                    if (checkHorario(StrHoraInicio, StrHoraHasta))
                    {
                        #region Inicia el proceso de conciliacion de TvCable
                        // Testing save Logs
                        Base.WriteLog(Base.ErrorTypeEnum.Start, 100, " --------------------------------------------");
                        Base.WriteLog(Base.ErrorTypeEnum.Start, 100, "Inicia el proceso de conciliacion de TvCable.");

                        // Verifica si existen archivos de conciliacion de los MDP pendientes de procesar(POR_PROCESAR).
                        var itemPorProcesar = nucleo.GetItemCatalogoPorCodigoItemCodigoCatalogo(Constants.CodeItemPorProcesar, Constants.CodeEstadoArchivosConciliacion);
                        var itemProcesandoArchivo = nucleo.GetItemCatalogoPorCodigoItemCodigoCatalogo(Constants.CodeItemProcesando, Constants.CodeEstadoArchivosConciliacion);
                        var itemProcesadoArchivo = nucleo.GetItemCatalogoPorCodigoItemCodigoCatalogo(Constants.CodeItemProcesado, Constants.CodeEstadoArchivosConciliacion);


                        // Archivos disponibles por procesar
                        var archivosDisponibles = Conciliacion.ObtenerArchivoConciliacionPorEstado(itemPorProcesar.IdItemCatalogo);
                        Base.WriteLog(Base.ErrorTypeEnum.Information, 101, "Verifica si existen archivos disponibles(MDP) por procesar.");

                        if (archivosDisponibles != null && archivosDisponibles.Tables.Count > 0)
                        {
                            if (archivosDisponibles.Tables[0].Rows.Count > 0)
                            {
                                Base.WriteLog(Base.ErrorTypeEnum.Information, 102, "Existen disponibles: " + archivosDisponibles.Tables[0].Rows.Count + " archivos por procesar.");

                                var dtArchivosPorProcesar = archivosDisponibles.Tables[0];
                                foreach (DataRow rowFile in dtArchivosPorProcesar.Rows)
                                {
                                    #region Inicia el procesamiento de los archivos

                                    // Valida el formato de los archivos
                                    var idArchivo = (int)rowFile["DAC_ID"];
                                    var codigoMdp = rowFile["DAC_CODIGO_MDP"].ToString();
                                    var nombreFileMdp = rowFile["DAC_NOMBRE_ARCHIVO"].ToString();
                                    fechaTransacciones = Convert.ToDateTime(rowFile["DAC_FECHA_TRX"].ToString());
                                    var numeroTrxFileMdp = (int)rowFile["DAC_NUM_TRX"];
                                    var byteArchivo = (Byte[])rowFile["DAC_FILE"];
                                    var mailNotificacion = rowFile["DAC_MAIL_NOTIFICACION"].ToString();
                                    Base.WriteLog(Base.ErrorTypeEnum.Trace, 103, "Procesasndo el archivo: IdArchivo: " + idArchivo + ". Codigo MDP: " + codigoMdp + ". Nombre del archivo: " + nombreFileMdp + ". Fecha de las transacciones a conciliar: " + fechaTransacciones + ". Numero de transacciones: " + numeroTrxFileMdp);

                                    // Actualiza el estado del archivo a: PROCESANDO
                                    var updateFileDataMdp = Conciliacion.ActualizaEstadoArchivo(idArchivo, itemProcesandoArchivo.IdItemCatalogo);
                                    Base.WriteLog(Base.ErrorTypeEnum.Trace, 104, "Se cambia el estado del archivo: " + idArchivo + " (" + codigoMdp + ") " + " a procesando.");
                                    #endregion

                                    #region Obtiene lo pagos de Tuves y lo registra en la tabla de trabajo
                                    // Obtiene la infomacion del codigo del MDP a enviar a la base de tuves
                                    var datosRecaudador = Conciliacion.ObtieneDatosRecaudadorPorCodigo(codigoMdp);

                                    // Elimina las Trx que esten en la tabla de Tuves
                                    var delTrxTuves = Conciliacion.EliminaTrxTuves(codigoMdp, fechaTransacciones);
                                    Base.WriteLog(Base.ErrorTypeEnum.Trace, 105, "Elimina los registros de la tabla de trabajo de Tuves: codigoMdp: " + codigoMdp + ". Fecha de las Trx a eliminar: " + fechaTransacciones);

                                    var idMdpEnTuves = datosRecaudador.CodigoTuves;
                                    Base.WriteLog(Base.ErrorTypeEnum.Trace, 106, "Se obtiene los datos de tuves (base Mysql) para insertarlos en la tabla de trabajo, IdMdp: " + idMdpEnTuves + ". Fecha de pagos a traer: " + fechaTransacciones);
                                    var dsPagosTuves = Tuves.ObtenerTransaccionesPorFechaMdp(idMdpEnTuves, fechaTransacciones);
                                    if (dsPagosTuves != null)
                                    {
                                        if (dsPagosTuves.Tables.Count > 0)
                                        {
                                            if (dsPagosTuves.Tables[0].Rows.Count > 0)
                                            {
                                                var dtPagosTuves = dsPagosTuves.Tables[0];
                                                foreach (DataRow row in dtPagosTuves.Rows)
                                                {
                                                    var idMdp = row["DealerKey"].ToString();
                                                    var codigoMdpTuv = row["CollectID"].ToString();
                                                    var fecha = Util.ConvertStringToDateTimeMysql(row["PayDate"].ToString());
                                                    var hora = Util.ConvertStringToDateTimeMysql(row["ts"].ToString());
                                                    var idTransaccionTuves = row["UniqueID"].ToString();
                                                    var idTrxMdp = row["PayDocN"].ToString();
                                                    var monto = Util.ConvertStringToDecimal(row["PayAmnt"].ToString());
                                                    var idCliente = row["CustIdent"].ToString();
                                                    var producto = row["CustPaym"].ToString();
                                                    var usuarioVenta = row["tsUser"].ToString();

                                                    var registerPagoTuves = Conciliacion.RegistraPagoTuvez(idMdp,
                                                        codigoMdp, fecha, hora, idTransaccionTuves, idTrxMdp, monto,
                                                        string.Empty, 0, idCliente, producto, usuarioVenta,
                                                        idArchivo, codigoMdpTuv);

                                                    if (registerPagoTuves == -1)
                                                        Base.WriteLog(Base.ErrorTypeEnum.Error, 111, "Se presentaron errores al tratar de insertar los registros de la base de datos de Mysql a la tabla de trabajo de Tuves: TRANSACCION_TVCABLE");
                                                }
                                                Base.WriteLog(Base.ErrorTypeEnum.Information, 110, "Se registraron: " + dtPagosTuves.Rows.Count + " pagos obtenidos de la tabla de Tuves con los parametros enviados: IdMdp: " + idMdpEnTuves + ". Fecha de pagos a traer: " + fechaTransacciones);
                                            }
                                            else
                                                Base.WriteLog(Base.ErrorTypeEnum.Information, 109, "No existen transacciones de pago en la base de tuves con los parametros enviados: IdMdp: " + idMdpEnTuves + ". Fecha de pagos a traer: " + fechaTransacciones);
                                        }
                                        else
                                            Base.WriteLog(Base.ErrorTypeEnum.Error, 108, "Eror al obtener los pagos de la base de Tuves, IdMdp: " + idMdpEnTuves + ". Fecha de pagos a traer: " + fechaTransacciones);
                                    }
                                    else
                                        Base.WriteLog(Base.ErrorTypeEnum.Error, 107, "Eror al obtener los pagos de la base de Tuves, IdMdp: " + idMdpEnTuves + ". Fecha de pagos a traer: " + fechaTransacciones);
                                    #endregion

                                    #region Obtiene y registra la data del MDP en la tabla de trabajo
                                    // Elimina data si existe en la tabla de trabajo
                                    var eliminaDatosTrabajo = Conciliacion.EliminaTrxMdp(fechaTransacciones, codigoMdp);
                                    Base.WriteLog(Base.ErrorTypeEnum.Information, 112, "Se elimina los registros de la tabla de trabajo del MDP: Fecha transacciones: " + fechaTransacciones + ". Codigo MDP: " + codigoMdp);

                                    Base.WriteLog(Base.ErrorTypeEnum.Information, 113, "Se decodifica el archivo enviado por el MDP al directorio FTP para iniciar el analisis de las transacciones que estan registradas en el mismo.");
                                    Stream streamFileMdp = new MemoryStream(byteArchivo);
                                    var numtrxFileMdp = 0;
                                    using (System.IO.TextReader tmpReader = new System.IO.StreamReader(streamFileMdp))
                                    {
                                        String lineFile;
                                        while ((lineFile = tmpReader.ReadLine()) != null)
                                        {
                                            // Registra el contenido del archivo(cada pago) en la base de datos

                                            // Obtengo el detalle de cada campo 
                                            var detallePago = Conciliacion.ObtieneDetalleTransaccionArchivo(lineFile);
                                            if (System.String.CompareOrdinal(detallePago.ResultCode, "0") == 0)
                                            {
                                                // Registro en la base de datos 
                                                var idDetallePago = Conciliacion.RegistraPagoMdp(codigoMdp, detallePago.FechaInicio,
                                                    detallePago.HoraInicio, detallePago.IdTransactionMdp,
                                                    detallePago.IdTransactionTvCable, detallePago.Cedula, detallePago.UsuarioVentas, detallePago.Producto, detallePago.Monto, detallePago.FechaFin,
                                                    detallePago.HoraFin, 0, idArchivo);
                                                numtrxFileMdp++;
                                            }
                                            else
                                            {
                                                // Registra Log de error alobtener un dato
                                            }
                                        }
                                    }
                                    Base.WriteLog(Base.ErrorTypeEnum.Information, 114, "Se registraron: " + numtrxFileMdp + " transacciones del archivo enviado por el MDP: " + codigoMdp + ". Fecha: " + fechaTransacciones);
                                    #endregion

                                    #region Inicia el proceso de conciliacion

                                    // Obtiene la informacion de las tablas de trabajo por el identificador del archivo
                                    // 1. MDP
                                    var dataMdp = Conciliacion.ObtieneDetallePagosMdpPorIdArchivo(idArchivo);
                                    Base.WriteLog(Base.ErrorTypeEnum.Information, 115, "Se obtiene la informacion del MDP a coinciliar.");

                                    // 2. TUVES
                                    var dataTuves = Conciliacion.ObtieneDetallePagosTuvesPorIdArchivo(idArchivo);
                                    Base.WriteLog(Base.ErrorTypeEnum.Information, 116, "Se obtiene la informacion del TUVES a coinciliar.");

                                    if (dataMdp != null)
                                    {
                                        if (dataMdp.Tables.Count > 0)
                                        {
                                            if (dataMdp.Tables[0].Rows.Count > 0)
                                            {

                                                if (dataTuves != null)
                                                {
                                                    if (dataTuves.Tables.Count > 0)
                                                    {
                                                        if (dataMdp.Tables[0].Rows.Count > 0)
                                                        {
                                                            Base.WriteLog(Base.ErrorTypeEnum.Information, 123, "Inicia el proceso de conciliacion: IdArchivo: " + idArchivo + ". Codigo Mdp: " + codigoMdp + ". Fecha Trx: " + fechaTransacciones);
                                                            var resultadoConciliacion = Conciliacion.GeneraProcesoConciliacion(idArchivo, codigoMdp, fechaTransacciones, dataMdp.Tables[0], dataTuves.Tables[0]);

                                                            // Valida el resultado de la conciliacion y registra su resultado.
                                                            #region Arma el resultado del proceos de conciliacion / arma el cuerpo mail
                                                            string xmlResponse = "<Response><ResultCode>{0}</ResultCode><ResultDescription>{1}</ResultDescription><TiempoEjecucion>{2}</TiempoEjecucion><TrxConciliadas>{3}</TrxConciliadas><TrxErrorMonto>{4}</TrxErrorMonto><TrxErrorCliente>{5}</TrxErrorCliente><TrxSobranteTuves>{6}</TrxSobranteTuves><TrxFaltanteTuves>{7}</TrxFaltanteTuves></Response>";
                                                            // Modificacion del formato de correo enviado
                                                            // Mauricio Camañero - 20 julio 2014
                                                            //string strBodyMail = "<br /><br />Resultado = {0}.<br />Descripción = {1}.<br />Tiempo de Ejecucion (milisegundos): {2}.<br />Transacciones  Conciliadas: {3}.<br />Transacciones Error Monto: {4}.<br />Transacciones Error Cliente: {5}.<br />Transacciones Sobrante TUVES: {6}.<br />Transacciones Faltante TUVES: {7}.<br /><br />";
                                                            //string strBodyMail = "<br /><hr><b>Medio de Pago:</b> {0}.<br /> <b>Fecha de Recargas Conciliadas:</b> {1}.<br /><b>Resultado del proceso de Conciliacion:</b> {2}.<br /><b>TRANSACCIONES CONCILIADAS CORRECTAMENTE:</b> {3}.<hr><br /><br /><table><tr><td><b>ERRORES:</b><br /><b>Transacciones con Error por diferencia entre el Monto reportado por el Proveedor versus SMS Lite:</b> {4}.<br /><b>Transacciones con Error por diferencia del Documento de Identidad del Cliente entre el Proveedor versus SMS Lite:</b> {5}.<br /><b>Transacciones que NO fueron reportadas por el Proveedor pero que SI existen en SMS Lite:</b> {6}.<br /><b>Transacciones que SI fueron reportadas por el Proveedor pero que NO existen en el sistema SMS Lite:</b> {7}.<br /><br />";
                                                            int totalErrores = resultadoConciliacion.TransaccionesErrorMonto +
                                                                resultadoConciliacion.TransaccionesErrorCliente +
                                                                resultadoConciliacion.TransaccionesSobrantesTuves +
                                                                resultadoConciliacion.TransaccionesFaltantesTuves;
                                                            string strBodyMail = "<br /><hr><b>Medio de Pago:</b> {0}.<br /> <b>Fecha de Recargas Conciliadas:</b> {1}.<br /><b>Resultado del proceso de Conciliacion:</b> {2}.<br /><b>TRANSACCIONES CONCILIADAS CORRECTAMENTE:</b> {3}.<hr><br /><br /><table><tr><td><b>TOTAL ERRORES:</b></td><td><b> " +
                                                                totalErrores.ToString()+
                                                                " </b></td></tr><tr><td><b>Transacciones con Error por diferencia entre el Monto reportado por el Proveedor versus SMS Lite:</b></td><td> {4}</td></tr><tr><td><b>Transacciones con Error por diferencia del Documento de Identidad del Cliente entre el Proveedor versus SMS Lite:</b></td><td> {5}</td></tr><tr><td><b>Transacciones que NO fueron reportadas por el Proveedor pero que SI existen en SMS Lite:</b></td><td> {6}</td></tr><tr><td><b>Transacciones que SI fueron reportadas por el Proveedor pero que NO existen en el sistema SMS Lite:</b></td><td> {7}</td></tr></table><br />";

                                                            var strResponse = string.Format(xmlResponse, resultadoConciliacion.ResultCode,
                                                                resultadoConciliacion.ResultDescription,
                                                                resultadoConciliacion.TiempoEjecucion,
                                                                resultadoConciliacion.TransaccionesConciliadas,
                                                                resultadoConciliacion.TransaccionesErrorMonto,
                                                                resultadoConciliacion.TransaccionesErrorCliente,
                                                                resultadoConciliacion.TransaccionesSobrantesTuves,
                                                                resultadoConciliacion.TransaccionesFaltantesTuves);

                                                            var strResponseMail = string.Format(strBodyMail, codigoMdp,
                                                                fechaTransacciones,
                                                                resultadoConciliacion.ResultDescription,
                                                                resultadoConciliacion.TransaccionesConciliadas,
                                                                resultadoConciliacion.TransaccionesErrorMonto,
                                                                resultadoConciliacion.TransaccionesErrorCliente,
                                                                resultadoConciliacion.TransaccionesSobrantesTuves,
                                                                resultadoConciliacion.TransaccionesFaltantesTuves);

                                                            Base.WriteLog(Base.ErrorTypeEnum.Information, 124, "El resultado del proceso de conciliacion es: ResultCode: " + resultadoConciliacion.ResultCode + ". Result description: " + resultadoConciliacion.ResultDescription + ". Transacciones conciliadas: " + resultadoConciliacion.TransaccionesConciliadas + ". Transacciones Error Monto: " + resultadoConciliacion.TransaccionesErrorMonto + ". Transacciones Error Cliente: " + resultadoConciliacion.TransaccionesErrorCliente + ". Transacciones Sobrantes Tuves: " + resultadoConciliacion.TransaccionesSobrantesTuves + ". Transacciones Faltantes Tuves: " + resultadoConciliacion.TransaccionesFaltantesTuves);

                                                            var registraResponse = Conciliacion.RegistraXmlResultadoConciliacion(idArchivo, strResponse);
                                                            Base.WriteLog(Base.ErrorTypeEnum.Information, 125, "Se registra el XML RESPONSE en el detalle del archivo: " + strResponse);

                                                            #endregion

                                                            #region Si existe algun error envia una mail de notificacion

                                                            var resSendMail = 0;
                                                            Base.WriteLog(Base.ErrorTypeEnum.Information, 130, "Se envia la notificacion al MDP: Con el resultado de la conciliacion.");
                                                            var bodyMailNotification = string.Format(SubjectMailResultConciliacion, codigoMdp, strResponseMail);
                                                            var mails = mailNotificacion.Split(SeparatorMails);
                                                            string resultSendMail = string.Empty;

                                                            if (mails.Length > 0)
                                                            {
                                                                foreach (var mail in mails)
                                                                {
                                                                    Base.WriteLog(Base.ErrorTypeEnum.Information, 131, "Se envia el mail de notificacion a: FROM: " + FromMailNotification + ". To: " + mail + ". Asunto: " + AsuntoMailNotificacion + ". Mail: " + bodyMailNotification);
                                                                    resSendMail = Util.SendMail(FromMailNotification, mail, AsuntoMailNotificacion + " - " + codigoMdp.ToUpper(), bodyMailNotification, null, string.Empty, out resultSendMail);
                                                                    Base.WriteLog(Base.ErrorTypeEnum.Information, 132, "Resultado del envio del mail: Estado: " + resSendMail + ". Mensaje: " + resultSendMail);
                                                                }
                                                            }

                                                            #endregion

                                                            #region Envia notificacion por mail a los Administradores
                                                            Base.WriteLog(Base.ErrorTypeEnum.Information, 133, "Se envia la notificacion al MDP: Con el resultado de la conciliacion.");
                                                            var bodyMailNotificationAdmin = string.Format(SubjectMailResultConciliacion, codigoMdp, strResponseMail);
                                                            var mailsAdmin = MailNotificacionAdmin.Split(SeparatorMails);
                                                            string resultSendMailAdmin = string.Empty;

                                                            if (mailsAdmin.Length > 0)
                                                            {
                                                                foreach (var mail in mailsAdmin)
                                                                {
                                                                    Base.WriteLog(Base.ErrorTypeEnum.Information, 134, "Se envia el mail de notificacion al ADMIN: FROM: " + FromMailNotification + ". To: " + ". Asunto: " + AsuntoMailNotificacion + ". Mail: " + bodyMailNotificationAdmin);
                                                                    resSendMail = Util.SendMail(FromMailNotification, mail, AsuntoMailNotificacion + codigoMdp , bodyMailNotificationAdmin, null, string.Empty, out resultSendMailAdmin);
                                                                    Base.WriteLog(Base.ErrorTypeEnum.Information, 135, "Resultado del envio del mail al Admin: Estado: " + resSendMail + ". Mensaje: " + resultSendMailAdmin);
                                                                }
                                                            }
                                                            #endregion
                                                        }
                                                        else
                                                            Base.WriteLog(Base.ErrorTypeEnum.Error, 122, "Existe un error al obtener la informacion de la tabla de trabajo de Tuves: TRANSACCION_TVCABLE con los datos: IdArchivo: " + idArchivo);
                                                    }
                                                    else
                                                        Base.WriteLog(Base.ErrorTypeEnum.Error, 121, "Existe un error al obtener la informacion de la tabla de trabajo de Tuves: TRANSACCION_TVCABLE con los datos: IdArchivo: " + idArchivo);
                                                }
                                                else
                                                    Base.WriteLog(Base.ErrorTypeEnum.Error, 120, "Existe un error al obtener la informacion de la tabla de trabajo de Tuves: TRANSACCION_TVCABLE con los datos: IdArchivo: " + idArchivo);
                                            }
                                            else
                                                Base.WriteLog(Base.ErrorTypeEnum.Error, 119, "Existe un error al obtener la informacion de la tabla de trabajo del MDP: TRANSACCION_MDP con los datos: IdArchivo: " + idArchivo);
                                        }
                                        else
                                            Base.WriteLog(Base.ErrorTypeEnum.Error, 118, "Existe un error al obtener la informacion de la tabla de trabajo del MDP: TRANSACCION_MDP con los datos: IdArchivo: " + idArchivo);
                                    }
                                    else
                                        Base.WriteLog(Base.ErrorTypeEnum.Error, 117, "Existe un error al obtener la informacion de la tabla de trabajo del MDP: TRANSACCION_MDP con los datos: IdArchivo: " + idArchivo);


                                    #endregion

                                    #region Actualiza el estado del archivo a: PROCESADO
                                    var updateFileFinalMdp = Conciliacion.ActualizaEstadoArchivo(idArchivo, itemProcesadoArchivo.IdItemCatalogo);
                                    Base.WriteLog(Base.ErrorTypeEnum.Information, 126, "Se actualiza el estado del archivo a PROCESADO: IdArchivo: " + idArchivo + ". Codigo MDP: " + codigoMdp + ". Fecha Transacciones: " + fechaTransacciones);

                                    Base.WriteLog(Base.ErrorTypeEnum.Information, 127, "Termina el proceso de conciliacion del archivo: IdArchivo: " + idArchivo + ". Codigo MDP: " + codigoMdp + ". Fecha Transacciones: " + fechaTransacciones);
                                    #endregion
                                }

                            }
                            else
                            {
                                Base.WriteLog(Base.ErrorTypeEnum.Information, 102, "NO existen ARCHIVOS DISPONIBLES por procesar.");
                            }
                        }
                        else
                        {
                            Base.WriteLog(Base.ErrorTypeEnum.Information, 102, "NO existen ARCHIVOS DISPONIBLES por procesar.");
                        }
                        #endregion 
                    }                            
                }
                catch (Exception ex)
                {
                    Base.WriteLog(Base.ErrorTypeEnum.Error, 128, "Se presento un error en el proceso de conciliacion: Mensaje: " + ex.Message + ". Excepcion: " + ex.ToString());
                }

                try
                {
                    //Mauricio - 20 julio 2014 - verifica si existen archivos no colocados en este directorio FTP
                    string filename = null;
                    string fecha;
                    DateTime fechaTransacciones;
                    int i;
                    int totalRecargas = 0;


                    #region Verifica si existen archivos de conciliacion no colocados
                    // Verifico si el servicio esta dentro de parametro permitido para ejecutar el proceso
                    if (checkHorario(StrHoraInicioFiles, StrHoraHastaFiles))
                    {
                        // Testing save Logs
                        Base.WriteLog(Base.ErrorTypeEnum.Start, 400, "Inicia el proceso de verificacion de archivos de conciliacion de TvCable.");
                        Base.WriteLog(Base.ErrorTypeEnum.Information, 401, "Obtiene todos los codigos de recaudadores que no tienen archivos de conciliacion.");
                        DataSet recaudadores = Conciliacion.ObtenerRecaudadoresSinArchivo();
                        fechaTransacciones = DateTime.Now.AddDays(-1);
                        fecha = fechaTransacciones.ToString("yyyyMMdd");
                        try
                        {
                            if (recaudadores != null)
                            {
                                for (i = 0; i < recaudadores.Tables[0].Rows.Count; i++)
                                {
                                    string codigoMDP = recaudadores.Tables[0].Rows[i]["REC_CODIGO"].ToString();
                                    filename = codigoMDP + "-" + fecha + ".csv";

                                    string cuentaCorreo = ConfigurationManager.AppSettings["cuentaDestinoMail" + codigoMDP] != null ? ConfigurationManager.AppSettings["cuentaDestinoMail"] : ConfigurationManager.AppSettings["CuentaCorreoOrigen"];
                                    //cuentaCorreo = cuentaCorreo + SeparatorMails + ConfigurationManager.AppSettings["cuentaDestinoMail"];

                                    Base.WriteLog(Base.ErrorTypeEnum.Information, 402, "Archivo de conciliacion no encontrado en la base de datos: " + filename + ".");
                                    Base.WriteLog(Base.ErrorTypeEnum.Information, 403, "Obteniendo transacciones en SMS Lite para el recaudador: " + codigoMDP + " para la fecha " + fechaTransacciones.ToString("yyyy-MM-dd") + ".");

                                    var dsPagosTuves = Tuves.ObtenerTransaccionesPorFechaMdp(codigoMDP, Convert.ToDateTime(fechaTransacciones.ToString("yyyy-MM-dd")));
                                    if (dsPagosTuves != null && dsPagosTuves.Tables.Count > 0)
                                    {
                                        for (int contador = 0; contador < dsPagosTuves.Tables[0].Rows.Count; contador++)
                                        {
                                            totalRecargas = dsPagosTuves.Tables[0].Rows.Count;
                                        }
                                        


                                        if (totalRecargas > 0)
                                        {
                                            #region Verificar si existen archivos de conciliacion en server FTP
                                            string resultado = "";
                                            try
                                            {
                                                Base.WriteLog(Base.ErrorTypeEnum.Information, 404, "Intentar registrar archivo [" + filename + "] de conciliacion usando el servicio web.");
                                                TvCableConciliacionServicesClient ws = new TvCableConciliacionServicesClient();
                                                resultado = ws.RegistrarArchivoConciliacionMdp(filename, fecha, codigoMDP, 0, 0, cuentaCorreo, "proceso generado automaticamente");
                                                Base.WriteLog(Base.ErrorTypeEnum.Information, 405, "Resultado del proceso de registro del archivo [" + filename + "] de conciliacion usando el servicio web." + resultado);
                                            }
                                            catch (Exception e)
                                            {
                                                Base.WriteLog(Base.ErrorTypeEnum.Error, 405, "Error en el proceso de registro del archivo [" + filename + "] usando el servicio web.");
                                            }
                                            if (resultado.IndexOf("Could not find file") > 0)
                                            {
                                                //string emailRecaudador;
                                                string strBodyMail = ConfigurationManager.AppSettings["bodyVerificacionArchivoConciliacion"];
                                                string strSubjectMail = ConfigurationManager.AppSettings["subjectVerificacionArchivoConciliacion"];
                                                if (codigoMDP.ToLower().Equals("broadnet"))
                                                {
                                                    cuentaCorreo = cuentaCorreo + SeparatorMails + ConfigurationManager.AppSettings["cuentaDefaultBROADNET"];
                                                }
                                                cuentaCorreo = cuentaCorreo + SeparatorMails + ConfigurationManager.AppSettings["cuentaDestinoMail"];
                                                Base.WriteLog(Base.ErrorTypeEnum.Information, 406, "El archivo [" + filename + "] no se encuentra en el directorio FTP, enviando notificación por correo.");
                                                #region En el caso que tenga errores enviar el correo
                                                strBodyMail = string.Format(strBodyMail, codigoMDP  + saltoLinea + saltoLinea,
                                                                            saltoLinea + saltoLinea + lineaAzul + negrilla,
                                                                            cierreNegrilla + codigoMDP + saltoLinea + negrilla,
                                                                            cierreNegrilla + fechaTransacciones.ToString("yyyy-MM-dd") + saltoLinea + negrilla,
                                                                            cierreNegrilla + filename + lineaAzul + saltoLinea + saltoLinea,
                                                                            saltoLinea);

                                                var resSendMail = 0;

                                                Base.WriteLog(Base.ErrorTypeEnum.Information, 407, "Se envia la notificacion al MDP indicando error en la colocación del archivo de conciliacion.");
                                                var mails = cuentaCorreo.Split(SeparatorMails);
                                                string resultSendMail = string.Empty;

                                                if (mails.Length > 0)
                                                {
                                                    foreach (var mail in mails)
                                                    {
                                                        Base.WriteLog(Base.ErrorTypeEnum.Information, 408, "Se envia el mail de notificacion de colocación de archivo de conciliacion a: FROM: " + FromMailNotification + ". To: " + mail + ". Asunto: " + strSubjectMail + ". Mail: " + strBodyMail);
                                                        resSendMail = Util.SendMail(FromMailNotification, mail, strSubjectMail, strBodyMail, null, string.Empty, out resultSendMail);
                                                        Base.WriteLog(Base.ErrorTypeEnum.Information, 409, "Resultado del envio del mail de notificacion de colocación de archivo de conciliacion: Estado: " + resSendMail + ". Mensaje: " + resultSendMail);
                                                    }
                                                }
                                                #endregion
                                            }
                                            //errores = errores + "</ br>Proveedor: {0}</ br> Fecha: {1}</ br>Nombre de archivo: {2}</ br>";
                                            #endregion
                                        }
                                        else
                                        {
                                            Base.WriteLog(Base.ErrorTypeEnum.Information, 403, "Archivo de conciliacion [" + filename + "] del proveedor " + codigoMDP + " no tiene pagos registrados para la fecha [" + fechaTransacciones + "].");
                                        }
                                    }
                                    else
                                    {
                                        Base.WriteLog(Base.ErrorTypeEnum.Information, 402, "Archivo de conciliacion [" + filename + "] del proveedor " + codigoMDP + " no tiene pagos registrados para la fecha [" + fechaTransacciones + "].");
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Base.WriteLog(Base.ErrorTypeEnum.Information, 000, "Error al procesar [" + filename + "]. " + e.Message);
                        }
                    }
                    #endregion
                }
                catch (Exception e)
                {
                    Base.WriteLog(Base.ErrorTypeEnum.Error, 001, "Error al verificar los archivos de conciliacion no enviados. " + e.Message);
                }

                try
                {
                    
                    Thread.Sleep(Convert.ToInt32(DefaultInterval));
                    //Mauricio - 4 de agosto de 2014
                    //Thread.Sleep(new TimeSpan(0, 0, DefaultInterval));
                }
                catch (Exception ex)
                {
                    Base.WriteLog(Base.ErrorTypeEnum.Error, 129, "Se presento un error en el proceso de conciliacion: Mensaje: " + ex.Message + ". Excepcion: " + ex.ToString());
                }
                finally
                {
                    
                }
            }
        }

        private Boolean checkHorario(string inicio, string fin)
        {
            // Verifico si el servicio esta dentro de parametro permitido para ejecutar el proceso
            Boolean resultado = false;
            string[] strArrayInicio = inicio.Split(':');
            string[] strArrayHasta = fin.Split(':');

            if (strArrayInicio.Length == 2 && strArrayHasta.Length == 2)
            {
                if ((DateTime.Now.Hour > Convert.ToInt16(strArrayInicio[0])) ||
                    ((DateTime.Now.Hour == Convert.ToInt16(strArrayInicio[0])) &&
                        (DateTime.Now.Minute > Convert.ToInt16(strArrayInicio[1]))))
                {
                    if ((DateTime.Now.Hour < Convert.ToInt16(strArrayHasta[0])) ||
                        ((DateTime.Now.Hour == Convert.ToInt16(strArrayHasta[0])) &&
                            (DateTime.Now.Minute < Convert.ToInt16(strArrayHasta[1]))))
                    {

                        resultado = true;
                    }
                }
            }
            return resultado;
        }
        
        protected override void OnStop()
        {
            if (_workerThread != null)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Error, 116,"Stop, servicio de conciliacion TvCable.");
                _shouldStop = true;
                _workerThread.Join();
            }
        }
    }
}
