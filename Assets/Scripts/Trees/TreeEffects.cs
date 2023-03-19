using Assets.Scripts.Core;
using Assets.Scripts.Effects;
using Assets.Scripts.Pooling;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    public class TreeEffects : MonoBehaviour
    {
        [SerializeField] private List<EffectBinding> effectsBindings;
        [SerializeField] private TreeObject thisTree;

        private void OnEnable()
        {
            EventMaster.AddListener<TreeObject>(EventStrings.TREE_KICKED, SpawnParticlesIfThis);
            EventMaster.AddListener<TreeObject>(EventStrings.TREE_CUTTED, SpawnParticlesIfThis);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<TreeObject>(EventStrings.TREE_CUTTED, SpawnParticlesIfThis);
        }

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