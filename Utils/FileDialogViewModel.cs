using System.Windows.Input;

namespace AudioRecorder.Utils
{
    class FileDialogViewModel
    {
        public FileDialogViewModel()
        {
            this.OpenCommand = new RelayCommand(OpenFiles);
        }

        public string[] FileNames { get; set; }
        public string Title { get; set; }
        public string Extension { get; set; }
        public string Filter { get; set; }

        public string CurrentFolder { get; set; }
        public ICommand OpenCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        private void OpenFiles()
        {
            FileService fileServices = new FileService();

            FileNames = fileServices.OpenFile(Extension, Filter, Title,CurrentFolder);
        }
    }
}
