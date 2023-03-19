using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private float disableDelay = 0.3f;
        [SerializeField] private Animator buttonAnimator;
        [SerializeField] private LevelStatsContainer stats;
        [SerializeField] private TextMeshProUGUI levelLabel;
        [SerializeField] private Transform player;
        private Camera _cam;

        private void OnEnable()
        {
            EventMaster.AddListener<int>(EventStrings.LEVEL_UP, UpdateLevelLabel);
            EventMaster.AddListener(EventStrings.READY_TO_UPGRADE_LEVEL, SetActiveButton);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<int>(EventStrings.LEVEL_UP, UpdateLevelLabel);
            EventMaster.RemoveListener(EventStrings.READY_TO_UPGRADE_LEVEL, SetActiveButton);
        }

        private void Start()
        {
            _cam = Camera.main;
            button.gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            levelLabel.transform.position = _cam.WorldToScreenPoint(player.position);
        }

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