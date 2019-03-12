using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace DownloadingFiles
{
    public static class Model
    {
        public static WebClient wc = new WebClient();
        public static string Error;
        public static string FilePath;
        private static string _bytesRecieved;
        private static string _bytesTotal;
        public static void SelectFolder(string url)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = "c:\\";
            saveFileDialog.FileName = System.IO.Path.GetFileNameWithoutExtension(url);
            saveFileDialog.DefaultExt = System.IO.Path.GetExtension(url) + " ";
            saveFileDialog.Filter = "All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {      
                FilePath = saveFileDialog.FileName;
            }
        }

        public static void DownloadFile(string url, bool openDownloadFile, Action<double> progressNotify) 
        {
            try
            {
                Error = "";
                wc.Proxy = null;
                SelectFolder(url);
                wc.DownloadProgressChanged += (o, args) => progressNotify(args.ProgressPercentage);
                wc.DownloadFileAsync(new Uri(url), FilePath);
                if (openDownloadFile == true)
                {
                    OpenFile(url);
                }
            }
            catch (ArgumentNullException e)
            {
                
            }
            catch (Exception e)
            {
                Error = "Ошибка при скачивании! Проверьте URL\n и не забудьте указать путь";
            }
        }
        public static void OpenFile(string url)
        {
            Process.Start(FilePath);
        }

        public static string BytesRecieved
        {
            get { return _bytesRecieved; }
            set
            {
                _bytesRecieved = value;
            }
        }

        public static string BytesTotal
        {
            get { return _bytesTotal; }
            set
            {
                _bytesTotal = value;
            }
        }

        public static void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            _bytesRecieved = e.BytesReceived.ToString();
            _bytesTotal = e.TotalBytesToReceive.ToString();
        }
    }
}
