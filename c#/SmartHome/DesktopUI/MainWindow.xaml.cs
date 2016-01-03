using System.Windows;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

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

            AddNewArduinoGroup.Click += AddNewArduinoGroup_Click;
            DeleteArduinoGroup.Click += DeleteArduinoGroupOnClick;

            AssignLocationToArduinoGroup.Click += AssignLocationToArduinoGroupOnClick;
            RemoveLocationFromArduinoGroup.Click += RemoveLocationFromArduinoGroupOnClick;

            Open.Click += OpenOnClick;
            Save.Click += SaveOnClick;
            Generate.Click += GenerateOnClick;
            Print.Click += PrintOnClick;

        }

        private void RemoveLocationFromArduinoGroupOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _mainViewModel.AssignLocationToArduinoGroupsLocation = (string)AssignLocationToArduinoGroupsLocation.SelectedItem;
            _mainViewModel.AssignLocationToArduinoGroupsGroup = (string)AssignLocationToArduinoGroupsGroup.SelectedItem;
            _mainViewModel.RemoveLocationFromArduinoGroup();
            
        }

        private void AssignLocationToArduinoGroupOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _mainViewModel.AssignLocationToArduinoGroupsLocation = (string)AssignLocationToArduinoGroupsLocation.SelectedItem;
            _mainViewModel.AssignLocationToArduinoGroupsGroup = (string)AssignLocationToArduinoGroupsGroup.SelectedItem;
            _mainViewModel.AssignLocationToArduinoGroup();

            RefreshArduinoGroups();
        }

        private void DeleteArduinoGroupOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _mainViewModel.DeleteArduinoGroup();
            RefreshArduinoGroups();
        }

        void AddNewArduinoGroup_Click(object sender, RoutedEventArgs e)
        {
            _mainViewModel.AddArduinoGroup();
            RefreshArduinoGroups();
        }

        private void RefreshArduinoGroups()
        {
            ArdGroups.DataContext = null;
            ArdGroups.DataContext = _mainViewModel;
        }

        private void OpenOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var openFileDialog = new OpenFileDialog {DefaultExt = "smh", Filter = "SmartHome Files|*.smh"};
            var showDialog = openFileDialog.ShowDialog();
            if (((bool) showDialog))
            {
                _mainViewModel.OpenGraph(openFileDialog.FileName);
            }

        }

        private void SaveOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var saveFileDialog = new SaveFileDialog {DefaultExt = "smh", Filter = "SmartHome Files|*.smh"};

            var showDialog = saveFileDialog.ShowDialog();
            if (((bool) showDialog) )
            {
                _mainViewModel.SaveGraph(saveFileDialog.FileName);
            }
  
        }

        private void PrintOnClick(object sender, RoutedEventArgs e)
        {
            var printDialog = new System.Windows.Controls.PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(GraphLayout, "Graph");
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
            var folder = new FolderBrowserDialog();

            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _mainViewModel.GenerateArduinos(folder.SelectedPath);
            }

            System.Windows.Forms.MessageBox.Show(@"Generation finished, please check folder: " + folder.SelectedPath,
                                                           @"Generation finsihed", MessageBoxButtons.OK,
                                                           MessageBoxIcon.Information);

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
