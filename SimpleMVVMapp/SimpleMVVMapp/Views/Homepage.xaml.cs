using System.Windows;
using System.Windows.Controls;

namespace SimpleMVVMApp.Views
{
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
        }

        // 👇 Add this method here, just below the constructor
        private void ClearFields_Click(object sender, RoutedEventArgs e)
        {
            // Get the ViewModel currently linked to this page
            if (DataContext is SimpleMVVMApp.ViewModels.MainViewModel viewModel)
            {
                // Reset input fields
                viewModel.NewName = string.Empty;
                viewModel.NewAge = string.Empty;

                // Deselect the current person
                viewModel.SelectedPerson = null;
            }
        }
    }
}
