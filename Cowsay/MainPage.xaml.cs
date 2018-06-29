using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Cowsay
{
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer IdleTimer { get; set; } = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
        private Queue<string> TextQueue { get; set; } = new Queue<string>();

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += this.OnLoaded;
            this.IdleTimer.Tick += this.OnIdleTimerTicked;
        }

        
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoaded;
            this.Cowsay(_message.PlaceholderText);
        }

        

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void Cowsay(string message)
        {
            this.IdleTimer.Stop();
            this.TextQueue.Enqueue(message);
            this.IdleTimer.Start();
            
        }

        private async Task DoCowsayAsync(string message)
        {
            var cs = CowsayService.Default;
            var output = await cs.SayAsync(message);
            _output.Text = output;
        }

        private void OnMessageTextChanged(object sender, TextChangedEventArgs e)
        {
            this.Cowsay(_message.Text.Trim());
        }

        private async void OnIdleTimerTicked(object sender, object e)
        {
            this.IdleTimer.Stop();
            var q = this.TextQueue;

            var message = string.Empty;
            while(q.TryDequeue(out string tmp))
            {
                message = tmp;
            }

            await this.DoCowsayAsync(message);

            lock(q)
            {
                if(q.Count>0)
                {
                    this.IdleTimer.Start();
                }
            }
        }

    }
}
