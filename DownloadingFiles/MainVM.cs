using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadingFiles
{ 
    class MainVM : INotifyPropertyChanged
    {
        public string Error => Model.Error;
        private string url;
        private RelayCommand downloadCommand;
        private RelayCommand cancelCommand;
        private double progressBarValue;
        private string bytesReceived;
        private string bytesTotal;
        private string speed;
        private string time;
        public string URL
        {
            get => url;
            set
            {
                url = value;
                OnPropertyChanged(nameof(Error));
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
            set
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
                if (progressBarValue >= 100 && OpenDownloadedFile)
                    Model.OpenFile(url);
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
            if (url != null || url != "")
                Model.DownloadFile(url, p => ProgressBarValue = p, b => BytesTotal = b,
                    r  => BytesReceived = r, s => Speed = s, t => Time = t);
            OnPropertyChanged(nameof(Error));
        }

        private void CancelButton_Click(object obj)
        {
            if (url != null || url != "")
                Model.CancelDownloading(p => ProgressBarValue = p, t => BytesTotal = t, r => bytesReceived = r);
            OnPropertyChanged(nameof(Error));
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
    }
}
