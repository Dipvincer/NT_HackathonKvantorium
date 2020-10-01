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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cats
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

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MenuButton.Visibility = Visibility.Hidden;
            CloseMenuButton.Visibility = Visibility.Visible;
            DarkMask.Visibility = Visibility.Visible;

            StoryboardStart("SlideMenuOpen");
        }

        private async void CloseMenuButton_Click(object sender, RoutedEventArgs e)
        {        
            MenuButton.Visibility = Visibility.Visible;

            StoryboardStart("SlideMenuClose");

            await Task.Delay(500);
            DarkMask.Visibility = Visibility.Hidden;
        }

        private async void YourCatButton_Click(object sender, RoutedEventArgs e)
        {
            MenuButton.Visibility = Visibility.Visible;            
            YourCatPage.Visibility = Visibility.Visible;

            WelcomePage.Visibility = Visibility.Hidden;
            GaleryPage.Visibility = Visibility.Hidden;

            NowPage.Text = "Личная страничка";

            StoryboardStart("SlideMenuClose");

            await Task.Delay(500);
            DarkMask.Visibility = Visibility.Hidden;
        }

        private async void GaleryButton_Click(object sender, RoutedEventArgs e)
        {
            MenuButton.Visibility = Visibility.Visible;            
            GaleryPage.Visibility = Visibility.Visible;

            WelcomePage.Visibility = Visibility.Hidden;
            YourCatPage.Visibility = Visibility.Hidden;

            NowPage.Text = "Галерея";

            StoryboardStart("SlideMenuClose");

            await Task.Delay(500);
            DarkMask.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Реализует запуск указанной анимации
        /// </summary>
        /// <param name="name">Название анимации</param>
        private void StoryboardStart(string name)
        {
            Storyboard storyboardStart = this.Resources[name] as Storyboard;
            storyboardStart.Begin();
        }
    }
}
