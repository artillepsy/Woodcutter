using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Sound
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [Space]
        [SerializeField] private List<AudioClipBinding> treeKickAudios;
        [SerializeField] private List<AudioClipBinding> treeCutAudios;
        [SerializeField] private List<AudioClipBinding> stepsAudios;
        [SerializeField] private AudioClipBinding levelUpAudio;
        [SerializeField] private AudioClipBinding levelUpRequestAudio;
        [Space]
        [SerializeField] private float playStepsSoundInterval = 0.3f;
        private Vector2 _joystickData;
        private float _timeSinceLastStepSound = 0f;

        private void OnEnable()
        {
            EventMaster.AddListener(EventStrings.TREE_KICKED, () => PlayOnce(treeKickAudios));
            EventMaster.AddListener(EventStrings.TREE_CUTTED, () => PlayOnce(treeCutAudios));
            EventMaster.AddListener(EventStrings.LEVEL_UP, () => PlayOnce(levelUpAudio));
            EventMaster.AddListener(EventStrings.LEVEL_UP_REQUEST, () => PlayOnce(levelUpRequestAudio));
            EventMaster.AddListener<Vector2>(EventStrings.JOYSTICK_INPUT, (data) => _joystickData = data);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener(EventStrings.TREE_KICKED, () => PlayOnce(treeKickAudios));
            EventMaster.RemoveListener(EventStrings.TREE_CUTTED, () => PlayOnce(treeCutAudios));
            EventMaster.RemoveListener(EventStrings.LEVEL_UP, () => PlayOnce(levelUpAudio));
            EventMaster.RemoveListener(EventStrings.LEVEL_UP_REQUEST, () => PlayOnce(levelUpRequestAudio));
            EventMaster.RemoveListener<Vector2>(EventStrings.JOYSTICK_INPUT, (data) => _joystickData = data);
        }

        private void Update()
        {
            if (_joystickData == Vector2.zero)
            {
                _timeSinceLastStepSound = 0f;
                return;
            }
            _timeSinceLastStepSound += Time.deltaTime;

            if (_timeSinceLastStepSound > playStepsSoundInterval)
            {
                _timeSinceLastStepSound = 0f;
                PlayOnce(stepsAudios);
            }
        }

        private void PlayOnce(AudioClipBinding binding)
        {
            audioSource.PlayOneShot(binding.Clip, binding.Volume);
        }

        private void PlayOnce(List<AudioClipBinding> bindings)
        {
            var binding = bindings[Random.Range(0, bindings.Count)];
            audioSource.PlayOneShot(binding.Clip, binding.Volume);
        }
    }

    [System.Serializable]
    public class AudioClipBinding
    {
        [field: SerializeField] public AudioClip Clip { get; private set; }
        [field: SerializeField, Range(0, 1)] public float Volume { get; private set; } = 1f;
    }
}