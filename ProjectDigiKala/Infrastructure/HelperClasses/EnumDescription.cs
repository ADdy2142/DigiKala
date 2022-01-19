using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Infrastructure.HelperClasses
{
    public static class EnumDescription
    {
        public static IEnumerable<string> GetDescription(Type type)
        {
            List<string> descriptions = new List<string>();
            var names = Enum.GetNames(type);
            foreach (var name in names)
            {
                var field = type.GetField(name);
                var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                foreach (var attribute in attributes)
                {
                    descriptions.Add(((DescriptionAttribute)attribute).Description);
                }
            }

            return descriptions;
        }
    }
}
