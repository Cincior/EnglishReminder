using EnglishReminder2.Views;
using Plugin.LocalNotification;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EnglishReminder2
{
    public partial class App : Application
    {
        public App()
        {
            Routing.RegisterRoute("Main", typeof(Main));
            Routing.RegisterRoute("LastTen", typeof(LastTen));
            Routing.RegisterRoute("Diki", typeof(Diki));
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
