using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PID_API_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : ContentPage
    {
        public string type = null; Color vehicleColor;

        public MainMenu()
        {
            InitializeComponent();
        }

        private void btn_buses_Clicked(object sender, EventArgs e)
        {
            type = "Autobusy"; vehicleColor = Color.FromHex("#047aa9");
            Navigation.PushAsync(new VehiclesList(type, vehicleColor));
        }

        private void btn_trams_Clicked(object sender, EventArgs e)
        {
            type = "Tramvaje"; vehicleColor = Color.FromHex("#7a0404");
            Navigation.PushAsync(new VehiclesList(type, vehicleColor));
        }

        private void btn_trains_Clicked(object sender, EventArgs e)
        {
            type = "Vlaky"; vehicleColor = Color.FromHex("#313867");
            Navigation.PushAsync(new VehiclesList(type, vehicleColor));
        }

        private void btn_others_Clicked(object sender, EventArgs e)
        {
            type = "Loď"; vehicleColor = Color.FromHex("#00b3cb");
            Navigation.PushAsync(new VehiclesList(type, vehicleColor));
        }

        private void btn_questionmark_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Nápověda", @"Vítejte v této aplikaci. Nyní se nacházíte v hlavním menu, kde si můžete vybrat jednotlivé druhy dopravy. Po kliknutí na příslušné tlačítko se zobrazí seznam všech linek s informacemi - číslo linky, dopravce, aktuální počet vozidel na lince (PV) a průměrné zpoždění všech těchto vozidel. Pro vyhledání konktrétní linky můžete využít řádek vyhledávání. 

Pro zobrazení Regionálních autobusů (linky číslo 300+) vyberte v hlavním menu možnost Autobusy a poté klikněte na nadpis (AUTOBUSY) v horní části stránky - seznam se aktualizuje a nadpis se změní. Stejným způsobem se vrátíte zpět na městské autobusové linky.", "Ok");
        }
    }
}