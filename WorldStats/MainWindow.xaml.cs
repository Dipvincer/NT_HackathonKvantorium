using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace WorldStats
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (RegionPopulation.Visibility == Visibility.Hidden)
            {
                Welcome.Visibility = Visibility.Hidden;
                Koronavirus.Visibility = Visibility.Hidden;
                RegionPopulation.Visibility = Visibility.Visible;

                Headline.Content = "Коронавирус";
            }
            else if (RegionPopulation.Visibility == Visibility.Visible)
            {
                RegionPopulation.Visibility = Visibility.Hidden;
                Koronavirus.Visibility = Visibility.Visible;

                Headline.Content = "Мировое население";
            }
        }
    }
}
