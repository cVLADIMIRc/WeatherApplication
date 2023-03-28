using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Model
{
    public class WeatherToday : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public coord _coord;
        public main _main;
        public string _name;
        public DayOfWeek _day;
        public coord coord
        {
            get { return _coord; }
            set
            {
                _coord = value; OnPropertyChanged();
            }
        }
        public main main
        {
            get { return _main; }
            set { _main = value; OnPropertyChanged(); }
        }
        public string name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }
        public DayOfWeek day
        {
            get { return _day; }
            set { _day = value; OnPropertyChanged(); }
        }
    }



    public class coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }
    public class main
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public double pressure { get; set; }
    }
}
