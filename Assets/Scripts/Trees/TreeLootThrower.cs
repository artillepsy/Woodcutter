using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    public class TreeLootThrower : MonoBehaviour
    {
        [field: SerializeField] public int WoodCount { get; set; } = 2;
        [SerializeField] private TreeObject thisTree;

        private void OnEnable()
        {
            EventMaster.AddListener<TreeObject>(EventStrings.TREE_CUTTED, ThrowLootIfThisCutted);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<TreeObject>(EventStrings.TREE_CUTTED, ThrowLootIfThisCutted);
        }

        private void ThrowLootIfThisCutted(TreeObject tree)
        {
            if (thisTree != tree)
            {
                return;
            }

            EventMaster.PushEvent(EventStrings.TREE_LOOT_DROP, WoodCount);
        }
    }
}