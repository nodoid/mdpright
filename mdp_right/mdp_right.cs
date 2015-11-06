using System;

using Xamarin.Forms;

namespace mdp_right
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            MainPage = new CustomMDP()
            {
                Master = new ContentPage
                {
                    BackgroundColor = Color.White,
                    IsVisible = true,
                    Content = new Label
                    {
                        Text = "Master Label",
                        TextColor = Color.Black
                    }
                },
                Detail = new ContentPage
                {
                    BackgroundColor = Color.Red,
                    Content = new Label
                    {
                        Text = "Detail Label"
                    }
                },
                Orientation = CustomMDP.OrientationType.Right,
                MasterPercent = 0.8f
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

