﻿using Assets.Scripts.Core;
using Assets.Scripts.Pooling;
using Assets.Scripts.Trees;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Stumps
{
    /// <summary>
    /// Пень, появляющийся после срубки дерева и 
    /// вызывающий через промежуток времени ивент для респавна дерева.
    /// Автоматически убирает себя в пул объектов.
    /// </summary>
    public class Stump : PoolableMonoBehaviour
    {
        public int TreeID { get; private set; }

        public void StartSpawnTimer(TreeObject tree, float growDelay)
        {
            TreeID = tree.ID;
            StartCoroutine(DelayGrowCo(growDelay));
        }

        private IEnumerator DelayGrowCo(float delay)
        {
            yield return new WaitForSeconds(delay);
            EventMaster.PushEvent(EventStrings.STUMP_GROWED, this);
            Pool.Add(this);
        }
    }
}