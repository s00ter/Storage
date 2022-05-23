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
using Storage.Database;
using Storage.Database.Entities;

namespace Storage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly StorageContext _context;

        public MainWindow(StorageContext context)
        {
            _context = context;
            InitializeComponent();


            var products = _context.Products.ToList();

            var str = products
                .Aggregate("", (current, product) => current + (" Id: " + product.Id + " Name: " + product.Name + " Cost " + product.Cost));

            DBContext.Text = str;
        }

        private void ButtonFind_Click(object sender, RoutedEventArgs e)
        {
            var contentFind = TextFind.Text.Trim();

            MessageBox.Show(contentFind.Length == 0 ? "Пустое поле ввода" : contentFind);
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
