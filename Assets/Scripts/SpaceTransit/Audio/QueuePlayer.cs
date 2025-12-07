using System.Collections.Generic;
using UnityEngine;

namespace SpaceTransit.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public sealed class QueuePlayer : MonoBehaviour
    {

        private AudioSource _source;

        private double _playAt;

        private readonly Queue<(AudioClip, float)> _queue = new();

        public bool IsYapping => _playAt > AudioSettings.dspTime || _source.isPlaying;

        private void Awake() => _source = GetComponent<AudioSource>();

        public void Enqueue(AudioClip clip)
        {
            if (clip)
                _queue.Enqueue((clip, clip.length));
        }

        public void Enqueue(AudioClip clip, float length) => _queue.Enqueue((clip, length));

        public void Delay(float length) => _queue.Enqueue((null, length));

        private void Update()
        {
            var dspTime = AudioSettings.dspTime;
            if (_playAt > dspTime || !_queue.TryDequeue(out var tuple))
                return;
            if (tuple.Item1)
                _source.PlayOneShot(tuple.Item1);
            _playAt = dspTime + tuple.Item2;
        }

    }

}
