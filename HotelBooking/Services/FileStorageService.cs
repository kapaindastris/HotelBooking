using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using HotelBooking.Models;

namespace HotelBooking.Services
{
    public class FileStorageService
    {
        private const string FileName = "bookings.json";

        public void Save(ObservableCollection<Booking> bookings)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(bookings, options);
            File.WriteAllText(FileName, json);
        }

        public ObservableCollection<Booking> Load()
        {
            if (!File.Exists(FileName))
                return new ObservableCollection<Booking>();

            string json = File.ReadAllText(FileName);
            return JsonSerializer.Deserialize<ObservableCollection<Booking>>(json)
                   ?? new ObservableCollection<Booking>();
        }
    }
}
