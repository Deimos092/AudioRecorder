using System;
using System.Windows.Forms;
using NAudio.Wave;

namespace AudioRecorder.Model
{
    class AudioPlayBack : IDisposable
    {
        private IWavePlayer playbackDevice;
        private WaveStream fileStream;
        private SampleAggregator SampleAggregator;
        public event EventHandler<FftEventArgs> FftCalculated;

        public event EventHandler<MaxSampleEventArgs> MaximumCalculated;

        public void Load(string fileName)
        {
            Stop();
            CloseFile();
            EnsureDeviceCreated();
            OpenFile(fileName);
        }

        private void CloseFile()
        {
            fileStream?.Dispose();
            fileStream = null;
        }

        private void OpenFile(string fileName)
        {
            try
            {
                var inputStream = new MediaFoundationReader(fileName);
                fileStream = inputStream;

                SampleAggregator = new SampleAggregator(fileStream.ToSampleProvider());
                SampleAggregator.NotificationCount = inputStream.WaveFormat.SampleRate / 100;
                SampleAggregator.PerformFFT = true;
                SampleAggregator.FftCalculated += (s, a) => FftCalculated?.Invoke(this, a);
                SampleAggregator.MaximumCalculated += (s, a) => MaximumCalculated?.Invoke(this, a);
                Volume = 0.5f;
                playbackDevice.Volume = Volume;
                playbackDevice.Init(SampleAggregator);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Проблема открытия файла");
                CloseFile();
            }
        }
        public float Volume
        {
            get { return playbackDevice.Volume; }
            set
            {
                if (playbackDevice != null)
                    playbackDevice.Volume = value;
            }
        }

        public long CurrentPosition
        {
            get
            {
                return fileStream.Position;
            }
            set
            {
                fileStream.Position = value;
            }
        }
        public long Length
        {
            get { return fileStream.Length; }
        }

        private void EnsureDeviceCreated()
        {
            if (playbackDevice == null)
            {
                CreateDevice();
            }
        }
        private void CreateDevice()
        {
            playbackDevice = new WaveOut { DesiredLatency = 200 };
        }

        public string CurrentTime
        {
            get {
                if (fileStream != null)
                    return fileStream.CurrentTime.ToString();
                else
                    return "00:00:00";
            }
            set
            {
                fileStream.CurrentTime = TimeSpan.Parse(value);
            }
        }
        public PlaybackState PlaybackState { get; private set; }
        public void Play()
        {
            if (playbackDevice != null && fileStream != null && playbackDevice.PlaybackState != PlaybackState.Playing)
            {
                this.PlaybackState = playbackDevice.PlaybackState;
                playbackDevice.Play();
            }
        }

        public void Pause()
        {
            playbackDevice?.Pause();
        }

        public void Stop()
        {
            playbackDevice?.Stop();
            if (fileStream != null)
            {
                fileStream.Position = 0;
            }
        }

        public void Dispose()
        {
            Stop();
            CloseFile();
            playbackDevice?.Dispose();
            playbackDevice = null;
        }
    }
}
