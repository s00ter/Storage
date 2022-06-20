using Microsoft.EntityFrameworkCore;
using Storage.Database;
using Storage.Database.Entities;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Storage.Database.Entities.ProductInfos;

namespace Storage.ProductWindows
{
    /// <summary>
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        private readonly StorageContext _context;
        private readonly ObservableCollection<ReportModel> _reports = new();

        public ReportWindow(StorageContext context)
        {
            InitializeComponent();

            _context = context;

            ProductDataGrid.ItemsSource = _reports;
        }

        private void CountButton_Click(object sender, RoutedEventArgs e)
        {
            if (FirstDataPicker.SelectedDate == null || SecondDataPicker.SelectedDate == null)
            {
                MessageBox.Show("Необходимо выбрать промежуток");
                return;
            }

            if (FirstDataPicker.SelectedDate > SecondDataPicker.SelectedDate)
            {
                MessageBox.Show("Неверно выбранный промежуток");
                return;
            }

            var firstValue = FirstDataPicker.SelectedDate.Value;
            var secondValue = SecondDataPicker.SelectedDate.Value;

            var productInfosToSum = _context.ProductInfo
                .AsNoTracking()
                .Include(x => x.Product)
                .ToList()
                .Where(x => DateTime.Parse(x.Date) >= firstValue && DateTime.Parse(x.Date) <= secondValue)
                .GroupBy(x => x.ProductId)
                .Select(x => GetReportFromProductInfo(x.ToList()))
                .ToList();

            if (productInfosToSum.Count == 0)
            {
                MessageBox.Show("Ничего не найдено");
                return;
            }

            var result = new List<ReportModel>
            {
                new ReportModel
                {
                    ProductName = "Общая",
                    ComingCount = productInfosToSum.Sum(x => x.ComingCount),
                    ExpenseCount = productInfosToSum.Sum(x => x.ExpenseCount),
                    ComingSum = productInfosToSum.Sum(x => x.ComingSum),
                    ExpenseSum = productInfosToSum.Sum(x => x.ExpenseSum)
                }
            };

            result.AddRange(productInfosToSum);

            _reports.Clear();
            result.ForEach(x => _reports.Add(x));
        }

        private static ReportModel GetReportFromProductInfo(List<ProductInfo> productInfos)
        {
            var product = productInfos.First().Product;

            var comingInfos = productInfos.Where(z => z.Action == "Приход");
            var comingCount = comingInfos.Any()
                ? comingInfos.Sum(x => x.Amount)
                : 0;

            var expenseInfos = productInfos.Where(z => z.Action == "Расход");
            var expenseCount = expenseInfos.Any()
                ? expenseInfos.Sum(x => x.Amount)
                : 0;

            var comingSum = product.Cost * comingCount;
            var expenseSum = product.Cost * expenseCount;

            return new ReportModel
            {
                ProductName = product.Name,
                ComingCount = comingCount,
                ComingSum = comingSum,
                ExpenseCount = expenseCount,
                ExpenseSum = expenseSum
            };
        }
    }
}
