using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using System.Collections.ObjectModel;
using AudioRecorder.Utils;
using AudioRecorder.Model;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Windows.Forms;
using System.Windows.Threading;

namespace AudioRecorder.ViewModel
{
    class SpectrumAnalyzerVisualization : IVisualizationPlugin
    {
        private readonly SpectrumAnalyser spectrumAnalyser = new SpectrumAnalyser();

        public string Name => "Spectrum Analyser";

        public object Content => spectrumAnalyser;


        public void OnMaxCalculated(float min, float max)
        {
            // nothing to do
        }

        public void OnFftCalculated(NAudio.Dsp.Complex[] result)
        {
            spectrumAnalyser.Update(result);
        }
    }
    class MainViewModel : BaseViewModel
    {
        //--------------------------------------------------------------------------------------------------------
        public MainViewModel()
        {
            RecordCollection = new CollectionRecord();

            this.visualizations = new SpectrumAnalyzerVisualization();
            this.AudioPlayBack = new AudioPlayBack();

            AudioPlayBack.MaximumCalculated += audioGraph_MaximumCalculated;
            AudioPlayBack.FftCalculated += audioGraph_FftCalculated;

            var enumerator = new MMDeviceEnumerator();
            CaptureDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToArray();
            var defaultDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
            SelectedDevice = CaptureDevices.FirstOrDefault(c => c.ID == defaultDevice.ID);
            synchronizationContext = SynchronizationContext.Current;

            this.OpenFileCommand = new RelayCommand(this.OpenFolderAndLoadSound);

            this.PlayCommand = new RelayCommand(this.Play);
            this.StopCommand = new RelayCommand(this.Stop);

            this.NextSound = new RelayCommand(this.NextTrack);
            this.PreviousSound = new RelayCommand(this.PreviosTrack);

            this.StartRecordCommand = new RelayCommand(this.Record);
            this.StopRecordCommand = new RelayCommand(this.StopRecord);

            this.CloseWindowCommand = new RelayCommand(this.CloseWindow);
            //---------------------------------------------------------------
            timer.Interval = TimeSpan.FromMilliseconds(900);//Интервал вызова - Обновлять позицию каждые 900 миллисекунд
            timer.Tick += TimerOnTick;//Привязка метода к таймеру
        }

        //------------------------- ОКРЫТЬ И ЗАГРУЗИТЬ В КОЛЛЕКЦИЮ СПИСОК ФАЙЛОВ ---------------------------------
        private void OpenFolderAndLoadSound()
        {
            FileDialogViewModel DialogViewModel = new FileDialogViewModel();
            DialogViewModel.CurrentFolder = Directory.GetCurrentDirectory();
            DialogViewModel.Extension = "*.mp3";
            DialogViewModel.Filter = "Wave file|*.wav|Mp3 file|*.mp3|All Files(.*)|*.*";
            DialogViewModel.Title = "Выбор медиа файла";

            if (DialogViewModel.OpenCommand.CanExecute(null))
                DialogViewModel.OpenCommand.Execute(null);

            RecordCollection.AddRange(DialogViewModel.FileNames);
            MyRecords = new ObservableCollection<Record>(RecordCollection.GetList());
            OnPropertyChanged("MyRecords");
        }

        //------------------------- Основная коллекция -----------------------------------------------------------
        public ObservableCollection<Record> MyRecords { get; set; }

        //--------------------------------------------------------------------------------------------------------
        //------------------------- <КОМАНДЫ ДЛЯ Bindings в интерфейсе> ------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        public RelayCommand OpenFileCommand { get; set; }
        public RelayCommand PlayCommand { get; private set; }
        public RelayCommand StopCommand { get; private set; }
        public RelayCommand NextSound { get; private set; }
        public RelayCommand PreviousSound { get; private set; }
        public RelayCommand StartRecordCommand { get; private set; }
        public RelayCommand StopRecordCommand { get; private set; }

        public RelayCommand CloseWindowCommand { get; private set; }
        //--------------------------------------------------------------------------------------------------------
        //------------------------- </КОМАНДЫ ДЛЯ Bindings в интерфейсе> ------------------------------------------
        //--------------------------------------------------------------------------------------------------------



        #region ============= ЗАПУСК и ОСТАНОВКА ЗАПИСИ =============
        //------------------------- <PLAY and STOP> --------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        // ================== ПОЛЯ ===============================================================================
        private AudioPlayBack AudioPlayBack;
        private Record selectrecord;//для выбора записи SelectRecord
        private float volume;
        private DispatcherTimer timer = new DispatcherTimer();

        // ================== СВОЙСТВА ===========================================================================
        public CollectionRecord RecordCollection { get; set; }

