using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Avalonia.ViewModels
{
    public class ListItem<T> : INotifyPropertyChanged where T : struct
    {
        private static readonly Type type = typeof(T);

        public ListItem(string displayName, T value)
        {
            this._displayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            this._value = value;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                if (_displayName != value)
                {
                    _displayName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
                }
            }
        }
        private string _displayName = default!;


        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!_value.Equals(value))
                {
                    _value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }
        private T _value;

        public override string ToString()
        {
            if (type.IsEnum)
            {
                var enumNumeric = $"{Value:d}".PadLeft(3, '0');
                return $"{DisplayName} ({enumNumeric} {Value:f})";
            }
            else
            {
                return $"{DisplayName}";
            }
        }
    }
}
