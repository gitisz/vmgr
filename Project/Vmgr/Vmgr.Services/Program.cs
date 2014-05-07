using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;

namespace Vmgr.Services
{
    static class Program
    {

#if CONSOLE
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            System.Console.WriteLine("Press ENTER to start.");
            System.Console.ReadLine();
            
            Service s = new Service();
            s.Start();

            System.Console.WriteLine("Press ENTER to stop.");
            System.Console.ReadLine();

            s.Stop();
        }
#else	
        static void Main()
        {
            ServiceBase[] ServicesToRun;

            ServicesToRun = new ServiceBase[] 
			{ 
				new Service() 
			};
            ServiceBase.Run(ServicesToRun);

        }
#endif
    }
}
