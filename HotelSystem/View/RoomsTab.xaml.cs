using HotelSystem.DataLayer.Models;
using System;
using System.Windows.Controls;

namespace HotelSystem.View
{
    /// <summary>
    /// Interaction logic for RoomsTab.xaml
    /// </summary>
    public partial class RoomsTab : UserControl
    {
        public RoomsTab()
        {
            InitializeComponent();
            RoomTypeCb.ItemsSource = RtCbFilter.ItemsSource = Enum.GetNames(typeof(RoomTypes));
        }
    }
}
