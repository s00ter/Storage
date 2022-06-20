using System.Collections.Generic;
using Storage.Database.Entities.Products;

namespace Storage.NotificationWindows
{
    public partial class ProductsWindow
    {
        public ProductsWindow(List<Product> initValue)
        {
            InitializeComponent();

            AllDataGrid.ItemsSource = initValue;
        }
    }
}