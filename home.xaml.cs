using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

using System.Runtime.Serialization;
using Windows.UI.Core;
using NotificationsExtensions.ToastContent;
using Windows.UI.Notifications;
using System.Collections;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices.Query;
using Windows.Networking.Connectivity;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Home_remote
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class home : Page
    {
        public string notiTOP, notiBOTTOM;
        int lt1, lt1s, lt2, lt2s, lt3, lt3s, fn, fns;
        int dlt1s, dlt2s, dlt3s, dfns;
        int flag1 = 0, flag2 = 0, flag3 = 0, flag4 = 0;
        string time1 = "", time2 = "", time3 = "", time4 = "";
        string runningtime1 = "", runningtime2 = "", runningtime3 = "", runningtime4 = "";
        double p1 = 0, p2 = 0, p3 = 0, p4 = 0;

        public class data
        {
            public string Id { get; set; }
            [JsonProperty(PropertyName = "__createdAt")]
            public DateTime createdAt { get; set; }
            [JsonProperty(PropertyName = "l1")]
            public int l1 { get; set; }
            public int l1s { get; set; }
            public int l2 { get; set; }
            public int l2s { get; set; }
            public int l3 { get; set; }
            public int l3s { get; set; }
            public int f { get; set; }
            public int fs { get; set; }
        }

        IMobileServiceTable<data> itemsTable;
        public MobileServiceCollection<data, data> items;

        private void accountbttn_Click(object sender, RoutedEventArgs e)
        {
            getstarted();
        }

        public home()
        {
            this.InitializeComponent();
            detail_grid.Visibility = Visibility.Collapsed;
            getstarted();
            dlt1s = lt1s;
            dlt2s = lt2s;
            dlt3s = lt3s;
            dfns = fns;
        }

        //************************************** Internet Connection *********************************
        public static bool IsInternet()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }
        public async void getstarted()
        {
            if (!IsInternet())
            {
                await new MessageDialog("Sorry, No Internet Connection", "Error loading items").ShowAsync();
            }
            else
            {
                pring.IsActive = true;
                MobileServiceClient client = new MobileServiceClient("https://homeremote.azure-mobile.net", "SwjMwPXsSsBEtuiLGFXdIDQqHZbavx83");
                itemsTable = client.GetTable<data>();
                detail_grid.Visibility = Visibility.Collapsed;
                open.Begin();
                await RefreshTodoItems();
                check();
            }
        }
        

        //*************************************** For Reload Time Period ******************************

        DispatcherTimer timer;
        int maxTime = 0;
        async void timer_Tick(object sender, object e)
        {
            maxTime++;
            if (maxTime == 6)
            {
                timer.Stop();
                await RefreshTodoItems();
                check();
                pring.IsActive = false;
            }
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
        }


        //************************************** For device on timer ********************************

        DispatcherTimer detailtimer1, detailtimer2, detailtimer3, detailtimer4;

        int sec1 = 0, sec2 = 0, sec3 = 0, sec4 = 0;
        int hour1 = 0, hour2 = 0, hour3 = 0, hour4 = 0;
        int min1 = 0, min2 = 0, min3 = 0, min4 = 0;

        private void detail_grid_Loaded(object sender, RoutedEventArgs e)
        {
            detailtimer1 = new DispatcherTimer();
            detailtimer1.Interval = TimeSpan.FromSeconds(1);
            detailtimer1.Tick += timer_Tickdetail1;

            detailtimer2 = new DispatcherTimer();
            detailtimer2.Interval = TimeSpan.FromSeconds(1);
            detailtimer2.Tick += timer_Tickdetail2;

            detailtimer3 = new DispatcherTimer();
            detailtimer3.Interval = TimeSpan.FromSeconds(1);
            detailtimer3.Tick += timer_Tickdetail3;

            detailtimer4 = new DispatcherTimer();
            detailtimer4.Interval = TimeSpan.FromSeconds(1);
            detailtimer4.Tick += timer_Tickdetail4;
        }

        void timer_Tickdetail1(object sender, object e)
        {
            sec1++;
            if (sec1 > 59) { min1++; sec1 = 0; }
            if (min1 > 59) { hour1++; min1 = 0; }
            runningtime1 = hour1 + ":" + min1 + ":" + sec1.ToString();
            Running1.Text = runningtime1;
            p1 += 1.00/1200;
            powerC1.Text = Math.Round(p1, 4).ToString() + " Wh";
        }

        void timer_Tickdetail2(object sender, object e)
        {
            sec2++;
            if (sec2 > 59) { min2++; sec2 = 0; }
            if (min2 > 59) { hour2++; min2 = 0; }
            runningtime2 = hour2 + ":" + min2 + ":" + sec2.ToString();
            Running2.Text = runningtime2;
            p2 += 1.00 / 1200;
            powerC2.Text = Math.Round(p2, 4).ToString() + " Wh";
        }

        void timer_Tickdetail3(object sender, object e)
        {
            sec3++;
            if (sec3 > 59) { min3++; sec3 = 0; }
            if (min3 > 59) { hour3++; min3 = 0; }
            runningtime3 = hour3 + ":" + min3 + ":" + sec3.ToString();
            Running3.Text = runningtime3;
            p3 += 1.00 / 1200;
            powerC3.Text = Math.Round(p3, 4).ToString() + " Wh";
        }

        void timer_Tickdetail4(object sender, object e)
        {
            sec4++;
            if (sec4 > 59) { min4++; sec4 = 0; }
            if (min4 > 59) { hour4++; min4 = 0; }
            runningtime4 = hour4 + ":" + min4 + ":" + sec4.ToString();
            Running4.Text = runningtime4;
            p4 += 3.52 / 3600;
            powerC4.Text = Math.Round(p4, 4).ToString() + " Wh";
        }

        //********************************* To collect the data from Azure database and use it ************************

        private async Task RefreshTodoItems()
        {
            pring.IsActive = true;
            MobileServiceInvalidOperationException exception = null;
            try {
                items = await itemsTable.OrderByDescending(x => x.createdAt).Take(1).ToCollectionAsync();
                var myitem = items[0];
                lt1 = myitem.l1;
                lt1s = myitem.l1s;
                lt2 = myitem.l2;
                lt2s = myitem.l2s;
                lt3 = myitem.l3;
                lt3s = myitem.l3s;
                fn = myitem.f;
                fns = myitem.fs;
                //notiBOTTOM = lt1.ToString() + " " + lt1s.ToString() + " " + lt2.ToString() + " " + lt2s.ToString()
                //+ " " + lt3.ToString() + " " + lt3s.ToString() + " " + fn.ToString() + " " + fns.ToString();
                //notification();
            }
            catch (MobileServiceInvalidOperationException e){
                exception = e;
            }
            if (exception != null){
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }else{
                ListItems.ItemsSource = items;
            }
            pring.IsActive = false;
        }

        void check()
        {
            if (lt1s == 1)
            {
                on1.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/glow1.png", UriKind.Absolute) };
                Lighttoggle.Header = "State: ON";
                if (flag1 == 1)
                {
                    Detailname.Text = "Living Room";
                    dImage.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/glow1.png", UriKind.Absolute) };
                    Status.Text = "Online";
                    time1 = DateTime.Now.ToString("h:mm:ss tt");
                    flag1 = 0;
                    Started.Text = time1;
                    detailtimer1.Start();
                    notiBOTTOM = "Turned On";
                    notiTOP = "Living Room";
                    notification();
                }
            }
            else if (lt1s == 0)
            {
                on1.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/noglow1.png", UriKind.Absolute) };
                Lighttoggle.Header = "State: OFF";
                if (flag1 == 1)
                {
                    Detailname.Text = "Living Room";
                    Started.Text = "N/A";
                    Status.Text = "Offline";
                    dImage.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/noglow1.png", UriKind.Absolute) };
                    flag1 = 0;
                    detailtimer1.Stop();
                    Running1.Text = runningtime1;
                    notiBOTTOM = "Turned Off";
                    notiTOP = "Living Room";
                    notification();
                }
            }
            if (lt2s == 1)
            {
                on2.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/glow1.png", UriKind.Absolute) };
                Lighttoggle2.Header = "State: ON";
                if (flag2 == 1)
                {
                    Detailname.Text = "Kitchen";
                    dImage.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/glow1.png", UriKind.Absolute) };
                    Status.Text = "Online";
                    time2 = DateTime.Now.ToString("h:mm:ss tt");
                    flag2 = 0;
                    Started.Text = time2;
                    detailtimer2.Start();
                    notiBOTTOM = "Turned On";
                    notiTOP = "Kitchen";
                    notification();
                }
            }
            else if (lt2s == 0)
            {
                on2.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/noglow1.png", UriKind.Absolute) };
                Lighttoggle2.Header = "State: OFF";
                if (flag2 == 1)
                {
                    Detailname.Text = "Kitchen";
                    Status.Text = "Offline";
                    dImage.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/noglow1.png", UriKind.Absolute) };
                    Started.Text = "N/A";
                    flag2 = 0;
                    detailtimer2.Stop();
                    Running2.Text = runningtime1;
                    notiBOTTOM = "Turned Off";
                    notiTOP = "Kitchen";
                    notification();
                }
            }
            if (lt3s == 1)
            {
                on3.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/glow1.png", UriKind.Absolute) };
                Lighttoggle3.Header = "State: ON";
                if (flag3 == 1)
                {
                    Detailname.Text = "Bathroom";
                    dImage.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/glow1.png", UriKind.Absolute) };
                    Status.Text = "Online";
                    time3 = DateTime.Now.ToString("h:mm:ss tt");
                    Started.Text = time3;
                    flag3 = 0;
                    detailtimer3.Start();
                    notiBOTTOM = "Turned On";
                    notiTOP = "Bathroom";
                    notification();
                }
            }
            else if (lt3s == 0)
            {
                on3.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/noglow1.png", UriKind.Absolute) };
                Lighttoggle3.Header = "State: OFF";
                if (flag3 == 1)
                {
                    Detailname.Text = "Bathroom";
                    Status.Text = "Offline";
                    dImage.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/noglow1.png", UriKind.Absolute) };
                    Started.Text = "N/A";
                    flag3 = 0;
                    detailtimer3.Stop();
                    Running3.Text = runningtime3;
                    notiBOTTOM = "Turned Off";
                    notiTOP = "Bathroom";
                    notification();
                }
            }
            if (fns == 1)
            {
                on.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/rotating-fan.png", UriKind.Absolute) };
                rotate.Begin();
                fanttoggle.Header = "State: ON";
                if (flag4 == 1)
                {
                    Detailname.Text = "Ceiling Fan";
                    dImage.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/rotating-fan.png", UriKind.Absolute) };
                    Drotate.Begin();
                    Status.Text = "Rotating";
                    time4 = DateTime.Now.ToString("h:mm:ss tt");
                    flag4 = 0;
                    Started.Text = time4;
                    detailtimer4.Start();
                    notiBOTTOM = "Turned On";
                    notiTOP = "Ceiling Fan";
                    notification();
                }
            }
            else if (fns == 0)
            {
                rotate.Stop();
                on.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/stopping-fan.png", UriKind.Absolute) };
                fanttoggle.Header = "State: OFF";
                if (flag4 == 1)
                {
                    Detailname.Text = "Ceiling Fan";
                    Status.Text = "Stopped";
                    Drotate.Stop();
                    dImage.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/stopping-fan.png", UriKind.Absolute) };
                    Started.Text = "N/A";
                    flag4 = 0;
                    detailtimer4.Stop();
                    Running4.Text = runningtime4;
                    notiBOTTOM = "Turned Off";
                    notiTOP = "Ceiling Fan";
                    notification();
                }
            }
        }

        //********************************* To open/close Detail Grid *************************************************
        private void backbtn_Click(object sender, RoutedEventArgs e)
        {
            detailgrid_close.Begin();
        }

        private void detail_Tapped(object sender, RoutedEventArgs e)
        {
            detail_grid.Visibility = Visibility.Visible;
            detailgrid_open.Begin();

            Button btn = (Button)sender;
            if(btn.Name.ToString() == "detail1")
            {
                Detailname.Text = "Living Room";
                Running1.Visibility = Visibility.Visible;
                Running2.Visibility = Visibility.Collapsed;
                Running3.Visibility = Visibility.Collapsed;
                Running4.Visibility = Visibility.Collapsed;
                powerC1.Visibility = Visibility.Visible;
                powerC2.Visibility = Visibility.Collapsed;
                powerC3.Visibility = Visibility.Collapsed;
                powerC4.Visibility = Visibility.Collapsed;

                dImageL.Visibility = Visibility.Visible;
                dImage.Visibility = Visibility.Collapsed;
                if (lt1s == 1)
                {
                    dImageL.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/glow1.png", UriKind.Absolute) };
                    Status.Text = "Online";
                    Started.Text = time1;
                }
                else if(lt1s == 0)
                {
                    Status.Text = "Offline";
                    dImageL.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/noglow1.png", UriKind.Absolute) };
                    Started.Text = "N/A";
                    Running1.Text = runningtime1;
                }
            }
            else if(btn.Name.ToString() == "detail2")
            {
                Detailname.Text = "Kitchen";
                Running1.Visibility = Visibility.Collapsed;
                Running2.Visibility = Visibility.Visible;
                Running3.Visibility = Visibility.Collapsed;
                Running4.Visibility = Visibility.Collapsed;
                powerC1.Visibility = Visibility.Collapsed;
                powerC2.Visibility = Visibility.Visible;
                powerC3.Visibility = Visibility.Collapsed;
                powerC4.Visibility = Visibility.Collapsed;
                dImageL.Visibility = Visibility.Visible;
                dImage.Visibility = Visibility.Collapsed;
                if (lt2s == 1)
                {
                    dImageL.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/glow1.png", UriKind.Absolute) };
                    Status.Text = "Online";
                    Started.Text = time2;
                }
                else if (lt2s == 0)
                {
                    Status.Text = "Offline";
                    dImageL.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/noglow1.png", UriKind.Absolute) };
                    Started.Text = "N/A";
                    Running2.Text = runningtime2;
                }
            }
            else if (btn.Name.ToString() == "detail3")
            {
                Detailname.Text = "Bathroom";
                Running1.Visibility = Visibility.Collapsed;
                Running2.Visibility = Visibility.Collapsed;
                Running3.Visibility = Visibility.Visible;
                Running4.Visibility = Visibility.Collapsed;
                powerC1.Visibility = Visibility.Collapsed;
                powerC2.Visibility = Visibility.Collapsed;
                powerC3.Visibility = Visibility.Visible;
                powerC4.Visibility = Visibility.Collapsed;
                dImageL.Visibility = Visibility.Visible;
                dImage.Visibility = Visibility.Collapsed;
                if (lt3s == 1)
                {
                    dImageL.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/glow1.png", UriKind.Absolute) };
                    Status.Text = "Online";
                    Started.Text = time3;
                }
                else if (lt3s == 0)
                {
                    Status.Text = "Offline";
                    dImageL.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/noglow1.png", UriKind.Absolute) };
                    Started.Text = "N/A";
                    Running3.Text = runningtime3;
                }
            }
            else if (btn.Name.ToString() == "detail4")
            {
                Detailname.Text = "Ceiling Fan";
                Running1.Visibility = Visibility.Collapsed;
                Running2.Visibility = Visibility.Collapsed;
                Running3.Visibility = Visibility.Collapsed;
                Running4.Visibility = Visibility.Visible;
                powerC1.Visibility = Visibility.Collapsed;
                powerC2.Visibility = Visibility.Collapsed;
                powerC3.Visibility = Visibility.Collapsed;
                powerC4.Visibility = Visibility.Visible;
                dImage.Visibility = Visibility.Visible;
                dImageL.Visibility = Visibility.Collapsed;
                if (fns == 1)
                {
                    dImage.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/rotating-fan.png", UriKind.Absolute) };
                    Drotate.Begin();
                    Status.Text = "Rotating";
                    Started.Text = time4;
                }
                else if (fns == 0)
                {
                    Status.Text = "Stopped";
                    Drotate.Stop();
                    dImage.Source = new BitmapImage() { UriSource = new Uri("ms-appx:///Assets/stopping-fan.png", UriKind.Absolute) };
                    Started.Text = "N/A";
                    Running4.Text = runningtime4;
                }
            }  
        }

        //*********************************** To Display Message Dialog ******************************************
        private async void MessageBox(string message, string m)
        {
            var dialog = new MessageDialog(message.ToString(), m.ToString());
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
        }

        //*********************************** To insert data into Azure Table ************************************
        private async Task InsertTodoItem(data value)
        {
            try
            {
                await itemsTable.InsertAsync(value);
            }
            catch (Exception ex)
            {
                var messagedialog = new MessageDialog("Please check your internet connection.", "Something is wrong!");
                await messagedialog.ShowAsync();
            }
        }

        //*********************************** For Notification Popup *******************************************
        public void notification()
        {
            IToastText02 trying_toast = ToastContentFactory.CreateToastText02();
            trying_toast.TextHeading.Text = notiTOP;
            trying_toast.TextBodyWrap.Text = notiBOTTOM;
            ScheduledToastNotification giveittime;
            giveittime = new ScheduledToastNotification(trying_toast.GetXml(), DateTime.Now.AddSeconds(0.5));
            giveittime.Id = "Any_Id";
            ToastNotificationManager.CreateToastNotifier().AddToSchedule(giveittime);
        }


        //********************************** For Device status Toggle ******************************************
        private async void Lighttoggle_Toggled(object sender, RoutedEventArgs e)
        { 
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    if (lt1 == 0)
                    {
                        var item = new data { l1 = 1, l2 = lt2, l3 = lt3, f = fn };
                        await InsertTodoItem(item);
                    }
                    else
                    {
                        var item = new data { l1 = 0, l2 = lt2, l3 = lt3, f = fn };
                        await InsertTodoItem(item); 
                    }
                }else{
                    if (lt1 == 1)
                    {
                        var item = new data { l1 = 0, l2 = lt2, l3 = lt3, f = fn };
                        await InsertTodoItem(item);
                    }
                    else
                    {
                        var item = new data { l1 = 1, l2 = lt2, l3 = lt3, f = fn };
                        await InsertTodoItem(item);
                    }
                }
            }
            flag1 = 1;
            maxTime = 0;
            timer.Start();
            pring.IsActive = true;
        }

        private async void Lighttoggle2_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    if (lt2 == 0)
                    {
                        var item = new data { l1 = lt1, l2 = 1, l3 = lt3, f = fn };
                        await InsertTodoItem(item);
                    }
                    else
                    {
                        var item = new data { l1 = lt1, l2 = 0, l3 = lt3, f = fn };
                        await InsertTodoItem(item);
                    }
                }
                else
                {
                    if (lt2 == 1)
                    {
                        var item = new data { l1 = lt1, l2 = 0, l3 = lt3, f = fn };
                        await InsertTodoItem(item);
                    }
                    else
                    {
                        var item = new data { l1 = lt1, l2 = 1, l3 = lt3, f = fn };
                        await InsertTodoItem(item);
                    }
                }
            }
            flag2 = 1;
            maxTime = 0;
            timer.Start();
            pring.IsActive = true;
        }

        private async void Lighttoggle3_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    if(lt3 == 0)
                    {
                        var item = new data { l1 = lt1, l2 = lt2, l3 = 1, f = fn };
                        await InsertTodoItem(item);
                    }
                    else
                    {
                        var item = new data { l1 = lt1, l2 = lt2, l3 = 0, f = fn };
                        await InsertTodoItem(item);
                    }
                }
                else
                {
                    if(lt3 == 1)
                    {
                        var item = new data { l1 = lt1, l2 = lt2, l3 = 0, f = fn };
                        await InsertTodoItem(item);
                    }
                    else
                    {
                        var item = new data { l1 = lt1, l2 = lt2, l3 = 1, f = fn };
                        await InsertTodoItem(item);
                    }
                }
            }
            flag3 = 1;
            maxTime = 0;
            timer.Start();
            pring.IsActive = true;
        }

        private async void fan_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    if(fn == 0)
                    {
                        var item = new data { l1 = lt1, l2 = lt2, l3 = lt3, f = 1 };
                        await InsertTodoItem(item);
                    }
                    else
                    {
                        var item = new data { l1 = lt1, l2 = lt2, l3 = lt3, f = 0 };
                        await InsertTodoItem(item);
                    }
                }
                else
                {
                    if(fn == 1)
                    {
                        var item = new data { l1 = lt1, l2 = lt2, l3 = lt3, f = 0 };
                        await InsertTodoItem(item);
                    }
                    else
                    {
                        var item = new data { l1 = lt1, l2 = lt2, l3 = lt3, f = 1};
                        await InsertTodoItem(item);      
                    }
                }
            }
            maxTime = 0;
            timer.Start();
            pring.IsActive = true;
            flag4 = 1;
        }

        //***************************************** End ********************************************************
    }
}
