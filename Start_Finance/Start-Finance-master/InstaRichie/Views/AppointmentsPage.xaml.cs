﻿using System;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppointmentsPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public AppointmentsPage()
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
            conn.CreateTable<Appointments>();

            /// Refresh Data
            var query = conn.Table<Appointments>();
            AppointmentListView.ItemsSource = query.ToList();
            EventDateCalendar.Date = DateTime.Now;
        }

        private async void AddData(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EventNameText.Text.ToString() == "" )
                {
                    MessageDialog dialog = new MessageDialog("No Appointment Name has been entered", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (LocationText.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No Appointment Address has been entered", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (EventDateCalendar.Date < DateTime.Now)
                {
                    MessageDialog dialog = new MessageDialog("Appointment Date must be later than the current date", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.Insert(new Appointments()
                    {
                        EventName = EventNameText.Text,
                        Location = LocationText.Text,
                        EventDate = DateTime.Parse(EventDateCalendar.Date.ToString()).ToString("MM/dd/yyyy"),
                        StartTime = DateTime.Parse(StartTimeBox.Time.ToString()).ToString("hh:mm tt"),
                        EndTime = DateTime.Parse(EndTimeBox.Time.ToString()).ToString("hh:mm tt"),
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
                    MessageDialog dialog = new MessageDialog("A Similar Appointment Nane already exists, Try a different name", "Oops..!");
                    await dialog.ShowAsync();
                }
            }
        }

        private async void ClearFileds_Click(object sender, RoutedEventArgs e)
        {
            EventNameText.Text = string.Empty;

            MessageDialog ClearDialog = new MessageDialog("Cleared", "information");
            await ClearDialog.ShowAsync();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }

        private async void DeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AppointmentSelection = ((Appointments)AppointmentListView.SelectedItem).ID.ToString();
                if (AppointmentSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<Appointments>();
                    var query1 = conn.Table<Appointments>();
                    var query3 = conn.Query<Appointments>("DELETE FROM Appointments WHERE ID ='" + AppointmentSelection + "'");
                    AppointmentListView.ItemsSource = query1.ToList();
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
