using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    public class TreeHealth : MonoBehaviour
    {
        [field: SerializeField] public int MaxHealth { get; set; } = 4;
        [SerializeField] private TreeObject thisTree;
        public Vector3 PlayerKickPos { get; private set; }
        public int HealthPoints { get; set; }
        public bool Active => HealthPoints > 0;

        public void ResetHealth() => HealthPoints = MaxHealth;

        public void TakeDamage(int damagePoints, Vector3 playerPos)
        {
            if (!Active)
            {
                return;
            }

            HealthPoints -= damagePoints;
            PlayerKickPos = playerPos;
            
            if (HealthPoints > 0)
            {
                EventMaster.PushEvent(EventStrings.TREE_KICKED);
                return;
            }

            HealthPoints = 0;
            EventMaster.PushEvent(EventStrings.TREE_CUTTED, thisTree);
            Debug.DrawRay(transform.position, Vector3.one * 10f, Color.green, 5f);

        }
    }
}