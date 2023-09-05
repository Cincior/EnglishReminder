﻿using Plugin.LocalNotification;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.AppleSignInAuthenticator;

namespace EnglishReminder2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : ContentPage
    {
        static string srvrdbname = "EnglishReminder";
        static string srvrname = "192.168.2.106";
        static string srvrusername = "EnglishReminderAdmin";
        static string srvrpassword = "12345";
        static string sqlconn = $"Data Source={srvrname};Initial Catalog ={srvrdbname}; User ID ={srvrusername}; Password ={srvrpassword}";
        SqlConnection sqlConnection = new SqlConnection(sqlconn);
        public Main()
        {
            //Hosting at somee.com
            InitializeComponent();

            UpdateAmountOfWords();
            UpdateLastWord();

            NotificationCenter.Current.NotificationReceived += Current_NotificationReceived;
            NotificationCenter.Current.NotificationTapped += Current_NotificationTapped;
        }

        private void UpdateLastWord()
        {
            sqlConnection.Open();
            SqlCommand GetWord = new SqlCommand("SELECT TOP 1 Slowko FROM dbo.SlowkaWpisane ORDER BY Id DESC", sqlConnection);
            SqlDataReader readerWord = GetWord.ExecuteReader();
            readerWord.Read();
            LastWord.Text = Convert.ToString(readerWord["Slowko"]);
            readerWord.Close();
            sqlConnection.Close();
        }
        private void UpdateAmountOfWords()
        {
            sqlConnection.Open();
            SqlCommand cmd2 = new SqlCommand("SELECT IleSlowek FROM dbo.LicznikSlowek WHERE Id = 1", sqlConnection);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            reader2.Read();
            HowMany.Text = Convert.ToString(reader2["IleSlowek"]);
            reader2.Close();
            sqlConnection.Close();
        }

        private void Current_NotificationTapped(NotificationTappedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Shell.Current.GoToAsync("/Main");
                DisplayAlert("xd", "tapped", "xd");
            });
        }

        private void Current_NotificationReceived(NotificationReceivedEventArgs e)
        {
            int StanSlowek;
            Device.BeginInvokeOnMainThread(() =>
            {
                //Amount
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("SELECT IleSlowek FROM dbo.LicznikSlowek WHERE Id = 1", sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                StanSlowek = Convert.ToInt32(reader["IleSlowek"]);
                StanSlowek++;
                reader.Close();
                SqlCommand cmdUpdate = new SqlCommand($"UPDATE dbo.LicznikSlowek SET IleSlowek = {StanSlowek} WHERE Id = 1", sqlConnection);
                cmdUpdate.ExecuteNonQuery();
                sqlConnection.Close();

                UpdateAmountOfWords();
                
                //Word
                string Slowko, Tlumaczenie;
                Slowko = (e.Description.Split('-')[0]).Trim();
                Tlumaczenie = (e.Description.Split('-')[1]).Trim();
                sqlConnection.Open();
                SqlCommand cmdInsert = new SqlCommand($"INSERT INTO SlowkaWpisane VALUES('{Slowko}', '{Tlumaczenie}')", sqlConnection);
                cmdInsert.ExecuteNonQuery();

                UpdateLastWord();
                DisplayAlert("xd", "received", "xd");
            });
        }

        private async void main_Clicked(object sender, EventArgs e)
        {
            await main.ScaleTo(1.25, 100);
            await main.ScaleTo(1, 100);
            //Shell.Current.GoToAsync("//Main");
        }

        private void SendNotification_Clicked(object sender, EventArgs e)
        {
            bool IsWord = false, IsTranslate = false, IsTime = false;
            int Time = 30;

            if (EntryWord.Text != String.Empty)
            {
                FrameWord.BorderColor = Color.White;
                IsWord = true;
            }
            else
            {
                FrameWord.BorderColor = Color.Red;
                IsWord = false;
            }

            if (EntryTranslate.Text != String.Empty)
            {
                FrameTranslate.BorderColor = Color.White;
                IsTranslate = true;
            }
            else
            {
                FrameTranslate.BorderColor = Color.Red;
                IsTranslate = false;
            }

            if (EntryTime.Text != String.Empty && int.TryParse(EntryTime.Text, out Time))
            {
                FrameTime.BorderColor = Color.White;
                IsTime = true;
            }
            else
            {
                FrameTime.BorderColor = Color.Red;
                IsTime = false;
            }

            if (IsWord && IsTranslate && IsTime)
            {
                var notification = new NotificationRequest
                {
                    BadgeNumber = 1,
                    Description = EntryWord.Text + " - " + EntryTranslate.Text,
                    Title = "Twoje słówko:",
                    ReturningData = "bk",
                    NotificationId = 1,
                    NotifyTime = DateTime.Now.AddSeconds(Time),
                    Android =
                    {
                        IconName = "icon.png",
                    }
                };
                NotificationCenter.Current.Show(notification);

                DisplayAlert("Uwaga", "Przypomnienie zaplanowane pomyślnie", "OK");

                EntryWord.Text = String.Empty;
                EntryTranslate.Text = String.Empty;
                EntryTime.Text = String.Empty;
            }
            else
            {
                DisplayAlert("Error", "Wypełnij poprawnie wszystkie pola", "OK");
            }
        }

        private void CancelNotification_Clicked(object sender, EventArgs e)
        {
            //tablica z id powiadomienia i jezeli cos jest to przycisk anuluj aktywny, jesli nie to nieaktywny
            //NotificationCenter.Current.CancelAll();
            //DisplayAlert("Anulowano", "Pomyślnie anulowano powiadomienia", "OK");
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await FrameInfo.ScaleTo(1.15, 100);
            await FrameInfo.ScaleTo(1, 100);
            await Shell.Current.GoToAsync("/LastTen");
        }

        private void infoButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Jak działa aplikacja?", "Wprowadź słówko a my je ci przypomnimy. Jeśli będziesz czuł, że znasz je doskonale możesz wpisać następne. Jeśli jednak uważasz, że potzebujesz jeszcze przypomnienia - wpisz je jeszcze raz. Jak zdecydujesz, że nie chcesz otrzymać przypomnienia ostatnio wpisanego słowka - kliknij przycisk anuluj", "Kumam");
        }

        private void TapGestureRecognizer_QuestionMark(object sender, EventArgs e)
        {
            DisplayAlert("Słówko", "Tutaj wpisz słówko, które ciężko Ci zapamiętać, a my je Ci przypomnimy", "OK");
        }

        private void TapGestureRecognizer_Dictionary(object sender, EventArgs e)
        {
            DisplayAlert("Tłumaczenie", "Sprawdź tłumaczenie słówka i wpisz je obok", "OK");
        }

        private void TapGestureRecognizer_Clock(object sender, EventArgs e)
        {
            DisplayAlert("Czas", "Tutaj możesz spersonalizować czas, po którym otrzymasz przypomnienie swojego słówka", "OK");
        }

        private void TapGestureRecognizer_LabelDictionary(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("Diki");
        }
    }
}