using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace MediaPlayer
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_GetFile_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            openPicker.FileTypeFilter.Add(".mp3");
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".rmvb");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                Player.SetSource(stream, file.ContentType);
            }
            else
            {
                return;
            }


        }
        private async void Play_Click(object sender, RoutedEventArgs e)
        {
            Uri pathUri = new Uri(onlineSrc.Text);

            Player.Source = pathUri;

            Player.Play();
        }
        private async void Download_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var httpClient = new HttpClient();
                var buffer = await httpClient.GetBufferAsync(new Uri(onlineSrc.Text));
                var file = await KnownFolders.MusicLibrary.CreateFileAsync("neusong.mp3", CreationCollisionOption.ReplaceExisting);
                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await stream.WriteAsync(buffer);
                    await stream.FlushAsync();
                }
                Uri pathUri = new Uri(onlineSrc.Text);
                Player.Source = pathUri;
                Player.Play();
                return ;

            }
            catch { }
            return ;

        }
    }
}
