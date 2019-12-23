using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Verse;

namespace LiteUpdateNotice
{
    [StaticConstructorOnStartup]
    public class LoadTracker
    {
        public static bool NotificationSent = false;
        public static bool VersionExists => version != null;
        public static string version = null;        

        public static string ModName = null, ModID = null;
        public static string FileName
        {
            get
            {
                if (ModName == null || ModID == null) return null;
                return Path.Combine(GenFilePaths.ConfigFolderPath, GenText.SanitizeFilename("LiteUpdateNotice_" + ModID + "_" + ModName));
            }
        }

        LoadTracker()
        {
            GetModInfo();
            if (ModName == null)
            {
                Log.Error("[LiteUpdateNotice] ModName was null. This shouldn't happen!");
            }
            else
            {
                try
                {
                    string filename = FileName;
                    if (File.Exists(filename))
                    {
                        Scribe.loader.InitLoading(filename);
                        try
                        {
                            Scribe_Values.Look(ref version, "version");
                        }
                        finally
                        {
                            Scribe.loader.FinalizeLoading();
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Warning("[LiteUpdateNotice] Caught exception while loading version information for mod " + ModName + ": " + e.ToString(), false);
                }
            }
        }

        public static void GetModInfo()
        {
            Assembly current = typeof(LoadTracker).Assembly;
            foreach(ModContentPack mcp in LoadedModManager.RunningMods)
            {
                foreach (Assembly a in mcp.assemblies.loadedAssemblies) if (a == current)
                    {
                        ModName = mcp.Name;
                        ModID = mcp.Identifier;
                        return;
                    }
            }
        }
    }
}
