using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnglishReminder2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Diki : ContentPage
	{
		public Diki ()
		{
			InitializeComponent ();
			DisplayAlert("Uwaga", "Możesz sprawdzić tłumaczenie słówka w polecanym słowniku online", "OK");
		}
	}
}