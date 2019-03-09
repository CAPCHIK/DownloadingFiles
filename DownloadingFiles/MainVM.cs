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
        public string URL
        {
            get { return _url; }
            set
            {
                _url = value;
                OnPropertyChanged("Error");
            }
        }
        private bool _openDownloadedFile;
        public bool OpenDownloadedFile
        {
            get { return _openDownloadedFile; }
            set
            {
                _openDownloadedFile = value;
            }
        }
        private RelayCommand updateCommand;
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
            if (_url != null)
            {
                Model.DownloadFile(_url, _openDownloadedFile);
            }
            OnPropertyChanged("Error");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
