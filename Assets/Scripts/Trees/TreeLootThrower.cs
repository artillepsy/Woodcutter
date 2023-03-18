using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    public class TreeLootThrower : MonoBehaviour
    {
        [field: SerializeField] public int WoodCount { get; set; } = 2;
        private TreeObject _thisTree;

        private void Awake() => _thisTree = GetComponent<TreeObject>();

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
            if (_thisTree != tree)
            {
                return;
            }
            Debug.Log($"Throw {WoodCount} wood");
        }
    }
}