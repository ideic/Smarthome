﻿using System.Windows;
using Microsoft.Win32;

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
            DataContext = _mainViewModel;

            InitializeComponent();
            AddNewLocation.Click += AddNewLocationClick;
            DeleteLocation.Click += DeleteLocationOnClick;
            
            AddNewSwitch.Click += AddNewSwitchClick;
            DeleteSwitch.Click += DeleteSwitchOnClick;
            AddNewLight.Click += AddNewLightClick;
            DeleteLight.Click += DeleteLightOnClick;

            AssignSwitchToLocation.Click += AssignSwitchToLocationClick;
            RemoveSwitchFromLocation.Click += RemoveSwitchFromLocationOnClick;

            AssignSwitchToLight.Click += AssignSwitchToLightClick;
            RemoveLightFromLocation.Click += RemoveLightFromLocationOnClick;

            AssignLightToLocation.Click += AssignLightToLocationClick;
            RemoveSwitchFromLight.Click += RemoveSwitchFromLightOnClick;

            Save.Click += SaveOnClick;
            Generate.Click += GenerateOnClick;

        }

        private void SaveOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "smh";
            saveFileDialog.Filter = "SmartHome Files|*.smh";

            var showDialog = saveFileDialog.ShowDialog();
            if ((showDialog?? false) )
            {
                _mainViewModel.SaveGraph(saveFileDialog.FileName);
            }
  
        }

        private void RemoveSwitchFromLightOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _mainViewModel.RemoveSwitchFromLight();
        }

        private void RemoveLightFromLocationOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _mainViewModel.RemoveLightFromLocation();
        }

        private void RemoveSwitchFromLocationOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _mainViewModel.RemoveSwitchFromLocation();
        }

        private void DeleteSwitchOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _mainViewModel.DeleteSwitch();
        }

        private void DeleteLightOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _mainViewModel.DeleteLight();
        }


        private void GenerateOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _mainViewModel.GenerateArduinos("filename");
        }

        private void AddNewLocationClick(object sender, RoutedEventArgs e)
        {
            _mainViewModel.CreateNewLocation();
        }

        private void DeleteLocationOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _mainViewModel.DeleteLocation();
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