        public Record SelectRecord
        {
            get { return selectrecord; }
            set
            {
                selectrecord = value;
                if (SelectRecord.PathToFile != null)
                    AudioPlayBack.Load(SelectRecord.PathToFile);
                Volumne = AudioPlayBack.Volume;
                OnPropertyChanged("SelectRecord");
            }
        }

        public float Volumne
        {
            get { return volume; }
            set
            {
                volume = (value >= 0.0f || value <= 1.0f) ? value : 0.4f;
                AudioPlayBack.Volume = volume;
                OnPropertyChanged("Volumne");
            }
        }

        // ================== МЕТОДЫ =============================================================================
        private void Play()
        {
            if (SelectRecord != null && AudioPlayBack.PlaybackState == NAudio.Wave.PlaybackState.Stopped)
            {
                Dispose();
                AudioPlayBack.Load(SelectRecord.PathToFile);
                AudioPlayBack.Play();
                timer.Start();
            }
        }
        private void Stop()
        {
            AudioPlayBack.Stop();
        }
        public void Dispose()
        {
            AudioPlayBack.Dispose();
        }

        private void PreviosTrack()
        {
            if (SelectRecord != null)
            {
                int Current_index = RecordCollection.IndexOf(SelectRecord);
                if (Current_index < RecordCollection.Count || Current_index > 1)
                {
                    SelectRecord = RecordCollection.FindByIndex(--Current_index);
                    Dispose();

                    Play();
                }
            }
        }
        private void NextTrack()
        {
            if (SelectRecord != null)
            {
                int Current_index = RecordCollection.IndexOf(SelectRecord);
                if (Current_index < RecordCollection.Count - 1)
                {
                    SelectRecord = RecordCollection.Next(++Current_index);
                    Dispose();

                    Play();
                }
            }
        }
        //------------------------- </PLAY and STOP> -------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        #endregion

        #region ============= ПРОКРУТКА СЛАЙДЕРОМ ===================
        //------------------------- <SLIDER POSITION and PROGRESS> -----------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        // ================== ПОЛЯ ===============================================================================
        const double sliderMax = 100.0;
        private double sliderPosition;
        // ================== СВОЙСТВА ===========================================================================
        public double SliderPosition
        {
            get { return sliderPosition; }
            set
            {
                if (sliderPosition != value && SelectRecord != null)
                {
                    sliderPosition = value;//Обновляем значение слайдера
                    if (AudioPlayBack != null)
                    {
                        var pos = (long)(AudioPlayBack.Length * sliderPosition / sliderMax);//обновляем зачение для потока
                        AudioPlayBack.CurrentPosition = pos; //переходим на текущее место в потоке музыки
                    }
                }
            }
        }
        // ================== МЕТОДЫ =============================================================================
        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            if (AudioPlayBack != null)
            {
                SliderPosition = AudioPlayBack.CurrentPosition * sliderMax / AudioPlayBack.Length;//расчитываем текущую позицию слайдера 
                OnPropertyChanged("SliderPosition");
            }
        }

        //--------------------------------------------------------------------------------------------------------
        //------------------------- </SLIDER POSITION and PROGRESS> -----------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        #endregion

        #region ============= ВИЗУАЛИЗАЦИЯ ==========================
        //--------------------------------------------------------------------------------------------------------
        //------------------------- <VISUALISATION> --------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        // ================== ПОЛЯ ===============================================================================
        private IVisualizationPlugin visualizations;
        // ================== СВОЙСТВА ===========================================================================
        public SpectrumAnalyser Visualizations
        {
            get { return (SpectrumAnalyser)Visualization; }
        }
        // ================== МЕТОДЫ =============================================================================
        public object Visualization
        {
            get
            {
                return this.visualizations.Content;
            }
        }
        void audioGraph_FftCalculated(object sender, FftEventArgs e)
        {
            this.visualizations.OnFftCalculated(e.Result);
        }
        void audioGraph_MaximumCalculated(object sender, MaxSampleEventArgs e)
        {
            this.visualizations.OnMaxCalculated(e.MinSample, e.MaxSample);
        }
        //--------------------------------------------------------------------------------------------------------
        //------------------------- </VISUALISATION> ---------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        #endregion

        #region ============= ЗАПИСЬ И ЗАХВАТ МИКРОФОНА =============
        //--------------------------------------------------------------------------------------------------------
        //------------------------- <RECORD MICROPHONE> ----------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------

        // ================== ПОЛЯ ===============================================================================
        int sampleTypeIndex, sampleRate, bitDepth, channelCount, shareModeIndex;
        private WasapiCapture captureDevice;
        private float peak, recordLevel;
        private WaveFileWriter writer;
        MMDevice selectedDevice;
        string currentFileName, message;
        private bool isRecord; //Флаг для записи микрофона и остановки
        private readonly SynchronizationContext synchronizationContext;
        // ================== СВОЙСТВА ===========================================================================
        public IEnumerable<MMDevice> CaptureDevices { get; private set; }

