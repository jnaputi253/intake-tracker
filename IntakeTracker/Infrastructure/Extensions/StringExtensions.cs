namespace IntakeTracker.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string word)
        {
            if (word == null)
            {
                return true;
            }

            return word.Trim().Length == 0;
        }
    }
}