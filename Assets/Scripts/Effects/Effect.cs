﻿using Assets.Scripts.Pooling;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    /// <summary>
    /// контейнер системы частиц с автоматическим добавлением
    /// себя в пул объектов после единоразового проигрывания
    /// </summary>
    public class Effect : PoolableMonoBehaviour
    {
        [SerializeField] private ParticleSystem ps;

        public void Play(float delay = 0f)
        {
            StartCoroutine(DelayedPlayCo(delay));
        }

        private IEnumerator DelayedPlayCo(float delay = 0f)
        {
            yield return new WaitForSeconds(delay);
            ps.Play();
            yield return new WaitUntil(() => !ps.isEmitting && ps.particleCount == 0);
            Pool.Add(this);
        }
    }
}