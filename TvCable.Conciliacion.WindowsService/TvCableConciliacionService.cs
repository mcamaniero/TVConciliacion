using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Configuration;
using System.Threading;
using TvCable.Conciliacion.DTO;

namespace TvCable.Conciliacion.WindowsService
{
    partial class TvCableConciliacionService : ServiceBase
    {
        #region Variables

        internal DateTime DateTransaction;
        internal string fechaSwitch = string.Empty;
        private const double DefaultInterval = 30000;
        
        internal string StrHoraInicio = ConfigurationManager.AppSettings["horaInicioServicio"];
        internal string StrHoraHasta = ConfigurationManager.AppSettings["horaFinServicio"];
        internal string SeparadorCamposFile = ConfigurationManager.AppSettings["SeparadorCamposFile"] ?? ";";
        
        internal string StrDireccionFtp = ConfigurationManager.AppSettings["DireccionFTP"] != null ? (ConfigurationManager.AppSettings["DireccionFTP"]) : "";
        internal string UsuarioAccesoFtp = ConfigurationManager.AppSettings["UsuarioAccesoFTP"] != null ? (ConfigurationManager.AppSettings["UsuarioAccesoFTP"]) : "";
        internal string ClaveAccesoFtp = ConfigurationManager.AppSettings["ClaveAccesoFTP"] != null ? (ConfigurationManager.AppSettings["ClaveAccesoFTP"]) : "";
        internal string PuertoFtp = ConfigurationManager.AppSettings["PuertoAccesoFTP"] != null ? (ConfigurationManager.AppSettings["PuertoAccesoFTP"]) : "";
        
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

        internal System.Diagnostics.TraceSource trace;
        bool _shouldStop = false;

        #endregion

        public TvCableConciliacionService()
        {
            trace = new System.Diagnostics.TraceSource("TvCableConWinService");
            System.Diagnostics.Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            System.Diagnostics.Trace.CorrelationManager.StartLogicalOperation("TvCableConWinService");

            InitializeComponent();
        }

        //protected override void OnStart(string[] args)
        public void OnStart()
        {
            _workerThread = new Thread(this.DoWork);
            _workerThread.Start();
        }

        private void DoWork()
        {
            while (!_shouldStop)
            {
                try
                {
                    var nucleo = new BusinessLayer.Nucleo();

                    //DateTransaction = System.DateTime.Now.AddDays(-1);
                    DateTransaction = System.DateTime.Now;
                    fechaSwitch = DateTransaction.ToString("yyyy/MM/dd");

                    // Verifico si el servicio esta dentro de parametro permitido para ejecutar el proceso
                    string[] strArrayInicio = StrHoraInicio.Split(':');
                    string[] strArrayHasta = StrHoraHasta.Split(':');

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
                                // Testing save Logs
                                trace.TraceEvent(TraceEventType.Start, 100, "Inicia el proceso de conciliacion de TvCable. Fecha actual: " + DateTime.Now);

                                // Verifica si existen archivos de conciliacion de los MDP pendientes de validar su formato(ENTREGADO).
                                var itemEntregado = nucleo.GetItemCatalogoPorCodigoItemCodigoCatalogo(Constants.CodeEstadoArchivosConciliacion, Constants.CodeItemEntregado);
                                

                                trace.TraceEvent(TraceEventType.Stop, 116, "Termina el proceso de conciliacion de TvCable. Fecha actual: " + DateTime.Now);
                                System.Diagnostics.Trace.CorrelationManager.StopLogicalOperation();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                try
                {
                    Thread.Sleep(Convert.ToInt32(DefaultInterval));
                }
                catch (Exception ex)
                {
                    
                }
                finally
                {
                    
                }
            }
        }

        protected override void OnStop()
        {
            if (_workerThread != null)
            {
                trace.TraceEvent(TraceEventType.Stop, 116, "Stop, servicio de conciliacion TvCable");
                System.Diagnostics.Trace.CorrelationManager.StopLogicalOperation();

                _shouldStop = true;
                _workerThread.Join();
            }
        }
    }
}
