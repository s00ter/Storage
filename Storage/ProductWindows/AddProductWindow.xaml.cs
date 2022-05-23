using System.Windows;
using Storage.Database.Entities;

namespace Storage.ProductWindows
{
    public partial class AddProductWindow : Window
    {
        public Product Result { get; set; }

        public AddProductWindow()
        {
            InitializeComponent();
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(CostTextBox.Text, out var cost))
            {
                MessageBox.Show("Стоимость должна быть числом");
                return;
            }

            Result = new Product
            {
                Name = NameTextBox.Text,
                Cost = cost
            };

            DialogResult = true;
        }
    }
}