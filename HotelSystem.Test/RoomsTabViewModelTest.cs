using HotelSystem.DataLayer;
using HotelSystem.Model;
using HotelSystem.ViewModel;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
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
            } else if (index > 0)  // Not the first in the list, report the once before it
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
                Assert.IsEmpty(propertyChanges, "Unexpected Property Changes to the folowing Properties:");
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
    }
}
