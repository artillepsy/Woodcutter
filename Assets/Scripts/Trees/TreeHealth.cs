using Assets.Scripts.Core;
using Assets.Scripts.Pooling;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    public class TreeHealth : MonoBehaviour
    {
        [field: SerializeField] public int MaxHealth { get; set; } = 4;
        private Tree _thisTree;
        public int HealthPoints { get; set; }

        private void Awake() => _thisTree = GetComponent<Tree>();

        public void ResetHealth() => HealthPoints = MaxHealth;

        public void TakeDamage(int damagePoints)
        {
            HealthPoints -= damagePoints;
            
            if (HealthPoints > 0)
            {
                return;
            }

            HealthPoints = 0;
            EventMaster.PushEvent(EventStrings.TREE_CUTTED, _thisTree);
        }
    }
}