using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace LiteUpdateNotice
{
    class GameComponent_Notifier : GameComponent
    {
        private bool LetterStackContains(LetterDef def)
        {
            foreach(Letter let in Find.LetterStack.LettersListForReading) if (let.label.Equals(def.label)) return true;               
            return false;
        }
        public GameComponent_Notifier(Game game)
        {
        }
        public override void StartedNewGame()
        {
            base.StartedNewGame();
            TrySendNotification();
        }
        public override void LoadedGame()
        {
            base.LoadedGame();
            TrySendNotification();
        }
        public void TrySendNotification()
        {
            if(!LoadTracker.NotificationSent)
            {
                foreach (NoticeDef def in DefDatabase<NoticeDef>.AllDefsListForReading)
                {
                    Log.Message(": " + def.defName + " | " + def.version + " | " + assemblyVersion[def]);
                    if (!assemblyVersion.ContainsKey(def))
                    {
                        Log.Message("Added");
                        assemblyVersion.Add(def, def.version);
                    }
                    if (!LetterStackContains(def.letter) && assemblyVersion[def] != def.version)
                    {
                        Find.LetterStack.ReceiveLetter(def.LabelCap, def.description, def.letter);
                        assemblyVersion[def] = def.version;
                    }
                }
                LoadTracker.NotificationSent = true;
            }
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref assemblyVersion, "assemblyVersion", LookMode.Def, LookMode.Value);
        }

        private Dictionary<NoticeDef, string> assemblyVersion = new Dictionary<NoticeDef, string>();
    }
}
