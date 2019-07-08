namespace IntakeTracker.Database.Errors
{
    public static class DbErrors
    {
        public static string ServerError { get; } =
            "There was an error on our side.  We shall resolve it as soon as we can!";

        public static string ResourceExists { get; } = "The resource already exists";
    }
}