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

namespace Storage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            
        }

        private void ButtonFind_Click(object sender, RoutedEventArgs e)
        {
            string content_find = TextFind.Text.Trim();

            if (content_find.Length == 0)
            {
                MessageBox.Show("Пустое поле ввода");
                
            }
            else
            {
                MessageBox.Show(content_find);
            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
