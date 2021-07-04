using System.Diagnostics;
using Gtk;

namespace SkyEditorUI.Infrastructure
{
  public static class UIUtils
  {
    public static void ShowErrorDialog(Window? parent, string title, string text)
    {
      var dialog = new MessageDialog(parent, DialogFlags.Modal, MessageType.Error,
        ButtonsType.Ok, false, text);

      dialog.Title = title;
      dialog.Run();
      dialog.Destroy();
    }

    public static void OpenInFileBrowser(string path)
    {
      Process.Start(new ProcessStartInfo
      {
        FileName = path,
        UseShellExecute = true,
        Verb = "open"
      });
    }

    public static void OpenInVSCode(string path, Window parent)
    {
      try
      {
        var startInfo = new ProcessStartInfo
        {
          FileName = "code",
          UseShellExecute = false,
        };
        startInfo.ArgumentList.Add(path);

        Process.Start(startInfo);
      }
      catch
      {
        System.Console.WriteLine("???????????????????");
        ShowErrorDialog(parent, "Error",
          "Couldn't open path in VS Code. Please ensure that VS Code is installed and added to the PATH environment variable.");
      }
    }

    public static void ConsumePendingEvents()
    {
      while (Application.EventsPending())
      {
        Application.RunIteration();
      }
    }
  }
}
