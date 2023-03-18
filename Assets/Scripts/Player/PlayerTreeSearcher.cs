using Assets.Scripts.Core;
using System.Collections.Generic;
using UnityEngine;
using TreeObject = Assets.Scripts.Trees.TreeObject;

namespace Assets.Scripts.Player
{
    public class PlayerTreeSearcher : MonoBehaviour
    {
        private List<TreeObject> _treesInRadius = new List<TreeObject>();
        public int TreeCount => _treesInRadius.Count;

        private void OnEnable()
        {
            EventMaster.AddListener<TreeObject>(EventStrings.TREE_CUTTED, RemoveTreeFromList);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<TreeObject>(EventStrings.TREE_CUTTED, RemoveTreeFromList);
        }

        public TreeObject GetNearestAvailableTree()
        {
            if (TreeCount == 0)
            {
                return null;
            }

            if (TreeCount == 1)
            {
                return _treesInRadius[0];
            }

            var minDistance = float.MaxValue;
            TreeObject nearestTree = _treesInRadius[0]; 

            foreach(var tree in _treesInRadius)
            {
                var currentDistance = (tree.transform.position - transform.position).sqrMagnitude;

                if (currentDistance >= minDistance)
                {
                    continue;
                }

                minDistance = currentDistance;
                nearestTree = tree;
            }

            return nearestTree;
        }

        private void RemoveTreeFromList(TreeObject tree)
        {
            if (_treesInRadius.Contains(tree))
            {
                _treesInRadius.Remove(tree);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            var treeComponent = other.GetComponentInParent<TreeObject>();

            if (!treeComponent)
            {
                return;
            }

            if (!_treesInRadius.Contains(treeComponent))
            {
                _treesInRadius.Add(treeComponent);
                Debug.Log($"Tree added. Count: {TreeCount}");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var treeComponent = other.GetComponentInParent<TreeObject>();

            if (!treeComponent)
            {
                return;
            }

            if (_treesInRadius.Contains(treeComponent))
            {
                _treesInRadius.Remove(treeComponent);
                Debug.Log($"Tree removed. Count: {TreeCount}");
            }
        }
    }
}