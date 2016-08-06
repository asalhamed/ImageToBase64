using System;
using System.Drawing;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace ImgToBase64
{
    internal class MainModel : BindableBase
    {
        private readonly MainWindow _mainWindow;

        public MainModel()
        {
            
        }
        public MainModel(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            CopyCommand = new DelegateCommand(
                (o) => Clipboard.SetText(_imageBase64), 
                (o => !string.IsNullOrEmpty(_imageBase64)));
            ClearCommand = new DelegateCommand(
                ((o) =>
                {
                    ImageBase64 = "";
                    FilePath = "";
                }),
                (o => !string.IsNullOrEmpty(_imageBase64)));
            CloseCommand = new DelegateCommand((o => _mainWindow.Close()));
            BrowseCommand = new DelegateCommand(DoBrowse);
        }

        private void DoBrowse(object obj)
        {
            var openFile = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Images|*.png;*.jpg;*.bmp|All Files|*.*",
                Title = "Select Image to Convert",
                Multiselect = false,
                ShowReadOnly = true,
                RestoreDirectory = true
            };

            if (!(openFile.ShowDialog(_mainWindow) ?? false)) return;
            
            using (var image = Image.FromFile(openFile.FileName))
            {
                using (var m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    FilePath = openFile.FileName;
                    ImageBase64 = Convert.ToBase64String(m.ToArray());
                }
            }
        }

        private string _filePath;
        public string FilePath
        {
            set { SetProperty(ref _filePath, value); }
            get { return _filePath; }
        }

        private string _imageBase64 ;
        public string ImageBase64
        {
            set
            {
                if (!SetProperty(ref _imageBase64, value)) return;
                CopyCommand.RaiseCanExecuteChanged();
                ClearCommand.RaiseCanExecuteChanged();
            }
            get { return _imageBase64; }
        }

        public DelegateCommand CopyCommand { get; }
        public DelegateCommand ClearCommand { get; }
        public DelegateCommand CloseCommand { private set; get; }
        public DelegateCommand BrowseCommand { private set; get; }


    }
}
