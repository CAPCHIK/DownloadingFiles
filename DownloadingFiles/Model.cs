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
using System.Threading;

namespace DownloadingFiles
{
    public static class Model
    {
        public static WebClient wc = new WebClient();
        public static Stopwatch sw = new Stopwatch();
        public static string Error;
        public static string FilePath;
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

        public static void DownloadFile(string url, Action<double> progressNotify, Action<string> bytesTotal, Action<string> bytesRecieved,
            Action<string> speed, Action<string> time) 
        {
            try
            {
                Error = "";
                wc.Proxy = null;
                SelectFolder(url);
                wc.DownloadProgressChanged += (o, args) => progressNotify(args.ProgressPercentage);
                wc.DownloadProgressChanged += (o, args) => bytesTotal(ProgressTotal(o, args));
                wc.DownloadProgressChanged += (o, args) => bytesRecieved(ProgressChanged(o, args));
                wc.DownloadProgressChanged += (o, args) => speed(DownloadingSpeed(o, args));
                wc.DownloadProgressChanged += (o, args) => time(DownloadingTime(o, args));
                sw.Start();
                wc.DownloadFileAsync(new Uri(url), FilePath);
                wc.DownloadFileCompleted += (o, args) => progressNotify(0);
                wc.DownloadFileCompleted += (o, args) => bytesTotal("Download completed!");
                wc.DownloadFileCompleted += (o, args) => bytesRecieved("");
                wc.DownloadFileCompleted += (o, args) => speed("");
                wc.DownloadFileCompleted += (o, args) => time("");
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
        public static void CancelDownloading(Action<double> progressNotify, Action<string> bytesTotal, Action<string> bytesRecieved)
        {
            wc.CancelAsync();
            wc.Dispose();
            wc.DownloadFileCompleted += (o, args) => progressNotify(0);
            wc.DownloadFileCompleted += (o, args) => bytesTotal("Canceled.");
            wc.DownloadFileCompleted += (o, args) => bytesRecieved("");
        }
        public static string ProgressChanged(object sender, DownloadProgressChangedEventArgs args)
        {
            if (args.BytesReceived < 1024)
                return args.BytesReceived.ToString() + "Б";
            else if (args.BytesReceived < Math.Pow(1024, 2))
                return (args.BytesReceived / 1024).ToString("F" + 2) + "КБ";
            else if (args.BytesReceived < Math.Pow(1024, 3))
                return (args.BytesReceived / Math.Pow(1024, 2)).ToString("F" + 2) + "МБ";
            else if (args.BytesReceived < Math.Pow(1024, 4))
                return (args.BytesReceived / Math.Pow(1024, 5)).ToString("F" + 2) + "ГБ";
            else
                return (args.BytesReceived / Math.Pow(1024, 4)).ToString("F" + 2) + "ТБ";
                
        }
        public static string ProgressTotal(object sender, DownloadProgressChangedEventArgs args)
        {
            if (args.TotalBytesToReceive < 1024)
                return args.TotalBytesToReceive.ToString() + "Б";
            else if (args.TotalBytesToReceive < Math.Pow(1024, 2))
                return (args.TotalBytesToReceive / 1024).ToString("F" + 2) + "КБ";
            else if (args.TotalBytesToReceive < Math.Pow(1024, 3))
                return (args.TotalBytesToReceive / Math.Pow(1024, 2)).ToString("F" + 2) + "МБ";
            else if (args.TotalBytesToReceive < Math.Pow(1024, 4))
                return (args.TotalBytesToReceive / Math.Pow(1024, 5)).ToString("F" + 2) + "ГБ";
            else
                return (args.TotalBytesToReceive / Math.Pow(1024, 4)).ToString("F" + 2) + "ТБ";
        }
        public static string DownloadingSpeed(object sender, DownloadProgressChangedEventArgs args)
        {
            return (args.BytesReceived / 1024 / 1024 /sw.Elapsed.TotalSeconds).ToString("F" + 2) + " МБ/СЕК";
        }
        public static string DownloadingTime(object sender, DownloadProgressChangedEventArgs args)
        {
            return ((args.TotalBytesToReceive / (args.BytesReceived / sw.Elapsed.TotalSeconds))-sw.Elapsed.TotalSeconds).ToString("F" + 1) + "СЕК";
        }
    }
}
