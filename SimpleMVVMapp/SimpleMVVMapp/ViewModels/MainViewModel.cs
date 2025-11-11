using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using SimpleMVVMApp.Models;

namespace SimpleMVVMApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // 🧱 Data Collection
        public ObservableCollection<Person> People { get; set; }

        // 🧠 Filtered View (for search)
        public ICollectionView FilteredPeople { get; private set; }

        // 🔍 Search Query
        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
                ApplyFilter(); // Filter as user types
            }
        }

        // 🧠 Input Properties
        private string _newName;
        public string NewName
        {
            get => _newName;
            set
            {
                _newName = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested(); // refresh button states
            }
        }

        private string _newAge;
        public string NewAge
        {
            get => _newAge;
            set
            {
                _newAge = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        // 🧍 Selected Person for Editing
        private Person _selectedPerson;
        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged();

                if (SelectedPerson != null)
                {
                    // Auto-fill the input boxes when selecting a person
                    NewName = SelectedPerson.Name;
                    NewAge = SelectedPerson.Age.ToString();
                }

                CommandManager.InvalidateRequerySuggested();
            }
        }

        // ⚙️ Commands
        public ICommand AddPersonCommand { get; set; }
        public ICommand DeletePersonCommand { get; set; }
        public ICommand UpdatePersonCommand { get; set; }

        // 🧩 Constructor
        public MainViewModel()
        {
            // 🧾 Load saved data when app starts
            var loadedPeople = DataService.LoadData();
            People = new ObservableCollection<Person>(loadedPeople);

            // Create a filtered view
            FilteredPeople = CollectionViewSource.GetDefaultView(People);

            // 🔄 Auto-save whenever collection changes
            People.CollectionChanged += People_CollectionChanged;

            // Initialize commands
            AddPersonCommand = new RelayCommand(AddPerson, CanAddPerson);
            DeletePersonCommand = new RelayCommand(DeletePerson);
            UpdatePersonCommand = new RelayCommand(UpdatePerson, CanUpdatePerson);
        }

        // 💾 Auto-save when data changes
        private void People_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            DataService.SaveData(People);
            ApplyFilter(); // Refresh filter after add/delete
        }

        // 🔍 Apply search filter
        private void ApplyFilter()
        {
            if (FilteredPeople == null) return;

            FilteredPeople.Filter = personObj =>
            {
                if (string.IsNullOrWhiteSpace(SearchQuery))
                    return true; // Show all if empty

                if (personObj is Person person)
                    return person.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase);

                return false;
            };

            FilteredPeople.Refresh();
        }

        // ➕ Add
        private void AddPerson(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(NewName) &&
                int.TryParse(NewAge, out int age) && age > 0)
            {
                People.Add(new Person { Name = NewName, Age = age });
                DataService.SaveData(People); // Save immediately
                NewName = string.Empty;
                NewAge = string.Empty;
                ApplyFilter();
            }
        }

        private bool CanAddPerson(object parameter)
        {
            return !string.IsNullOrWhiteSpace(NewName) &&
                   int.TryParse(NewAge, out int age) && age > 0;
        }

        // ❌ Delete (with confirmation)
        private void DeletePerson(object parameter)
        {
            if (parameter is Person person)
            {
                var result = System.Windows.MessageBox.Show(
                    $"Are you sure you want to delete:\n\nName: {person.Name}\nAge: {person.Age}",
                    "Confirm Delete",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Warning);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    People.Remove(person);
                    DataService.SaveData(People);

                    if (SelectedPerson == person)
                    {
                        SelectedPerson = null;
                        NewName = string.Empty;
                        NewAge = string.Empty;
                    }
                }
            }
        }

        // ✏️ Update
        private void UpdatePerson(object parameter)
        {
            if (SelectedPerson != null &&
                !string.IsNullOrWhiteSpace(NewName) &&
                int.TryParse(NewAge, out int age) && age > 0)
            {
                SelectedPerson.Name = NewName;
                SelectedPerson.Age = age;

                // Save after update
                DataService.SaveData(People);

                // Clear fields after update
                NewName = string.Empty;
                NewAge = string.Empty;
                SelectedPerson = null;
                ApplyFilter();
            }
        }

        private bool CanUpdatePerson(object parameter)
        {
            return SelectedPerson != null &&
                   !string.IsNullOrWhiteSpace(NewName) &&
                   int.TryParse(NewAge, out int age) && age > 0;
        }

        // 🔔 Notify UI that a property changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
