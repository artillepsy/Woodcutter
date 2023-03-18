using Assets.Scripts.Settings;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class LevelCounter : MonoBehaviour
    {
        private int _level;

        private void Start()
        {
            _level = LevelSettings.Inst.PlayerStatsSettings.StartLevel;
            EventMaster.PushEvent(EventStrings.LEVEL_UP, _level);
        }

        private void UpdateLevel()
        {
            _level++;
            EventMaster.PushEvent(EventStrings.LEVEL_UP, _level);
        }
    }
}