using System;
using Gtk;

namespace SkyEditorUI.Controllers
{
  abstract class SimpleController : Box
  {
    public SimpleController(string title, string? iconName = null, string? backIllust = null) : base(Orientation.Vertical, 0)
    {
      var align = new Alignment(0.5f, 0.5f, 0.8f, 0.8f);
      PackStart(align, true, true, 0);
      var contentBox = new Box(Orientation.Vertical, 0);
      contentBox.MarginTop = 40;
      align.Add(contentBox);

      Image? icon = null;
      if (iconName != null)
      {
        icon = Image.NewFromIconName(iconName, IconSize.LargeToolbar);
        icon.PixelSize = 127;
      }

      var titleLabel = new Label(title ?? "");
      titleLabel.StyleContext.AddClass("skytemple-view-main-label");

      if (icon != null)
      {
        icon.MarginTop = 20;
        titleLabel.MarginTop = 20;
        contentBox.PackStart(icon, false, true, 0);
      }
      else
      {
        titleLabel.MarginTop = 60;
      }

      var content = GetContent();

      contentBox.PackStart(titleLabel, false, true, 0);
      contentBox.PackStart(content, false, true, 0);

      ShowAll();
    }

    protected abstract Widget GetContent();

    protected Label CreateContentLabel(string text)
    {
      var label = new Label(text);
      label.Justify = Justification.Center;
      label.LineWrapMode = Pango.WrapMode.Word;
      label.LineWrap = true;
      label.MarginTop = 40;
      return label;
    }

    protected Button CreateContentButton(string text, string? iconName = null)
    {
      Image? image = null;
      if (iconName != null)
      {
        image = new Image();
        image.IconName = iconName;
      }

      var button = new Button();
      button.Label = text;
      button.AlwaysShowImage = true;
      button.Image = image;
      button.MarginTop = 40;
      button.Clicked += OnButtonClicked;
      return button;
    }

    protected Box CreateContentButtonWithLabel(string labelText, string buttonText, string? buttonIconName = null)
    {
      var box = new Box(Orientation.Vertical, 5);
      box.Expand = false;
      var label = CreateContentLabel(labelText);
      var button = CreateContentButton(buttonText, buttonIconName);

      box.PackStart(label, false, false, 0);
      box.PackStart(button, false, false, 0);
      return box;
    }

    protected virtual void OnButtonClicked(object? sender, EventArgs args)
    {
    }
  }
}
