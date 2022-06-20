using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Storage.Database;
using Storage.Database.Entities.ProductInfos;
using Storage.Database.Entities.Products;
using Storage.Models.DeletedProduct;
using Storage.Models.Settings;

namespace Storage.SettingsWindows
{
    public partial class SettingsWindow : Window
    {
        private readonly StorageContext _context;
        private const string SettingsPath = "appsettings.json";
        private readonly ObservableCollection<DeletedProductViewModel> _deletedProducts = new();

        public SettingsWindow(StorageContext context)
        {
            _context = context;

            InitializeComponent();

            BasketDataGrid.ItemsSource = _deletedProducts;

            UpdateNotificationSettings();
            UpdateBasket();
        }

        private void UpdateBasket()
        {
            var basketItems = _context.ProductBasket
                .AsNoTracking()
                .OrderBy(x => x.CreatedAt)
                .ToList()
                .Select(x => new DeletedProductViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Cost = x.Cost,
                    Coming = x.Coming,
                    Amount = x.Amount,
                    DimensionType = x.DimensionType,
                    VendorCode = x.VendorCode,
                    Status = x.Status,
                    Info = x.Info,
                    Provider = x.Provider,
                    ProductType = x.ProductType,
                    ProductOwner = x.ProductOwner,
                    CreatedAt = x.CreatedAt
                })
                .ToList();

            _deletedProducts.Clear();
            basketItems.ForEach(x => _deletedProducts.Add(x));
        }

        private void UpdateNotificationSettings()
        {
            var settings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(SettingsPath))!;

            AmountTextBox.Text = settings.DangerZones.Resources.Amount.ToString();
            LengthTextBox.Text = settings.DangerZones.Resources.Length.ToString();
            PieceTextBox.Text = settings.DangerZones.Resources.Piece.ToString();
        }

        private void SaveNotificationButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(AmountTextBox.Text, out var amount))
            {
                MessageBox.Show("Объем должен быть числом");
                return;
            }

            if (!int.TryParse(LengthTextBox.Text, out var length))
            {
                MessageBox.Show("Длина должна быть числом");
                return;
            }

            if (!int.TryParse(PieceTextBox.Text, out var pieces))
            {
                MessageBox.Show("Штуки должны быть выражены числом");
                return;
            }

            var current = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(SettingsPath))!;

            current.DangerZones.Resources.Amount = amount;
            current.DangerZones.Resources.Length = length;
            current.DangerZones.Resources.Piece = pieces;

            var serialized = JsonConvert.SerializeObject(current, Formatting.Indented);
            File.WriteAllText(SettingsPath, serialized);

            MessageBox.Show("Параметры были успешно сохранены");
        }

        private void ClearButton_OnClick(object sender, RoutedEventArgs e)
        {
            var productInfosToDelete = _context.ProductInfoBasket
                .AsNoTracking()
                .ToList();

            _context.ProductInfoBasket.RemoveRange(productInfosToDelete);
            _context.SaveChanges();

            var productsToDelete = _context.ProductBasket
                .AsNoTracking()
                .ToList();

            _context.ProductBasket.RemoveRange(productsToDelete);
            _context.SaveChanges();

            UpdateBasket();

            MessageBox.Show("Удалено успешно");
        }

        private void RestoreButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (BasketDataGrid.SelectedItem is DeletedProductViewModel model)
            {
                var itemToRestore = _context.ProductBasket
                    .AsNoTracking()
                    .First(x => x.Id == model.Id);

                var itemsToRestore = _context.ProductInfoBasket
                    .AsNoTracking()
                    .Where(x => x.DeletedProductId == x.Id)
                    .ToList();

                var product = new Product
                {
                    Name = itemToRestore.Name,
                    Cost = itemToRestore.Cost,
                    Coming = itemToRestore.Coming,
                    Amount = itemToRestore.Amount,
                    DimensionType = itemToRestore.DimensionType,
                    VendorCode = itemToRestore.VendorCode,
                    Status = itemToRestore.Status,
                    Info = itemToRestore.Info,
                    Provider = itemToRestore.Provider,
                    ProductType = itemToRestore.ProductType,
                    ProductOwner = itemToRestore.ProductOwner,
                    ProductInfos = itemsToRestore
                        .Select(x => new ProductInfo
                        {
                            Date = x.Date,
                            Amount = x.Amount,
                            Action = x.Action
                        })
                        .ToList()
                };

                _context.ProductInfoBasket.RemoveRange(itemsToRestore);
                _context.SaveChanges();

                _context.ProductBasket.Remove(itemToRestore);
                _context.SaveChanges();

                _context.Products.Add(product);
                _context.SaveChanges();
                _context.Entry(product).State = EntityState.Detached;

                UpdateBasket();
            }
            else
            {
                MessageBox.Show("Необходимо выбрать элемент");
            }
        }
    }
}