﻿using System;
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
        private MainViewModel _mainViewModel;

        public MainWindow()
        {
            _mainViewModel = new MainViewModel();
            this.DataContext = _mainViewModel;

            InitializeComponent();
            AddNewLocation.Click += AddNewLocationClick;
            AddNewSwitch.Click += AddNewSwitchClick;
            AddNewLight.Click += AddNewLightClick;

            AssignNewSwitch.Click += AssignNewSwitchClick;
            AssignNewLight.Click += AssignNewLightClick;

        }
        private void AddNewLocationClick(object sender, RoutedEventArgs e)
        {
            //_mainViewModel.CreateNewLocation();
        }

        private void AddNewSwitchClick(object sender, RoutedEventArgs e)
        {
            //_mainViewModel.CreateNewLocation();
        }

        private void AddNewLightClick(object sender, RoutedEventArgs e)
        {
            //_mainViewModel.CreateNewLocation();
        }

        private void AssignNewSwitchClick(object sender, RoutedEventArgs e)
        {
            //_mainViewModel.CreateNewLocation();
        }

        private void AssignNewLightClick(object sender, RoutedEventArgs e)
        {
            //_mainViewModel.CreateNewLocation();
        }

    }
}