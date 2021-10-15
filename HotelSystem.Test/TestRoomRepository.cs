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
            throw new System.NotImplementedException();
        }

        public void ChangeRoom(int? roomId, Room updatedInfo)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteRoom(int? roomId)
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