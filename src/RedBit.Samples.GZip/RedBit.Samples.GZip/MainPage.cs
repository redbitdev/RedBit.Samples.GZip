using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RedBit.Samples.GZip
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            this.Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = 
                {
                    new Label 
                    {
                        XAlign = TextAlignment.Center,
                        Text = "Welcome to Xamarin Forms!"
                    }
                }
            };
        }
    }
}
