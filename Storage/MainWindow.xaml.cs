﻿using Microsoft.EntityFrameworkCore;
using Storage.Database;
using Storage.Database.Constants;
using Storage.Database.Entities;
using Storage.ProductWindows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

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

            ProductDataGrid.MouseDoubleClick += ProductDataGrid_MouseDoubleClick;
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

        private void UpdateProductTable(IEnumerable<Product> initValue = null)
        {
            _products.Clear();

            if (initValue != null)
            {
                initValue
                    .ToList()
                    .ForEach(product => _products.Add(product));
                return;
            }

            _context.Products
                .AsNoTracking()
                .ToList()
                .ForEach(product => _products.Add(product));
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
            if (ProductDataGrid.SelectedItem is Product product)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                UpdateProductTable();
            }
        }


        private void ChangeData_Click(object sender, RoutedEventArgs e)
        {
            if (ProductDataGrid.SelectedItem is Product product)
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

            return true;
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
                .Where(x => CheckAvailable(x));

            if (contentFind.Length is not 0)
            {
                result = result
                    .Where(x => x.Name.ToLower().Contains(contentFind));
            }

            UpdateProductTable(result);
        }
    }
}
