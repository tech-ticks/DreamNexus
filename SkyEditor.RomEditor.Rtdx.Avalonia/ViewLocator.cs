using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using SkyEditor.RomEditor.Avalonia.ViewModels;

namespace SkyEditor.RomEditor.Avalonia
{
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
            var typeName = data.GetType().FullName;
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException("Unable to get full name of the data's type", nameof(data));
            }

            var name = typeName.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                var control = (Control?)Activator.CreateInstance(type);
                if (control != null)
                {
                    return control;
                }
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}