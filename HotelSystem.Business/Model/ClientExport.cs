using System;
using HotelSystem.DataLayer.Models;

namespace HotelSystem.BusinessLayer.Model
{
    internal class ClientExport
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Account { get; set; }
        public string RoomNumber { get; set; }
        public ClientTypes Type { get; set; }
    }
}