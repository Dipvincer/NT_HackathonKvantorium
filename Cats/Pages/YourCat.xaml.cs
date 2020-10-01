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
    /// Логика взаимодействия для YourCat.xaml
    /// </summary>
    public partial class YourCat : UserControl
    {
        public YourCat()
        {
            InitializeComponent();

            if (File.Exists(Environment.CurrentDirectory + @"/info.cts"))
            {
                string[] fromInfo = new string[5];

                using (StreamReader reader = new StreamReader(Environment.CurrentDirectory + @"/info.cts", System.Text.Encoding.Default))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        fromInfo[i] = reader.ReadLine();
                    }                    
                }

                string pathToImage = fromInfo[0];
                string catName = fromInfo[1];
                string catBreed = fromInfo[2];
                string catAge = fromInfo[3];
                string catDescription = fromInfo[4];

                CatsPhoto.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\" + pathToImage));
                CatsName.Text = catName;
                CatsBreed.Text = catBreed;
                CatsAge.Text = catAge;
                CatsDescriptions.Text = catDescription;
            }
        }

        private void InputField_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (InputField.Text == "Написать сообщение")
            {
                InputField.Opacity = 1;
                InputField.Clear();
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrWhiteSpace(InputField.Text)) && InputField.Opacity != 0.5)
            {
                Grid grid = new Grid();

                grid.Margin = new Thickness(0, 10, 0, 0);

                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100) });
                grid.ColumnDefinitions.Add(new ColumnDefinition());

                ImageBrush imgBrush = new ImageBrush();
                imgBrush.ImageSource = new BitmapImage(new Uri(@"https://cs9.pikabu.ru/post_img/2017/05/12/8/1494592816133830021.jpg"));

                Ellipse avatar = new Ellipse
                {
                    Width = 70,
                    Height = 70,
                    Fill = imgBrush,
                };

                Grid.SetColumn(avatar, 0);
                Grid.SetRowSpan(avatar, 2);

                TextBlock name = new TextBlock
                {
                    FontFamily = new FontFamily("Montserrat"),
                    FontSize = 20,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#a34b45")),
                    Margin = new Thickness(10, 5, 0, 0),
                    Text = "Любитель Котов",
                };

                Grid.SetColumn(name, 1);

                TextBlock comment = new TextBlock
                {
                    FontFamily = new FontFamily("Montserrat"),
                    FontSize = 14,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2b2b2b")),
                    Margin = new Thickness(10, 5, 0, 0),
                    Text = InputField.Text,
                };

                Grid.SetColumn(comment, 1);
                Grid.SetRow(comment, 1);

                grid.Children.Add(avatar);
                grid.Children.Add(name);
                grid.Children.Add(comment);

                CommentsField.Children.Add(grid);

                InputField.Text = "Написать сообщение";
                InputField.Opacity = 0.5;
            }
            else if (string.IsNullOrWhiteSpace(InputField.Text))
            {
                InputField.Text = "Написать сообщение";
                InputField.Opacity = 0.5;
            }
        }

        private void InputField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Send_Click(sender, e);
            }
        }
    }
}

