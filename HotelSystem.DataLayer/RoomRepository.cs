using HotelSystem.HotelDbContext;
using HotelSystem.DataLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace HotelSystem.DataLayer
{
    public class RoomRepository : IRoomRepository
    {
        private HotelContext Context { get; }

        public RoomRepository(HotelContext hotelContext)
        {
            Context = hotelContext;
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

        public void ChangeRoom(int roomId, Room updatedInfo)
        {
            Room selectedRoom = GetRoom(roomId);

            selectedRoom.Number = updatedInfo.Number;
            selectedRoom.Type = updatedInfo.Type;

            Context.SaveChanges();
        }

        public void DeleteRoom(int roomId)
        {
            Room selectedRoom = GetRoom(roomId);
            Context.Remove(selectedRoom);
            Context.SaveChanges();
        }

        private Room GetRoom(int roomId)
        {
            Room SelectedRoom = Context.Rooms.Find(roomId);

            if (SelectedRoom == null)
            {
                throw new DataLayerException(string.Format("Room with id {0} not found", roomId));
            }

            return SelectedRoom;
        }
    }
}