        // Binding на кнопку записи в интерфейсе
        public bool StartStopRecord
        {
            get { return isRecord; }
            set
            {
                isRecord = value;
                if (isRecord)
                    Record();
                else
                    StopRecord();
                OnPropertyChanged("StartStopRecord");
            }
        }

        public float Peak
        {
            get { return peak; }
            set
            {
                if (peak != value)
                {
                    peak = value;
                    OnPropertyChanged("Peak");
                }
            }
        }

        public MMDevice SelectedDevice
        {
            get { return selectedDevice; }
            set
            {
                if (selectedDevice != value)
                {
                    selectedDevice = value;
                    OnPropertyChanged("SelectedDevice");
                    GetDefaultRecordingFormat(value);
                }
            }
        }

        public float RecordLevel
        {
            get { return recordLevel; }
            set
            {
                if (recordLevel != value)
                {
                    recordLevel = value;
                    if (captureDevice != null)
                    {
                        SelectedDevice.AudioEndpointVolume.MasterVolumeLevelScalar = value;
                    }
                    OnPropertyChanged("RecordLevel");
                }
            }
        }

        public int ShareModeIndex
        {
            get { return shareModeIndex; }
            set
            {
                if (shareModeIndex != value)
                {
                    shareModeIndex = value;
                    OnPropertyChanged("ShareModeIndex");
                }
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }
        // =======================================================================================================
        private void GetDefaultRecordingFormat(MMDevice value)
        {
            using (var c = new WasapiCapture(value))
            {
                sampleTypeIndex = c.WaveFormat.Encoding == WaveFormatEncoding.IeeeFloat ? 0 : 1;
                sampleRate = c.WaveFormat.SampleRate;
                bitDepth = c.WaveFormat.BitsPerSample;
                channelCount = c.WaveFormat.Channels;
                Message = "";
            }
        }

        private void Record()
        {
            try
            {
                captureDevice = new WasapiCapture(SelectedDevice);
                captureDevice.ShareMode = ShareModeIndex == 0 ? AudioClientShareMode.Shared : AudioClientShareMode.Exclusive;
                captureDevice.WaveFormat =
                    sampleTypeIndex == 0 ? WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount) :
                    new WaveFormat(sampleRate, bitDepth, channelCount);
                currentFileName = String.Format("RecordDemo {0:dd.MM.yyyy HH-mm-ss}.wav", DateTime.Now);
                RecordLevel = SelectedDevice.AudioEndpointVolume.MasterVolumeLevelScalar;
                captureDevice.StartRecording();
                captureDevice.RecordingStopped += OnRecordingStopped;
                captureDevice.DataAvailable += CaptureOnDataAvailable;
                StartRecordCommand.IsEnabled = false;
                StopRecordCommand.IsEnabled = true;
                Message = "Recording...";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void StopRecord()
        {
            if (captureDevice != null)
            {
                captureDevice.StopRecording();
                Message = "";
            }
        }

        //--------------------------------------- ЗАХВАТ ГОЛОСА И ЗАПИСь В ФАЙЛ ----------------------------------
        private void CaptureOnDataAvailable(object sender, WaveInEventArgs waveInEventArgs)
        {
            if (writer == null)
            {
                writer = new WaveFileWriter(Path.Combine(Directory.GetCurrentDirectory(), currentFileName), captureDevice.WaveFormat);
            }

            writer.Write(waveInEventArgs.Buffer, 0, waveInEventArgs.BytesRecorded);

            UpdatePeakMeter();
        }

        //--------------------------------------- УРОВЕНЬ ГОЛОСА ПРИ ЗАПИСИ --------------------------------------
        void UpdatePeakMeter()
        {
            synchronizationContext.Post(s => Peak = SelectedDevice.AudioMeterInformation
                .MasterPeakValue, null);
        }

        //--------------------------------------- ОСТАНОВКА ЗАПИСИ -----------------------------------------------
        private void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            writer.Dispose();
            writer = null;

            captureDevice.Dispose();
            captureDevice = null;
            StartRecordCommand.IsEnabled = true;
            StopRecordCommand.IsEnabled = false;
        }
        //--------------------------------------------------------------------------------------------------------
        //------------------------- </RECORD MICROPHONE> ---------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
        #endregion


        private void CloseWindow()
        {
            timer.Stop();
            Dispose();
            AudioPlayBack.Dispose();
            visualizations = null;
            SelectedDevice = null;
            AudioPlayBack = null;
            
            Environment.Exit(0);
        }
    }

}
