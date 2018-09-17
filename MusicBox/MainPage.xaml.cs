using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MusicBox
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size(1366, 768);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string info = "";

            if (Username.Text.Equals(""))
            {
                ErrorUsername.Text = "Username not empty";
            }
            else
            {
                ErrorUsername.Text = "";
                info = info + Username.Text + " -- ";
                info = info + Password.Password.ToString() + "--";
                info = info + RetypePassword.Password.ToString() + "--";
                info = info + Email.Text + "--";
                info = info + Fullname.Text + "--";
                ComboBoxItem Cbi = (ComboBoxItem)Gender.SelectedValue;
                info = info + Cbi.Content.ToString();
                var dialog = new MessageDialog(info);
                dialog.ShowAsync();
            }
        }
    }
}
