using Storage.Database.Entities;
using System;
using System.Windows;

namespace Storage.ProductWindows
{
    public partial class AddProductWindow : Window
    {
        private readonly int _currentId = 0;

        public Product Result { get; set; }


        public AddProductWindow()
        {
            InitializeComponent();
        }

        public AddProductWindow(Product product)
        {
            InitializeComponent();

            NameTextBox.Text = product.Name;
            CostTextBox.Text = product.Cost.ToString("f2");

            ComingPicker.SelectedDate = DateTime.Parse(product.Coming);

            AmountTextBox.Text = product.Amount.ToString();
            DimensionTypeTextBox.Text = product.DimensionType;
            VendorCodeTextBox.Text = product.VendorCode;
            StatusComboBox.Text = product.Status;
            InfoTextBox.Text = product.Info;
            ClientTextBox.Text = product.Client;
            ProviderTextBox.Text = product.Provider;
            ProductTypeComboBox.Text = product.ProductType;
            _currentId = product.Id;
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
                DimensionType = DimensionTypeTextBox.Text,
                VendorCode = VendorCodeTextBox.Text,
                Status = StatusComboBox.Text,
                Info = InfoTextBox.Text,
                Client = ClientTextBox.Text,
                Provider = ProviderTextBox.Text,
                ProductType = ProductTypeComboBox.Text


            };

            DialogResult = true;
        }
    }
}