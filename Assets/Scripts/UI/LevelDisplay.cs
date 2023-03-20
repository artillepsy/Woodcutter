using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core
{
    /// <summary>
    /// Отображает уровень игрока над персонажем.
    /// Содержит в себе кнопку повышения уровня и проигрывает соответствующие анимации
    /// </summary>
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] private Animation levelLabelAnimation;
        [SerializeField] private Button button;
        [SerializeField] private float disableDelay = 0.3f;
        [SerializeField] private Animator buttonAnimator;
        [SerializeField] private LevelStatsContainer stats;
        [SerializeField] private TextMeshProUGUI levelLabel;
        [SerializeField] private Transform player;
        private Camera _cam;

        private void OnEnable()
        {
            EventMaster.AddListener(EventStrings.READY_TO_UPGRADE_LEVEL, SetActiveButton);
            EventMaster.AddListener<int>(EventStrings.LEVEL_UP, UpdateLevelLabel);
            EventMaster.AddListener<int>(EventStrings.LEVEL_UP, PlayLabelAnimation);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener(EventStrings.READY_TO_UPGRADE_LEVEL, SetActiveButton);
            EventMaster.RemoveListener<int>(EventStrings.LEVEL_UP, UpdateLevelLabel);
            EventMaster.RemoveListener<int>(EventStrings.LEVEL_UP, PlayLabelAnimation);
        }

        private void Start()
        {
            _cam = Camera.main;
            DeactivateButton();
        }

        /// <summary>
        /// Присваивание позиции счетчика уровня к позиции игрока
        /// </summary>
        private void LateUpdate()
        {
            levelLabel.transform.position = _cam.WorldToScreenPoint(player.position);
        }

        /// <summary>
        /// ПРоигрывает анимацию счетчика после повышения уровня
        /// </summary>
        private void PlayLabelAnimation(int level)
        {
            if (level != 1)
            {
                levelLabelAnimation.Play();
            }
        }

        /// <summary>
        /// ПРи нажатии на кнопку уровень повышается взамен
        /// нужному количеству дерева для него
        /// </summary>
        public void OnClickLevelUp()
        {
            buttonAnimator.SetTrigger(AnimatorStrings.PRESSED);
            button.interactable = false;
            Invoke(nameof(DeactivateButton), disableDelay);
            EventMaster.PushEvent(EventStrings.LEVEL_UP_REQUEST);
            Debug.Log("Level up");
        }

        private void ActivateButton()
        {
            button.gameObject.SetActive(true);
        }

        private void DeactivateButton()
        {
            button.gameObject.SetActive(false);
        }

        /// <summary>
        /// Кнопка поднятия уровня появляется, если дерева
        /// хватает для повышения
        /// </summary>
        private void SetActiveButton()
        {
            CancelInvoke();
            button.gameObject.SetActive(true);
            buttonAnimator.SetTrigger(AnimatorStrings.APPEAR);
            button.interactable = true;
        }

        private void UpdateLevelLabel(int level)
        {
            levelLabel.text = $"Lv. {level}";
        }
    }
}