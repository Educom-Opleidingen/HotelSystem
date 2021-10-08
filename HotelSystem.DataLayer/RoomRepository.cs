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

        public void AddRoom(Room room)
        {
            Context.Rooms.Add(room);
            Context.SaveChanges();
        }

        public void ChangeRoom(int? roomId, Room updatedInfo)
        {
            Room selectedRoom = GetRoom(roomId);

            selectedRoom.Number = updatedInfo.Number;
            selectedRoom.Type = updatedInfo.Type;

            Context.SaveChanges();
        }

        public void DeleteRoom(int? roomId)
        {
            Room selectedRoom = GetRoom(roomId);
            Context.Remove(selectedRoom);
            Context.SaveChanges();
        }

        private Room GetRoom(int? roomId)
        {
            if (!roomId.HasValue)
            {
                throw new DataLayerException("No room selected");
            }
            Room SelectedRoom = Context.Rooms.Find(roomId.Value);

            if (SelectedRoom == null)
            {
                throw new DataLayerException(string.Format("Room with id {0} not found", roomId.Value));
            }

            return SelectedRoom;
        }
    }
}
