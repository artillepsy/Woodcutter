﻿using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    /// <summary>
    /// Класс здоровья дерева. Вызывает ивенты при
    /// ударе по нему или при срубке
    /// </summary>
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
            Debug.Log("Hitted");
            HealthPoints -= damagePoints;
            PlayerKickPos = playerPos;
            
            if (HealthPoints > 0)
            {
                EventMaster.PushEvent(EventStrings.TREE_KICKED, thisTree);
                return;
            }

            HealthPoints = 0;
            EventMaster.PushEvent(EventStrings.TREE_CUTTED, thisTree);
            Debug.DrawRay(transform.position, Vector3.one * 10f, Color.green, 5f);

        }
    }
}