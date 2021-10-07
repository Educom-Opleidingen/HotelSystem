using HotelSystem.HotelDbContext;
using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelSystem.DataLayer
{
    public class RoomRepository
    {
        public HotelContext Context { get; }

        public RoomRepository(HotelContext hotelContext)
        {
            Context = hotelContext;
        }
        public bool HasRooms()
        {
            return Context.Rooms.Any();
        }

        public IEnumerable<Room> GetAllRooms()
        {
            return Context.Rooms.ToList();
        }

    }
}
