using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmgr.SSLTool
{
    class Options
    {
        [Option('r', "port", Required = true, HelpText = "Provide the port to be used for certificate binding.")]
        public int Port { get; set; }

        [Option('s', "protocol", Required = false, HelpText = "Provide the protocol to be used to determine certificate binding.")]
        public string Protocol { get; set; }

        [Option('t', "thumbprint", Required = true, HelpText = "Provide the certificate hash, also referred to as the thumbprint.")]
        public string Thumbprint { get; set; }

        [Option('u', "username", Required = true, HelpText = "Provide the user account for reserving a URL.")]
        public string Username { get; set; }
    }
}
