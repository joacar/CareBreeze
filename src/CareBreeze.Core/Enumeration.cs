using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CareBreeze.Core
{
    /// <summary>
    /// 
    /// <remarks>
    /// Extended and modified https://gist.github.com/slovely/1076365
    /// </remarks>
    /// </summary>
    public abstract class Enumeration : IComparable
    {
        public class InvalidValueException : Exception
        {
            const string MessageFormat = "'{0}' is not a valid {1} in {2}";

            public string Name { get; }

            public string Value { get;}

            public Type Type { get; }

            internal protected InvalidValueException(string name, string value, Type type)
                : base(string.Format(value, name, type))
            {
                Name = name;
                Value = value;
                Type = type;
            }

            internal static InvalidValueException Error<T, K>(string name, K value)
            {
                return new InvalidValueException(name, value as string, typeof(T));
            }
        }

        public int Value { get; private set; }

        public string Name { get; private set; }

        protected Enumeration(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public static IEnumerable<T> All<T>() where T : Enumeration
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
            return fields.Select(field => field.GetValue(null)).OfType<T>();
        }

        protected static T Parse<T, K>(K value, string name, Func<T, bool> predicate) where T : Enumeration
        {
            var matchingItem = All<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                throw InvalidValueException.Error<T, K>(name, value);
            }
            return matchingItem;
        }

        public static T FromValue<T>(int value) where T : Enumeration
            => Parse<T, int>(value, nameof(Value), e => e.Value == value);

        public static T FromName<T>(string name) where T : Enumeration
            => Parse<T, string>(name, nameof(Name), e => e.Name == name);

        public override string ToString() => Name;

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj)
        {
            var other = obj as Enumeration;
            if (other == null)
            {
                return false;
            }
            return GetType().Equals(other.GetType()) && other.Value == Value;
        }

        public int CompareTo(object obj) => Value.CompareTo(((Enumeration)obj).Value);

    }
}
