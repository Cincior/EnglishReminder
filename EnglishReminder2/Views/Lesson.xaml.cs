using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnglishReminder2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Lesson : ContentPage
    {
       
        public Lesson()
        {
            InitializeComponent();
        }
        private async void main_Clicked(object sender, EventArgs e)
        {
            await main.ScaleTo(1.25, 100);
            await main.ScaleTo(1, 100);
            await Shell.Current.GoToAsync("//Main");
        }
    }
}