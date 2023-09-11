using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnglishReminder2.Views.PopUps
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InfoPopup : Popup
	{
		public InfoPopup ()
		{
			InitializeComponent ();
		}

        private void Cancel_Clicked(object sender, EventArgs e)
        {
			Dismiss(null);
        }
    }
}