using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Password.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Secret : Page
    {
        public Secret()
        {
            this.InitializeComponent();
            this.Loaded += Secret_Loaded;
        }

        private void Secret_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.SecretBox.Text = Logic.SettingsManager.GetSecret();
            }
            catch (Exception)
            {

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Logic.SettingsManager.SaveSecret(this.SecretBox.Text);
            Frame.Navigate(typeof(Master));
        }
    }
}
