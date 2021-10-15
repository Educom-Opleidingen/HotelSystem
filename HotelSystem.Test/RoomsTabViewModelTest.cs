using HotelSystem.DataLayer;
using HotelSystem.Model;
using HotelSystem.ViewModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HotelSystem.Test
{
    public class RoomsTabViewModelTest
    {
        readonly List<string> propertyChanges = new List<string>();

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            propertyChanges.Add(e.PropertyName);
        }

        [Test]
        public void TestSetSelectedRoom()
        {
            // prepare
            IRoomRepository repository = new TestRoomRepository();
            RoomsTabViewModel rtvm = new RoomsTabViewModel(repository);
            rtvm.PropertyChanged += PropertyChangedHandler;

            Room testRoom = new Room() { Number = "123", Type = RoomTypes.StandardRoom };

            // run
            rtvm.SelectedRoom = testRoom;
            var result = rtvm.SelectedRoom;

            // validate
            Assert.NotNull(result);
            Assert.AreEqual(testRoom.Number, result.Number);
            Assert.AreEqual(testRoom.Type, result.Type);
            Assert.Contains(nameof(rtvm.SelectedRoom), propertyChanges);
        }
    }
}
