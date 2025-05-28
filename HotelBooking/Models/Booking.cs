using System;

namespace HotelBooking.Models
{
    public class Booking
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string FullName { get; set; }
        public string RoomType { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string ImagePath { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
