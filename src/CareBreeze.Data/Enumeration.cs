﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CareBreeze.Data
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
            public string Name { get; private set; }

            public string Value { get; private set; }

            public IList<string> Values { get; private set; }

            internal protected InvalidValueException(string message)
                : base(message)
            {
            }

            internal static InvalidValueException Error<T, K>(string name, K value) where T : Enumeration
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, name, typeof(T));
                return new InvalidValueException(message)
                {
                    Value = value as string,
                    Name = name,
                    Values = All<T>().Select(e => e.Name).ToList()
                };
            }
        }

        public int Value { get; protected internal set; }

        public string Name { get; protected internal set; }

        internal Enumeration()
        {
            // Empty constructor for EF
        }

        protected internal Enumeration(string name, int value)
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

        public static bool operator ==(Enumeration lhs, Enumeration rhs)
        {
            return ReferenceEquals(lhs, rhs) || (lhs?.Equals(rhs) ?? false);
        }

        public static bool operator !=(Enumeration lhs, Enumeration rhs)
        {
            return !(lhs == rhs);
        }

        public int CompareTo(object obj) => Value.CompareTo(((Enumeration)obj).Value);

    }
}
