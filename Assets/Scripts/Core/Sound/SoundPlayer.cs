using Assets.Scripts.Trees;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Scripts.Core.Sound
{
    /// <summary>
    /// Класс проигрывания всех звуков на сцене в 2D и 3D пространстве
    /// </summary>
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Transform player;
        [SerializeField] private bool debug = true;
        [Space]
        [SerializeField] private List<AudioClipBinding> treeKickAudios;
        [SerializeField] private List<AudioClipBinding> treeCutAudios;
        [SerializeField] private List<AudioClipBinding> treeGrowAudios;
        [SerializeField] private List<AudioClipBinding> stepsAudios;
        [SerializeField] private AudioClipBinding levelUpAudio;
        [Space]
        [SerializeField, Min(0)] private float minFadeRadius = 10f;
        [SerializeField, Min(0)] private float maxFadeRadius = 30f;
        [Space]
        [SerializeField] private float playStepsSoundInterval = 0.3f;
        private float _timeSinceLastStepSound = 0f;
        private Vector2 _joystickData;

        /// <summary>
        /// Подписка на все события-триггеры для звуков
        /// </summary>
        private void OnEnable()
        {
            EventMaster.AddListener(EventStrings.TREE_KICKED, () => PlayOnce(treeKickAudios));
            EventMaster.AddListener(EventStrings.TREE_CUTTED, () => PlayOnce(treeKickAudios));
            EventMaster.AddListener(EventStrings.TREE_CUTTED, () => PlayOnce(treeCutAudios));
            EventMaster.AddListener<int>(EventStrings.LEVEL_UP, PlayLevelUpClip);
            EventMaster.AddListener<Vector2>(EventStrings.JOYSTICK_INPUT, (data) => _joystickData = data);
            EventMaster.AddListener<TreeObject>(EventStrings.TREE_GROWED,
                (tree) => PlayOnce(treeGrowAudios, tree.transform.position));
        }

        /// <summary>
        /// отписка от этих событий
        /// </summary>
        private void OnDisable()
        {
            EventMaster.RemoveListener(EventStrings.TREE_KICKED, () => PlayOnce(treeKickAudios));
            EventMaster.RemoveListener(EventStrings.TREE_CUTTED, () => PlayOnce(treeKickAudios));
            EventMaster.RemoveListener(EventStrings.TREE_CUTTED, () => PlayOnce(treeCutAudios));
            EventMaster.RemoveListener<int>(EventStrings.LEVEL_UP, PlayLevelUpClip);
            EventMaster.RemoveListener<Vector2>(EventStrings.JOYSTICK_INPUT, (data) => _joystickData = data);
            EventMaster.RemoveListener<TreeObject>(EventStrings.TREE_GROWED,
                (tree) => PlayOnce(treeGrowAudios, tree.transform.position));
        }

        /// <summary>
        /// Проигрывание звука шагов во время ходьбы
        /// </summary>
        private void Update()
        {
            if (_joystickData == Vector2.zero)
            {
                return;
            }
            _timeSinceLastStepSound += Time.deltaTime;

            if (_timeSinceLastStepSound > playStepsSoundInterval)
            {
                _timeSinceLastStepSound = 0f;
                PlayOnce(stepsAudios);
            }
        }

        private void PlayLevelUpClip(int level)
        {
            if (level != 1)
            {
                PlayOnce(levelUpAudio);
            }
        }

        private void PlayOnce(AudioClipBinding binding)
        {
            audioSource.PlayOneShot(binding.Clip, binding.Volume);
        }

        private void PlayOnce(List<AudioClipBinding> bindings)
        {
            var binding = bindings[Random.Range(0, bindings.Count)];
            PlayOnce(binding);
        }

        /// <summary>
        /// Проигрывание звука в 3D пространстве в 
        /// зависимости от удалённости объекта от игрока
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="pos"></param>
        private void PlayOnce(AudioClipBinding binding, Vector3 pos)
        {
            var volume = GetInfluencedVolume(pos, binding.Volume);
            
            if (debug)
            {
                Debug.Log($"volume: {volume}");
            }

            if (volume == 0f)
            {
                return;
            }

            audioSource.PlayOneShot(binding.Clip, volume);
        }

        private void PlayOnce(List<AudioClipBinding> bindings, Vector3 pos)
        {
            var binding = bindings[Random.Range(0, bindings.Count)];
            PlayOnce(binding, pos);
        }

        /// <summary>
        /// Рассчёт расстояния а настройка звука
        /// </summary>
        private float GetInfluencedVolume(Vector3 pos, float startValue)
        {
            var distance = Vector3.Distance(pos, player.position);

            if (distance <= minFadeRadius)
            {
                return startValue;
            }
            else if (distance >= maxFadeRadius)
            {
                return 0f;
            }

            return Mathf.InverseLerp(maxFadeRadius, minFadeRadius, distance) * startValue;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!debug)
            {
                return;
            }

            Handles.color = Color.red;
            var rotation = Quaternion.Euler(90, 0, 0);
            var position = player ? player.position : transform.position;
            Handles.CircleHandleCap(0, position, rotation, minFadeRadius, EventType.Repaint);
            Handles.color = Color.green;
            Handles.CircleHandleCap(0, position, rotation, maxFadeRadius, EventType.Repaint);
        }
#endif
    }

    /// <summary>
    /// Класс-контейнер звуков и их громкости для удобной настройки в инспекторе
    /// </summary>
    [System.Serializable]
    public class AudioClipBinding
    {
        [field: SerializeField] public AudioClip Clip { get; private set; }
        [field: SerializeField, Range(0, 1)] public float Volume { get; private set; } = 1f;
    }
}