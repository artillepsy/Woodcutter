using Assets.Scripts.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class WoodCountDisplay : MonoBehaviour
    {
        [SerializeField] private Animation woodStatsAnimation;
        [SerializeField] private TextMeshProUGUI woodAddedCountLabel;
        [SerializeField] private TextMeshProUGUI woodCountLabel;
        [SerializeField] private Image sliderImage;
        [Space]
        [SerializeField] private AnimationClip woodLootAnim;
        [SerializeField] private AnimationClip levelUpAnim;

        private void OnEnable()
        {
            EventMaster.AddListener(EventStrings.TREE_LOOT_DROP,
                () => woodStatsAnimation.Play(woodLootAnim.name));

            EventMaster.AddListener<int>(EventStrings.LEVEL_UP, PlayWoodStatsAnimation);

            EventMaster.AddListener<int>(EventStrings.TREE_LOOT_DROP,
                (count) => woodAddedCountLabel.text = $"+{count}");
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener(EventStrings.TREE_LOOT_DROP,
                () => woodStatsAnimation.Play(woodLootAnim.name));

            EventMaster.RemoveListener<int>(EventStrings.LEVEL_UP, PlayWoodStatsAnimation);

            EventMaster.RemoveListener<int>(EventStrings.TREE_LOOT_DROP,
                (count) => woodAddedCountLabel.text = $"+{count}");
        }

        public void UpdateWoodCount(int count, int needCountToNextLevel)
        {
            woodCountLabel.text = $"{count} / {needCountToNextLevel}";
            sliderImage.fillAmount = (float)count / needCountToNextLevel;
        }

        private void PlayWoodStatsAnimation(int level)
        {
            if (level != 1)
            {
                woodStatsAnimation.Play(levelUpAnim.name);
            }
        }
    }
}