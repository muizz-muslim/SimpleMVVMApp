using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SimpleMVVMApp.Models;

namespace SimpleMVVMApp.ViewModels
{
    public static class DataService
    {
        private static readonly string FilePath = "people.json";

        public static void SaveData(IEnumerable<Person> people)
        {
            var json = JsonSerializer.Serialize(people, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public static List<Person> LoadData()
        {
            if (!File.Exists(FilePath))
                return new List<Person>();

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Person>>(json);
        }
    }
}

