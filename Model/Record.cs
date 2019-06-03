namespace AudioRecorder.Model
{
    class Record
    {
        public Record() { }

        public Record(string path)
        {
            PathToFile = path;
        }

        public string Size { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string PathToFile { get; set; }

        public string BitRate { get; set; }

        public string TrackLenght { get; set; }
       
    }
}
