using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using PID_API_App.Models;
using Xamarin.Essentials;

namespace PID_API_App
{
    public partial class Splashscreen : ContentPage
    {
        ResourceData resourceData = new ResourceData();

        public Splashscreen()
        {       
            InitializeComponent();

            CheckInternetConnection();
        }

        private void CheckInternetConnection()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                bool success = GetAPIData();

                if (success == false)
                {
                    Navigation.PushAsync(new MainMenu());
                }
                else
                {
                    DisplayAlert("Chyba", "Nastala chyba při stahování dat ze serveru", "Ok");
                }
            }
            else
            {
                DisplayAlert("Chyba sítě", "Zkontrolujte své připojení k internetu", "Ok");
            }
        }

        private bool GetAPIData()
        {
            bool b = resourceData.MainMethod();

            if (b == false)
            {
                resourceData.DeseliazeJSON();
                return false;
            }
            else
            {
                return true;
            }

        }

        private void btn_connection_Clicked(object sender, EventArgs e)
        {
            CheckInternetConnection();
        }
    }
}
