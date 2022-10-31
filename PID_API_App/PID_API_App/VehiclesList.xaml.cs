using PID_API_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PID_API_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VehiclesList : ContentPage
    {
        ResourceData resourceData = new ResourceData();
        List<AverageLine> sortedLines = new List<AverageLine>();
        List<AverageLine> searchList = new List<AverageLine>();

        public VehiclesList(string type, Color vehicleColor)
        {
            InitializeComponent();
            lb_lastUpdate.Text = "Poslední aktualizace: " + resourceData.GetUpdateTime();

            switch (type)
            {
                case "Autobusy": lb_typeOfVehicle.Text = type;
                    sortedLines = resourceData.SortBuses();
                    break;
                case "Tramvaje":
                    lb_typeOfVehicle.Text = type;
                    sortedLines = resourceData.SortTrams();
                    break;
                case "Vlaky":
                    lb_typeOfVehicle.Text = type;
                    sortedLines = resourceData.SortTrains();
                    break;
                case "Loď":
                    lb_typeOfVehicle.Text = type;
                    sortedLines = resourceData.SortBoats();
                    break;
                default:
                    break;
            }

            lb_typeOfVehicle.TextColor = vehicleColor;
            lv_allSortedLines.ItemsSource = sortedLines;
        }

        private void entry_searchbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchList.Clear();
            string search = entry_searchbar.Text;

            if (search == string.Empty)
            {
                lv_allSortedLines.ItemsSource = sortedLines;
            }
            else
            {
                foreach (AverageLine item in sortedLines)
                {
                    if (item.LineNumber.ToString().Contains(search.ToUpper()))
                    {
                        searchList.Add(item);
                    }
                }

                foreach (AverageLine item in sortedLines)
                {
                    if (item.AgencyName.ToString().Contains(search.ToUpper()))
                    {
                        searchList.Add(item);
                    }
                }

                lv_allSortedLines.ItemsSource = sortedLines; //Řešení pro Refresh ListView
                lv_allSortedLines.ItemsSource = searchList;
            }        
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (lb_typeOfVehicle.Text == "Autobusy")
            {
                lb_typeOfVehicle.Text = "Reg. Autobusy";
                sortedLines = resourceData.SortRegBuses();
                lv_allSortedLines.ItemsSource = sortedLines;
            }
            else if (lb_typeOfVehicle.Text == "Reg. Autobusy")
            {
                lb_typeOfVehicle.Text = "Autobusy";
                sortedLines = resourceData.SortBuses();
                lv_allSortedLines.ItemsSource = sortedLines;
            }
        }
    }
}