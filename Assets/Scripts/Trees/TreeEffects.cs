using Assets.Scripts.Core;
using Assets.Scripts.Effects;
using Assets.Scripts.Pooling;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    public class TreeEffects : MonoBehaviour
    {
        [SerializeField] private Effect kickPsPrefab;
        [SerializeField] private Transform effectsPlaceTarget;
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

            Debug.Log("Play");

            var inst = Pool.Get(kickPsPrefab.ID) as Effect;
            inst.transform.position = effectsPlaceTarget.position;
            inst.gameObject.SetActive(true);
            inst.Play();
        }
    }
}