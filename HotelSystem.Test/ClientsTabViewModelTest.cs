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
    public class ClientsTabViewModelTest
    {
        private TestClientRepository clientRepository;
        private TestRoomRepository roomRepository;
        private ClientsTabViewModel ctvm;

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
            clientRepository = new TestClientRepository();
            roomRepository = new TestRoomRepository();
            ctvm = new ClientsTabViewModel(clientRepository, roomRepository);
            roomRepository.CreateDefaultRooms();
            ctvm.PropertyChanged += PropertyChangedHandler;
        }

        [TearDown]
        public void RunAfterEachTest()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed)
            {
                Assert.IsEmpty(propertyChanges, "Unexpected Property Changes to the following Properties:");
            }
        }

        private void CreateDefaultClients()
        {
            clientRepository.Clients.Add(new Client() { Id = 8, FirstName = "Pietje", LastName = "Puk", Birthdate = new DateTime(1914, 11, 24), Account = "PukDynasty", Room = roomRepository.Rooms[2] });
            clientRepository.Clients.Add(new Client() { Id = 9, FirstName = "Agent", LastName = "Langdraad", Birthdate = new DateTime(1903, 01, 15), Account = "AgentenClub", Room = roomRepository.Rooms[0] });
            clientRepository.Clients.Add(new Client() { Id = 10, FirstName = "Bertus", LastName = "Kromspijker", Birthdate = new DateTime(1899, 02, 24), Account = "PukDynasty", Room = roomRepository.Rooms[1] });
        }
        #endregion


        [Test]
        public void TestFilteredClientList()
        {
            // prepare 

            // run

            // validate
        }

        [Test]
        public void TestAddClient()
        {
            // prepare            
            Client testClient = new Client() { Id = 8, FirstName = "Pietje", LastName = "Puk", Birthdate = new DateTime(1914, 11, 24), Account = "PukDynasty", Room = roomRepository.Rooms[0] };
            

            // run
            ctvm.ClientInfo = testClient;

            var add = ctvm.AddClientCommand;
            var result = add.CanExecute(null);

            // validate
            Assert.IsTrue(result);

            // second run
            add.Execute(null);

            // second validate
            Assert.Contains(testClient, clientRepository.Clients);
            AssertPropertyChanged(nameof(ctvm.Clients));
        }

        [Test]
        public void TestAddClientNoClient()
        {
            // prepare 


            // run
            var add = ctvm.AddClientCommand;
            var result = add.CanExecute(null);

            // validate
            Assert.IsFalse(result);

        }

        [Test]
        public void TestAddClientDuplicateClient()
        {
            // prepare 


            // run


            // validate


        }



    }
}
