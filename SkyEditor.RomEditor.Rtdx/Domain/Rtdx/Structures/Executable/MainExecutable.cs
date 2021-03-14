using NsoElfConverterDotNet;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using BindingFlags = System.Reflection.BindingFlags;
using SkyEditor.IO.Binary;
#if !NETSTANDARD2_0
using Il2CppInspector;
using Il2CppInspector.Reflection;
using Il2CppInspector.Model;
using NoisyCowStudios.Bin2Object;
#endif

namespace SkyEditor.RomEditor.Domain.Rtdx.Structures.Executable
{
  public interface IMainExecutable
  {
    IReadOnlyList<StarterFixedPokemonMap> StarterFixedPokemonMaps { get; }
    PegasusActDatabase ActorDatabase { get; }
    public ExecutableVersion Version { get; }
    public byte[] Data { get; set; }

    byte[] ToElf();
    byte[] ToNso(INsoElfConverter? nsoElfConverter = null);

    public void WriteStartersToBinaryFile(string path);

#if !NETSTANDARD2_0
    public AppModel IlAppModel { get; }
    public ulong CodeSectionOffset { get; }

    /// <summary>Retrieve the offset of a method relative to the start of the code ELF segment.</summary>
    ulong GetIlMethodOffset(string fullTypeName, string methodName, string[] paramTypeNames);

    /// <summary>Retrieve the offset of a constructor relative to the start of the code ELF segment.</summary>
    ulong GetIlConstructorOffset(string fullTypeName, string[] paramTypeNames);
#endif
  }

  public class MainExecutable : IMainExecutable
  {
    public static MainExecutable LoadFromNso(byte[] file, byte[] il2CppMetadata, INsoElfConverter? nsoElfConverter = null)
    {
      nsoElfConverter ??= NsoElfConverter.Instance;
      var data = nsoElfConverter.ConvertNsoToElf(file);
      return new MainExecutable(data, il2CppMetadata);
    }

    public MainExecutable(byte[] elfData, byte[] il2CppMetadata)
    {
      if (elfData == null)
      {
        throw new ArgumentNullException(nameof(elfData));
      }
      if (elfData.Length <= (0x04BA3B0C + 0x90))
      {
        throw new ArgumentException("Data is not long enough to contain known sections", nameof(elfData));
      }

      this.Data = elfData ?? throw new ArgumentNullException(nameof(elfData));
      this.Il2CppMetadata = il2CppMetadata ?? throw new ArgumentNullException(nameof(il2CppMetadata));

      Init();
    }

    const int starterFixedPokemonMapOffsetOriginal = 0x04BA3B0C;
    const int starterFixedPokemonMapOffsetUpdate = 0x04BA4DBC;

    private void Init()
    {
      Init(starterFixedPokemonMapOffsetOriginal);
      if (StarterFixedPokemonMaps.All(m => m.PokemonId == CreatureIndex.NONE))
      {
        Init(starterFixedPokemonMapOffsetUpdate);
        Version = ExecutableVersion.Update1;
      }
      else
      {
        Version = ExecutableVersion.Original;
      }
    }

    private void Init(int starterFixedPokemonMapOffset)
    {
      var starterFixedPokemonMaps = new List<StarterFixedPokemonMap>();
      for (int i = 0; i < 16; i++)
      {
        var offset = starterFixedPokemonMapOffset + (8 * i);
        starterFixedPokemonMaps.Add(new StarterFixedPokemonMap
        {
          PokemonId = (CreatureIndex)BitConverter.ToInt32(Data, offset),
          FixedPokemonId = (FixedCreatureIndex)BitConverter.ToInt32(Data, offset + 4)
        });
      }
      this.StarterFixedPokemonMaps = starterFixedPokemonMaps;
    }

    public byte[] ToElf()
    {
      if (actorDatabase != null) // No need to write if we haven't even accessed it
      {
        ActorDatabase.Write();
      }

      int starterFixedPokemonMapOffset = Version switch
      {
        ExecutableVersion.Original => starterFixedPokemonMapOffsetOriginal,
        ExecutableVersion.Update1 => starterFixedPokemonMapOffsetUpdate,
        _ => throw new InvalidOperationException("Unsupported executable version")
      };

      for (int i = 0; i < 16; i++)
      {
        var map = StarterFixedPokemonMaps[i];
        var offset = starterFixedPokemonMapOffset + (8 * i);
        BitConverter.GetBytes((int)map.PokemonId).CopyTo(Data, offset);
        BitConverter.GetBytes((int)map.FixedPokemonId).CopyTo(Data, offset + 4);
      }
      return Data;
    }

    public byte[] ToNso(INsoElfConverter? nsoElfConverter = null)
    {
      nsoElfConverter ??= NsoElfConverter.Instance;
      return nsoElfConverter.ConvertElfToNso(ToElf());
    }

