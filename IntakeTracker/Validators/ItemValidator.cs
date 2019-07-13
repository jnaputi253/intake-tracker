using System;
using System.Collections.Generic;
using IntakeTracker.Database.Errors.Resources;
using IntakeTracker.Entities;
using IntakeTracker.Infrastructure.Extensions;
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
            
            if (item.Name.IsEmpty())
            {
                errorDictionary.Add(nameof(item.Name), ItemErrors.EmptyItemName);
            }

            if (item.Category.IsEmpty())
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