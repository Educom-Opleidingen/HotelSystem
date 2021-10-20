using HotelSystem.DataLayer;
using HotelSystem.Model;
using System.Collections.Generic;

namespace HotelSystem.Test
{
    internal class TestRoomRepository : IRoomRepository
    {
        public List<Room> Rooms { get; } = new List<Room>();

        public void AddRoom(Room room)
        {
            Rooms.Add(room);
        }
        public void DeleteRoom(int? roomId)
        {
            var foundRoom = Rooms.Find(r => r.Id == roomId);
            Rooms.Remove(foundRoom);
        }
        public void ChangeRoom(int? roomId, Room updatedInfo)
        {
            throw new System.NotImplementedException();
        }
        public IEnumerable<Room> GetAllRooms()
        {
            return Rooms;
        }

        public bool HasRooms()
        {
            throw new System.NotImplementedException();
        }
    }
}