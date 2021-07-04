using Gtk;

namespace SkyEditorUI.Controllers
{
  abstract class SimpleController : Box
  {
    public class Args
    {
      public string? Title { get; set; }
      public string? Content { get; set; }
      public string? IconName { get; set; }
      public string? BackIllust { get; set; }
    }

    public SimpleController(Args args) : base(Orientation.Vertical, 0)
    {
      var align = new Alignment(0.5f, 0.5f, 0.8f, 0.8f);
      PackStart(align, true, true, 0);
      var contentBox = new Box(Orientation.Vertical, 0);
      contentBox.MarginTop = 40;
      align.Add(contentBox);

      Image? icon = null;
      if (args.IconName != null)
      {
        icon = Image.NewFromIconName(args.IconName, IconSize.LargeToolbar);
        icon.PixelSize = 127;
      }

      var titleLabel = new Label(args.Title ?? "");
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

      var contentLabel = new Label(args.Content);
      contentLabel.Justify = Justification.Center;
      contentLabel.LineWrapMode = Pango.WrapMode.Word;
      contentLabel.LineWrap = true;
      contentLabel.MarginTop = 40;

      contentBox.PackStart(titleLabel, false, true, 0);
      contentBox.PackStart(contentLabel, false, true, 0);

      ShowAll();
    }
  }
}
