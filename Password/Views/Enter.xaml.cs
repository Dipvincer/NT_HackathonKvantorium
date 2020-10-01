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
    public sealed partial class Enter : Page
    {
        public Enter()
        {
            this.InitializeComponent();
            this.Loaded += Enter_Loaded;
        }

        private async void Enter_Loaded(object sender, RoutedEventArgs e)
        {
            string hello;
            var hour = DateTime.Now.Hour;
            if (hour < 6)
            {
                hello = "Доброй ночи, ";
            }
            else if (hour < 11)
            {
                hello = "Доброе утро, ";
            }
            else if (hour < 17)
            {
                hello = "Добрый день, ";
            }
            else if (hour < 23)
            {
                hello = "Добрый вечер, ";
            }
            else
            {
                hello = "Доброй ночи, ";
            }
            this.Hello.Text = hello + "подтвердите свою личность";

            if (Logic.SettingsManager.GetWinHelloState().Value)
            {
                if (await Logic.SettingsManager.WinHelloAuthAsync())
                {
                    Frame.Navigate(typeof(Master));
                }
            }
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            if (Logic.SettingsManager.CheckPassword(this.EnterPassBox.Password))
            {
                Frame.Navigate(typeof(Master));
            }
        }

        private async void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog resetDialog = new ContentDialog
            {
                Title = "Удалить все данные приожение?",
                Content = "Если вы продолжите, все пароли будут БЕЗВОЗВРАТНО удалены. Вы уверены?",
                PrimaryButtonText = "Да",
                CloseButtonText = "Отмена"
            };

            ContentDialogResult result = await resetDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                Logic.SettingsManager.ResetAll();
                Frame.Navigate(typeof(FirstLaunch));
            }
        }
    }
}
