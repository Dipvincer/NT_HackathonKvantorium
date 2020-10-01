using System;
using System.Collections.Generic;
using System.IO;
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

namespace Cats.Pages
{
    /// <summary>
    /// Логика взаимодействия для Galery.xaml
    /// </summary>
    public partial class Galery : UserControl
    {
        public Galery()
        {
            InitializeComponent();

            if (File.Exists("galeryInfo.txt"))
            {
                using (StreamReader reader = new StreamReader(Environment.CurrentDirectory + "\\" +"galeryInfo.txt"))
                {
                    string pathToImage;
                    
                    while ((pathToImage = reader.ReadLine()) != null)
                    {
                        if (File.Exists(Environment.CurrentDirectory + "\\" + pathToImage))
                        {
                            Grid grid = new Grid();
                            grid.Margin = new Thickness(15);

                            Image image = new Image();
                            image.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\" + pathToImage));
                            image.Height = 250;

                            grid.Children.Add(image);
                            GaleryPage.Children.Add(grid);
                        }
                    }
                }
            }
        }

        [STAThread]
        private void AddingPhotos_Click(object sender, RoutedEventArgs e)
        {
            string openPath = string.Empty;
            Microsoft.Win32.OpenFileDialog OPF = new Microsoft.Win32.OpenFileDialog();
            OPF.Filter = "Image (*.BMP;*.GIF;*.ICO;*.JPG;*.PNG;*.SVG;)|*.BMP;*.GIF;*.ICO;*.JPG;*.PNG;*.SVG;";

            if (OPF.ShowDialog() == true)
            {
                openPath = OPF.FileName;
                File.Copy(OPF.FileName, @"Galery\" + System.IO.Path.GetFileName(openPath), true);
            }

            if (openPath != string.Empty)
            {
                Grid grid = new Grid();

                grid.Margin = new Thickness(15);

                Image image = new Image();
                image.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Galery\" + System.IO.Path.GetFileName(openPath)));

                image.Height = 250;

                grid.Children.Add(image);

                using (StreamWriter writer = new StreamWriter(Environment.CurrentDirectory + "\\" + "galeryInfo.txt", true))
                {
                    GaleryPage.Children.Add(grid);
                    writer.WriteLine(@"Galery\" + System.IO.Path.GetFileName(openPath));
                }
            }
        }
    }
}
