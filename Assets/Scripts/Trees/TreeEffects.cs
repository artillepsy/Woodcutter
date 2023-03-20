using Assets.Scripts.Core;
using Assets.Scripts.Effects;
using Assets.Scripts.Pooling;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    /// <summary>
    /// Класс, отвечающий за эффекты во время событий,
    /// связанных с деревом
    /// </summary>
    public class TreeEffects : MonoBehaviour
    {
        [SerializeField] private EffectBinding growEffectBinding;
        [SerializeField] private List<EffectBinding> effectsBindings;
        [SerializeField] private TreeObject thisTree;

        private void OnEnable()
        {
            EventMaster.AddListener<TreeObject>(EventStrings.TREE_KICKED, SpawnParticlesIfThis);
            EventMaster.AddListener<TreeObject>(EventStrings.TREE_CUTTED, SpawnParticlesIfThis);
            EventMaster.AddListener<TreeObject>(EventStrings.TREE_GROWED, SpawnGrowPartilesIfThis);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<TreeObject>(EventStrings.TREE_KICKED, SpawnParticlesIfThis);
            EventMaster.RemoveListener<TreeObject>(EventStrings.TREE_CUTTED, SpawnParticlesIfThis);
            EventMaster.RemoveListener<TreeObject>(EventStrings.TREE_GROWED, SpawnGrowPartilesIfThis);
        }

        /// <summary>
        /// После роста дерева появляется эффект опавших листьев
        /// </summary>
        public void SpawnGrowParticles()
        {
            var inst = Pool.Get(growEffectBinding.EffectPrefab.ID) as Effect;
            inst.transform.position = growEffectBinding.PlaceTarget.position;
            inst.gameObject.SetActive(true);
            inst.Play();
        }

        private void SpawnGrowPartilesIfThis(TreeObject tree)
        {
            if (thisTree != tree)
            {
                return;
            }

            SpawnGrowParticles();
        }

        /// <summary>
        /// Спавнит все системы частиц из списка
        /// </summary>
        /// <param name="tree"></param>
        private void SpawnParticlesIfThis(TreeObject tree)
        {
            if (thisTree != tree)
            {
                return;
            }

            foreach(var binding in effectsBindings)
            {
                var inst = Pool.Get(binding.EffectPrefab.ID) as Effect;
                inst.transform.position = binding.PlaceTarget.position;
                inst.gameObject.SetActive(true);
                inst.Play();
            }
        }
    }

    [Serializable]
    public class EffectBinding
    {
        [field: SerializeField] public Effect EffectPrefab { get; private set; }
        [field: SerializeField] public Transform PlaceTarget { get; private set; }
    }
}