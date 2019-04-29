

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
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
using Windows.UI.Xaml.Navigation;
using SQLite;
using StartFinance.Models;
using Windows.UI.Popups;
using SQLite.Net;

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContactDetailsPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        ContactDetails c1 = new ContactDetails();

        public ContactDetailsPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            Results();
        }

        public void Results()
        {
            // Creating table
            conn.CreateTable<ContactDetails>();

            /// Refresh Data
            var query = conn.Table<ContactDetails>();
            ContactListView.ItemsSource = query.ToList();
        }

        private async void AddData(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FirstNametbx.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No FirstName");
                    await dialog.ShowAsync();
                }
                else if (LastNametbx.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No Last Name");
                    await dialog.ShowAsync();
                }
                else if (CompanyNametbx.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No Company Name");
                    await dialog.ShowAsync();
                }
                else if (Mobiletbx.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No Mobile");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.Insert(new ContactDetails()
                    {
                        FirstName = FirstNametbx.Text,
                        LastName = LastNametbx.Text,
                        CompanyName = CompanyNametbx.Text,
                        MobilePhone = Mobiletbx.Text,
                    });
                    Results();
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You forgot to enter the Value or entered an invalid data", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("A Similar Contact already exists, Try again", "Oops..!");
                    await dialog.ShowAsync();
                }
            }
        }

        private async void ClearFileds_Click(object sender, RoutedEventArgs e)
        {
            
            FirstNametbx.Text = string.Empty;
            MessageDialog ClearDialog = new MessageDialog("Cleared", "information");
            await ClearDialog.ShowAsync();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }

        private async void DeleteAccout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ContactSelection = ((ContactDetails)ContactListView.SelectedItem).ContactID.ToString();
                if (ContactSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<ContactDetails>();
                    var query1 = conn.Table<ContactDetails>();
                    var query3 = conn.Query<ContactDetails>("DELETE FROM ContactDetails WHERE ContactId ='" + ContactSelection + "'");
                    ContactListView.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        private void ContactListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            c1 = (ContactDetails)ContactListView.SelectedItem;
        }

        private async void Edit_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                FirstNametbx.Text = c1.FirstName;
                LastNametbx.Text = c1.LastName;
                CompanyNametbx.Text = c1.CompanyName;
                Mobiletbx.Text = c1.MobilePhone;
            }

            catch {
                MessageDialog dialog = new MessageDialog("Not selected the Item for edit", "Oops..!");
                await dialog.ShowAsync();
            }

        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ContactSelection = ((ContactDetails)ContactListView.SelectedItem).FirstName;
                if (ContactSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    c1.FirstName = FirstNametbx.Text;
                    c1.LastName = LastNametbx.Text;
                    c1.CompanyName = CompanyNametbx.Text;
                    c1.MobilePhone = Mobiletbx.Text;

                    var query1 = conn.Update(c1);
                    var query3 = conn.Table<ContactDetails>();
                    ContactListView.ItemsSource = query3.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                await dialog.ShowAsync();
            }
        }
    }
}
