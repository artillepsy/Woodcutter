using Assets.Scripts.Core;
using Assets.Scripts.Pooling;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    public class TreeAnimation : MonoBehaviour
    {
        [SerializeField] private Transform meshHolder;
        [SerializeField] private Transform temporalMeshHolder;
        [SerializeField] private Collider treeCollider;
        [SerializeField] private TreeObject thisTree;
        [Space]
        [SerializeField] private float fallDownTime = 1f;
        [SerializeField] private float rotateDegrees = 70f;
        [SerializeField] private AnimationCurve fallRotationLerpCurve;
        [Space]
        [SerializeField] private float growTime = 1f;
        [SerializeField] private AnimationCurve scaleLerpCurve;
        [SerializeField] private TreeEffects treeEffects;
        private Vector3 _endMeshScale;
        private bool _firstGrow = true;

        private void Awake()
        {
            _endMeshScale = meshHolder.localScale;
            thisTree = GetComponent<TreeObject>();
        }

        private void OnEnable()
        {
            EventMaster.AddListener<TreeObject>(EventStrings.TREE_CUTTED, StartCutAnimationIfThis);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<TreeObject>(EventStrings.TREE_CUTTED, StartCutAnimationIfThis);
        }

        public void ResetValues()
        {
            treeCollider.enabled = true;
            meshHolder.rotation = Quaternion.identity;
        }

        public void PlayGrowAnimation(float growDelay)
        {
            StartCoroutine(GrowCo(growDelay));
        }

        private void StartCutAnimationIfThis(TreeObject instance)
        {
            if (instance != thisTree)
            {
                return;
            }
            if (!gameObject.activeSelf)
            {
                return;
            }

            StartCoroutine(FlyDownCo());
        }

        private IEnumerator FlyDownCo()
        {
            treeCollider.enabled = false;

            var time = 0f;
            var oldParent = meshHolder.parent;
            var playerPos = thisTree.Health.PlayerKickPos;

            var startForward = (transform.position - playerPos).normalized;
            temporalMeshHolder.rotation = Quaternion.LookRotation(startForward, Vector3.up);
            var endForward = Quaternion.AngleAxis(rotateDegrees, temporalMeshHolder.right) * startForward;
            meshHolder.SetParent(temporalMeshHolder);

            while (time < fallDownTime)
            {
                var lerpValue = fallRotationLerpCurve.Evaluate(time / fallDownTime);
                temporalMeshHolder.forward = Vector3.Lerp(startForward, endForward, lerpValue);
                time += Time.deltaTime;
                yield return null;
            }
            meshHolder.SetParent(oldParent);
            treeEffects.SpawnGrowParticles();
            Pool.Add(thisTree);
        }

        private IEnumerator GrowCo(float growDelay)
        {
            var startScale = Vector3.zero;
            meshHolder.localScale = startScale;

            yield return new WaitForSeconds(growDelay);

            var time = 0f;

            while (time < growTime)
            {
                var lerpValue = scaleLerpCurve.Evaluate(time / growTime);
                meshHolder.localScale = Vector3.Lerp(startScale, _endMeshScale, lerpValue);
                time += Time.deltaTime;
                yield return null;
            }

            meshHolder.localScale = _endMeshScale;

            if (_firstGrow)
            {
                _firstGrow = false;
                yield break;
            }

            EventMaster.PushEvent(EventStrings.TREE_GROWED, thisTree);
        }
    }
}