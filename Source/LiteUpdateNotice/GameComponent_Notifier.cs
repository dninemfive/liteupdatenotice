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
            foreach (Letter let in Find.LetterStack.LettersListForReading) if (let.label.Equals(def.label)) return true;               
            return false;
        }
        public GameComponent_Notifier(Game game)
        {
        }

        public override void ExposeData()
        {
            //do nothing to avoid breaking saves
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
            if (!LoadTracker.NotificationSent)
            {
                foreach (NoticeDef def in LoadTracker.thisModNotices) if (!LetterStackContains(def.letter)) Find.LetterStack.ReceiveLetter(def.LabelCap, def.description, def.letter);
                LoadTracker.NotificationSent = true;
            }
        }
    }
}
