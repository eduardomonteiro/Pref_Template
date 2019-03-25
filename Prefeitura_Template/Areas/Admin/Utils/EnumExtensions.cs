using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using Prefeitura_Template.Models;
using Prefeitura_Template.General;

namespace Prefeitura_Template.Areas.Admin.Utils
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            return GetEnumDisplayName(value.GetType(), value);
        }

        public static IEnumerable<string> GetEnumDisplayNames(Type type)
        {
            if (!type.IsEnum) throw new ArgumentException(String.Format("Type '{0}' is not Enum", type));

            foreach (var item in Enum.GetNames(type))
            {
                var member = type.GetMember(item).First();
                var attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);
                if (attributes.Length == 0)
                {
                    yield return member.Name;
                }
                else
                {
                    yield return ((DisplayAttribute)attributes[0]).GetName();
                }
            }
        }

        public static string GetEnumDisplayName(Type type, object value)
        {
            if (value == null)
            {
                return "";
            }
            if (!type.IsEnum) throw new ArgumentException(String.Format("Type '{0}' is not Enum", type));

            var memberName = Enum.GetName(type, value);
            if (string.IsNullOrEmpty(memberName))
            {
                return "";
            }

            var members = type.GetMember(memberName);
            if (members.Length == 0) throw new ArgumentException(String.Format("Member '{0}' not found in type '{1}'", value, type.Name));

            var member = members[0];
            var attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes.Length == 0)
            {
                return memberName;
            }

            var attribute = (DisplayAttribute)attributes[0];
            return attribute.GetName();
        }

        public static IEnumerable GetItems(Type type, int? defaultValue = null)
        {
            if (!type.IsEnum) throw new ArgumentException(String.Format("Type '{0}' is not Enum", type));

            var names = Enum.GetNames(type);
            var values = Enum.GetValues(type);
            for (var i = 0; i < values.Length; i++)
            {
                var member = type.GetMember(names[i]).First();
                var ignored = member.GetCustomAttributes(typeof(IgnoredAttribute), false);
                if (ignored.Length > 0)
                {
                    continue;
                }
                var attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);
                string name;
                if (attributes.Length == 0)
                {
                    name = member.Name;
                }
                else
                {
                    name = ((DisplayAttribute)attributes[0]).GetName();
                }
                yield return new
                {
                    Value = ((int)values.GetValue(i)).ToString(),
                    Text = name,
                    Selected = defaultValue != null && (int)values.GetValue(i) == defaultValue ? "selected" : ""
                };
            }
        }
    }
}