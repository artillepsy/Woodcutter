using UnityEngine;

namespace Assets.Scripts.Settings
{
    [CreateAssetMenu(menuName = nameof(PlayerStatsSettings), fileName = nameof(PlayerStatsSettings))]
    public class PlayerStatsSettings : ScriptableObject
    {
        [field: SerializeField, Min(1)] public int StartLevel { get; private set; } = 1;
        [field: SerializeField, Space, Min(0.1f)] public float TimeBetweenKiks { get; private set; } = 1f;

        [field: SerializeField, Space, Min(1)] public int StartKickPoints { get; private set; } = 1;
        [field: SerializeField, Min(1)] public int IncrementKickPointsPerLevel { get; private set; } = 1;

        [field: SerializeField, Space, Min(1)] public int StartWoodCountToNextLevel { get; private set; } = 2;
        [field: SerializeField, Min(1)] public int WoodIncrementCountPerLevel { get; private set; } = 2;


        public int GetPointsForLevel(int level)
        {
            return StartKickPoints + (level - 1) * IncrementKickPointsPerLevel;
        }

        public int GetNeedWoodCountForLevel(int level)
        {
            return StartWoodCountToNextLevel + (level - 1) * WoodIncrementCountPerLevel;
        }
    }
}