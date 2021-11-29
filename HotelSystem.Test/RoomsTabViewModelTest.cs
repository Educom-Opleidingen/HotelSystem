using HotelSystem.DataLayer.Models;
using HotelSystem.BusinessLayer.ViewModel;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


namespace HotelSystem.Test
{
    public class RoomsTabViewModelTest
    {
        private TestRoomRepository repository;
        private RoomsTabViewModel rtvm;

        #region PropertyChanges
        private readonly List<string> propertyChanges = new List<string>();

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            propertyChanges.Add(e.PropertyName);
        }

        private void AssertPropertyChanged(string expectedProperty)
        {
            Assert.IsNotEmpty(propertyChanges,
                              string.Format("Expected property change {0} not fired", expectedProperty));
            int index = propertyChanges.IndexOf(expectedProperty);
            propertyChanges.Remove(expectedProperty); // Remove it from the list as it was expected

            if (index < 0) // Not in the list
            {
                Assert.Fail(string.Format("Expected property change {0} not fired", expectedProperty));
            }
            else if (index > 0)  // Not the first in the list, report the once before it
                Assert.IsEmpty(propertyChanges.Take(index),
                               string.Format("Found other properties changed before the property change of {0}",
                                             expectedProperty));
        }
        #endregion

        #region Setup
        [SetUp]
        public void RunBeforeEachTest()
        {
            repository = new TestRoomRepository();
            rtvm = new RoomsTabViewModel(repository);
            rtvm.PropertyChanged += PropertyChangedHandler;
        }

