using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAudio.Wave;


namespace AudioRecorder.Model
{
    class CollectionRecord
    {

        Dictionary<string, Record> SoundFilesDic;
        public CollectionRecord() { SoundFilesDic = new Dictionary<string, Record>(); }
        public CollectionRecord(string[] Files)
        {
            SoundFilesDic = new Dictionary<string, Record>();
            if (Files.Count() > 0)
                AddRange(Files);
        }
        public Record this[string index]
        {
            get { return SoundFilesDic[index]; }
            set { SoundFilesDic[index] = value; }
        }
        
        public int Count
        {
            get { return SoundFilesDic.Values.Count; }
        }
        public Record Find(Predicate<Record> predicate)
        {
            return SoundFilesDic.Values.Cast<Record>().ToList().Find(predicate);
        }

        public List<Record> FindAll(Predicate<Record> predicate)
        {
            return SoundFilesDic.Values.Cast<Record>().ToList().FindAll(predicate);
        }

        public void Add(string path)
        {
            SoundFilesDic.Add(Path.GetFileNameWithoutExtension(path), new Record(path));
        }
        public void AddRange(string[] Files)
        {
            FileInfo Info;
            WaveFileReader SoundReader;
            Mp3FileReader SoundMp3Reader;
            foreach (var item in Files)
            {
                if (SoundFilesDic.Keys.Contains(Path.GetFileNameWithoutExtension(item)))
                    continue;

                Info = new FileInfo(item);
                Record record = new Record(Info.FullName);
                record.Size = ((int)Info.Length / 1024).ToString() + " Kb";
                record.Name = Path.GetFileNameWithoutExtension(Info.Name);
                record.Type = Info.Extension;
                if (record.Type == ".wav")
                {
                    SoundReader = new WaveFileReader(item);
                    record.BitRate = string.Format("{0} bit PCM: {1}Hz  {2} channels",
                                                                                        SoundReader.WaveFormat.BitsPerSample,
                                                                                        SoundReader.WaveFormat.SampleRate,
                                                                                        SoundReader.WaveFormat.Channels);
                    record.TrackLenght = string.Format("{0:0#}:{1:0#}:{2:0#}", 
                                                            SoundReader.TotalTime.Hours, 
                                                            SoundReader.TotalTime.Minutes, 
                                                            SoundReader.TotalTime.Seconds);
                }
                else if (record.Type == ".mp3")
                {
                    SoundMp3Reader = new Mp3FileReader(item);
                    record.BitRate = string.Format("{0} bit PCM: {1}Hz  {2} channels",
                                                                                        SoundMp3Reader.WaveFormat.BitsPerSample,
                                                                                        SoundMp3Reader.WaveFormat.SampleRate,
                                                                                        SoundMp3Reader.WaveFormat.Channels);
                    record.TrackLenght = string.Format("{0:0#}:{1:0#}:{2:0#}",
                                                            SoundMp3Reader.TotalTime.Hours,
                                                            SoundMp3Reader.TotalTime.Minutes,
                                                            SoundMp3Reader.TotalTime.Seconds);
                }
                SoundFilesDic.Add(record.Name, record);
            }
        }
        public Record Next(int value)
        {
            return SoundFilesDic.Values.ToList().Skip(value).FirstOrDefault();
        }
        public Record FindByIndex(int index)
        {
            if (index > 0)
                return SoundFilesDic.Values.ToList()[index];
            return SoundFilesDic.Values.ToList()[0];
        }
        public int IndexOf(Record record)
        {
            return SoundFilesDic.Values.ToList().FindIndex(x => x.Name == record.Name);
        }
        public List<Record> GetList()
        {
            return SoundFilesDic.Values.ToList();
        }
        public List<string> GetNames()
        {
            return SoundFilesDic.Keys.ToList();
        }

    }
}
