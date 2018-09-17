using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicBox
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainApp : Page
    {

        private List<Entity.RecentFile> ListFile = new System.Collections.Generic.List<Entity.RecentFile>();

        private IReadOnlyList<StorageFile> files;

        private int CurrentIndex = 0;

        private int TotalFile = 0;

        private bool isPlay = true;

        public MainApp()
        {
            this.InitializeComponent();

            volumeSlider.Value = 100;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

        }

        private void timer_Tick(object sender, object e)
        {
            if (_mediaPlayerElement.Source != null && _mediaPlayerElement.NaturalDuration.HasTimeSpan)
            {
                Progress.Minimum = 0;
                Progress.Maximum = _mediaPlayerElement.NaturalDuration.TimeSpan.TotalSeconds;
                Progress.Value = _mediaPlayerElement.Position.TotalSeconds;
                this.txt_Welcome.Text = _mediaPlayerElement.Position.TotalSeconds.ToString();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (string.IsNullOrEmpty(e.Parameter.ToString()))
            {
                this.txt_Welcome.Text = "WELCOME GUEST!!";
            }
            else
            {
                this.txt_Welcome.Text = "WELCOME " + e.Parameter.ToString();
            }
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
            openPicker.FileTypeFilter.Add(".mp3");
            openPicker.FileTypeFilter.Add(".mp4");
            files = await openPicker.PickMultipleFilesAsync();
            ListFile = new System.Collections.Generic.List<Entity.RecentFile>();
            int i = 0;
            foreach (var file in files)
            {
                var musicStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                MusicProperties musicProperties = await file.Properties.GetMusicPropertiesAsync();
                Entity.RecentFile recentFile = new Entity.RecentFile(i, musicProperties.Title, file.FolderRelativeId);
                ListFile.Add(recentFile);
                Debug.WriteLine(musicProperties.ToString());
                i++;
            }

            TotalFile = i;

            debugText.Text = "Total: " + TotalFile.ToString();
            btnPlay.Icon = new SymbolIcon(Symbol.Pause);

            MenuList.ItemsSource = ListFile;
            PlaySong(0);



        }

        private async void PlaySong(int i)
        {
            var firstMusic = await files[i].OpenAsync(Windows.Storage.FileAccessMode.Read);
            nowPlaying.Text = ListFile[i].FileName;
            _mediaPlayerElement.SetSource(firstMusic, files[i].ContentType);
            _mediaPlayerElement.Play();
        }

        private void StackPanel_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Grid thisSender = (Grid)sender;
            int i = (int)(thisSender).Tag;
            
            ListViewItem lvi = sender as ListViewItem;
            CurrentIndex = i;

            string str = "Current: " + CurrentIndex.ToString() + " - ";

            ItemCollection listItem = MenuList.Items;
            
            if (listItem != null)
            {
                foreach (var item in listItem)
                {
                    ListViewItem thisItem = item as ListViewItem;
                    if(thisItem != null)
                    {
                        str += thisItem.Name + ".";
                    }
                    else
                    {
                        str += item.GetType().ToString();
                    }

                }
            }
            else
            {
                str += "NULL ";
            }
            

            debugText.Text = str + ">> " + sender.GetType().ToString();

            

            this.txt_Welcome.Text = i.ToString();
            PlaySong(i);
        }

        private void Back_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

            if (CurrentIndex > 0)
            {
                CurrentIndex--;
                
            }
            else
            {
                CurrentIndex = TotalFile - 1;
            }
            this.txt_Welcome.Text = CurrentIndex.ToString();
            PlaySong(CurrentIndex);
            debugText.Text = "Current: " + CurrentIndex.ToString() + " - ";

        }

        private void Play_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

            AppBarButton btnSender = (AppBarButton) sender;

            if (isPlay)
            {
                _mediaPlayerElement.Pause();

                btnSender.Icon = new SymbolIcon(Symbol.Play);

                isPlay = false;
            }
            else
            {
                _mediaPlayerElement.Play();

                btnSender.Icon = new SymbolIcon(Symbol.Pause);

                isPlay = true;

            }
            


        }

        private void Next_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (CurrentIndex < TotalFile-1)
            {
                CurrentIndex++;
            }
            else
            {
                CurrentIndex = 0;
            }

            this.txt_Welcome.Text = CurrentIndex.ToString();
            PlaySong(CurrentIndex);

            debugText.Text = "Current: " + CurrentIndex.ToString();

        }

        private void Slider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            Slider vol = sender as Slider;

            if(vol != null)
            {
                _mediaPlayerElement.Volume = vol.Value/100;
                
                this.txt_Welcome.Text = vol.Value.ToString();
            }

            
        }

        private void _mediaPlayerElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            CurrentIndex++;
            PlaySong(CurrentIndex);
        }
    }
}
