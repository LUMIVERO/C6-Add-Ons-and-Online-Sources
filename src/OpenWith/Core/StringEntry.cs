using System;

namespace SwissAcademic.Addons.OpenWith
{
    internal class StringEntry
    {
        #region Constructors

        public StringEntry(string name, string value)
        {
            Name = name;
            Value = value;
        }

        #endregion

        #region Properties

        public bool IsMRUList => Name.Equals("MRUList", StringComparison.OrdinalIgnoreCase);

        public bool IsEmpty => string.IsNullOrEmpty(Value) || string.IsNullOrWhiteSpace(Value);

        public string Value { get; }
        public string Name { get; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"Name: {Name} Value: {Value}";
        }

        #endregion
    }
}
