using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TCMount
{
    using System.IO;
    using System.Threading;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Thread t = new Thread(() => {
                List<string> _drives = new List<string>();
                while (true)
                {
                    Thread.Sleep(100);
                    var drives = DriveInfo.GetDrives().Select(driveInfo => driveInfo.RootDirectory.FullName);
                    // Remove all ejected drives
                    _drives.RemoveAll(drive => !drives.Contains(drive));
                    foreach (var drive in drives)
                    {
                        if (!_drives.Contains(drive))
                        {
                            _drives.Add(drive);
                            var di = new DirectoryInfo(drive);
                            FileInfo[] candidates;
                            try
                            {
                                candidates = di.GetFiles("*.tc");
                            }
                            catch (Exception)
                            {
                                continue;
                            }

                            foreach (var candidate in candidates)
                            {
                                if (candidate.Name.Length > 4)
                                    continue;
                                var letter = candidate.Name.Remove(1);
                                Automounter mounter = new Automounter(candidate.FullName, letter);
                                frmEnterPassword enterPassword = new frmEnterPassword();
                                if (enterPassword.ShowDialog() == DialogResult.OK)
                                {
                                    string rest = mounter.Mount(enterPassword.Password);
                                    var diInsideKey = new DirectoryInfo(letter + ":\\");
                                    var keyCandidates = diInsideKey.GetFiles("*.key");
                                    foreach (var keyCandidate in keyCandidates)
                                    {
                                        if (keyCandidate.Name.Length != 5)
                                            continue;
                                        var letterTc = keyCandidate.Name.Remove(1);
                                        var di2 = new DirectoryInfo(letterTc + ":\\");
                                        if(di2.Exists)
                                        {
                                            MessageBox.Show("Drive " + letterTc + " is already mounted");
                                            continue;
                                        }
                                        var tcLocation = keyCandidate.FullName.Replace(".key", ".txt");
                                        if (!File.Exists(tcLocation)) continue;
                                        var containerLocation = File.ReadAllText(tcLocation);
                                        Automounter tcMount = new Automounter(containerLocation, letterTc, keyCandidate.FullName);
                                        tcMount.Mount(enterPassword.Password);
                                        _drives.Add(letterTc.ToUpper() + ":\\");
                                    }
                                    mounter.Dismount();
                                }
                            }
                        }
                    }
                }
                
            });
            t.Start();
            Application.Run(new frmMain());
            t.Abort();
        }
    }
}
