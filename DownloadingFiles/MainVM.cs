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
        private Model model = new Model();
        private string _url;
        private bool _openDownloadedFile;
        private RelayCommand downloadCommand;
        private RelayCommand cancelCommand;
        private double progressBarValue;
        private string bytesRecieved;
        private string bytesTotal;
        private string speed;
        private string time;


        public string Error => model.Error;

        public string URL
        {
            get { return _url; }
            set
            {
                _url = value;
                OnPropertyChanged(nameof(Error));
            }
        }
        
        public bool OpenDownloadedFile
        {
            get { return _openDownloadedFile; }
            set
            {
                _openDownloadedFile = value;
            }
        }

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

        public string BytesRecieved
        {
            get => bytesRecieved;
            set
            {
                bytesRecieved = value;
                if (progressBarValue == 100 && _openDownloadedFile == true)
                    model.OpenFile(_url);
                OnPropertyChanged(nameof(BytesRecieved));
            }
        }

        public string Speed
        {
            get => speed;
            set
            {
                speed = value;                
                OnPropertyChanged(nameof(Speed));
            }
        }

        public string Time
        {
            get => time;
            set
            {
                time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        public RelayCommand DownloadCommand
        {
            get
            {
                return downloadCommand ??
                    (downloadCommand = new RelayCommand(DownloadButton_Click));
            }
        }

        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                    (cancelCommand = new RelayCommand(CancelButton_Click));
            }
        }

        public void DownloadButton_Click(object obj)
        {
            if (_url != null || _url != "")
                model.DownloadFile(_url, p => ProgressBarValue = p, b => BytesTotal = b,
                    r  => BytesRecieved = r, s => Speed = s, t => Time = t);
            OnPropertyChanged(nameof(Error));
        }

        public void CancelButton_Click(object obj)
        {
            if (_url != null || _url != "")
                model.CancelDownloading(p => ProgressBarValue = p, t => BytesTotal = t, r => bytesRecieved = r);
            OnPropertyChanged(nameof(Error));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
