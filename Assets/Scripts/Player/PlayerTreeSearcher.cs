using System.Collections.Generic;
using UnityEngine;
using Tree = Assets.Scripts.Trees.Tree;

namespace Assets.Scripts.Player
{
    public class PlayerTreeSearcher : MonoBehaviour
    {
        private List<Tree> _treesInRadius = new List<Tree>();
        public int TreeCount => _treesInRadius.Count;

        public Tree GetNearestAvailableTree()
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
            Tree nearestTree = _treesInRadius[0]; 

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


        private void OnTriggerEnter(Collider other)
        {
            var treeComponent = other.GetComponentInParent<Tree>();

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
            var treeComponent = other.GetComponentInParent<Tree>();

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