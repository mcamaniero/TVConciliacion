using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using TvCable.Conciliacion.BusinessLayer;
using TvCable.Conciliacion.DTO;
using TvCable.Conciliacion.Libs;

namespace TvCable.Conciliacion.WebServices
{
    public class TvCableConciliacionServices : ITvCableConciliacionServices
    {
        internal Base Base = new Base();
        internal BusinessLayer.Conciliacion Conciliacion = new BusinessLayer.Conciliacion();
        internal Nucleo Nucleo = new Nucleo();
        internal string FormatoFechaArchivo = ConfigurationManager.AppSettings["FormatoFechaArchivoMDP"] != null ? (ConfigurationManager.AppSettings["FormatoFechaArchivoMDP"]) : "yyyyMMdd";
        internal char SeparatorMails = ConfigurationManager.AppSettings["SeparadorMails"] != null ? Convert.ToChar(ConfigurationManager.AppSettings["SeparadorMails"]) : Convert.ToChar(";");
        internal string FromMailNotification = ConfigurationManager.AppSettings["CuentaCorreoOrigen"].ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        internal string AsuntoMailNotificacion = ConfigurationManager.AppSettings["subjectNotificacionMail"].ToString(CultureInfo.InvariantCulture) ?? "Notificacion TvCable";
        internal string SubjectMailErrRegistro = ConfigurationManager.AppSettings["bodyNotificacionMail1"].ToString(CultureInfo.InvariantCulture) ?? "";
        internal string SubjectMailErrFormatFile = ConfigurationManager.AppSettings["bodyNotificacionMail2"].ToString(CultureInfo.InvariantCulture) ?? "";
        internal string MailNotificacionAdmin = ConfigurationManager.AppSettings["cuentaDestinoMail"].ToString(CultureInfo.InvariantCulture) ?? "";

