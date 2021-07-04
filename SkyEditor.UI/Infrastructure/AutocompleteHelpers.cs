using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gtk;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditorUI.Infrastructure
{
  public static class AutocompleteHelpers
  {
    public static readonly Regex idReferenceRegex = new Regex(".*\\(#(\\d+)\\).*", RegexOptions.Compiled);

    public static IEnumerable<string> GetPokemon(IRtdxRom rom)
    {
      return Enumerable.Range((int) CreatureIndex.NONE, (int) CreatureIndex.END - 1)
        .Select(id => FormatPokemon(rom, (CreatureIndex) id));
    }

    public static string FormatPokemon(IRtdxRom rom, CreatureIndex creatureId)
    {
      var pokemonStrings = rom.GetCommonStrings().Pokemon;
      string formattedId = $"#{((int) creatureId).ToString("000")}";
      if (pokemonStrings.TryGetValue(creatureId, out string? name))
      {
        if (!string.IsNullOrEmpty(name.Trim()))
        {
          return $"{name} ({formattedId})";
        }
      }

      var enumString = creatureId.ToString();
      if (enumString != string.Empty)
      {
        return $"{enumString} ({formattedId})";
      }
      
      return $"<unknown Pokémon> ({formattedId})";
    }

    public static CreatureIndex? ExtractPokemon(string text)
    {
      var match = idReferenceRegex.Match(text);
      var idString = match.Groups.Count > 1 ? match.Groups[1].Value : null;
      if (idString != null && int.TryParse(idString, out int result)) {
        var index = (CreatureIndex) result;
        if (index >= CreatureIndex.NONE && index < CreatureIndex.END) {
          return index;
        }
      }

      return null;
    }

    public static IEnumerable<string> GetMoves(IRtdxRom rom)
    {
      return Enumerable.Range((int) WazaIndex.NONE, (int) WazaIndex.END - 1)
        .Select(id => FormatMove(rom, (WazaIndex) id));
    }

    public static string FormatMove(IRtdxRom rom, WazaIndex moveId)
    {
      var moveStrings = rom.GetCommonStrings().Moves;
      string formattedId = $"#{((int) moveId).ToString("000")}";
      if (moveStrings.TryGetValue(moveId, out string? name))
      {
        if (!string.IsNullOrEmpty(name.Trim()))
        {
          return $"{name} ({formattedId})";
        }
      }

      var enumString = moveId.ToString();
      if (enumString != string.Empty)
      {
        return $"{enumString} ({formattedId})";
      }
      
      return $"<unknown Move> ({formattedId})";
    }

    public static WazaIndex? ExtractMove(string text)
    {
      var match = idReferenceRegex.Match(text);
      var idString = match.Groups.Count > 1 ? match.Groups[1].Value : null;
      if (idString != null && int.TryParse(idString, out int result)) {
        var index = (WazaIndex) result;
        if (index >= WazaIndex.NONE && index < WazaIndex.END) {
          return index;
        }
      }

      return null;
    }

    public static void AppendAll<T>(this ListStore store, IEnumerable<T> enumerable)
    {
      foreach (var item in enumerable)
      {
        store.AppendValues(item?.ToString() ?? "<null>");
      }
    }
  }
}
