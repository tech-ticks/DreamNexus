using SkyEditor.RomEditor.Rtdx.Reverse.Const;
using SkyEditor.RomEditor.Rtdx.Reverse.Const.pokemon;
using SkyEditor.RomEditor.Rtdx.Reverse.effect;
using SkyEditor.RomEditor.Rtdx.Reverse.gimmick;
using SkyEditor.RomEditor.Rtdx.Reverse.graphics;
using SkyEditor.RomEditor.Rtdx.Reverse.graphics.camera;
using SkyEditor.RomEditor.Rtdx.Reverse.Stubs.UnityEngine.CoreModule;
using System;
using System.Collections.Generic;

#nullable disable
namespace SkyEditor.RomEditor.Rtdx.Reverse
{
    public class GraphicsSystem : Singleton<GraphicsSystem>
    {
        public enum FrameBuffer3dResolutionType
        {
            LOW,
            NORMAL,
            HIGH,
            MAX
        }

        public enum DatabaseGetFileType
        {
            DRAMA_CHARACTER = 0,
            DRAMA_CHARACTER_EYE = 1,
            DRAMA_CHARACTER_FACE = 2,
            CHARACTER_ONPICTURE = 3,
            CHARACTER_TRUN = 4,
            CHARACTER_BASE_FILE = 5,
            CHARACTER_MOTION_BASE_FILE = 6,
            CHARACTER_SKILL_MOTION_BASE_FILE = 7,
            EFFECT_2D = 8,
            MANPU = 9,
            GIMMICK = 10,
            MAP_WATER = 11,
            MAP_BOTTOM = 12,
            ITEM_2D_LIST_ICON = 0x10,
            ITEM_2D_ONE_PICTURE = 17
        }

        public enum LayerId
        {
            DEFAULT = 0,
            BLEND_MESH = 9,
            BACK_LOG = 10,
            GHOST_BLEND_MESH = 14,
            RENDER_TARGET_MESH = 0xF,
            RENDER_TARGET_MESH2 = 0x10,
            RENDER_TARGET_ADD_EFFECT = 17
        }

        public enum CharacterShaderStatus
        {
            DEFAULT,
            BLEND,
            GHOST,
            RENDER_TEXTURE,
            MAX
        }

        public class CharacterDatabaseParameter
        {
            public string stFileName;
            public string stCommonMotionName;
            public string stCommonMotionName2;
            public string stTextureName;
            public string stCutPictureName;
            public string stCutInName;
            public string stFaceName;
            public string stDotName;
            public string stDotRightName;
            public uint gfxSymbol;
            public int sheetId;
            public EvolutionCameraType evolutionCameraType;
            public GraphicsBodySizeType bodySize;
            public float fBaseScale;
            public float fDungeonBaseScale;
            public float fPGRootBoneScale;
            public float fDotOffsetX;
            public float fDotOffsetY;
            public float fMotionScale;
            public float fWalkSpeedDist;
            public float fWalkSpeedDistG;
            public float fRunSpeedRatioG;
            public float fWalkCorrectionValueG;
            public float fRunCorrectionValueG;
            public float fFlyHeight;
            public float fShadowW;
            public float fShadowH;
            public float fNullHeadOffsetX;
            public float fNullHeadOffsetY;
            public float fNullHeadOffsetZ;
            public float fNullMouthOffsetX;
            public float fNullMouthOffsetY;
            public float fNullMouthOffsetZ;
            public float fNullRightHandOffsetX;
            public float fNullRightHandOffsetY;
            public float fNullRightHandOffsetZ;
            public float fNullLeftHandOffsetX;
            public float fNullLeftHandOffsetY;
            public float fNullLeftHandOffsetZ;
            public float fBDMinX;
            public float fBDMinY;
            public float fBDMinZ;
            public float fBDMaxX;
            public float fBDMaxY;
            public float fBDMaxZ;
            public bool bEnablePGRootBoneScaleDedicatedMotion;
            public bool bFace_HANTEN;
            public bool bFace_FEMALE;
            public bool bBig;
            public bool bEffectBaseSet_CH_OffsetPokemon;
            public List<string> animationPartsList;
        }

        public class CharacterShaderStatusParameter
        {
            public string stBaseShader;
            public string stBlendShader;
            public string stGhostShader;
            public string stRenderTextureShader;
        }

        public class MapDatabaseParameter
        {
            public string mapSymbol;
            public uint mapGfxSymbol;
            public string stFileName;
            public uint allEffectHashGfxSymbol;
            public uint warterHashGfxSymbol;
            public float fWaterHeight;
            public float fWaterColorR;
            public float fWaterColorG;
            public float fWaterColorB;
            public float fWaterColorA;
            public uint eyeAttachHash;
            public uint bottomHashGfxSymbol;
            public float fBottomHeight;
            public float fShadowColorR;
            public float fShadowColorG;
            public float fShadowColorB;
            public float fHeightScale;
            public string stAddFilter;
            public string stAddFilterNear;
            public string stMulFilter;
            public string stMulFilterNear;
            public string stBlinkAnimation;
            public float fLightDirectionDegreeX;
            public float fLightDirectionDegreeY;
            public float fLightDirectionDegreeZ;
            public float fLightColorR;
            public float fLightColorG;
            public float fLightColorB;
            public float fOuterColorR;
            public float fOuterColorG;
            public float fOuterColorB;
            public float fOuterColorA;
            public List<string> animationPartsList;
        }

