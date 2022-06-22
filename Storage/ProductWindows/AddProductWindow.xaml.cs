using Storage.Database.Entities;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using Storage.Database.Entities.Products;
using Storage.Database.Enums;

namespace Storage.ProductWindows
{
    public partial class AddProductWindow
    {
        private readonly int _currentId;
        private const string TempAddPath = "tempAdd.Json";

        public Product Result { get; set; }


        public AddProductWindow()
        {
            InitializeComponent();

            if (File.Exists(TempAddPath))
            {
                var product = JsonConvert.DeserializeObject<Product>(File.ReadAllText(TempAddPath));

                if (product is not null)
                {
                    NameTextBox.Text = product.Name;
                    CostTextBox.Text = product.Cost.ToString("f2");

                    ComingPicker.SelectedDate = product.Coming == null
                        ? null
                        : DateTime.Parse(product.Coming);

                    AmountTextBox.Text = product.Amount.ToString();
                    VendorCodeTextBox.Text = product.VendorCode;
                    InfoTextBox.Text = product.Info;
                    ProviderTextBox.Text = product.Provider;
                    _currentId = product.Id;

                    InitializeProductType(product.ProductType);
                    InitializeProductOwner(product.ProductOwner);
                    InitializeDimensionType(DimensionType.Штуки);
                }
                else
                {
                    InitializeProductOwner(ProductOwner.Простор);
                    InitializeProductType(ProductType.Расходник);
                    InitializeDimensionType(DimensionType.Штуки);
                }
            }
            else
            {
                InitializeProductOwner(ProductOwner.Простор);
                InitializeProductType(ProductType.Расходник);
                InitializeDimensionType(DimensionType.Штуки);
            }

            Closing += (_, _) =>
            {
                if (!double.TryParse(CostTextBox.Text, out var cost))
                {
                    cost = 0;
                }

                if (!int.TryParse(AmountTextBox.Text, out var amount))
                {
                    amount = 0;
                }

                var tempObject = new Product
                {
                    Name = NameTextBox.Text,
                    Cost = cost,
                    Coming = ComingPicker.SelectedDate.HasValue ? ComingPicker.SelectedDate.Value.ToString("d") : null,
                    Amount = amount,
                    DimensionType = (DimensionType)DimensionTypeTextBox.SelectedItem,
                    VendorCode = VendorCodeTextBox.Text,
                    Status =  ProductStatus.Наличие,
                    Info = InfoTextBox.Text,
                    Provider = ProviderTextBox.Text,
                    ProductType = (ProductType)ProductTypeComboBox.SelectedItem,
                    ProductOwner = (ProductOwner)ProductOwnerComboBox.SelectedItem
                };

                var serializedObject = JsonConvert.SerializeObject(tempObject);
                File.WriteAllText(TempAddPath, serializedObject);
            };
        }

        public AddProductWindow(Product product)
        {
            InitializeComponent();

            NameTextBox.Text = product.Name;
            CostTextBox.Text = product.Cost.ToString("f2");

            ComingPicker.SelectedDate = DateTime.Parse(product.Coming);

            AmountTextBox.Text = product.Amount.ToString();
            VendorCodeTextBox.Text = product.VendorCode;
            InfoTextBox.Text = product.Info;
            ProviderTextBox.Text = product.Provider;
            _currentId = product.Id;

            InitializeDimensionType(product.DimensionType);
            InitializeProductType(product.ProductType);
            InitializeProductOwner(product.ProductOwner);
        }

        private void InitializeDimensionType(DimensionType dimensionType)
        {
            var items = Enum.GetValues<DimensionType>();
            DimensionTypeTextBox.ItemsSource = items;
            DimensionTypeTextBox.SelectedItem = items.First(x => x == dimensionType);
        }

        private void InitializeProductType(ProductType productType)
        {
            var items = Enum.GetValues<ProductType>();
            ProductTypeComboBox.ItemsSource = items;
            ProductTypeComboBox.SelectedItem = items.First(x => x == productType);
        }

        private void InitializeProductOwner(ProductOwner productOwner)
        {
            var items = Enum.GetValues<ProductOwner>();
            ProductOwnerComboBox.ItemsSource = items;
            ProductOwnerComboBox.SelectedItem = items.First(x => x == productOwner);
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(CostTextBox.Text, out var cost))
            {
                MessageBox.Show("Стоимость должна быть числом");
                return;
            }

            DateTime? date = null;
            if (ComingPicker.SelectedDate is null)
            {
                var datePickerAns = MessageBox.Show("Не выбрана дата. Выбрать сегодня?", "Выбор", MessageBoxButton.YesNo);

                if (datePickerAns == MessageBoxResult.No)
                {
                    return;
                }
                date = DateTime.Now;
            }

            if (!int.TryParse(AmountTextBox.Text, out var amount))
            {
                MessageBox.Show("Количество должно быть числом");
                return;
            }

            Result = new Product
            {
                Id = _currentId,
                Name = NameTextBox.Text,
                Cost = cost,
                Coming = date == null
                    ? ComingPicker.SelectedDate.Value.ToString("d")
                    : date.Value.ToString("d"),
                Amount = amount,
                DimensionType = (DimensionType)DimensionTypeTextBox.SelectedItem,
                VendorCode = VendorCodeTextBox.Text,
                Status = amount == 0
                    ? ProductStatus.Закончилось
                    : ProductStatus.Наличие,
                Info = InfoTextBox.Text,
                Provider = ProviderTextBox.Text,
                ProductType = (ProductType)ProductTypeComboBox.SelectedItem,
                ProductOwner = (ProductOwner)ProductOwnerComboBox.SelectedItem
            };

            DialogResult = true;

            if (_currentId == 0 && File.Exists(TempAddPath))
            {
                File.Delete(TempAddPath);
            }
        }
    }
}