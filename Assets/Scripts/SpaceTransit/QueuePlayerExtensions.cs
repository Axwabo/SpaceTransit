using System;
using Katie.Unity;
using SpaceTransit.Menu;
using UnityEngine;

namespace SpaceTransit
{

    public static class QueuePlayerExtensions
    {

        public static void EnqueueWithSubtitles(this QueuePlayer queue, string name, string announcement, PhrasePack pack, Signal signal = null, bool showSubtitle = true)
        {
            if (!showSubtitle)
            {
                queue.EnqueueAnnouncement(announcement, pack, signal);
                return;
            }

            var currentEnd = queue.EndDspTime;
            var duration = queue.EnqueueAnnouncement(announcement, pack, signal);
            var delay = Math.Max(0, currentEnd - AudioSettings.dspTime) + (signal ? signal.Duration : 0);
            KatieSubtitleList.Add(name, announcement, delay, duration.TotalSeconds);
        }

        public static void EnqueueWithSubtitles(this QueuePlayer queue, string name, string announcement, PhrasePack pack, bool showSubtitle)
            => queue.EnqueueWithSubtitles(name, announcement, pack, null, showSubtitle);

    }

}
