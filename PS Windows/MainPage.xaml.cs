using PS_Windows.pages;
using PS_Windows.services;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PS_Windows
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private PsApi service;

        public MainPage() {
            this.InitializeComponent();

            this.service = PsApi.getInstance();
        }

        private String login;
        private String password;

        private async void handleClickLogin(object sender, RoutedEventArgs e) {
            if (await this.service.authenticateAsync(this.login, this.password)) {
                this.Frame.Navigate(typeof(Home));
            }
            else {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "Login failed",
                    Content = "Invalid Credentials",
                    PrimaryButtonText = "Ok"
                };
                await dialog.ShowAsync();
            }
        }
    }
}