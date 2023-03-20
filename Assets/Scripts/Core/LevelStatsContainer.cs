using Assets.Scripts.Settings;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Core
{
    /// <summary>
    /// класс, содержащий ключевые данные для уровня
    /// </summary>
    public class LevelStatsContainer : MonoBehaviour
    {
        [SerializeField] private WoodCountDisplay woodDisplay;
        [SerializeField] private LevelDisplay levelDisplay;
        public int Level { get; private set; }
        public int WoodCount { get; private set; }

        private int _needWoodCount => LevelSettings.Inst.PlayerStatsSettings.GetNeedWoodCountForLevel(Level);

        private void OnEnable()
        {
            EventMaster.AddListener<int>(EventStrings.TREE_LOOT_DROP, AddWoodCount);
            EventMaster.AddListener(EventStrings.LEVEL_UP_REQUEST, UpdateLevel);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<int>(EventStrings.TREE_LOOT_DROP, AddWoodCount);
            EventMaster.RemoveListener(EventStrings.LEVEL_UP_REQUEST, UpdateLevel);
        }

        /// <summary>
        /// Во время дроба подсчитывает количество дерева и вызывает соответствующий ивент.
        /// Также обновляет UI отображения дерева
        /// </summary>
        private void AddWoodCount(int count)
        {
            WoodCount += count;
            woodDisplay.UpdateWoodCount(WoodCount, _needWoodCount);
            
            if (WoodCount >= _needWoodCount)
            {
                EventMaster.PushEvent(EventStrings.READY_TO_UPGRADE_LEVEL);
            }
        }

        private void Start()
        {
            Level = LevelSettings.Inst.PlayerStatsSettings.StartLevel;
            EventMaster.PushEvent(EventStrings.LEVEL_UP, Level);
            woodDisplay.UpdateWoodCount(WoodCount, _needWoodCount);
        }

        /// <summary>
        /// Вызывает ивент обновлемя уровня персонажа
        /// </summary>
        private void UpdateLevel()
        {
            WoodCount -= _needWoodCount;
            Level++;
            EventMaster.PushEvent(EventStrings.LEVEL_UP, Level);
            woodDisplay.UpdateWoodCount(WoodCount, _needWoodCount);

            if (WoodCount >= _needWoodCount)
            {
                EventMaster.PushEvent(EventStrings.READY_TO_UPGRADE_LEVEL);
            }
        }
    }
}