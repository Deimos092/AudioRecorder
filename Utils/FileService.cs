using Microsoft.Win32;

namespace AudioRecorder.Utils
{
    class FileService
    {
        public string[] OpenFile(string defaultExtension, string filter,string title,string currentforlder)
        {
            OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.DefaultExt = defaultExtension;
            FileDialog.Filter = filter;
            FileDialog.Multiselect = true;
            FileDialog.Title = title;
            FileDialog.InitialDirectory = currentforlder;

            if (FileDialog.ShowDialog().Value)
            {
                if (FileDialog.FileNames.Length > 0)
                    return FileDialog.FileNames;
                return new string[0];
            }
            else
                return new string[0];
        }
    }
}