        public class GimmickDatabaseParameter
        {
            public uint gfxSymbol;
            public string stFileName;
            public string stDefaultMotionKey;
            public BillboardType eBillboardType;
            public float fOffsetX;
            public float fOffsetY;
            public float fOffsetZ;
            public PlacementType eAttachType;
            public bool bBillboard;
        }

        public class EffectDatabaseParameter
        {
            public class SeData
            {
                public string stSymbol;
                public float fStartSec;
            }

            public class ScriptData
            {
                public string stFunc;
                public float fStartSec;
            }


            public string stFileName;
            public string stSymbol;
            public int sheetId;
            public List<SeData> aSe;
            public List<ScriptData> aScript;
            public PlacementType ePlacementType;
            public float fBlock3OffsetX;
            public float fBlock3OffsetY;
            public float fBlock3OffsetZ;
            public float fBlock3Mag;
            public float fRetireTime;
            public bool bBodyAttach;
            public bool bBodyAttachContinueRotate;
            public bool bBodyAttachContinueScale;
            public bool bLoop;
            public bool bStripe;            
            public bool IsPlacementTypeAttach()
            {
                throw new NotImplementedException();
            }
        }

        public class ManpuDatabaseParameter
        {
            public string stFileName;
            public float fOffsetX;
            public float fOffsetY;
            public float fOffsetZ;
            public bool bBillboard;
        }

        public class CameraAnimationDatabaseParameter
        {
            public string stFileName;
        }

        public class EquipGraphicsDatabaseParameter
        {
            public uint gfxSymbol;
            public string stEquipMotionName;
            public string stDataName;
            public string stAttachBoneName;
            public Vector3 vEffectOffset;
        }

        private static Vector2[] s_aFrameBuffer3dResolutionSize;
        private FrameBuffer3dResolutionType eFrameBuffer3dResolutionType_;
        public bool bValidResolutionChangeMode_;
        private List<IGraphicsUpdater> aGraphicsUpdater_;
        private bool bRequestCommandBufferUpdate_;
        private const uint CRC32_MASK = uint.MaxValue;
        private static byte[] aExchangeShareBuffer_;
        private static byte[] aTempUpdateGLBuffer_;
        private static readonly uint[] s_aCRC32Table;
        private static Dictionary<uint, Dictionary<CharacterShaderStatus, string>> dicCharacterShaderStatus_;
        private DataExchangeByteMemoryStream temporaryByteStream;
        private static Dictionary<string, Material> loadMaterialTbl_;
        private static Dictionary<string, Shader> loadShadersTbl_;
        private static List<byte> tmpCrcBuff_;
        private Material fill2dShaderMaterial_;
        private Material texture2dShaderMaterial_;
        private Material debugFontShaderMaterial_;

