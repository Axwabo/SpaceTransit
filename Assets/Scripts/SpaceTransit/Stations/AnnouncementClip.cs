using UnityEngine;

namespace SpaceTransit.Stations
{

    public readonly struct AnnouncementClip
    {

        public AudioClip Clip { get; }

        public float Length { get; }

        public AnnouncementClip(AudioClip clip, float length)
        {
            Clip = clip;
            Length = length;
        }

        public static implicit operator AnnouncementClip(AudioClip clip) => new(clip, clip ? clip.length - 0.05f : 0);

        public static implicit operator AnnouncementClip(float delay) => new(null, delay);

    }

}
