using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem axePS;

        private void OnEnable()
        {
            EventMaster.AddListener(EventStrings.START_KICK, PlayAxeAnim);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener(EventStrings.START_KICK, PlayAxeAnim);
        }

        private void PlayAxeAnim()
        {
            axePS.Play();
        }
    }
}