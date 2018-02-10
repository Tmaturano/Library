using System;

namespace Library.Application.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static int GetCurrentAge(this DateTimeOffset dateTimeOffset,
            DateTimeOffset? dateOfDeath)
        {
            var currentDate = DateTime.UtcNow;

            if (dateOfDeath.HasValue)
                currentDate = dateOfDeath.Value.UtcDateTime;

            var age = currentDate.Year - dateTimeOffset.Year;

            if (currentDate < dateTimeOffset.AddYears(age))
            {
                age--;
            }

            return age;
        }
    }
}
