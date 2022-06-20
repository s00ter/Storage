using Microsoft.EntityFrameworkCore;
using Storage.Database;
using Storage.Database.Constants;
using Storage.Database.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Storage.Database.Entities.ProductInfos;
using Storage.Database.Entities.Products;
using Storage.Database.Enums;

namespace Storage.ProductWindows
{
    /// <summary>
    /// Логика взаимодействия для ProductInfoWindow.xaml
    /// </summary>
    public partial class ProductInfoWindow : Window
    {
        private Product _currentProduct;
        private readonly StorageContext _context;
        private readonly ObservableCollection<ProductInfo> _productInfos;

        public ProductInfoWindow(Product product, StorageContext context)
        {
            _currentProduct = product;
            _context = context;
            _productInfos = new ObservableCollection<ProductInfo>();

            InitializeComponent();

            ProductDataGrid.ItemsSource = _productInfos;
            Title = "Таблица изменений для " + product.Name;

            UpdateTable();
        }

        private void UpdateTable()
        {
            _productInfos.Clear();

            _context.ProductInfo
                .AsNoTracking()
                .Where(x => x.ProductId == _currentProduct.Id)
                .ToList()
                .ForEach(x => _productInfos.Add(x));
        }

        private void AddComing_Click(object sender, RoutedEventArgs e)
        {
            TryAddProductInfo(true);
        }

        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            TryAddProductInfo(false);
        }

        private void TryAddProductInfo(bool type)
        {
            if (!int.TryParse(ChangeValueTextBox.Text, out var ChangeValue))
            {
                MessageBox.Show("Стоимость должна быть числом");
                return;
            }

            DateTime? date = null;
            if (DateOfChangePicker.SelectedDate is null)
            {
                var datePickerAns = MessageBox.Show("Не выбрана дата. Выбрать сегодня?", "Выбор", MessageBoxButton.YesNo);

                if (datePickerAns == MessageBoxResult.No)
                {
                    return;
                }
                date = DateTime.Now;
            }

            if (!type && _currentProduct.Amount < ChangeValue)
            {
                MessageBox.Show("Приход не может быть зафиксирован");
                return;
            }

            var result = new ProductInfo
            {
                Action = type
                    ? "Приход"
                    : "Расход",
                Amount = ChangeValue,
                ProductId = _currentProduct.Id,
                Date = date == null
                    ? DateOfChangePicker.SelectedDate.Value.ToString("d")
                    : date.Value.ToString("d")
            };

            _context.Add(result);
            _context.SaveChanges();
            _context.Entry(result).State = EntityState.Detached;

            var productToUpdate = _context.Products.FirstOrDefault(x => x.Id == _currentProduct.Id);

            productToUpdate!.Amount += result.Amount * (type ? 1 : -1);

            productToUpdate.Status = productToUpdate.Amount == 0
                ? ProductStatus.Закончилось
                : ProductStatus.Наличие;

            _context.Update(productToUpdate);
            _context.SaveChanges();
            _context.Entry(productToUpdate).State = EntityState.Detached;

            _currentProduct = productToUpdate;

            UpdateTable();
        }
    }
}
