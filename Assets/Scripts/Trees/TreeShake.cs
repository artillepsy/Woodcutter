using Assets.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    public class TreeShake : MonoBehaviour
    {
        [SerializeField] private Transform meshHolder;
        [SerializeField] private TreeObject thisTree;
        [SerializeField, Min(0)] private float maxRotationAngle = 5f;
        [SerializeField, Min(0)] private float rotationTime = 0.4f;
        [SerializeField, Min(0)] private int rotationPoints = 3;
        private float _randomAngle => Random.Range(-maxRotationAngle, maxRotationAngle);

        private Vector3 _randomUp => Quaternion.Euler(_randomAngle, _randomAngle, _randomAngle) * meshHolder.up;


        private void OnEnable()
        {
            EventMaster.AddListener<TreeObject>(EventStrings.TREE_KICKED, RotateIfThis);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<TreeObject>(EventStrings.TREE_KICKED, RotateIfThis);
        }

        private void RotateIfThis(TreeObject instance)
        {
            if (thisTree != instance)
            {
                return;
            }

            StartCoroutine(RotateMeshCo());
        }

        private IEnumerator RotateMeshCo()
        {
            var upwards = new List<Vector3>();
            var timeBetweenTwoPoints = rotationTime / (rotationPoints + 1);
            int i;
            for(i = 0; i < rotationPoints; i++)
            {
                upwards.Add(_randomUp);
            }

            upwards.Insert(0, meshHolder.up);
            upwards.Insert(upwards.Count-1, meshHolder.up);

            var time = 0f;
            i = 0;
            
            var angle = Vector3.Angle(upwards[0], upwards[1]);
            angle *= timeBetweenTwoPoints;
            Debug.Log($"angle {angle}");

            while (true)
            {
                meshHolder.up = Vector3.MoveTowards(upwards[i], upwards[i + 1], angle * Time.deltaTime);
                time += Time.deltaTime;

                if (time < timeBetweenTwoPoints)
                {
                    yield return null;
                    continue;
                }

                time = 0f;
                i++;

                if (i == upwards.Count - 1)
                {
                    break;
                }
                angle = Vector3.Angle(upwards[i], upwards[i + 1]);
                angle *= timeBetweenTwoPoints;
                Debug.Log($"angle {angle / timeBetweenTwoPoints}");

            }
        }
    }
}