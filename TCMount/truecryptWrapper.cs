using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TCMount
{
    class TruecryptWrapper
    {
        public TruecryptWrapper()
        {

        }

        public string Mount(string volume, string letter, string password, string keyfile = null)
        {
            string key = "";
            if (keyfile != null)
                key = string.Format(" /keyfile \"{0}\" ", keyfile);
            ProcessStartInfo psi = new ProcessStartInfo("./truecrypt.exe", string.Format("/q /volume \"{0}\" /letter {1} {3} /password \"{2}\"", volume, letter, password, key));
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            Process p = Process.Start(psi);
            p.WaitForExit();
            return p.StandardError.ReadToEnd() + p.StandardOutput.ReadToEnd();
        }

        public string Dismount(string letter)
        {
            ProcessStartInfo psi = new ProcessStartInfo("./truecrypt.exe", string.Format("/q /d {0}", letter));
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            Process p = Process.Start(psi);
            p.WaitForExit();
            return p.StandardError.ReadToEnd() + p.StandardOutput.ReadToEnd();
        }
    }
}
