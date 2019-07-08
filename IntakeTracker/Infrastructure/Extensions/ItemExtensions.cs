using IntakeTracker.Entities;

namespace IntakeTracker.Infrastructure.Extensions
{
    public static class ItemExtensions
    {
        public static Item Move(this Item item)
        {
            return new Item
            {
                Name = item.Name.Trim(),
                Calories = item.Calories,
                Category = item.Category.Trim()
            };
        }
    }
}