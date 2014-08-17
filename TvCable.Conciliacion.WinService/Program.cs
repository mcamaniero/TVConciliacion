using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace TvCable.Conciliacion.WinService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
//#if !DEBUG 
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new TvCableConciliacionService() 
            };
            ServiceBase.Run(ServicesToRun);
            
//#else
       //TvCableConciliacionService obj = new TvCableConciliacionService();
            //obj.OnStart();     
//            TvCableConciliacionService inicio = new TvCableConciliacionService();
//            inicio.DoWork();
//#endif
        }
    }
}
