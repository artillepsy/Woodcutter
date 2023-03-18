using Assets.Scripts.Core;
using Assets.Scripts.Pooling;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    public class TreeAnimation : MonoBehaviour
    {
        [SerializeField] private Transform meshHolder;
        [SerializeField] private Collider treeCollider;
        [SerializeField] private TreeObject thisTree;
        [Space]
        [SerializeField] private float fallDownTime = 1f;
        [SerializeField] private float rotateDegrees = 70f;
        [SerializeField] private AnimationCurve fallRotationLerpCurve;
        [Space]
        [SerializeField] private float growTime = 1f;
        [SerializeField] private AnimationCurve scaleLerpCurve;
        private Vector3 _endMeshScale;

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
            var playerPos = thisTree.Health.PlayerKickPos;

            var localUpDirection = (transform.position - playerPos).normalized;
            var localRightDirection = Quaternion.Euler(0, 90, 0) * localUpDirection;
            var localForwardDirection = Quaternion.AngleAxis(rotateDegrees, localRightDirection) * localUpDirection;

            var startRotation = meshHolder.rotation;
            var endRotation = Quaternion.LookRotation(localForwardDirection, localUpDirection);
            var time = 0f;

            while (time < fallDownTime)
            {
                var lerpValue = fallRotationLerpCurve.Evaluate(time / fallDownTime);
                meshHolder.rotation = Quaternion.Lerp(startRotation, endRotation, lerpValue);
                time += Time.deltaTime;
                yield return null;
            }

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
        }
    }
}