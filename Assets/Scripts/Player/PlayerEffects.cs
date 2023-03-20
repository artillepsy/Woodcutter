using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Класс управления эффектами, связанных с персонажем игрока
    /// </summary>
    public class PlayerEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem levelUpPS;

        private void OnEnable()
        {
            EventMaster.AddListener<int>(EventStrings.LEVEL_UP, PlayLevelUpAnimation);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<int>(EventStrings.LEVEL_UP, PlayLevelUpAnimation);
        }

        private void PlayLevelUpAnimation(int level)
        {
            if (level != 1)
            {
                levelUpPS.Play();
            }
        }
    }
}