using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    public class LevelSettings : MonoBehaviour
    {
        public static LevelSettings Inst { get; private set; }
        [field: SerializeField] public TreeSettings TreeSettings { get; private set; }
        [field: SerializeField] public PlayerStatsSettings PlayerStatsSettings { get; private set; }

        private void Awake()
        {
            if (Inst == null)
            {
                Inst = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            TreeSettings.InitBindings();
        }
    }
}