using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class FirstLaunch : Page
    {
        public FirstLaunch()
        {
            this.InitializeComponent();
            this.Loaded += FirstLaunch_Loaded;
        }

        private async void FirstLaunch_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Logic.SettingsManager.IsFirstLaunch().Value)
            {
                Logic.SettingsManager.NotFirstLaunch();
                Frame.Navigate(typeof(Enter));
            }

            try
            {
                if (await Windows.Security.Credentials.UI.UserConsentVerifier.CheckAvailabilityAsync() != 
                    Windows.Security.Credentials.UI.UserConsentVerifierAvailability.Available)
                {
                    this.UseWindowsHello.IsEnabled = false;
                }
            }
            catch (Exception)
            {
                this.UseWindowsHello.IsEnabled = false;
            }
        }

        private async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.MasterKeyBox.Text.Length < 6)
            {
                await new MessageDialog("Не все поля заполнены верно", "Поле мастер-ключа не может содержать менее 6 символов").ShowAsync();
            }
            else if (this.EnterPassBox.Password.Length < 6)
            {
                await new MessageDialog("Не все поля заполнены верно", "Поле пароля не может содержать менее 6 символов").ShowAsync();
            }
            else if (this.EnterPassBox.Password != this.EnterPassRepeatBox.Password)
            {
                await new MessageDialog("Не все поля заполнены верно", "Введенные пароли не совпадают").ShowAsync();
            }
            else if (this.MasterKeyBox.Text != this.MasterKeyBox.Text)
            {
                await new MessageDialog("Не все поля заполнены верно", "Введенные мастер-ключи не совпадают").ShowAsync();
            }
            else
            {
                Logic.SettingsManager.SavePassword(this.EnterPassBox.Password);
                Logic.SettingsManager.SaveMasterKey(this.MasterKeyBox.Text);
                Logic.SettingsManager.SaveWinHelloState(this.UseWindowsHello.IsChecked);

                Logic.SettingsManager.NotFirstLaunch();
                this.Frame.Navigate(typeof(Enter));
            }
        }

        private async void UseWindowsHello_Checked(object sender, RoutedEventArgs e)
        {
            UseWindowsHello.IsChecked = await Logic.SettingsManager.WinHelloAuthAsync(true);
        }
    }
}
