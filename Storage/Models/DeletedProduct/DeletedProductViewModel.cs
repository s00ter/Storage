using System;

namespace Storage.Models.DeletedProduct
{
    public class DeletedProductViewModel : Database.Entities.Products.DeletedProduct
    {
        public string DateMark => GetFormattedDays();

        private string GetFormattedDays()
        {
            var span = DateTime.Now - CreatedAt.Date;

            return span.Days == 0
                ? "Меньше дня назад"
                : $"{span.Days} дней назад";
        }
    }
}