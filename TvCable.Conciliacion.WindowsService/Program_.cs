using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TvCable.Conciliacion.WindowsService
{
    static class Program_
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            /*
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new TvCableConciliacionService() 
			};
            ServiceBase.Run(ServicesToRun);
            */

            // Test
            TvCableConciliacionService obj = new TvCableConciliacionService();
            obj.OnStart();
        }
    }
}
