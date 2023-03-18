using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    public class TreeLootThrower : MonoBehaviour
    {
        [field: SerializeField] public int WoodCount { get; set; } = 2;
        private Tree _thisTree;

        private void Awake() => _thisTree = GetComponent<Tree>();

        private void OnEnable()
        {
            EventMaster.AddListener<Tree>(EventStrings.TREE_CUTTED, ThrowLootIfThisCutted);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<Tree>(EventStrings.TREE_CUTTED, ThrowLootIfThisCutted);
        }

        private void ThrowLootIfThisCutted(Tree tree)
        {
            if (_thisTree != tree)
            {
                return;
            }
            Debug.Log($"Throw {WoodCount} wood");
        }
    }
}