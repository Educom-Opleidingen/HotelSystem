using HotelSystem.DataLayer.Models;
using System;
using System.Windows.Controls;

namespace HotelSystem.View
{
    /// <summary>
    /// Interaction logic for ClientTab.xaml
    /// </summary>
    public partial class ClientsTab : UserControl
    {
        public ClientsTab()
        {
            InitializeComponent();
            ClientTypeCb.ItemsSource = CtCbFilter.ItemsSource = Enum.GetNames(typeof(ClientTypes));
        }

    }
}
