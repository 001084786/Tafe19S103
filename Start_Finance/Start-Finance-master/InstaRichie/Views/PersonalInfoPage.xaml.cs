using SQLite.Net;
using StartFinance.Models;
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

namespace StartFinance.Views
{
    public sealed partial class PersonalInfoPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");
        PersonalInfo p1 = new PersonalInfo();

        public PersonalInfoPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            // Creating a table
            Results();
        }

        public void Results()
        {
            // Creating table
            conn.CreateTable<PersonalInfo>();
            var query = conn.Table<PersonalInfo>();
            TransactionList.ItemsSource = query.ToList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }

        private async void AddPersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            // Checks if FirstName is null
            {
                //if (txtPersonalID.Text.ToString() == "")
                //{
                //    MessageDialog dialog = new MessageDialog("Personal ID not Entered", "Oops..!");
                //    await dialog.ShowAsync();
                //}

                if (txtFirstName.Text == "")
                {
                    MessageDialog dialog = new MessageDialog("First Name not Entered", "Oops..!");
                    await dialog.ShowAsync();
                }

                else if (txtLastName.Text == "")
                {
                    MessageDialog dialog = new MessageDialog("Last Name not Entered", "Oops..!");
                    await dialog.ShowAsync();
                }

                else if (txtDOB.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Date of Birth not Entered", "Oops..!");
                    await dialog.ShowAsync();
                }

                else if (txtGender.Text == "")
                {
                    MessageDialog dialog = new MessageDialog("Gender not Entered", "Oops..!");
                    await dialog.ShowAsync();
                }

                else if (txtEmailAddress.Text == "")
                {
                    MessageDialog dialog = new MessageDialog("Email not Entered", "Oops..!");
                    await dialog.ShowAsync();
                }

                else if (txtMobile.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Mobile not Entered", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.Insert(new PersonalInfo
                    {
                        //PersonalID = txtPersonalID.Text.ToString(),
                        FirstName = txtFirstName.Text,
                        LastName = txtLastName.Text,
                        DOB = txtDOB.Text.ToString(),
                        Gender = txtGender.Text,
                        EmailAddress = txtEmailAddress.Text,
                        Mobile = txtMobile.Text.ToString()
                    });
                    Results();
                }
            }

            catch (Exception ex)
            {   // Exception to display when amount is invalid or not numbers
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You forgot to enter the data or entered an invalid data", "Oops..!");
                    await dialog.ShowAsync();
                }   // Exception handling when SQLite contraints are violated
                else if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("Similar Data already exist, Try Different Data", "Oops..!");
                    await dialog.ShowAsync();
                }
            }
        }

        private async void DeletePersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string PersonLabel = ((PersonalInfo)TransactionList.SelectedItem).PersonalID.ToString();
                if (PersonLabel == "")

                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }

                else
                {
                    conn.CreateTable<PersonalInfo>();
                    var query1 = conn.Table<PersonalInfo>();
                    var query2 = conn.Query<PersonalInfo>("DELETE FROM PersonalInfo WHERE PersonalID = '" + PersonLabel + "'");
                    TransactionList.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        private void TransListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            p1 = (PersonalInfo)TransactionList.SelectedItem;
        }

        private void EditPersonalInfo_Click(object sender, RoutedEventArgs e)
        {

            txtFirstName.Text = p1.FirstName;
            txtLastName.Text = p1.LastName;
            txtDOB.Text = p1.DOB;
            txtGender.Text = p1.Gender;
            txtEmailAddress.Text = p1.EmailAddress;
            txtMobile.Text = p1.Mobile;

            //p1.FirstName = txtFirstName.Text;
            //p1.LastName = txtLastName.Text;
            //p1.DOB = txtDOB.Text.ToString();
            //p1.Gender = txtGender.Text;
            //p1.EmailAddress = txtEmailAddress.Text;
            //p1.Mobile = txtMobile.Text.ToString();

        }

        private async void SavePersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string PersonLabel = ((PersonalInfo)TransactionList.SelectedItem).FirstName;
                if (PersonLabel == "")

                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    p1.FirstName = txtFirstName.Text;
                    p1.LastName = txtLastName.Text;
                    p1.DOB = txtDOB.Text.ToString();
                    p1.Gender = txtGender.Text;
                    p1.EmailAddress = txtEmailAddress.Text;
                    p1.Mobile = txtMobile.Text.ToString();
                    var query = conn.Update(p1);
                    var query1 = conn.Table<PersonalInfo>();
                    //var query2 = conn.Query<PersonalInfo>("UPDATE PersonalInfo SET FirstName ='" + txtFirstName.Text + "', LastName = '" + txtLastName.Text + "', DOB = '" + txtDOB.Text + "', Gender = '" + txtGender.Text + "', Email = '" + txtEmailAddress.Text + "', Mobile = '" + txtMobile.Text + "', WHERE PersonalID = '" + PersonLabel + "'");
                    TransactionList.ItemsSource = query1.ToList();
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