using System;

namespace Vmgr.Packager
{
    class Program
    {
        static int Main(string[] args)
        {
            int result = 0;

            try
            {
                Program.Execute();

                Console.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                // Return error
                Console.WriteLine(ex.ToString());

                result = 1;
            }

            return result;
        }

        private static void Execute()
        {
            PackageHandler solutionHandler = new PackageHandler();
            solutionHandler.Execute();
        }
    }
}
