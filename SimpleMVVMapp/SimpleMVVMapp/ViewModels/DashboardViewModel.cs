using SimpleMVVMApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace SimpleMVVMApp.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Person> _people;
        public ObservableCollection<Person> People
        {
            get => _people;
            set { _people = value; OnPropertyChanged(); UpdateStats(); }
        }

        private int _totalPeople;
        public int TotalPeople { get => _totalPeople; set { _totalPeople = value; OnPropertyChanged(); } }

        private double _averageAge;
        public double AverageAge { get => _averageAge; set { _averageAge = value; OnPropertyChanged(); } }

        private int _oldestAge;
        public int OldestAge { get => _oldestAge; set { _oldestAge = value; OnPropertyChanged(); } }

        private int _youngestAge;
        public int YoungestAge { get => _youngestAge; set { _youngestAge = value; OnPropertyChanged(); } }

        public ObservableCollection<KeyValuePair<int, int>> AgeDistribution { get; set; } = new ObservableCollection<KeyValuePair<int, int>>();

        public DashboardViewModel(ObservableCollection<Person> people)
        {
            People = people;
            AgeDistribution = new ObservableCollection<KeyValuePair<int, int>>();
            UpdateStats();
            People.CollectionChanged += (s, e) => UpdateStats();
        }

        private void UpdateStats()
        {
            if (AgeDistribution == null)
                AgeDistribution = new ObservableCollection<KeyValuePair<int, int>>();

            if (People == null || People.Count == 0)
            {
                TotalPeople = 0;
                AverageAge = 0;
                OldestAge = 0;
                YoungestAge = 0;
                AgeDistribution.Clear();
                return;
            }

            TotalPeople = People.Count;
            AverageAge = Math.Round(People.Average(p => p.Age), 1);
            OldestAge = People.Max(p => p.Age);
            YoungestAge = People.Min(p => p.Age);

            var groups = People.GroupBy(p => p.Age / 10 * 10)
                               .Select(g => new KeyValuePair<int, int>(g.Key, g.Count() * 10))
                               .OrderBy(k => k.Key)
                               .ToList();

            AgeDistribution.Clear();
            foreach (var g in groups) AgeDistribution.Add(g);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
