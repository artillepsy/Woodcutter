using Assets.Scripts.Core;
using Assets.Scripts.Pooling;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    public class Tree : PoolableMonoBehaviour
    {
        [field: SerializeField] public TreeHealth Health { get; private set; }
        [field: SerializeField] public TreeLootThrower LootThrower { get; private set; }
        [SerializeField] private TreeAnimation anim;

        private void OnEnable()
        {
            Health.ResetHealth();
            EventMaster.AddListener<Tree>(EventStrings.TREE_CUTTED, AddToPoolIfThisCutted);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<Tree>(EventStrings.TREE_CUTTED, AddToPoolIfThisCutted);
        }

        public void StartGrow(float growTime) => StartCoroutine(GrowCo(growTime));

        private IEnumerator GrowCo(float growTime)
        {
            yield return new WaitForSeconds(growTime);
            anim.PlayGrowAnimation();
        }

        private void AddToPoolIfThisCutted(PoolableMonoBehaviour instance)
        {
            if (instance == this)
            {
                Pool.Add(this);
            }
        }
    }
}