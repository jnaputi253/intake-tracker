namespace IntakeTracker.Database.Errors.Resources
{
    public static class ItemErrors
    {
        public static string EmptyItemName { get; } = "The item name cannot be empty";
        public static string EmptyCategoryName { get; } = "The category cannot be empty";

        public static string InvalidCalorieRange { get; } =
            "The calorie range is invalid.  Must be between 0 and 10,000";

        public static string InvalidItem { get; } = "The item is invalid";
        public static string ItemExists { get; } = "The item already exists";
    }
}