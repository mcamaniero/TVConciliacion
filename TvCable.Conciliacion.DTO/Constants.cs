namespace TvCable.Conciliacion.DTO
{
    public class Constants
    {
        #region Parametros

        #endregion Parametros

        #region Generales

        public const string DefaultCodeCasOkCode = "0";

        public const string DefaultOkCode = "0";
        public const string DefaultOkDescription = "Petición Enviada";

        #endregion Generales

        #region Procedimientos almacenados

        #region Acceso a tuves

        #endregion Acceso a tuves

        public const string SpGetDataSample = "GetDataSample";
        public const string SpGetDataConciliacion = "Sp_GetDataConciliacion";
        public const string SpGetDataConciliacionAll = "Sp_GetDataConciliacionAll";
        public const string SpGetDataConciliacionPorTipo = "Sp_GetDataConciliacionPorTipo";
        public const string SpGetDataIdRecaudador = "Sp_GetDataIdRecaudador";
        public const string SpGetDataEstadoConciliacion = "Sp_GetDataEstadoConciliacion";
        public const string SpUpdateEstado = "Sp_UpdateEstado";
        public const string SpUpdateRechazado = "Sp_UpdateRechazado";
        public const string SpGetDataRecaudador = "Sp_GetDataRecaudador";
        public const string SpGetDataListadoConciliacion = "Sp_GetDataListadoConciliacion";
        public const string SpCatalogoPorCodigoCatalogo = "Sp_ObtieneCatalogoPorCodigoCatalogo";
        public const string SpInsertHistorialEstado = "Sp_InsertHistorialEstado";

        //Sp para Intermix
        public const string SpGetDataEmpresaFacturadora = "Sp_GetDataEmpresaFacturadora";

        public const string SpGetDataReacaudadorId = "Sp_GetDataReacaudadorId";
        public const string SpGetDataEmpresaFacturadoraId = "Sp_GetDataEmpresaFacturadoraId";
        public const string SpInsertIntermix = "Sp_InsertIntermix";
        public const string SpUpdateConciliacionPorIntermix = "Sp_UpdateConciliacionPorIntermix";
        public const string SpInsertHistorialIntermix = "Sp_InsertHistorialIntermix";

        //Historial
        public const string SpGetDataHistorialPorId = "Sp_GetDataHistorialPorId";

        public const string SpActualizaEstadoArchivoMdp = "Sp_ActualizaEstadoArchivoMdp";
        public const string SpInsertaArchivoConciliacionMdp = "Sp_InsertaArchivoConciliacionMdp";
        public const string SpObtieneArchivoConciliacionPorEstado = "Sp_GetArchivoConciliacionPorEstado";
        public const string SpActualizaEstadoArchivo = "Sp_ActualizaEstadoArchivo";
        public const string SpObtieneDetalleArchivoPorIdArchivo = "SP_ObtieneDetalleArchivoPorId";
        public const string SpRegistraDetallePago = "Sp_RegistraDetallePagoMdp";
        public const string SpRegistraDetallePagoTuves = "Sp_RegistraDetallePagoTuves";
        public const string SpObtienePagosTuvesPorIdArchivo = "Sp_ObtienePagosTuvesPorIdArchivo";
        public const string SpObtienePagosMdpPorIdArchivo = "Sp_ObtienePagosMdpPorIdArchivo";
        public const string SpGetItemCatalogoPorCodigoItemCatalogo = "Sp_ObtieneItemCatalogoPorCodigoItemCodigoCatalogo";
        public const string SpRegistraResultadoConciliacion = "Sp_RegistraResultadoConciliacion";
        public const string SpEliminaTrxMdp = "Sp_EliminaTransaccionesMdp";
        public const string SpEliminaTrxTuves = "Sp_EliminaTransaccionesTuves";
        public const string SpActualizaEstadoTrxMdp = "Sp_ActualizaEstadoTrxMdp";
        public const string SpActualizaEstadoTrxTuves = "Sp_ActualizaEstadoTrxTuves";
        public const string SpObtieneTrxTuvesPorEstado = "Sp_ObtieneTrxTuvesPorEstado";
        public const string SpRegistraXmlResultadoConciliacion = "Sp_RegistraXmlResultadoConciliacion";
        public const string SpObtieneDatosRecaudadorPorCodigo = "Sp_ObtieneDatosRecaudadorPorCodigo";
        public const string SpObtieneDatosArchivoConciliacionMdp = "Sp_ObtieneDatosArchivoConciliacionMdp";
        public const string SpObtieneSecuencialIntermix = "SP_GET_SIG_SECUENCIAL_INT";
        public const string SpEliminaTransaccionesTablaTrabajo = "Sp_EliminaTransaccionesTablaTrabajo";

        //Mauricio - 20 de julio de 2014
        public const string SpGetAllRecaudador = "Sp_getAllRecaudador";
        public const string SpObtieneDatosRecaudadorPorCodigoUsuarioVentas = "Sp_ObtieneDatosRecaudadorPorCodigoUsuarioVentas";

        //Usuario
        public const string SpUsuarioFuncionalidades = "Sp_UsuarioFuncionalidades";

        #endregion Procedimientos almacenados

        #region Dataset

        public const string DatasetArchivos = "Archivos";
        public const string DatasetItemCatalogo = "ItemCatalogo";

        #endregion Dataset

        #region Codigos

        // Codigo Catalogos
        public const string CodeEstadoArchivosConciliacion = "EST_FILE_MDP";

        public const string CodeEstadoPagos = "EST_PAGO";

        // Archivos
        public const string CodeItemEntregado = "ENTREGADO";

        public const string CodeItemPorProcesar = "POR_PROCESAR";
        public const string CodeItemInconsistente = "INCONSISTENTE";
        public const string CodeItemConsistente = "CONSISTENTE";
        public const string CodeItemProcesado = "PROCESADO";
        public const string CodeItemProcesando = "PROCESANDO";
        public const string CodeItemFileError = "ERROR";

        //Pagos
        public const string CodeItemPagoPendiente = "PENDIENTE";

        //Items Conciliacion web
        public const string CodeItemRecaudador = "REC_RAZONSOCIAL";

        public const string CodeIdRecaudador = "REC_ID";
        public const string CodeItemEstadoConci = "ITC_NOMBRE";
        public const string CodeIdEstadoConci = "NE_HIJO_ID";
        public const string CodeItemCatalogo = "NombreItemCatalogo";
        public const string CodeIdCatalogo = "IdItemCatalogo";

        //Codigo Catalogo
        public const string CodeEstadoConciliacion = "EST_CONCILIACION";

        public const string CodeResultError = "ERROR";
        public const string CodeResultCorregido = "CORREGIDO";
        public const string CodeResultAprobado = "APROBADO";
        public const string CodeResultConciliado = "CONCILIADOS";
        public const string CodeIdRechazado = "RECHAZADOS";
        public const string signodolar = "$";
        public const int formatoPopUp = 340;
        public const int formatoPopup = 100;

        public const string CodeEstadoIntConciliacion = "EST_CON_INTERNO";
        public const string CodeIntConciliadoTuves = "CONCILIADO";
        public const string CodeIntSobranteTuves = "SOB_TUVES";
        public const string CodeIntFaltanteTuves = "FAL_TUVES";
        public const string CodeMontoInvalido = "MONTO_INVALIDO";
        public const string CodeClientesDiferentes = "CLIENTES_DIFERENTES";

        //Formato Grilla
        public const string formatoFecha = "yyyy-MM-dd";

        public const string formatoFechahis = "{0:G}";

        //Formato TextFechas
        public const string formatoFechaText = "{0:dd/MM/yyyy}";

        //Historial
        public const string hObservacion = "HE_OBSERVACION";

        //URL
        public const string UrlMain = "MainConciliacionWeb.aspx";

        public const string UrlLogin = "Login.aspx";

        //Funcionalidad
        public const string GroSelc = "GropuBoxSelect";

        public const string GroInsert = "GrupBoxInsertaIntermix";

        //Intermix
        public const string EstadoAprobado = "APROBADO";

        public const string CodeItemEmpresaFacturadora = "EMF_NOMBRE";
        public const string CodeIdEmpresaFacturadora = "EMF_ID";
        public const string codempresa = "EMF_COD_EMPRESA";
        public const string codpuntoemision = "EMF_COD_AGENCIA";
        public const string ruc = "REC_RUC";
        public const string tipocliente = "REC_ID_TIPOCLIENTE";
        public const string telefono = "REC_TELEFONO";
        public const string tipopersona = "REC_ID_PERSONA";
        public const string tipocontribuyente = "REC_ID_CONTRIBUYENTE";
        public const string nombre = "REC_RAZONSOCIAL";
        public const string direccion = "REC_DIRECCION";
        public const string email = "REC_CORREO";
        public const string codservicio = "REC_COD_SERVICIO";
        public const int facturado = 0;

        //Mensajes
        public const string popupScript = "PopupScript";

        public const string buttonScript = "ButtonClickScript";

        #endregion Codigos
    }
}