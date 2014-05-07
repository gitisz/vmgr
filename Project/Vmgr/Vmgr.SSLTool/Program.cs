using CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmgr.SSLTool
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            //System.Diagnostics.Debugger.Launch();
#endif
            var options = new Options();

            if (Parser.Default.ParseArguments(args, options))
            {
                // Values are available here
                if (options.Port == 0)
                    throw new ApplicationException("Port value must be greater than 0.");

                if (options.Protocol == string.Empty)
                    throw new ApplicationException("Protocol value must be specified.");

                if (options.Username == string.Empty)
                    throw new ApplicationException("Username value must be specified.");
            }

            try
            {
                /*
                 * Bind Certificate to RT
                 */
                bool exists = false;
                
                bool add = options.Protocol.Equals("HTTPS", StringComparison.InvariantCultureIgnoreCase);

                string querySslCertOuput = Program.QuerySslCert(options.Port);

                if (querySslCertOuput.Contains(string.Format("0.0.0.0:{0}", options.Port)))
                {
                    exists = true;

                    Console.WriteLine("An existing IP Port binding was detected.");

                    if (!string.IsNullOrEmpty(options.Thumbprint))
                        if (querySslCertOuput.Contains(options.Thumbprint))
                        {
                            Console.WriteLine("The SSL Certificate is already bound to IP Port 0.0.0.0:{0}.", options.Port);
                        }
                }

                Program.DeleteSslBinding(options.Port);

                if (exists)
                {
                    Console.WriteLine("Deleting existing certificate binding for IP Port 0.0.0.0:{0}.", options.Port);

                    if (add)
                        if (!string.IsNullOrEmpty(options.Thumbprint))
                            Program.AddSslBinding(options.Port, options.Thumbprint);
                }
                else
                {
                    if (add)
                        if (!string.IsNullOrEmpty(options.Thumbprint))
                            Program.AddSslBinding(options.Port, options.Thumbprint);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to failed to bind certificate to port.  Exception: {0}", ex);

                throw ex;
            }

            try
            {

                /*
                 * Reserve port
                 */

                bool exists = false;

                string queryUrlAcl = Program.QueryUrlAcl(options.Port);

                if (queryUrlAcl.Contains(string.Format("https://*:{0}/", options.Port)))
                {
                    exists = true;

                    Console.WriteLine("An existing URL has been reserved.");
                }

                if (exists)
                {
                    Program.DeleteUrlReservation(options.Port);

                    Program.AddUrlReservation(options.Port, options.Username);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to failed to reserve URL.  Exception: {0}", ex);

                throw ex;
            }
        }

        private static string QuerySslCert(int port)
        {
            Process querySslCertProcess = new Process();
            querySslCertProcess.StartInfo.UseShellExecute = false;
            querySslCertProcess.StartInfo.RedirectStandardOutput = true;
            querySslCertProcess.StartInfo.FileName = "netsh.exe";
            querySslCertProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            querySslCertProcess.StartInfo.Arguments = string.Format("http show sslcert ipport=0.0.0.0:{0} "
                , port
                );
            querySslCertProcess.Start();

            string querySslCertOuput = querySslCertProcess.StandardOutput.ReadToEnd();
            querySslCertProcess.WaitForExit();

            return querySslCertOuput;
        }

        private static string QueryUrlAcl(int port)
        {
            Process queryUrlAclProcess = new Process();
            queryUrlAclProcess.StartInfo.UseShellExecute = false;
            queryUrlAclProcess.StartInfo.RedirectStandardOutput = true;
            queryUrlAclProcess.StartInfo.FileName = "netsh.exe";
            queryUrlAclProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            queryUrlAclProcess.StartInfo.Arguments = string.Format("http show urlacl url=https://*:{0}/ "
                , port
                );
            queryUrlAclProcess.Start();

            string queryUrlAcl = queryUrlAclProcess.StandardOutput.ReadToEnd();
            queryUrlAclProcess.WaitForExit();

            return queryUrlAcl;
        }

        private static void DeleteUrlReservation(int port)
        {
            Process deleteUrlAclProcess = new Process();
            deleteUrlAclProcess.StartInfo.UseShellExecute = false;
            deleteUrlAclProcess.StartInfo.RedirectStandardOutput = true;
            deleteUrlAclProcess.StartInfo.FileName = "netsh.exe";
            deleteUrlAclProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            deleteUrlAclProcess.StartInfo.Arguments = string.Format("http delete urlacl url=https://*:{0}/ "
                , port
                );
            deleteUrlAclProcess.Start();

            string deleteUrlAclOutput = deleteUrlAclProcess.StandardOutput.ReadToEnd();
            deleteUrlAclProcess.WaitForExit();

            Console.WriteLine(deleteUrlAclOutput);
        }

        private static void AddUrlReservation(int port, string username)
        {
            Process addUrlAclProcess = new Process();
            addUrlAclProcess.StartInfo.UseShellExecute = false;
            addUrlAclProcess.StartInfo.RedirectStandardOutput = true;
            addUrlAclProcess.StartInfo.FileName = "netsh.exe";
            addUrlAclProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            addUrlAclProcess.StartInfo.Arguments = string.Format("http add urlacl url=https://*:{0}/ user={1} listen=yes "
                , port
                , username
                );
            addUrlAclProcess.Start();

            string addUrlAclOutput = addUrlAclProcess.StandardOutput.ReadToEnd();
            addUrlAclProcess.WaitForExit();

            Console.WriteLine(addUrlAclOutput);
        }

        private static void AddSslBinding(int port, string thumbprint)
        {
            Process addSslCertProcess = new Process();
            addSslCertProcess.StartInfo.UseShellExecute = false;
            addSslCertProcess.StartInfo.RedirectStandardOutput = true;
            addSslCertProcess.StartInfo.FileName = "netsh.exe";
            addSslCertProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            addSslCertProcess.StartInfo.Arguments = string.Format("http add sslcert ipport=0.0.0.0:{0} certhash={1} appid={{87d246aa-db90-4b66-8b01-88f7af2e36bf}} certstorename=MY"
                , port
                , thumbprint
                );
            addSslCertProcess.Start();

            string addSslCertOuput = addSslCertProcess.StandardOutput.ReadToEnd();
            addSslCertProcess.WaitForExit();

            Console.WriteLine(addSslCertOuput);
        }

        private static void DeleteSslBinding(int port)
        {
            Process deleteSslCertProcess = new Process();
            deleteSslCertProcess.StartInfo.UseShellExecute = false;
            deleteSslCertProcess.StartInfo.RedirectStandardOutput = true;
            deleteSslCertProcess.StartInfo.FileName = "netsh.exe";
            deleteSslCertProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            deleteSslCertProcess.StartInfo.Arguments = string.Format("http delete sslcert ipport=0.0.0.0:{0}"
                , port
                );
            deleteSslCertProcess.Start();

            string deleteSslCertOuput = deleteSslCertProcess.StandardOutput.ReadToEnd();
            deleteSslCertProcess.WaitForExit();

            Console.WriteLine(deleteSslCertOuput);
        }
    }
}
