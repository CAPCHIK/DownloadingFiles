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
        private string _url;
        private bool _openDownloadedFile;
        private RelayCommand updateCommand;


        public string URL
        {
            get { return _url; }
            set
            {
                _url = value;
                OnPropertyChanged("Error");
                OnPropertyChanged("LoadPercentage");
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

        public RelayCommand UpdateCommand
        {
            get
            {
                return updateCommand ??
                    (updateCommand = new RelayCommand(Button_Click));
            }
        }
        public void Button_Click(object obj)
        {
            if (_url != null || _url != "")
                Model.DownloadFile(_url, _openDownloadedFile);
            OnPropertyChanged("Error");
            OnPropertyChanged("LoadPercentage");
        }

        public string LoadPercentage => Model.BytesRecieved + "/\n" + Model.BytesTotal;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
