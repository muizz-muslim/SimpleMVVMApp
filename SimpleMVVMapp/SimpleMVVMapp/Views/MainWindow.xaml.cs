using System;
using System.Windows;
using SimpleMVVMApp.ViewModels;

namespace SimpleMVVMApp.Views
{
    public partial class MainWindow : Window
    {
        private HomePage homePage;
        private DashboardView dashboardPage;

        public MainWindow()
        {
            InitializeComponent();

            homePage = new HomePage
            {
                DataContext = new MainViewModel()
            };

            dashboardPage = new DashboardView
            {
                DataContext = new DashboardViewModel(((MainViewModel)homePage.DataContext).People)
            };

            // Load Home by default
            MainFrame.Content = homePage;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = homePage;
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = dashboardPage;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Settings page coming soon!");
        }

        // 🌙 Theme toggle handlers
        private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            ApplyTheme("DarkTheme");
            ThemeToggle.Content = "☀️ Light Mode";
        }

        private void ThemeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            ApplyTheme("LightTheme");
            ThemeToggle.Content = "🌙 Dark Mode";
        }

        // 🧠 Helper method for switching themes dynamically
        private void ApplyTheme(string themeName)
        {
            var dict = new ResourceDictionary
            {
                Source = new Uri($"/SimpleMVVMApp;component/Themes/{themeName}.xaml", UriKind.Relative)
            };

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}
