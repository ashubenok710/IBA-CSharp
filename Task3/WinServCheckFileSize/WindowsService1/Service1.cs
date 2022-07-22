using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Text;

namespace WindowsService1
{
    [RunInstaller(true)]
    public partial class Service1 : ServiceBase
    {

        private string PathToFolder = ConfigurationSettings.AppSettings["PathToFolder"];

        private long FileSize = (long)Convert.ToDouble(ConfigurationSettings.AppSettings["FileSize"]);

        private string Path = ConfigurationSettings.AppSettings["PathToFile"];

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = PathToFolder;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.*";

            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);

            watcher.EnableRaisingEvents = true;
        }


        private void OnChanged(object source, FileSystemEventArgs e)
        {
            var info = new FileInfo(e.FullPath);
            var theSize = info.Length;

            if (theSize > FileSize)
            {
                PushToFile("File: " + e.Name + " " + e.ChangeType + " " + theSize + " bytes");
            }
        }
        public void PushToFile(string str)
        {
            var sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " " + str);

            Encoding isoLatin1Encoding = Encoding.GetEncoding("ISO-8859-1");
            TextWriter tw = new StreamWriter(Path, true, isoLatin1Encoding);
            tw.WriteLine(sb.ToString());
            tw.Close();
        }

        protected override void OnStop()
        {
        }
    }
}
