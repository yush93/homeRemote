using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Home_remote
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Setting : Page
    {
        string pin = "3344";
        string inpin = "";
        bool locked = false;
        public Setting()
        {
            this.InitializeComponent();
            if(locked==true)
            {
                keypad.Visibility = Visibility.Visible;
                lockunlock.Visibility = Visibility.Collapsed;
                image1.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/red.png", UriKind.Absolute) };


            }
            else if (locked == false)
            {
                keypad.Visibility = Visibility.Collapsed;
                lockunlock.Visibility = Visibility.Visible;
                image1.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/green.png", UriKind.Absolute) };

            }
        }

        private void one_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            if(b.Name.ToString() == "bs" )
            {
                inpin = pinBox.Password.Substring(0, pinBox.Password.Length - 1);
                pinBox.Password = inpin;
            }
            else
            {
                inpin += b.Content.ToString();
                pinBox.Password = inpin;
            }
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if(pin == inpin)
            {
                image.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/opendoor.png", UriKind.Absolute) };
                image1.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/green.png", UriKind.Absolute) };
                locked = false;
            }
            else
            {
                locked = true;
                inpin = "";
                pinBox.Password = "";

            }
        }

        private void lock_Click(object sender, RoutedEventArgs e)
        {
            Button lb = (Button)sender;
            if(lb.Content.ToString() == "Lock" && locked == false )
            {
                locked = true;
                image.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/closedoor.png", UriKind.Absolute) };
                image1.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/red.png", UriKind.Absolute) };
            }
            else if(lb.Content.ToString() == "Unlock" && locked == true)
            {
                locked = false;
                keypad.Visibility = Visibility.Visible;
                lockunlock.Visibility = Visibility.Collapsed;
                
            }
        }
    }
}
