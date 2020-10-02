using System;
using System.Collections.Generic;
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
using LiveCharts;
using LiveCharts.Wpf;

namespace WorldStats.Pages
{
    /// <summary>
    /// Логика взаимодействия для Country.xaml
    /// </summary>
    public partial class Country : UserControl
    {
        public Country()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Европа",
                    Values = new ChartValues<double> { 184000, 200000, 276000, 400000, 547403, 635855, 681582, 677986, 688452 }
                }
            };

            SeriesCollection.Add(new ColumnSeries
            {
                Title = "Азия",
                Values = new ChartValues<double> { 488000, 656000, 809000, 961000, 1398488, 2163118, 3207807, 3729737, 4352723 }
            });

            SeriesCollection.Add(new ColumnSeries
            {
                Title = "Северная Америка",
                Values = new ChartValues<double> { 2000, 7000, 26000, 82000, 171616, 231937, 283549, 315915, 355361 }
            });

            //also adding values updates and animates the chart automatically
            SeriesCollection[0].Values.Add(48d);

            Labels = new[] { "1750", "1800", "1850", "1900", "1950", "1970", "1990", "2000", "2013" };
            Formatter = value => value / 1000 + " млн.";

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
    }
}