        public string RegistrarArchivoConciliacionMdp(string nombreArchivo, string fechaTransacciones, string codigoMdp, int numTransacciones, decimal montoTotal, string mailNotificacion, string observaciones)
        {
            var xmlResponse = "<TrxResponse><ResultCode>{0}</ResultCode><ResultDescription>{1}</ResultDescription><TiempoValidacion>{2}</TiempoValidacion><StatusFile>{3}</StatusFile></TrxResponse>";
            try
            {
                int idArchivoMdp = 0;
                Base.WriteLog(Base.ErrorTypeEnum.Start, 201, "Inicia el registro de un nuevo archivo: Nombre del Archivo: " + nombreArchivo + ". Fecha Transacciones: " + fechaTransacciones + ". Codigo Mdp: " + codigoMdp + ". Num. Transacciones: " + numTransacciones + ". Monto total: " + montoTotal + ". Mail Notificacion: " + mailNotificacion + ". Observaciones: " + observaciones);

                DateTime dtTrxRegister;
                #region Valida la fecha ingresada
                try
                {
                    // Valida la fecha ingresada
                    Base.WriteLog(Base.ErrorTypeEnum.Information, 202, "Se valida el formato de la fecha que envia el MDP: " + fechaTransacciones + ". Formato admitido: " + FormatoFechaArchivo);
                    dtTrxRegister = Util.ConvertStringToDateTime(fechaTransacciones, FormatoFechaArchivo);
                }
                catch (Exception ex)
                {
                    Base.WriteLog(Base.ErrorTypeEnum.Error, 203, "Error al validar la fecha enviada por el MDP: Fecha enviada: " + fechaTransacciones + ". Formato admitido: " + FormatoFechaArchivo);
                    throw new System.ArgumentException("La fecha ingresada no cumple con el formato requerido: " + FormatoFechaArchivo);
                }
                #endregion

                // Verifica si existe algun archivo PENDIENTE DE PROCESAR o CONCILIADO OK
                Base.WriteLog(Base.ErrorTypeEnum.Information, 211, "Se verifica si no existe algun archivo previamente registrado que esten en estado: ENTREGADO, CONSISTENTE, PROCESANDO o POR_PROCESAR con los datos: Nombre del archivo: " + nombreArchivo + ". Fecha de las transacciones: " + dtTrxRegister + ". Codigo del MDP: " + codigoMdp);
                var infoExisFile = Conciliacion.ObtieneDatosArchivoConciliacionMdp(nombreArchivo, dtTrxRegister, codigoMdp);
                if (infoExisFile.PermiteRegistroArchivo)
                {
                    Base.WriteLog(Base.ErrorTypeEnum.Information, 212, "Se permite el registro del archivo. Resultado de la Validacion: ResultCode: " + infoExisFile.ResultCode + ". Result Description: " + infoExisFile.ResultDescription);

                    // Los archivos registrados por el MDp se registran con el estado (ENTREGADO) luego se validan y cambian a estado POR PROCESAR.
                    var itemCatalogoEntregado = Nucleo.GetItemCatalogoPorCodigoItemCodigoCatalogo(Constants.CodeItemEntregado, Constants.CodeEstadoArchivosConciliacion);
                    byte[] fileMdp;

                    // Registra la entrega del archivo
                    idArchivoMdp = Conciliacion.RegistraEntregaArchivoConciliacionMdp(nombreArchivo, dtTrxRegister,
                            codigoMdp, numTransacciones, montoTotal, itemCatalogoEntregado.IdItemCatalogo, mailNotificacion, observaciones,
                            DateTime.Now);

                    Base.WriteLog(Base.ErrorTypeEnum.Information, 204, "Se registra la entrega del archivo del MDP: Nombre del archivo: " + nombreArchivo + ". Fecha Transacciones: " + fechaTransacciones + ". Codigo Mdp: " + codigoMdp + ". Num. Transacciones: " + numTransacciones + ". Monto total: " + montoTotal + ". Mail Notificacion: " + mailNotificacion + ". Estado: " + itemCatalogoEntregado.IdItemCatalogo + ". Observaciones: " + observaciones);

                    // Si el archivo es correcto, actualiza el estado a POR PROCESAR y registra el contenido del archivo
                    var itemCatalogoPorProcesar = Nucleo.GetItemCatalogoPorCodigoItemCodigoCatalogo(Constants.CodeItemPorProcesar, Constants.CodeEstadoArchivosConciliacion);


                    Base.WriteLog(Base.ErrorTypeEnum.Information, 205, "Inicia el proceso de validacion del archivo del MDP: " + codigoMdp);
                    var validacionFile = Conciliacion.ValidaFormatoArchivoConciliacion(codigoMdp, nombreArchivo, dtTrxRegister, numTransacciones, out fileMdp);
                    Base.WriteLog(Base.ErrorTypeEnum.Information, 206, "Termina el proceso de validacion del archivo del MDP: " + codigoMdp + ". Status del Archivo: " + validacionFile.StatusFile + ". Result code: " + validacionFile.ResultCode + ". Result description: " + validacionFile.ResultDescription);

                    if (validacionFile.StatusFile)
                    {
                        var byteFileMdp = Conciliacion.ObtieneArchivoConciliacion(nombreArchivo);

                        // Actualiza el estado a: POR PROCESAR y el detalle del archivo
                        var updateFileData = Conciliacion.ActualizaDatosArchivo(idArchivoMdp, itemCatalogoPorProcesar.IdItemCatalogo, byteFileMdp);

                        Base.WriteLog(Base.ErrorTypeEnum.Information, 207, "Formato valido del archivo del MDP, se modifica el estado a POR PROCESAR y se registra el contenido del archivo en la base de datos.");

                    }
                    else
                    {
                        // Se marca al archivo en estado erroneo
                        var itemFileError = Nucleo.GetItemCatalogoPorCodigoItemCodigoCatalogo(Constants.CodeItemFileError, Constants.CodeEstadoArchivosConciliacion);
                        var updateFileMdpError = Conciliacion.ActualizaEstadoArchivo(idArchivoMdp, itemFileError.IdItemCatalogo);

                        // Registra el Response de la validacion al Archivo
                        var strResponse = string.Format(xmlResponse, validacionFile.ResultCode, validacionFile.ResultDescription, validacionFile.TiempoValidacion, validacionFile.StatusFile);
                        var registraResponseFile = Conciliacion.RegistraXmlResultadoConciliacion(idArchivoMdp, strResponse);

                        Base.WriteLog(Base.ErrorTypeEnum.Information, 208, "Formato NO valido del archivo del MDP, se modifica el estado a ERROR y se registra el Response, ID Archivo: " + idArchivoMdp + ". Response: " + strResponse);

                        #region Envia notificacion por mail al cliente
                        
                        var resSendMail = 0;
                        Base.WriteLog(Base.ErrorTypeEnum.Information, 218, "Se envia la notificacion al MDP: Correos de notificacion: " + mailNotificacion);
                        var bodyMailNotification = string.Format(SubjectMailErrFormatFile, codigoMdp, validacionFile.ResultDescription);
                        var mails = mailNotificacion.Split(SeparatorMails);
                        string resultSendMail = string.Empty;

                        if (mails.Length > 0)
                        {
                            foreach (var mail in mails)
                            {
                                Base.WriteLog(Base.ErrorTypeEnum.Information, 219, "Se envia el mail de notificacion a: FROM: " + FromMailNotification + ". To: " + ". Asunto: " + AsuntoMailNotificacion + ". Mail: " + bodyMailNotification);
                                resSendMail = Util.SendMail(FromMailNotification, mail, AsuntoMailNotificacion, bodyMailNotification, null, string.Empty, out resultSendMail);
                                Base.WriteLog(Base.ErrorTypeEnum.Information, 220, "Resultado del envio del mail: Estado: " + resSendMail + ". Mensaje: " + resultSendMail);
                            }
                        }
                        #endregion

                        #region Envia notificacion por mail a los Administradores
                        Base.WriteLog(Base.ErrorTypeEnum.Information, 221, "Se envia la notificacion a los Administradores: Correos de notificacion: " + MailNotificacionAdmin);
                        var bodyMailNotificationAdmin = string.Format(SubjectMailErrFormatFile, codigoMdp, validacionFile.ResultDescription);
                        var mailsAdmin = MailNotificacionAdmin.Split(SeparatorMails);
                        string resultSendMailAdmin = string.Empty;

                        if (mailsAdmin.Length > 0)
                        {
                            foreach (var mail in mailsAdmin)
                            {
                                Base.WriteLog(Base.ErrorTypeEnum.Information, 222, "Se envia el mail de notificacion al ADMIN: FROM: " + FromMailNotification + ". To: " + ". Asunto: " + AsuntoMailNotificacion + ". Mail: " + bodyMailNotificationAdmin);
                                resSendMail = Util.SendMail(FromMailNotification, mail, AsuntoMailNotificacion, bodyMailNotificationAdmin, null, string.Empty, out resultSendMailAdmin);
                                Base.WriteLog(Base.ErrorTypeEnum.Information, 223, "Resultado del envio del mail al Admin: Estado: " + resSendMail + ". Mensaje: " + resultSendMailAdmin);
                            }
                        }
                        #endregion
                    }

                    var responseGFeneral = string.Format(xmlResponse, validacionFile.ResultCode, validacionFile.ResultDescription, validacionFile.TiempoValidacion, validacionFile.StatusFile);
                    Base.WriteLog(Base.ErrorTypeEnum.Information, 209, "Se retorna el response general del registro del archivo: " + responseGFeneral);
                    return responseGFeneral;
                }
                else
                {
                    // No se puede registrar el archivo: 
                    Base.WriteLog(Base.ErrorTypeEnum.Information, 213, "No se permite el registro del archivo. existen registros con los datos enviados: ResultCode: " + infoExisFile.ResultCode + ". Result Description: " + infoExisFile.ResultDescription);

                    #region Envia notificacion por mail
                    // Se envia la notificacion al MDP.
                    var resSendMail = 0;
                    Base.WriteLog(Base.ErrorTypeEnum.Information, 214, "Se envia la notificacion al MDP: Correos de notificacion: " + mailNotificacion);
                    var bodyMailNotification = string.Format(SubjectMailErrRegistro, codigoMdp, infoExisFile.ResultDescription);
                    var mails = mailNotificacion.Split(SeparatorMails);
                    string resultSendMail = string.Empty;

                    if (mails.Length > 0)
                    {
                        foreach (var mail in mails)
                        {
                            Base.WriteLog(Base.ErrorTypeEnum.Information, 215, "Se envia el mail de notificacion a: FROM: " + FromMailNotification + ". To: " + mail + ". Asunto: " + AsuntoMailNotificacion + ". Mail: " + bodyMailNotification);
                            resSendMail = Util.SendMail(FromMailNotification, mail, AsuntoMailNotificacion, bodyMailNotification, null, string.Empty, out resultSendMail);
                            Base.WriteLog(Base.ErrorTypeEnum.Information, 216, "Resultado del envio del mail: Estado: " + resSendMail + ". Mensaje: " + resultSendMail);
                        }
                    }
                    #endregion

                    var responseGFeneral = string.Format(xmlResponse, "-2", HttpUtility.HtmlEncode(bodyMailNotification), "0", "False");
                    Base.WriteLog(Base.ErrorTypeEnum.Information, 217, "Se retorna el response general (Error) del registro del archivo: " + responseGFeneral);
                    return responseGFeneral;
                }
            }
            catch (Exception ex)
            {
                Base.WriteLog(Base.ErrorTypeEnum.Exception, 210, "Se presento una Excepcion: " + ex.Message + ". Detalle Excepcion: " + ex.ToString());
                return string.Format(xmlResponse, "-1", ex.Message, "0", "False");
            }
        }
    }
}
