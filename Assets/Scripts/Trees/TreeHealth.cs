using Assets.Scripts.Core;
using Assets.Scripts.Pooling;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    public class TreeHealth : MonoBehaviour
    {
        [field: SerializeField] public int MaxHealth { get; set; } = 4;
        private TreeObject _thisTree;
        public int HealthPoints { get; set; }
        public Vector3 PlayerKickPos { get; private set; }

        private void Awake() => _thisTree = GetComponent<TreeObject>();

        public void ResetHealth() => HealthPoints = MaxHealth;

        public void TakeDamage(int damagePoints, Vector3 playerPos)
        {
            HealthPoints -= damagePoints;
            PlayerKickPos = playerPos;
            Debug.Log($"take damage func, health: {HealthPoints}");
            
            if (HealthPoints > 0)
            {
                EventMaster.PushEvent(EventStrings.TREE_KICKED);
                return;
            }

            HealthPoints = 0;
            EventMaster.PushEvent(EventStrings.TREE_CUTTED, _thisTree);
        }
    }
}