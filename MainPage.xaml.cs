using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using NotificationsExtensions.ToastContent;
using Windows.UI.Notifications;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Home_remote
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MyFrame.Navigate(typeof(home));
            Title.Text = "Home User";
        }

        private void ItemListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Home.IsSelected)
            {
                MyFrame.Navigate(typeof(home));
                Title.Text = "Home User";
            }
            else if (aboutus.IsSelected)
            {
                MyFrame.Navigate(typeof(About_Us));
                Title.Text = "About Us";
            }
            else if (Settings.IsSelected)
            {
                MyFrame.Navigate(typeof(Setting));
                Title.Text = "Settings";
            }
            else if (feedback.IsSelected)
            {
                MyFrame.Navigate(typeof(feedback));
                Title.Text = "Feedback";
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }
    }
}
