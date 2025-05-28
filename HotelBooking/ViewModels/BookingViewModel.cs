using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Win32;
using HotelBooking.Models;
using HotelBooking.Services;

namespace HotelBooking.ViewModels
{
    public class BookingViewModel : INotifyPropertyChanged
    {
        private readonly FileStorageService _storage = new();

        public ObservableCollection<Booking> Bookings { get; set; }

        private Booking _newBooking = new();
        public Booking NewBooking
        {
            get => _newBooking;
            set { _newBooking = value; OnPropertyChanged(); }
        }

        private Booking _selectedBooking;
        public Booking SelectedBooking
        {
            get => _selectedBooking;
            set
            {
                _selectedBooking = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public List<string> RoomTypes { get; } = new()
        {
            "Одноместный",
            "Двухместный",
            "Люкс"
        };

        public ICommand LoadImageCommand { get; }
        public ICommand BookCommand { get; }
        public ICommand DeleteCommand { get; }

        public BookingViewModel()
        {
            Bookings = _storage.Load();

            LoadImageCommand = new RelayCommand(_ => LoadImage());
            BookCommand = new RelayCommand(_ => AddBooking(), _ => CanBook());
            DeleteCommand = new RelayCommand(_ => DeleteBooking(), _ => SelectedBooking != null);
        }

        private void LoadImage()
        {
            var dlg = new OpenFileDialog { Filter = "Image Files|*.jpg;*.png;*.jpeg" };
            if (dlg.ShowDialog() == true)
            {
                NewBooking.ImagePath = dlg.FileName;
                OnPropertyChanged(nameof(NewBooking));
            }
        }

        private void AddBooking()
        {
            int days = (NewBooking.CheckOutDate - NewBooking.CheckInDate).Days;
            decimal pricePerDay = NewBooking.RoomType switch
            {
                "Одноместный" => 3000,
                "Двухместный" => 5000,
                "Люкс" => 8000,
                _ => 4000
            };

            NewBooking.TotalPrice = days * pricePerDay;
            Bookings.Add(NewBooking);
            _storage.Save(Bookings);

            NewBooking = new Booking();
            OnPropertyChanged(nameof(NewBooking));
        }

        private bool CanBook()
            => !string.IsNullOrWhiteSpace(NewBooking.FullName)
               && !string.IsNullOrWhiteSpace(NewBooking.RoomType)
               && NewBooking.CheckInDate < NewBooking.CheckOutDate;

        private void DeleteBooking()
        {
            if (SelectedBooking == null) return;

            Bookings.Remove(SelectedBooking);
            _storage.Save(Bookings);

            SelectedBooking = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
