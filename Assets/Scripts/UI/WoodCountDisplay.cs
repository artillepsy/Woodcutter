using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class WoodCountDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI woodCountLabel;
        [SerializeField] private Image sliderImage;

        public void UpdateWoodCount(int count, int needCountToNextLevel)
        {
            woodCountLabel.text = $"{count} / {needCountToNextLevel}";
            sliderImage.fillAmount = (float)count / needCountToNextLevel;
        }
    }
}