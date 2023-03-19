using Assets.Scripts.Pooling;
using UnityEngine;
using Assets.Scripts.Trees;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Scripts.Player
{
    public class PlayerTreeSearcher : MonoBehaviour
    {
        [SerializeField] private float searchRadius = 3f;
        [SerializeField] private Transform helpCircle;
        public TreeObject NearestTree { get; private set; }
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            helpCircle.localScale = Vector3.one * searchRadius * 2f;
        }

        private void OnValidate()
        {
            if (helpCircle)
            {
                helpCircle.localScale = Vector3.one * searchRadius * 2f;
            }
        }

        private void Update()
        {
            if (_rb.velocity != Vector3.zero)
            {
                return;
            }

            UpdateTreeInfo();
        }

        private void UpdateTreeInfo()
        {
            var minDistance = searchRadius;
            var trees = Pool.GetActiveByType(PoolableType.Tree);
            NearestTree = null;

            foreach (var tree in trees)
            {
                if (!(tree as TreeObject).Health.Active)
                {
                    continue;
                }

                var currentDistance = (tree.transform.position - transform.position).magnitude;

                if (currentDistance >= minDistance)
                {
                    continue;
                }

                minDistance = currentDistance;
                NearestTree = tree as TreeObject;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = new Color(1, 0, 0, 0.1f);
            Handles.DrawSolidDisc(transform.position + Vector3.up * 0.1f, Vector3.up, searchRadius);
        }
#endif

    }
}