    public void WriteStartersToBinaryFile(string path)
    {
      using var file = new BinaryFile(new MemoryStream());

      foreach (var starter in StarterFixedPokemonMaps)
      {
        file.WriteInt32(file.Length, (int) starter.PokemonId);
        file.WriteInt32(file.Length, (int) starter.FixedPokemonId);
      }

      File.WriteAllBytes(path, file.ReadArray());
    }

    public byte[] Data { get; set; }
    public byte[] Il2CppMetadata { get; }
    public ExecutableVersion Version { get; private set; }

    public IReadOnlyList<StarterFixedPokemonMap> StarterFixedPokemonMaps { get; private set; } = default!;

    private PegasusActDatabase? actorDatabase;
    public PegasusActDatabase ActorDatabase
    {
      get
      {
        if (actorDatabase == null)
        {
          actorDatabase = new PegasusActDatabase(this);
        }
        return actorDatabase;
      }
    }

#if !NETSTANDARD2_0
    private ulong? codeSectionOffset;

    public ulong CodeSectionOffset
    {
      get
      {
        if (codeSectionOffset == null)
        {
          LoadIlAppModel();
        }
        return codeSectionOffset!.Value;
      }
    }

    private AppModel? ilAppModel;

    public AppModel IlAppModel
    {
      get
      {
        {
          if (ilAppModel == null)
          {
            LoadIlAppModel();
          }
          return ilAppModel!;
        }
      }
    }

    private void LoadIlAppModel()
    {
      // Lazily create an Il2CppInspector model.
      // TODO: Consider caching the result in a file. This is horribly slow.
      var binaryStream = new MemoryStream(Data.ToArray());
      var metadataStream = new MemoryStream(Il2CppMetadata);

      var oldOut = Console.Out;
      // Disable Console.WriteLine() since Il2CppInspector logs text
      Console.SetOut(TextWriter.Null);
      try
      {
        // Disable the plugin manager since it would crash due to a missing "plugins" folder
        PluginManager.Enabled = false;

        var inspector = Il2CppInspector.Il2CppInspector
          .LoadFromStream(binaryStream, metadataStream, silent: true).FirstOrDefault();

        if (inspector == null)
        {
          throw new Exception("Couldn't extract Il2Cpp metadata.");
        }

        ilAppModel = new AppModel(new TypeModel(inspector));
        codeSectionOffset = inspector.BinaryImage.GetSections()
          .First(section => section.IsExec).ImageStart;
      }
      finally
      {
        Console.SetOut(oldOut);
      }
    }

    public ulong GetIlMethodOffset(string fullTypeName, string methodName, string[] paramTypeNames)
    {
      var methodInfos = GetIlType(fullTypeName).GetMethods(methodName);
      var overloadMethodInfo = FindMethodOverload(methodInfos, paramTypeNames);
      if (overloadMethodInfo == null || !overloadMethodInfo.VirtualAddress.HasValue)
      {
        throw new ArgumentException("The method doesn't exist.");
      }
      return overloadMethodInfo.VirtualAddress.Value.Start;
    }

    public ulong GetIlConstructorOffset(string fullTypeName, string[] paramTypeNames)
    {
      var methodInfos = GetIlType(fullTypeName).DeclaredConstructors.ToArray();
      var overloadMethodInfo = FindMethodOverload(methodInfos, paramTypeNames);
      if (overloadMethodInfo == null || !overloadMethodInfo.VirtualAddress.HasValue)
      {
        throw new ArgumentException("The constructor doesn't exist.");
      }
      return overloadMethodInfo.VirtualAddress.Value.Start;
    }

    private TypeInfo GetIlType(string fullTypeName)
    {
      return IlAppModel.TypeModel.GetType(fullTypeName);
    }

    private MethodBase? FindMethodOverload(MethodBase[] methodInfos, string[] paramTypeNames)
    {
      var paramTypes = paramTypeNames.Select(GetIlType).ToArray();
      for (int i = 0; i < methodInfos.Length; i++)
      {
        var methodInfo = methodInfos[i];
        bool isPossibleCandidate = methodInfo.DeclaredParameters.Count == paramTypeNames.Length;
        for (int j = 0; j < methodInfo.DeclaredParameters.Count && isPossibleCandidate; j++)
        {
          if (paramTypes[j] != methodInfo.DeclaredParameters[j].ParameterType)
          {
            isPossibleCandidate = false;
          }
        }

        if (isPossibleCandidate)
        {
          return methodInfo;
        }
      }

      return null;
    }

#endif
  }

  [DebuggerDisplay("StarterFixedPokemonMap: {PokemonId} -> {FixedPokemonId}")]
  public class StarterFixedPokemonMap
  {
    public CreatureIndex PokemonId { get; set; }
    public FixedCreatureIndex FixedPokemonId { get; set; }
  }
}
