using Microsoft.EntityFrameworkCore;
using Storage.Database;
using Storage.Database.Entities;
using Storage.ProductWindows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Storage.Database.Enums;

namespace Storage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly StorageContext _context;
        private readonly ObservableCollection<Product> _alls;
        private readonly ObservableCollection<Product> _products;
        private readonly ObservableCollection<Product> _mains;
        private readonly ObservableCollection<Product> _recources;

        public MainWindow(StorageContext context)
        {
            _context = context;
            InitializeComponent();

            _alls = new ObservableCollection<Product>();
            AllDataGrid.ItemsSource = _alls;

            _products = new ObservableCollection<Product>();
            ProductDataGrid.ItemsSource = _products;

            _mains = new ObservableCollection<Product>();
            MainDataGrid.ItemsSource = _mains;

            _recources = new ObservableCollection<Product>();
            ResourceDataGrid.ItemsSource = _recources;

            UpdateProductTable();

            ProductDataGrid.MouseDoubleClick += ProductDataGrid_MouseDoubleClick;

            AlenSearchCheckBox.Checked += (_, _) =>
            {
                UpdateProductTable(_alls.Where(x => x.ProductOwner == ProductOwner.АленСтрой).ToList());
            };

            AlenSearchCheckBox.Unchecked += (_, _) =>
            {
                UpdateProductTable(_alls.Where(x => x.ProductOwner != ProductOwner.АленСтрой).ToList());
            };

            ProstoreSearchCheckBox.Checked += (_, _) =>
            {
                UpdateProductTable(_alls.Where(x => x.ProductOwner == ProductOwner.Простор).ToList());
            };

            ProstoreSearchCheckBox.Unchecked += (_, _) =>
            {
                UpdateProductTable(_alls.Where(x => x.ProductOwner != ProductOwner.Простор).ToList());
            };
        }

        private void ProductDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ProductDataGrid.SelectedItem is Product product)
            {
                var infoWindow = new ProductInfoWindow(product, _context);
                infoWindow.ShowDialog();
                TextFind_TextChanged(default, default);
            }
        }

        private void UpdateProductTable(List<Product> initValue = null)
        {
            _products.Clear();
            _alls.Clear();
            _mains.Clear();
            _recources.Clear();

            initValue ??= _context.Products
                .AsNoTracking()
                .ToList();

            if (initValue.Count == 0)
            {
                return;
            }

            initValue
                .ForEach(product => _alls.Add(product));

            initValue
                .Where(product => product.ProductType == ProductType.Расходник)
                .ToList()
                .ForEach(product => _recources.Add(product));

            initValue
                .Where(product => product.ProductType == ProductType.Продукт)
                .ToList()
                .ForEach(product => _products.Add(product));

            initValue
                .Where(product => product.ProductType == ProductType.Основной)
                .ToList()
                .ForEach(product => _mains.Add(product));
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
            _context.Entry(addWindow.Result).State = EntityState.Detached;

            if (addWindow.Result.Amount is not 0)
            {
                var productInfo = new ProductInfo
                {
                    Amount = addWindow.Result.Amount,
                    ProductId = addWindow.Result.Id,
                    Date = addWindow.Result.Coming,
                    Action = "Приход"
                };

                _context.ProductInfo.Add(productInfo);
                _context.SaveChanges();
                _context.Entry(productInfo).State = EntityState.Detached;
            }

            UpdateProductTable();
        }

        private void DeleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = DataGridsTabControl.SelectedIndex switch
            {
                0 => AllDataGrid,
                1 => ProductDataGrid,
                2 => MainDataGrid,
                3 => ResourceDataGrid,
                _ => null
            };

            if (selected?.SelectedItem is Product product)
            {
                if (MessageBox.Show("Вы действительно хотите удалить?", string.Empty, MessageBoxButton.YesNo) ==
                    MessageBoxResult.No)
                {
                    return;
                }
                _context.Products.Remove(product);
                _context.SaveChanges();
                TextFind_TextChanged(default, default);
            }
        }


        private void ChangeData_Click(object sender, RoutedEventArgs e)
        {
            var selected = DataGridsTabControl.SelectedIndex switch
            {
                0 => AllDataGrid,
                1 => ProductDataGrid,
                2 => MainDataGrid,
                3 => ResourceDataGrid,
                _ => null
            };

            if (selected?.SelectedItem is Product product)
            {
                var addWindow = new AddProductWindow(product);
                if (addWindow.ShowDialog() != true)
                {
                    return;
                }

                _context.Products.Update(addWindow.Result);
                _context.SaveChanges();
                _context.Entry(addWindow.Result).State = EntityState.Detached;
                UpdateProductTable();
            }
        }

        private bool CheckAvailable(Product product)
        {
            var result = AlenSearchCheckBox.IsChecked == true && product.ProductOwner == ProductOwner.АленСтрой;
            result = result || ProstoreSearchCheckBox.IsChecked == true && product.ProductOwner == ProductOwner.Простор;

            return result;
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ReportWindow(_context);
            window.ShowDialog();
        }

        private void TextFind_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var contentFind = TextFind.Text.Trim().ToLower();

            var result = _context.Products
                .AsNoTracking()
                .ToList()
                .Where(CheckAvailable)
                .ToList();

            if (contentFind.Length is not 0)
            {
                result = result
                    .Where(x => x.Name.ToLower().Contains(contentFind))
                    .ToList();
            }

            UpdateProductTable(result);
        }
    }
}
