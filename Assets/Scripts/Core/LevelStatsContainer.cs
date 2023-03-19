using Assets.Scripts.Settings;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class LevelStatsContainer : MonoBehaviour
    {
        [SerializeField] private WoodCountDisplay woodDisplay;
        public int Level { get; private set; }
        public int WoodCount { get; private set; }

        private int _needWoodCount => LevelSettings.Inst.PlayerStatsSettings.GetNeedWoodCountForLevel(Level);

        private void OnEnable()
        {
            EventMaster.AddListener<int>(EventStrings.TREE_LOOT_DROP, AddWoodCount);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<int>(EventStrings.TREE_LOOT_DROP, AddWoodCount);
        }

        private void AddWoodCount(int count)
        {
            WoodCount += count;
            woodDisplay.UpdateWoodCount(WoodCount, _needWoodCount);
        }

        private void Start()
        {
            Level = LevelSettings.Inst.PlayerStatsSettings.StartLevel;
            EventMaster.PushEvent(EventStrings.LEVEL_UP, Level);
            woodDisplay.UpdateWoodCount(WoodCount, _needWoodCount);
        }

        private void UpdateLevel()
        {
            Level++;
            EventMaster.PushEvent(EventStrings.LEVEL_UP, Level);
        }
    }
}