        public FrameBuffer3dResolutionType Resolution3dType
        {
            
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool ValidResolutionChangeMode
        {
            
            get
            {
                throw new NotImplementedException();
            }
            
            set
            {
                throw new NotImplementedException();
            }
        }

        public static byte[] DataExchangeShareBuffer
        {
            
            get
            {
                throw new NotImplementedException();
            }
        }

        public static byte[] DataExchangeTempBuffer
        {
            
            get
            {
                throw new NotImplementedException();
            }
        }

        public static Dictionary<uint, Dictionary<CharacterShaderStatus, string>> DicCharacterShaderStatus
        {
            
            get
            {
                throw new NotImplementedException();
            }
        }

        public Vector2 CurrentFrameBuffer3dResolutionSize
        {
            
            get
            {
                throw new NotImplementedException();
            }
        }
        
        public DataExchangeByteMemoryStream GetTemporaryDataExchangeByteMemoryStream()
        {
            throw new NotImplementedException();
        }
        
        public static void LoadResourcesForBootstrap()
        {
            throw new NotImplementedException();
        }
        
        public static Material FindShaderMaterial(string materialName)
        {
            throw new NotImplementedException();
        }
        
        public static Shader FindShader(string shaderName)
        {
            throw new NotImplementedException();
        }
        
        public static uint CalcCrc32(byte[] array)
        {
            throw new NotImplementedException();
        }
        
        public static uint CalcCrc32(List<byte> array)
        {
            throw new NotImplementedException();
        }
        
        public static uint CalcCrc32(string str)
        {
            throw new NotImplementedException();
        }
        
        public new static void Startup()
        {
            throw new NotImplementedException();
        }
        
        public void InitializeForGameMain()
        {
            throw new NotImplementedException();
        }
        
        public string GetFileNameFromNativeDatabase(string graphicsSymbol, DatabaseGetFileType fileType)
        {
            throw new NotImplementedException();
        }
        
        public string GetFileNameFromNativeDatabase(uint graphicHashSymbol, DatabaseGetFileType fileType)
        {
            throw new NotImplementedException();
        }
        
        public uint ConvertCreatureSymbolToGraphicsSymbol(uint creatureHashSymbol)
        {
            throw new NotImplementedException();
        }
                
        public static uint CreatureIndexToGfxSymbol(Index index, FormType form)
        {
            throw new NotImplementedException();
        }
        
        public static CharacterDatabaseParameter GetCharacterDataFromNativeDatabase(Index index, FormType form)
        {
            throw new NotImplementedException();
        }
                
        public static CharacterDatabaseParameter GetCharacterDataFromNativeDatabase(uint graphicHashSymbol)
        {
            throw new NotImplementedException();
        }
        
        public CharacterShaderStatusParameter GetCharacterShaderStatusDataFromNativeDatabase(string symbol)
        {
            throw new NotImplementedException();
        }
        
        public List<uint> GetCharacterShaderStatusList()
        {
            throw new NotImplementedException();
        }
        
        public CharacterShaderStatusParameter GetCharacterShaderStatusDataFromNativeDatabase(uint graphicHashSymbol)
        {
            throw new NotImplementedException();
        }
        
        public List<uint> GetCharacterList()
        {
            throw new NotImplementedException();
        }
        
        public MapDatabaseParameter GetMapDataFromNativeDatabase(uint graphicHashSymbol)
        {
            throw new NotImplementedException();
        }
        
        public List<string> GetMapList()
        {
            throw new NotImplementedException();
        }
        
        public GimmickDatabaseParameter GetGimmickDataFromNativeDatabase(uint graphicHashSymbol)
        {
            throw new NotImplementedException();
        }
        
        public List<uint> GetDungeonGimmickGfxSymbolListFromNativeDatabase()
        {
            throw new NotImplementedException();
        }
        
        public List<string> GetDungeonGimmickBundleListFromNativeDatabase()
        {
            throw new NotImplementedException();
        }
        
        public EffectDatabaseParameter GetEffectDataFromNativeDatabase(uint graphicHashSymbol)
        {
            throw new NotImplementedException();
        }
        
        public List<uint> GetEffectList()
        {
            throw new NotImplementedException();
        }
        
        public ManpuDatabaseParameter GetManpuDataFromNativeDatabase(uint graphicHashSymbol)
        {
            throw new NotImplementedException();
        }
        
        public List<uint> GetManpuList()
        {
            throw new NotImplementedException();
        }
        
        public CameraParameter GetCameraParameterFromNativeDatabase(string symbol)
        {
            throw new NotImplementedException();
        }
        
        public CameraParameter GetCameraParameterFromNativeDatabase(uint graphicHashSymbol)
        {
            throw new NotImplementedException();
        }
        
        public CameraAnimationDatabaseParameter GetCameraAnimationParameterFromNativeDatabase(uint graphicHashSymbol)
        {
            throw new NotImplementedException();
        }
        
        public EquipGraphicsDatabaseParameter GetEquipGraphicsDataFromNativeDatabase(uint graphicHashSymbol)
        {
            throw new NotImplementedException();
        }
        
        public List<uint> GetEquipList()
        {
            throw new NotImplementedException();
        }
                
        public void LoadShader2d()
        {
            throw new NotImplementedException();
        }
        
        public Material GetFill2dShader()
        {
            throw new NotImplementedException();
        }
        
        public Material GetTexture2dShader()
        {
            throw new NotImplementedException();
        }
                
        public Material GetDebugFontShader()
        {
            throw new NotImplementedException();
        }
                
        public static float CalcGrayScaleValue(float r, float g, float b)
        {
            throw new NotImplementedException();
        }
                
        public void RequestCommandBufferUpdate()
        {
            throw new NotImplementedException();
        }
                
        public void SetFrameBuffer3dResolutionType(FrameBuffer3dResolutionType eType, bool bForce)
        {
            throw new NotImplementedException();
        }
                
        public void UpdateFromGameSystem()
        {
            throw new NotImplementedException();
        }
                
        public void RegistFrameBuffer3dUpdater(IGraphicsUpdater updater)
        {
            throw new NotImplementedException();
        }
                
        public void UnregistFrameBuffer3dUpdater(IGraphicsUpdater updater)
        {
            throw new NotImplementedException();
        }
                
        public bool IsExistFrameBuffer3dUpdater(IGraphicsUpdater updater)
        {
            throw new NotImplementedException();
        }
                
        public GraphicsSystem()
        {
            throw new NotImplementedException();
        }
    }

}
#nullable restore