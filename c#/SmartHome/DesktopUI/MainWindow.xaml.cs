using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainViewModel;

        public MainWindow()
        {
            _mainViewModel = new MainViewModel();
            this.DataContext = _mainViewModel;

            InitializeComponent();
            AddNewLocation.Click += AddNewLocationClick;
            AddNewSwitch.Click += AddNewSwitchClick;
            AddNewLight.Click += AddNewLightClick;

            AssignSwitchToLocation.Click += AssignSwitchToLocationClick;
            AssignSwitchToLight.Click += AssignSwitchToLightClick;

            AssignLightToLocation.Click += AssignLightToLocationClick;

            Generate.Click += GenerateOnClick;

        }

        private void GenerateOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _mainViewModel.GenerateArduinos("filename");
        }

        private void AddNewLocationClick(object sender, RoutedEventArgs e)
        {
            _mainViewModel.CreateNewLocation();
        }

        private void AddNewSwitchClick(object sender, RoutedEventArgs e)
        {
            _mainViewModel.CreateNewSwitch();
        }

        private void AddNewLightClick(object sender, RoutedEventArgs e)
        {
            _mainViewModel.CreateNewLight();
        }

          private void AssignLightToLocationClick(object sender, RoutedEventArgs e)
        {
            _mainViewModel.AssignLight2LocationLight = (string)AssignLight2LocationLight.SelectedItem;
            _mainViewModel.AssignLight2LocationLocation = (string)AssignLight2LocationLocation.SelectedItem;
            _mainViewModel.AssignLightToLocation();
        }


        
        private void AssignSwitchToLocationClick(object sender, RoutedEventArgs e)
        {
            _mainViewModel.AssignSwitchToLocationLocation = (string)AssignSwitchToLocationLocation.SelectedItem;
            _mainViewModel.AssignSwitchToLocationSwitch = (string)AssignSwitchToLocationSwitch.SelectedItem;

            _mainViewModel.AssignSwitchToLocation();
        }

        private void AssignSwitchToLightClick(object sender, RoutedEventArgs e)
        {
            _mainViewModel.AssignSwitchToLightLight = (string)AssignSwitchToLightLight.SelectedItem;
            _mainViewModel.AssignSwitchToLightSwitch = (string)AssignSwitchToLightSwitch.SelectedItem;

            _mainViewModel.AssignSwitchToLight();
        }

    }
}
