using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Snowball.Infrastructure.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum e)
        {
            var type = e.GetType();
            var field = type.GetField(e.ToString());
            var attr = (DisplayAttribute)field.GetCustomAttribute(typeof(DisplayAttribute));
            
            return attr == null ? e.ToString() : attr.Name;
        }
    }
}
