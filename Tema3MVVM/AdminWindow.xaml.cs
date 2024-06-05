using System.Windows;
using Tema3MVVM.ViewModels;

namespace Tema3MVVM
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            DataContext = new AdminViewModel();
        }

        private void TableSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}