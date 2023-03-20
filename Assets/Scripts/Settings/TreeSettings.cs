using TreeObject = Assets.Scripts.Trees.TreeObject;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Settings
{
    /// <summary>
    /// Содержит настройки характеристик деревьев, их спавна и роста
    /// </summary>
    [CreateAssetMenu(menuName = nameof(TreeSettings), fileName = nameof(TreeSettings))]
    public class TreeSettings : ScriptableObject
    {
        // общий лимит спавна деревьев на сцене
        [SerializeField, Min(0)] private int MinSpawnCount = 5;
        [SerializeField] private int MaxSpawnCount = 10;

        // время, через которое дерево снова прорастёт
        [SerializeField, Space, Min(0)] private float MinGrowTime = 20;
        [SerializeField, Space] private float MaxGrowTime = 30;

        // стартовая задержка роста деревьев для красивого хаотичного появления на карте
        [SerializeField, Space, Min(0)] private float MinStartGrowTime = 4;
        [SerializeField, Space] private float MaxStartGrowTime = 30;

        [SerializeField, Space] private List<TreeStatsBinding> treePrefabsBindings; 

        public int RandomSpawnCount => Random.Range(MinSpawnCount, MaxSpawnCount);
        public float RandomGrowTime => Random.Range(MinGrowTime, MaxGrowTime);
        public float RandomStartGrowTime => Random.Range(MinStartGrowTime, MaxStartGrowTime);

        /// <summary>
        /// Настройка префабов деревьев
        /// </summary>
        public void InitBindings()
        {
            treePrefabsBindings.ForEach(b => b.InitValues());
        }


        public TreeObject GetRandomPrefab()
        {
            return TreeStatsBinding.GetPrefabByProb(treePrefabsBindings);
        }
    }
}