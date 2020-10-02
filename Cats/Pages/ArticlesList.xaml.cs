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
    /// Логика взаимодействия для Articles.xaml
    /// </summary>
    public partial class ArticlesList : UserControl
    {
        public ArticlesList()
        {
            InitializeComponent();
        }

        private void art1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => ArticleConstructor("art1.txt");

        private void art2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => ArticleConstructor("art2.txt");

        private void art3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => ArticleConstructor("art3.txt");

        public void ArticleConstructor(string pathToArticle)
        {
            List.Visibility = Visibility.Hidden;
            Article.Visibility = Visibility.Visible;

            using (StreamReader reader = new StreamReader(Environment.CurrentDirectory + @"\Articles\" + pathToArticle, System.Text.Encoding.Default))
            {
                string line;

                string heading = reader.ReadLine();
                string pathToImageWeb = reader.ReadLine();

                TextBlock headingText = new TextBlock
                {
                    FontFamily = new FontFamily("Montserrat Bold"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 24,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#a34b45")),
                    Margin = new Thickness(10),
                    Text = heading,
                };

                Image image = new Image();
                image.Source = new BitmapImage(new Uri(pathToImageWeb));

                image.Height = 300;

                StackPanel stackPanel = new StackPanel();

                stackPanel.CanVerticallyScroll = true;

                stackPanel.Children.Add(headingText);
                stackPanel.Children.Add(image);

                while ((line = reader.ReadLine()) != null)
                {
                    TextBlock paragraph = new TextBlock
                    {
                        FontFamily = new FontFamily("Montserrat"),
                        FontSize = 16,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        TextWrapping = TextWrapping.Wrap,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2b2b2b")),
                        Margin = new Thickness(5),
                        Text = line,
                    };

                    stackPanel.Children.Add(paragraph);
                }                

                Article.Children.Add(stackPanel);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Article.Children.RemoveAt(1);
            List.Visibility = Visibility.Visible;
            Article.Visibility = Visibility.Hidden;
        }
    }
}