        [TearDown]
        public void RunAfterEachTest()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed)
            {
                Assert.IsEmpty(propertyChanges, "Unexpected Property Changes to the following Properties:");
            }
        }

       
        #endregion

        [Test]
        public void TestSetSelectedRoom()
        {
            // prepare
            Room testRoom = new Room() { Number = "123", Type = RoomTypes.StandardRoom };

            // run
            rtvm.SelectedRoom = testRoom;
            var result = rtvm.SelectedRoom;

            // validate
            Assert.NotNull(result);
            Assert.AreEqual(testRoom.Number, result.Number);
            Assert.AreEqual(testRoom.Type, result.Type);
            AssertPropertyChanged(nameof(rtvm.SelectedRoom));
        }

        [Test]
        public void TestAddRoom_EmptyRoom()
        {
            // prepare 
            // repo already defined higher in this class file. 
            // prop changed already defined higher in this class file. 


            // run
            var add = rtvm.AddRoomCommand;
            var result = add.CanExecute(null);


            // validate
            Assert.IsFalse(result);

        }

        [Test]
        public void TestAddRoom_AddDuplicateRoom()
        {
            // prepare
            Room testRoom = new Room() { Number = "123", Type = RoomTypes.StandardRoom };

            repository.CreateDefaultRooms();
            // run
            rtvm.RoomInfo = testRoom;

            var add = rtvm.AddRoomCommand;
            var result = add.CanExecute(null);

            // validate
            Assert.IsFalse(result);
        }

        [Test]
        public void TestAddRoom_AddNewRoom()
        {
            // prepare
            Room testRoom = new Room() { Number = "007", Type = RoomTypes.StandardRoom };

            repository.CreateDefaultRooms();

            // run
            rtvm.RoomInfo = testRoom;

            var add = rtvm.AddRoomCommand;
            var result = add.CanExecute(null);

            // validate
            Assert.IsTrue(result);

            // second run
            add.Execute(null);

            // second validate
            Assert.Contains(testRoom, repository.Rooms);
            AssertPropertyChanged(nameof(rtvm.Rooms));
        }

        [Test]
        public void TestDeleteRoom_NoRoomsSelected()
        {
            // prepare 
            repository.CreateDefaultRooms();

            // run
            var del = rtvm.DeleteRoomCommand;
            var result = del.CanExecute(null);

            // validate
            Assert.IsFalse(result);

        }

        [Test]
        public void TestDeleteRoom_RoomsSelected()
        {
            // prepare 
            repository.CreateDefaultRooms();

            var selectedRoom = repository.Rooms[0];
            rtvm.SelectedRoom = selectedRoom;
            propertyChanges.Clear();

            // run
            var del = rtvm.DeleteRoomCommand;
            var result = del.CanExecute(null);

            // validate
            Assert.IsTrue(result);


            // second run
            del.Execute(null);

            // second validate
            Assert.IsFalse(repository.Rooms.Contains(selectedRoom));
            AssertPropertyChanged(nameof(rtvm.Rooms));
        }

        [Test]
        public void TestDeleteRoom_RoomDoesNotExist()
        {
            // prepare 
            repository.CreateDefaultRooms();

            var unSelectedRoom = new Room { Id = 999, Number = "666", Type = RoomTypes.PresidentialSuite };
            rtvm.SelectedRoom = unSelectedRoom;
            propertyChanges.Clear();

            // run
            var del = rtvm.DeleteRoomCommand;
            var result = del.CanExecute(null);

            // validate
            Assert.IsFalse(result);
        }


        [Test]
        public void TestChangeRoom_RoomUnknown()
        {
            // prepare 
            repository.CreateDefaultRooms();
            Room changedRoom = new Room() {Id = 999, Number = "666", Type = RoomTypes.StandardRoom };

            var unSelectedRoom = new Room { Id = 999, Number ="656", Type = RoomTypes.JuniorSuite };
            rtvm.SelectedRoom = unSelectedRoom;
            propertyChanges.Clear();
            // run
            rtvm.RoomInfo = changedRoom;

            var update = rtvm.UpdateRoomCommand;
            var result = update.CanExecute(null);

            // validate 
            Assert.IsFalse(result);

        }


        [Test]
        public void TestChangeRoom_ChangedRoom()
        {
            // prepare 
            repository.CreateDefaultRooms();
            Room newRoomInfo = new Room() { Number = "124", Type = RoomTypes.StandardRoom };
            var selectedRoom = repository.Rooms[0];
            rtvm.SelectedRoom = selectedRoom;
            propertyChanges.Clear();

            // run
            rtvm.RoomInfo = newRoomInfo;
            
            var change = rtvm.UpdateRoomCommand;
            var result = change.CanExecute(null);

            // validate
            Assert.IsTrue(result);

            // Second run
            change.Execute(null);

            // Second validate
            Assert.AreEqual(new Room { Number="124", Type = RoomTypes.StandardRoom}, repository.Rooms[0]); // werkt wel, klopt niet als een check. bespreken met Jeroen
            AssertPropertyChanged(nameof(rtvm.Rooms));
        }

        [Test]
        public void TestChangeRoom_ChangedRoom_NumberAndType()
        {
            // prepare 
            repository.CreateDefaultRooms();
            Room newRoomInfo = new Room() { Number = "124", Type = RoomTypes.JuniorSuite };
            var selectedRoom = repository.Rooms[2];
            rtvm.SelectedRoom = selectedRoom;
            propertyChanges.Clear();

            // run
            rtvm.RoomInfo = newRoomInfo;

            var change = rtvm.UpdateRoomCommand;
            var result = change.CanExecute(null);

            // validate
            Assert.IsTrue(result);

            // Second run
            change.Execute(null);

            // Second validate
            Assert.AreEqual(new Room { Number = "124", Type = RoomTypes.JuniorSuite }, repository.Rooms[2]); // werkt wel, klopt niet als een check. bespreken met Jeroen
            AssertPropertyChanged(nameof(rtvm.Rooms));

            Assert.IsFalse(repository.Rooms.Contains(new Room { Number = "789", Type = RoomTypes.PresidentialSuite}));
        }

        [Test]
        public void TestChangeRoom_NoChanges()
        {
            // prepare 
            repository.CreateDefaultRooms();
            var selectedRoom = repository.Rooms[0];
            rtvm.SelectedRoom = selectedRoom;
            propertyChanges.Clear();

            // run
            var change = rtvm.UpdateRoomCommand;
            var result = change.CanExecute(null);

            // validate
            Assert.IsFalse(result);

        }

        [Test]
        public void TestChangeRoom_DuplicateData()
        {
            // prepare 
            repository.CreateDefaultRooms();
            Room newRoomInfo = new Room() { Number = "123", Type = RoomTypes.StandardRoom };
            var selectedRoom = repository.Rooms[0];
            rtvm.SelectedRoom = selectedRoom;
            propertyChanges.Clear();

            // run
            rtvm.RoomInfo = newRoomInfo;

            var change = rtvm.UpdateRoomCommand;
            var result = change.CanExecute(null);

            // validate
            Assert.IsFalse(result);
        }
    }
}
