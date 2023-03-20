using TreeObject = Assets.Scripts.Trees.TreeObject;
using Random = UnityEngine.Random;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Settings
{
    /// <summary>
    /// Контейнер настроек дерева для удобной нстройки 
    /// в инспекторе
    /// </summary>
    [Serializable]
    public class TreeStatsBinding
    {
        public TreeObject Prefab;
        [Min(1)] public int HealthPoints = 10;
        [Min(1)] public int LootCount = 10;
        [Range(0, 1)] public float SpawnProbability = 0.5f;

        public void InitValues()
        {
            Prefab.Health.MaxHealth = HealthPoints;
            Prefab.LootThrower.WoodCount = LootCount;
        }

        /// <summary>
        /// Получает префаб объекта в зависимости от вероятности выпадения.
        /// Учитывает вероятность выпадения каждого префаба в списке
        /// </summary>
        /// <param name="prefabs"></param>
        /// <returns></returns>
        public static TreeObject GetPrefabByProb(List<TreeStatsBinding> prefabs)
        {
            var total = 0f;

            foreach (var prefab in prefabs)
            {
                total += prefab.SpawnProbability;
            }

            var randomPoint = Random.value * total;

            for (int i = 0; i < prefabs.Count; i++)
            {
                if (randomPoint < prefabs[i].SpawnProbability)
                {
                    return prefabs[i].Prefab;
                }
                else
                {
                    randomPoint -= prefabs[i].SpawnProbability;
                }
            }

            return prefabs[prefabs.Count - 1].Prefab;
        }
    }
}