using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Clock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _timer = new()
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        private readonly HttpClient httpClient = new HttpClient();

        

        private readonly String[] URLs = {
            "https://api.open-meteo.com/v1/forecast?latitude=54.3523&longitude=18.6491&current=temperature_2m,relative_humidity_2m",
            "https://api.open-meteo.com/v1/forecast?latitude=52.2298&longitude=21.0118&current=temperature_2m,relative_humidity_2m"
        };

        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            UpdateTime();

            
        }

        private void UpdateTime()
        {
            DateTime now = DateTime.Now;

            HourLabel.Text = now.ToString("HH:mm:ss");
            

            DateLabel.Text = now.ToString("dddd d MMMM yyyy");

            
        }

        public MainWindow()
        {
            InitializeComponent();

            _timer.Tick += DispatcherTimer_Tick;
            UpdateTime();
            _timer.Start();
        }
        int licznikPobran = 0;
        private async Task LoadWeatherAsync(int locationIndex)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            string json = await httpClient.GetStringAsync(URLs[locationIndex]);
            
            ResultLabel.Text = json + '\n' + '\n' + "Pobrano o: " + DateTime.Now.ToString("HH:mm:ss");
            stopwatch.Stop();
            licznikPobran += 1;
            ResultLabel.Text = ResultLabel.Text + '\n' + "Czas pobierania: " + stopwatch.ElapsedMilliseconds + " ms" +'\n'+"Pobrano "+licznikPobran+" razy.";
            LocationList.IsHitTestVisible = true;
            LocationList.Focusable = true;

        }

        private void LocationList_Selected(object sender, RoutedEventArgs e)
        {
            ResultLabel.Text = "Pobieram...";
            LocationList.IsHitTestVisible = false;
            LocationList.Focusable = false;
            int locationIndex = LocationList.Items.IndexOf(LocationList.SelectedItem);
            LoadWeatherAsync(locationIndex);
        }
    }
}