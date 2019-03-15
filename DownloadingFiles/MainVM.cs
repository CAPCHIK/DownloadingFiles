using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadingFiles
{
    class MainVM : INotifyPropertyChanged
    {

        private string url;
        private RelayCommand downloadCommand;
        private RelayCommand cancelCommand;
        private double progressBarValue;
        private string bytesReceived;
        private string bytesTotal;
        private string speed;
        private string time;
        private string error;
        private readonly Model model;
        private long totalBytes;

        public MainVM()
        {
            model = new Model();
            model.FileSizeChanged += bytes => BytesTotal = PrettyBytes(totalBytes = bytes);
            model.DownloadBytesChanged += (bytes, time) =>
            {
                BytesReceived = PrettyBytes(bytes);
                Speed = DownloadingSpeed(bytes, time);
                Time = DownloadingTime(bytes, totalBytes, time);
            };
            model.ProgressPercentageChanged += percentage => ProgressBarValue = percentage;
            model.DownloadComplete += () =>
            {
                BytesReceived = "";
                BytesTotal = "";
                Speed = "";
                Time = "";
                ProgressBarValue = 0;
            };
        }

        public string Error
        {
            get => error;
            private set
            {
                error = value;
                OnPropertyChanged(nameof(Error));
            }
        }
        public string URL
        {
            get => url;
            set
            {
                url = value;
                OnPropertyChanged(nameof(URL));
            }
        }

        public bool OpenDownloadedFile { get; set; }

        public double ProgressBarValue
        {
            get => progressBarValue;
            set
            {
                progressBarValue = value;
                OnPropertyChanged(nameof(ProgressBarValue));
            }
        }

        public string BytesTotal
        {
            get => bytesTotal;
            private set
            {
                bytesTotal = value;
                OnPropertyChanged(nameof(BytesTotal));
            }
        }

        public string BytesReceived
        {
            get => bytesReceived;
            private set
            {
                bytesReceived = value;
                OnPropertyChanged(nameof(BytesReceived));
            }
        }

        public string Speed
        {
            get => speed;
            private set
            {
                speed = value;
                OnPropertyChanged(nameof(Speed));
            }
        }

        public string Time
        {
            get => time;
            private set
            {
                time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        public RelayCommand DownloadCommand =>
            downloadCommand ??
            (downloadCommand = new RelayCommand(DownloadButton_Click));

        public RelayCommand CancelCommand =>
            cancelCommand ??
            (cancelCommand = new RelayCommand(CancelButton_Click));

        private void DownloadButton_Click(object obj)
        {
            if (url == null && url == "") return;
            try
            {
                model.DownloadFile(url, OpenDownloadedFile);
            }
            catch (Exception e)
            {
                Error = e.Message;
            }
        }

        private void CancelButton_Click(object obj)
        {
            if (url != null || url != "")
                model.CancelDownloading();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        private static string PrettyBytes(double bytes)
        {
            if (bytes < 1024)
                return bytes + "Б";
            if (bytes < Math.Pow(1024, 2))
                return (bytes / 1024).ToString("F" + 2) + "КБ";
            if (bytes < Math.Pow(1024, 3))
                return (bytes / Math.Pow(1024, 2)).ToString("F" + 2) + "МБ";
            if (bytes < Math.Pow(1024, 4))
                return (bytes / Math.Pow(1024, 5)).ToString("F" + 2) + "ГБ";
            return (bytes / Math.Pow(1024, 4)).ToString("F" + 2) + "ТБ";
        }

        public static string DownloadingSpeed(long received, TimeSpan time)
        {
            return ((double)received / 1024 / 1024 / time.TotalSeconds).ToString("F" + 2) + " МБ/СЕК";
        }
        public static string DownloadingTime(long received, long total, TimeSpan time)
        {
            var receivedD = (double) received;
            var totalD = (double) total;
            return ((totalD / (receivedD / time.TotalSeconds)) - time.TotalSeconds).ToString("F" + 1) + "СЕК";
        }
    }
}
