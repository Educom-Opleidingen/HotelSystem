using HotelSystem.Model;
using System.Collections.Generic;

namespace HotelSystem.DataLayer
{
    public interface IRoomRepository
    {
        void AddRoom(Room room);
        void ChangeRoom(int? roomId, Room updatedInfo);
        void DeleteRoom(int? roomId);
        IEnumerable<Room> GetAllRooms();
        bool HasRooms();
    }
}