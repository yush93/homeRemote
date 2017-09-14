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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Home_remote
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class feedback : Page
    {
        public feedback()
        {
            this.InitializeComponent();
        }

        private async void submit_Click(object sender, RoutedEventArgs e)
        {
            var messagedialog = new MessageDialog("Are you sure?", "Confirmation");
            messagedialog.Commands.Add(new UICommand("Yes", (UICommandInvokeHandler) =>
            {

            }));
            messagedialog.Commands.Add(new UICommand("No", (UICommandInvokeHandler) =>
            {

            }));
            await messagedialog.ShowAsync();
        }
    }
}
