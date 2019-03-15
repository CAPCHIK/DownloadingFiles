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
    public class Model
    {
        private readonly WebClient webClient = new WebClient();
        private readonly Stopwatch stopWatch = new Stopwatch();


        public event Action<long> FileSizeChanged;
        public event Action<long, TimeSpan> DownloadBytesChanged;
        public event Action<double> ProgressPercentageChanged;
        public event Action DownloadComplete;

        public void DownloadFile(string url, bool openAfterDownload)
        {
            if (webClient.IsBusy)
                throw new Exception("Данный клиент для скачивания занят");
            try
            {
                var startDownloading = DateTime.UtcNow;
                webClient.Proxy = null;
                if (!SelectFolder(Path.GetFileName(url), out var filePath))
                    throw DownloadingError();
                webClient.DownloadProgressChanged += (o, args) =>
                {
                    ProgressPercentageChanged?.Invoke(args.ProgressPercentage);
                    FileSizeChanged?.Invoke(args.TotalBytesToReceive);
                    DownloadBytesChanged?.Invoke(args.BytesReceived, DateTime.UtcNow - startDownloading);
                    if (args.ProgressPercentage >= 100 && openAfterDownload)
                        Process.Start(filePath);
                };
                webClient.DownloadFileCompleted += (o, args) => DownloadComplete?.Invoke();
                stopWatch.Start();
                webClient.DownloadFileAsync(new Uri(url), filePath);
            }
            catch (Exception e)
            {
                throw DownloadingError();
            }
        }

        public void CancelDownloading()
        {
            webClient.CancelAsync();
            webClient.Dispose();
            DownloadComplete?.Invoke();
        }

        private static Exception DownloadingError()
            => new Exception("Ошибка при скачивании! Проверьте URL\n и не забудьте указать путь");

        private static bool SelectFolder(string fileName, out string filePath)
        {
            var saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = "c:\\",
                FileName = fileName,
                DefaultExt = Path.GetExtension(fileName) + " ",
                Filter = "All files (*.*)|*.*"
            };
            filePath = "";
            if (saveFileDialog.ShowDialog() != true) return false;
            filePath = saveFileDialog.FileName;
            return true;
        }
    }
}
