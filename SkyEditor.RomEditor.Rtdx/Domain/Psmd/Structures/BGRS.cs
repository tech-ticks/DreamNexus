using SkyEditor.IO.Binary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Domain.Psmd.Structures
{
    public class BGRS
    {
        public BGRS(IReadOnlyBinaryDataAccessor data)
        {
            Magic = data.ReadNullTerminatedString(0, System.Text.Encoding.ASCII);
            ReferencedBchFileName = (data.ReadNullTerminatedString(0x8, System.Text.Encoding.ASCII)).TrimEnd('0'); // Max length: 0x40
            Type = (BgrsType)data.ReadInt32(0x48);

            switch (Type)
            {
                case BgrsType.Normal:
                    OpenInternalNormal(data);
                    break;
                case BgrsType.Extension:
                    OpenInternalExtended(data);
                    break;
                default:
                    throw new NotSupportedException("Unsupported BGRS type: " + Type.ToString());
            }
        }

        public BGRS(byte[] data) : this(new BinaryFile(data))
        {
        }

        public string Magic { get; set; }

        public string ReferencedBchFileName { get; set; }

        public List<Animation> Animations { get; set; } = new List<Animation>();

        public string? BgrsName { get; set; }

        public string? BgrsDevName { get; set; }

        public BgrsType Type { get; set; }

        public List<ModelPart> Parts { get; set; } = new List<ModelPart>();

        public byte[]? UnknownModelPartsFooter { get; set; }

        private bool UsesDevNames { get; set; }

        private void OpenInternalNormal(IReadOnlyBinaryDataAccessor f)
        {
            BgrsName = f.ReadNullTerminatedString(0x58, Encoding.ASCII); // Max length: &H40
            BgrsDevName = f.ReadNullTerminatedString(0x58 + 0x40, Encoding.ASCII); // Max length: &H80

            // Yes, the counts of these two sections are in a different order than the sections themselves
            var animationCount = f.ReadInt32(0x118);
            var partCount = f.ReadInt32(0x11C);

            for (var partIndex = 0x140; partIndex <= 0x140 + (0x80 * partCount) - 1; partIndex += 0x80)
            {
                var partName = f.ReadNullTerminatedString(partIndex + 0x18, System.Text.Encoding.ASCII);
                Parts.Add(new ModelPart(f.ReadArray(partIndex, 0x80), partName));
            }
            UnknownModelPartsFooter = f.ReadArray(0x140 + (0x80 * partCount), 0x18);
            OpenInternalAnimations(f, 0x140 + (0x80 * partCount) + 0x18, animationCount);
        }

        private void OpenInternalExtended(IReadOnlyBinaryDataAccessor f)
        {
            var animationCount = f.ReadInt32(0x4);

            OpenInternalAnimations(f, 0x58, animationCount);

            // Set BGRS name, inferred from the animation names. Animation names are in the form of bgrs_name__animation_name
            BgrsName = Animations.FirstOrDefault()?.BgrsName;
        }

        private void OpenInternalAnimations(IReadOnlyBinaryDataAccessor f, int animationIndex, int animationCount)
        {
            for (var i = animationIndex; i <= animationIndex + (0xC4 * animationCount) - 1; i += 0xC4)
            {
                var animName = (f.ReadNullTerminatedString(i, System.Text.Encoding.ASCII)); // Max length: &H40
                var animDevName = (f.ReadNullTerminatedString(i + 0x40, System.Text.Encoding.ASCII)); // Max length: &H80
                if (!string.IsNullOrEmpty(animDevName))
                    // If any animation has a dev name, then this is a file format that requires them
                    UsesDevNames = true;
                var animType = (AnimationType)f.ReadInt32(i + 0xC0);
                Animation anim = new Animation(animName, animDevName, animType);
                Animations.Add(anim);
            }
        }

        /// <summary>
        /// Gets the name of the animation set to be used if the current one is unavailable
        /// </summary>
        public string? GetFallbackAnimationSetName()
        {
            return Animations.FirstOrDefault(a => a.BgrsName != this.BgrsName)?.BgrsName;
        }


        #region Enums
        public enum AnimationType
        {
            Unknown = 0,
            SkeletalAnimation = 1,
            MaterialAnimation = 2,

            /// <summary>
            /// A skeletal animation that belongs to another BGRS file
            /// </summary>
            Remote = 0x8000000
        }

        public enum BgrsType
        {
            Extension = 0,
            Normal = 1
        }
        #endregion

        #region Child Classes
        public class ModelPart
        {
            public ModelPart(byte[] rawData, string partName)
            {
                this.RawData = rawData;
                this.PartName = partName;
            }

            public byte[] RawData { get; set; }

            public string PartName { get; set; }

            public byte[] GetRawData()
            {
                var partNameBytes = Encoding.ASCII.GetBytes(PartName);
                partNameBytes.CopyTo(RawData, 0x18);
                RawData[0x18 + partNameBytes.Length] = 0;
                return RawData;
            }
        }

        public class Animation
        {
            public Animation(string name, string devName, AnimationType animationType)
            {
                this.Name = name;
                this.AnimationType = animationType;
                this.DevName = devName;
            }

            /// <summary>
            /// The raw name of the animation. Ex. bgrs_name__animation_name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Apparently the name of the *.dae file during development. Always null in PSMD.
            /// </summary>
            public string DevName { get; set; }

            /// <summary>
            /// The name of the BGRS to which this animation belongs. If the raw name is bgrs_name__animation_name, this returns bgrs_name
            /// </summary>
            public string BgrsName
            {
                get
                {
                    return Name.Replace("__", "!").Split('!')[0]; // ! is just a temporary character that won't appear in these names
                }
            }

            /// <summary>
            /// The name of the animation action. If the raw name is bgrs_name__animation_name, this returns animation_name
            /// </summary>
            public string AnimationName
            {
                get
                {
                    return Name.Replace("__", "!").Split('!')[1]; // ! is just a temporary character that won't appear in these names
                }
            }

            public AnimationType AnimationType { get; set; }

            public Animation Clone()
            {
                return new Animation(Name, DevName, AnimationType);
            }

            public override string ToString()
            {
                return Name ?? base.ToString() ?? "null";
            }
        }
        #endregion
    }
}
