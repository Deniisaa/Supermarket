using System.Windows;

namespace Tema3MVVM
{
    public partial class InputDialog : Window
    {
        public InputDialog(string question)
        {
            InitializeComponent();
            lblQuestion.Content = question;
        }

        public string Answer
        {
            get { return txtAnswer.Text; }
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}