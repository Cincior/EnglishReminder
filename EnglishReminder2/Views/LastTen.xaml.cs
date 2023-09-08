using EnglishReminder2.Models;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static EnglishReminder2.Models.DbInfo;

namespace EnglishReminder2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LastTen : ContentPage
	{
        SqlConnection sqlConnection = new SqlConnection(sqlconn);
        ViewCell lastCell;
        ObservableCollection<Word> WordList = new ObservableCollection<Word>();
        public LastTen()
		{
			InitializeComponent();
            int i = 1;
            sqlConnection.Open();
            SqlCommand GetWords = new SqlCommand("SELECT TOP 10 Slowko, Tlumaczenie FROM dbo.SlowkaWpisane ORDER BY Id DESC", sqlConnection);
            SqlDataReader readerWords = GetWords.ExecuteReader();
            while (readerWords.Read())
            {
                WordList.Add(new Word { Id = i, Slowko = Convert.ToString(readerWords["Slowko"]), Tlumaczenie = Convert.ToString(readerWords["Tlumaczenie"]) });
                i++;
            }
            readerWords.Close();
            sqlConnection.Close();

            LastTenList.ItemsSource = WordList;
            //Po kliknieciu na ktorys z listy jest mozliwosc ponownego zaplanowania przypomnienia.
        }

        private async void ViewCell_Tapped(object sender, EventArgs e)
        {
            var viewCell = (ViewCell)sender;
            viewCell.View.BackgroundColor = Color.FromHex("#ffdab0");
            var confirm = await DisplayAlert("Uwaga", "Jesteś pewien, że chcesz jeszcze raz przypomnieć to słówko?", "Tak", "Nie");
            if (confirm)
            {
                var notification = new NotificationRequest
                {
                    BadgeNumber = 1,
                    Description = WordList.First(x => x.Id == (Convert.ToInt32(viewCell.ClassId))).Slowko + " - " + WordList.FirstOrDefault(x => x.Id == (Convert.ToInt32(viewCell.ClassId))).Tlumaczenie,
                    Title = "Twoje słówko:",
                    ReturningData = "bk",
                    NotificationId = 1,
                    NotifyTime = DateTime.Now.AddSeconds(10),
                    Android =
                    {
                        IconName = "icon.png",
                    }
                };
                NotificationCenter.Current.Show(notification);
                await DisplayAlert("poszlo", "ok", "k");
            }
            viewCell.View.BackgroundColor = Color.White;
            lastCell = viewCell;
        }


        private async void LastTenList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //Header.Text = e.Item.ToString();

        }
    }
}