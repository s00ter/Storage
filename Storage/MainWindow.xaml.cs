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
        private readonly StorageContext _context;

        public MainWindow(StorageContext context)
        {
            _context = context;
            InitializeComponent();

            /*
            List<Product> products = db.Products.ToList();

            string str = "";
            foreach (Product product in products)
                str += " Id: " + product.id + " Name: " + product.name + " Cost " + product.cost;

            DBContext.Text = str;
            */
        }

        private void ButtonFind_Click(object sender, RoutedEventArgs e)
        {
            var content_find = TextFind.Text.Trim();

            MessageBox.Show(content_find.Length == 0 ? "Пустое поле ввода" : content_find);
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
