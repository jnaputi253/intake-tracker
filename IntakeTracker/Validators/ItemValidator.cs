using System;
using System.Collections.Generic;
using IntakeTracker.Database.Errors.Resources;
using IntakeTracker.Entities;
using JetBrains.Annotations;

namespace IntakeTracker.Validators
{
    public static class ItemValidator
    {
        public static Dictionary<string, string> Validate([NotNull] Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "The item cannot be null");
            }
            var errorDictionary = new Dictionary<string, string>();
            
            if (string.IsNullOrEmpty(item.Name))
            {
                errorDictionary.Add(nameof(item.Name), ItemErrors.EmptyItemName);
            }

            if (string.IsNullOrEmpty(item.Category.Trim()))
            {
                errorDictionary.Add(nameof(item.Category), ItemErrors.EmptyCategoryName);
            }

            if (item.Calories < 0 || item.Calories > 10_000)
            {
                errorDictionary.Add(nameof(item.Calories), ItemErrors.InvalidCalorieRange);
            }

            return errorDictionary;
        }
    }
}