using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleMVVMApp.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SimpleMVVMApp.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<Person> People { get; set; }
        public string NewName { get; set; }
        public string NewAge { get; set; }

        public ICommand AddPersonCommand { get; set; }
        public ICommand DeletePersonCommand { get; set; } // 👈 Add this

        public MainViewModel()
        {
            People = new ObservableCollection<Person>
            {
                new Person { Name = "Alice", Age = 30 },
                new Person { Name = "Bob", Age = 25 }
            };

            AddPersonCommand = new RelayCommand(AddPerson, CanAddPerson);
            DeletePersonCommand = new RelayCommand(DeletePerson); // 👈 initialize it
        }

        private void AddPerson(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(NewName) &&
                int.TryParse(NewAge, out int age) && age > 0)
            {
                People.Add(new Person { Name = NewName, Age = age });
                NewName = string.Empty;
                NewAge = string.Empty;
            }
        }

        private bool CanAddPerson(object parameter)
        {
            return !string.IsNullOrWhiteSpace(NewName) &&
                   int.TryParse(NewAge, out int age) && age > 0;
        }

        private void DeletePerson(object parameter)
        {
            if (parameter is Person person)
            {
                People.Remove(person);
            }
        }
    }
}
