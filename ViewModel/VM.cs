using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Model;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace ViewModel
{
    public class VM : INotifyPropertyChanged
    {
        private readonly string apiKey = File.ReadAllText("apiKey.txt");
        private FutureWeather futureWeather;
        private WeatherToday newWeather;
        public WeatherToday NewWeather
        {
            get { return newWeather; }
            set { newWeather = value; OnPropertyChanged(); }
        }
        public VM() 
        {
            newWeather = new WeatherToday();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private ICommand getJsonDataLocationCommand;
        public ICommand GetJsonDataLocationCommand => getJsonDataLocationCommand ?? (getJsonDataLocationCommand = new RelayCommand(GetJsonDataLocation));
        public async void GetJsonDataLocation()
        {
            using (FileStream file = new FileStream("weather_on_locate.json", FileMode.Create))
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage httpResponse = await httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat=56.0090968&lon=92.8725147&units=metric&appid={apiKey}");
                string value = await httpResponse.Content.ReadAsStringAsync();
                var buffer = Encoding.UTF8.GetBytes(value);
                file.Write(buffer, 0, buffer.Length);
            }
            NewWeather = JsonConvert.DeserializeObject<WeatherToday>(File.ReadAllText("weather_on_locate.json"));
            NewWeather.day = DateTime.Now.DayOfWeek;
        }
    }
}
