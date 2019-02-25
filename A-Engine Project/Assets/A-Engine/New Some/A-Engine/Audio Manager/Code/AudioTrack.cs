// A-Engine, Code version: 1

using UnityEngine;
using System;

namespace AEngine.Audio
{
    [Serializable]
    public class AudioTrack
    {
        private const float DEFAULT_CLIP_VOLUME = 1f;

        [SerializeField] private AudioClip _clip;
        [SerializeField] private float _volume = DEFAULT_CLIP_VOLUME;
    }
}
