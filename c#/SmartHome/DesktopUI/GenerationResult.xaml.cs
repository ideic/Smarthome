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
using System.Windows.Shapes;

namespace DesktopUI
{
    /// <summary>
    /// Interaction logic for GenerationResult.xaml
    /// </summary>
    public partial class GenerationResult : Window
    {
        private MainViewModel _mainviewModel;

        public GenerationResult(MainViewModel mainViewModel)
        {
            this.DataContext = mainViewModel;
            _mainviewModel = mainViewModel;
            InitializeComponent();
        }
    }
}
