using Assets.Scripts.Core;
using Assets.Scripts.Pooling;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    public class TreeAnimation : MonoBehaviour
    {
        [SerializeField] private Transform meshHolder;
        [SerializeField] private float flyDownTime = 1f;
        [SerializeField] private float rotateDegrees = 70f;
        private TreeObject _thisTree;

        private void Awake()
        {
            _thisTree = GetComponent<TreeObject>();
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
            meshHolder.rotation = Quaternion.identity;
        }

        public void PlayGrowAnimation(float growTime)
        {
            StartCoroutine(GrowCo(growTime));
            Debug.Log("Play grow animation");
        }

        private void StartCutAnimationIfThis(TreeObject instance)
        {
            if (instance != _thisTree)
            {
                return;
            }

            StartCoroutine(FlyDownCo());
        }

        private IEnumerator FlyDownCo()
        {
            Debug.Log("FlyDownCO");
            var playerPos = _thisTree.Health.PlayerKickPos;
            var fallDirection = (transform.position - playerPos).normalized;
            // var rotateVector = Quaternion.Euler(0, 90, 0) * fallDirection;

            var startRotation = meshHolder.rotation;
            var toRotation = Quaternion.LookRotation(Vector3.down, fallDirection);
            var time = 0f;
            var angleSpeed = rotateDegrees / flyDownTime;

            while (time < flyDownTime)
            {
                meshHolder.rotation = Quaternion.RotateTowards(startRotation, toRotation, angleSpeed * Time.deltaTime);
                time += Time.deltaTime;
                yield return null;
            }

            Pool.Add(_thisTree);
        }

        private IEnumerator GrowCo(float growTime)
        {
            yield return new WaitForSeconds(growTime);
        }
    }
}