using System.Collections.Generic;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Runtime.InteropServices;
using System.Net.Http;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using System.Linq;

namespace SkyEditorUI.Infrastructure
{

    public static class SpriteBotImport
    {
        private const string RepositoryRootUrl = "https://raw.githubusercontent.com/PMDCollab/SpriteCollab/master";
        private const string TrackerJsonUrl = RepositoryRootUrl + "/tracker.json";
        private const string PortraitsUrl = RepositoryRootUrl + "/portrait";

        private static readonly Dictionary<string, FaceType> SpriteBotToRtdxIndex = new Dictionary<string, FaceType>
        {
            { "Normal", FaceType.NORMAL },
            { "Happy", FaceType.HAPPY },
            { "Pain", FaceType.PAIN },
            { "Angry", FaceType.ANGRY },
            { "Worried", FaceType.THINK },
            { "Sad", FaceType.SAD },
            { "Crying", FaceType.WEEP },
            { "Shouting", FaceType.SHOUT },
            { "Teary-Eyed", FaceType.TEARS },
            { "Determined", FaceType.DECIDE },
            { "Joyous", FaceType.GLADNESS },
            { "Inspired", FaceType.EMOTION },
            { "Surprise", FaceType.SURPRISE },
            { "Dizzy", FaceType.FAINT },
            { "Sigh", FaceType.RELIEF },
            { "Stunned", FaceType.CATCHBREATH },
            { "Special0", FaceType.SPECIAL01 },
            { "Special1", FaceType.SPECIAL02 },
            { "Special2", FaceType.SPECIAL03 },
            { "Special3", FaceType.SPECIAL04 }
        };

        public static async Task<Dictionary<int, SpriteBotTrackerEntry>> DownloadTracker()
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);
            var trackerJsonString = await client.GetStringAsync(TrackerJsonUrl);
            var tracker = JsonConvert.DeserializeObject<Dictionary<int, SpriteBotTrackerEntry>>(trackerJsonString,
                new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
                });
            if (tracker == null)
            {
                throw new Exception("Failed to download metadata");
            }
            return tracker;
        }

        public static async Task<Image<Bgra32>> DownloadPortraitSheet(
            SpriteBotTrackerEntry trackerEntry, string path, Action<string> onProgress)
        {
            string baseUrl = $"{PortraitsUrl}/{path}";
            var downloadedPortraits = new Dictionary<FaceType, byte[]>();

            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);
            var portraitsToDownload = trackerEntry.PortraitFiles?.Keys
                .Where(key => SpriteBotToRtdxIndex.ContainsKey(key)).ToArray() ?? new string[0];
            for (int i = 0; i < portraitsToDownload.Length; i++)
            {
                var portraitName = portraitsToDownload[i];
                var type = SpriteBotToRtdxIndex[portraitName];
                onProgress($"Downloading portrait files ({i+1}/{portraitsToDownload.Length})");
                string url = $"{baseUrl}/{portraitName}.png";
                downloadedPortraits.Add(type, await client.GetByteArrayAsync(url));
            }

            onProgress("Processing...");
            
            var portraitSheet = new Image<Bgra32>(256, 256);
            using var normalPortraitSheet = Image.Load<Bgra32>(downloadedPortraits[FaceType.NORMAL]);
            CreateBorder(normalPortraitSheet);
            for (int i = 0; i < (int) FaceType.MAX; i++)
            {
                var dst = new Point(i % 6 * 40, i / 6 * 40);

                if (downloadedPortraits.TryGetValue((FaceType) i, out var bytes) && (FaceType) i != FaceType.NORMAL)
                {
                    using var portrait = Image.Load<Bgra32>(bytes);
                    CreateBorder(portrait);
                    portraitSheet.Mutate(x => x.DrawImage(portrait, dst, 1f));
                }
                else
                {
                    // Use the "normal" portrait as a fallback
                    portraitSheet.Mutate(x => x.DrawImage(normalPortraitSheet, dst, 1f));
                }
            }

            foreach (var (type, bytes) in downloadedPortraits)
            {
                using var image = Image.Load<Bgra32>(bytes);
                if (image.Width != 40 || image.Height != 40)
                {
                    throw new InvalidOperationException(
                        "Expected 40x40 px, but the downloaded portrait has a different resolution.");
                }
            }
            return portraitSheet;
        }

        public static void CreateBorder(Image<Bgra32> image)
        {
            var col = new Bgra32(0, 0, 0, 0);
            image[0, 0] = col;
            image[1, 1] = col;
            image[0, 1] = col;
            image[1, 0] = col;
            image[0, 2] = col;
            image[2, 0] = col;

            image[39, 39] = col;
            image[38, 38] = col;
            image[39, 38] = col;
            image[38, 39] = col;
            image[37, 39] = col;
            image[39, 37] = col;

            image[0, 39] = col;
            image[1, 38] = col;
            image[1, 39] = col;
            image[0, 38] = col;
            image[0, 37] = col;
            image[2, 39] = col;

            image[39, 0] = col;
            image[38, 1] = col;
            image[38, 0] = col;
            image[39, 1] = col;
            image[37, 0] = col;
            image[39, 2] = col;
        }
    }

    public class SpriteBotTrackerEntry
    {
        public string? Name { get; set; }
        public bool Canon { get; set; }
        public bool Modreward { get; set; }

        // Contains names of all files that are present. The bool indicates if the portrait is locked.
        public Dictionary<string, bool>? PortraitFiles { get; set; }
        public PortraitCredit? PortraitCredit { get; set; }
        public string? PortraitLink { get; set; } // Cache link
        public int PortraitComplete { get; set; }
        public DateTime? PortraitModified { get; set; }
        public bool PortraitRequired { get; set; }
        public Dictionary<int, SpriteBotTrackerEntry>? Subgroups { get; set; }
    }

    public class PortraitCredit
    {
        public string? Primary { get; set; }
        public string[]? Secondary { get; set; }
        public int Total { get; set; }
    }
}
