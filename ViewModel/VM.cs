using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Model;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Security.Principal;

namespace ViewModel
{
    public class VM : INotifyPropertyChanged
    {
        private readonly string apiKey = File.ReadAllText("apiKey.txt");
        private WeatherToday newWeather;
        private FutureWeather futureWeather;
        public WeatherToday NewWeather
        {
            get { return newWeather; }
            set { newWeather = value; OnPropertyChanged(); }
        }
        public FutureWeather FutureWeather
        {
            get { return futureWeather;}
            set { futureWeather = value; OnPropertyChanged();}
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
            GetJsonDataFutureWeather();
        }
        public async void GetJsonDataFutureWeather()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"http://api.openweathermap.org/data/2.5/forecast?lat=56.0090968&lon=92.8725147&units=metric&appid={apiKey}");
            string value = await httpResponseMessage.Content.ReadAsStringAsync();
            FutureWeather = JsonConvert.DeserializeObject<FutureWeather>(value);
        }
    }
}
