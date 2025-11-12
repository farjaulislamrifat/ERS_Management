using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ERS_Management.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static SelectList ToSelectList<TEnum>(this TEnum enumValue)
            where TEnum : struct, Enum
        {
            var values = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new
                {
                    Value = Convert.ToInt32(e),
                    e,
                    Text = GetDisplayName(e)
                })
                .ToList();

            return new SelectList(values, "Value", "Text", enumValue);
        }

        private static string GetDisplayName<TEnum>(TEnum value)
        {
            var field = typeof(TEnum).GetField(value.ToString());
            var display = field?.GetCustomAttribute<DisplayAttribute>();
            return display?.Name ?? value.ToString();
        }
    }
}
