using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem levelUpPS;

        private void OnEnable()
        {
            EventMaster.AddListener<int>(EventStrings.LEVEL_UP, PlayAxeAnim);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<int>(EventStrings.LEVEL_UP, PlayAxeAnim);
        }

        private void PlayAxeAnim(int level)
        {
            if (level != 1)
            {
                levelUpPS.Play();
            }
        }
    }
}