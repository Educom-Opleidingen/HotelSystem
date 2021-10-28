﻿using HotelSystem.DataLayer;
using HotelSystem.Model;
using System.Collections.Generic;
using System.Linq;

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
            Room selectedRoom = Rooms.Find(r => r.Id == roomId);
            updatedInfo.Id = selectedRoom.Id;
            selectedRoom.Number = updatedInfo.Number;
            selectedRoom.Type = updatedInfo.Type;

            Rooms.Remove(selectedRoom);
            Rooms.Add(updatedInfo);
            
        }
        public IEnumerable<Room> GetAllRooms()
        {
            return Rooms;
        }

        public bool HasRooms()
        {
            if (Rooms.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}