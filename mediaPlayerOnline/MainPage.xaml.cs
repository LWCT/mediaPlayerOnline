using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace mediaPlayerOnline
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
        public void PlayerOnline()
        {
            MediaPlayer.Source = (new Uri("http://www.neu.edu.cn/indexsource/neusong.mp3"));
            MediaPlayer.Play();
        }
        public async Task PlayerCacheAsync()
        {
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            var buffer =await httpClient.GetBufferAsync(new Uri("http://www.neu.edu.cn/indexsource/neusong.mp3"));
            if (buffer == null) return;

            FileSavePicker fileSavePicker = new FileSavePicker();
            fileSavePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.MusicLibrary;
            fileSavePicker.FileTypeChoices.Add("校歌", new List<string>() { ".mp3" });
            var storageFile = await fileSavePicker.PickSaveFileAsync();
            if (storageFile == null) return;


            CachedFileManager.DeferUpdates(storageFile);
            await FileIO.WriteBufferAsync(storageFile, buffer);
            await CachedFileManager.CompleteUpdatesAsync(storageFile);

            var stream =await storageFile.OpenAsync(FileAccessMode.Read);
            MediaPlayer.SetSource(stream ,"");

        }
        
         

        private async void Download_Tapped(object sender, TappedRoutedEventArgs e)
        {
           await PlayerCacheAsync();
           
        }

        private void PlayOnline_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PlayerOnline();
        }
    }
}
