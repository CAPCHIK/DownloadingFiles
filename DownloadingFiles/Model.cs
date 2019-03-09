using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DownloadingFiles
{
    public static class Model
    {
        public static string Error;
        public static string FilePath;
        public static void SelectFolder()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                FilePath = saveFileDialog.FileName;
        }
        public static void DownloadFile(string url)
        {

            try
            {
                WebClient myWebClient = new WebClient();
                myWebClient.DownloadFile(url, FilePath);
            }
            catch (Exception e)
            {
                Error = "Ошибка при скачивании! Проверьте URL\n и не забудьте указать путь";
            }
        }
    }
}
