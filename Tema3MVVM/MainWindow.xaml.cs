using System.Windows;
using Tema3MVVM.ViewModels;

namespace Tema3MVVM
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel(this);
        }
    }
}