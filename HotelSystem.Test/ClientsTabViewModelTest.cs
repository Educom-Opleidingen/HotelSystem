using HotelSystem.DataLayer.Models;
using HotelSystem.ViewModel;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace HotelSystem.Test
{
    public class ClientsTabViewModelTest
    {
        private TestClientRepository clientRepository;
        private TestRoomRepository roomRepository;
        private TestStandardDialog standardDialog;
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
            standardDialog = new TestStandardDialog();
            ctvm = new ClientsTabViewModel(clientRepository, roomRepository, standardDialog);
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

        private void DeleteStandardDialogFile()
        {
            File.Delete(standardDialog.Location);
        }

        [Test]
        public void TestFilteredClientList()
        {
            // prepare 
            CreateDefaultClients();
            ctvm.ClientFilter.FirstName = "P";


            // run
            var filter = ctvm.ClientsFilterChangedCommand;
            filter.Execute(null);

            var result = ctvm.FilteredClientList.ToList();

            // validate
            Assert.Contains(clientRepository.Clients[0], result);
            Assert.AreEqual(1, result.Count());
            AssertPropertyChanged(nameof(ctvm.FilteredClientList));

        }

        #region TestAddClient
        [Test]
        public void TestAddClient()
        {
            // prepare
            Client testClient = new Client() { Id = 20, FirstName = "Nina", LastName = "de Ruiter", Birthdate = new DateTime(2012, 04, 04), Account = "BlokkenBouwClub", Room = roomRepository.Rooms[0] };

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
            CreateDefaultClients();

            Client testClient = new Client() { Id = 8, FirstName = "Pietje", LastName = "Puk", Birthdate = new DateTime(1914, 11, 24), Account = "PukDynasty", Room = roomRepository.Rooms[2] };
            var selectedClient = clientRepository.Clients[0];
            ctvm.SelectedClient = selectedClient;
            propertyChanges.Clear();

            // run
            ctvm.ClientInfo = testClient;

            var add = ctvm.AddClientCommand;
            var result = add.CanExecute(null);

            // validate
            Assert.IsFalse(result);



        }

        #endregion

        #region TestChangeClient
        [Test]
        public void TestChangeClient_ChangedDataLastNameAndBirthDate()
        {
            // prepare
            CreateDefaultClients();

            Client newClientInfo = new Client() { Id = 8, FirstName = "Pietje", LastName = "Kup", Birthdate = new DateTime(1914, 04, 24), Account = "PukDynasty", Room = roomRepository.Rooms[0] };
            var selectedClient = clientRepository.Clients[0];
            ctvm.SelectedClient = selectedClient;
            propertyChanges.Clear();

            // run
            ctvm.ClientInfo = newClientInfo;

            var change = ctvm.UpdateClientCommand;
            var result = change.CanExecute(null);

            // validate 
            Assert.IsTrue(result);

            // Second run
            change.Execute(null);

            // Second validate
            Assert.AreEqual(new Client() { Id = 8, FirstName = "Pietje", LastName = "Kup", Birthdate = new DateTime(1914, 04, 24), Account = "PukDynasty", Room = roomRepository.Rooms[0] }, clientRepository.Clients[0]);
            AssertPropertyChanged(nameof(ctvm.Clients));


        }

        [Test]
        public void TestChangeClient_DuplicateData()
        {
            // prepare
            CreateDefaultClients();

            Client newClientInfo = new Client() { Id = 8, FirstName = "Pietje", LastName = "Puk", Birthdate = new DateTime(1914, 11, 24), Account = "PukDynasty", Room = roomRepository.Rooms[2] };
            var selectedClient = clientRepository.Clients[0];

            ctvm.SelectedClient = selectedClient;
            propertyChanges.Clear();

            // run
            ctvm.ClientInfo = newClientInfo;

            var change = ctvm.UpdateClientCommand;
            var result = change.CanExecute(null);

            // validate
            Assert.IsFalse(result);
        }

        [Test]
        public void TestChangeClient_ClientUnknown()
        {
            // prepare
            CreateDefaultClients();

            Client changedClientInfo = new Client() { Id = 50, FirstName = "Jantje", LastName = "Kup", Birthdate = new DateTime(1914, 04, 24), Account = "PukDynasty", Room = roomRepository.Rooms[0] };
            var unSelectedClient = new Client() { Id = 50, FirstName = "Elsje", LastName = "Schaap", Birthdate = new DateTime(1914, 03, 24), Account = "PukDynasty", Room = roomRepository.Rooms[1] };
            ctvm.SelectedClient = unSelectedClient;
            propertyChanges.Clear();

            // run
            ctvm.ClientInfo = changedClientInfo;

            var change = ctvm.UpdateClientCommand;
            var result = change.CanExecute(null);

            // validate
            Assert.IsFalse(result);
        }

        [Test]
        public void TestChangeClient_NoChanges()
        {
            // prepare
            CreateDefaultClients();

            var selectedClient = clientRepository.Clients[0];
            ctvm.SelectedClient = selectedClient;
            propertyChanges.Clear();

            // run
            var change = ctvm.UpdateClientCommand;
            var result = change.CanExecute(null);

            // validate
            Assert.IsFalse(result);


        }
        #endregion

        #region TestExportClients
        [Test]
        public void TestExportClient_FileLocation()
        {
            // prepare
            CreateDefaultClients();
            standardDialog.Location = @"C:\temp\test123456.tsv";
            DeleteStandardDialogFile();

            // run
            var export = ctvm.ExportClientsCommand;
            var result = export.CanExecute(null);

            // validate
            Assert.IsTrue(result);

            // second run
            export.Execute(null);

            // second validate
            FileAssert.Exists(standardDialog.Location);
            string actual = "FirstName\tLastName\tBirthdate\tAccount\tRoomNumber\r\n" +
                            "Pietje\tPuk\t24/11/1914\tPukDynasty\t789\r\n" +
                            "Agent\tLangdraad\t15/01/1903\tAgentenClub\t123\r\n" +
                            "Bertus\tKromspijker\t24/02/1899\tPukDynasty\t456\r\n";

            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(actual);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream actualExpectedStream = new MemoryStream(byteArray);


            FileStream input = new FileStream(standardDialog.Location, FileMode.Open, FileAccess.Read);
            FileAssert.AreEqual(actualExpectedStream, input);
        }

        // c# convert string to stream 

        // compare stream to stream

        #endregion

    }
}
