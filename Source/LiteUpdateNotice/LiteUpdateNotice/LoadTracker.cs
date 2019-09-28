using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace LiteUpdateNotice
{
    [StaticConstructorOnStartup]
    public class LoadTracker
    {
        public static bool NotificationSent = false;
    }
}
