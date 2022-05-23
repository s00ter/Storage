using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
using MaterialDesignThemes.Wpf;
using Storage.Database;
using Storage.Database.Entities;
using Storage.ProductWindows;

namespace Storage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly StorageContext _context;

        private readonly ObservableCollection<Product> _products;

        public MainWindow(StorageContext context)
        {
            _context = context;
            InitializeComponent();

            _products = new ObservableCollection<Product>();
            ProductDataGrid.ItemsSource = _products;

            UpdateProductTable();
        }

        private void UpdateProductTable()
        {
            _products.Clear();

            _context.Products.ToList().ForEach(product => _products.Add(product));
        }

        private void ButtonFind_Click(object sender, RoutedEventArgs e)
        {
            var contentFind = TextFind.Text.Trim();

            MessageBox.Show(contentFind.Length == 0 ? "Пустое поле ввода" : contentFind);
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddProductWindow();
            if (addWindow.ShowDialog() != true)
            {
                return;
            }

            _context.Products.Add(addWindow.Result);
            _context.SaveChanges();
            UpdateProductTable();
        }
    }
}
