using System;
using System.Collections.Generic;

namespace ConstructionLine.CodingChallenge
{
    public class Size : IComparable
    {
        public Guid Id { get; }

        public string Name { get; }

        private Size(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Size Small = new Size(Guid.NewGuid(), "Small");
        public static Size Medium = new Size(Guid.NewGuid(), "Medium");
        public static Size Large = new Size(Guid.NewGuid(), "Large");

        public static List<Size> All =
            new List<Size>
            {
            Small,
            Medium,
            Large
            };

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Size otherSize = obj as Size;
            if (otherSize != null)
            {
                return this.Name.CompareTo(otherSize.Name);
            }
            else
            {
                throw new ArgumentException("Object is not a Size");
            }
        }
    }

}