using Assets.Scripts.Pooling;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    /// <summary>
    /// Главный класс дерева, агрегирующий все важные компоненты
    /// </summary>
    public class TreeObject : PoolableMonoBehaviour
    {
        [field: SerializeField] public TreeHealth Health { get; private set; }
        [field: SerializeField] public TreeLootThrower LootThrower { get; private set; }
        [SerializeField] private TreeAnimation animationPlayer;

        private void OnEnable() => Health.ResetHealth();

        private void OnDisable() => animationPlayer.ResetValues();

        public void StartGrow(float growDelay) => animationPlayer.PlayGrowAnimation(growDelay);
    }
}