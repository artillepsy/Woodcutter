using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerRotationToTreeSetter : MonoBehaviour
    {
        [SerializeField] private float angleSpeed = 500f;
        private PlayerTreeSearcher _treeSearcher;
        private bool _isCutting = false;
        private bool _isRotating = false;
        private Vector3 _currentTreePos;

        private void Awake()
        {
            _treeSearcher = GetComponent<PlayerTreeSearcher>();
            Debug.Log("Awake");
        }

        private void OnEnable()
        {
            Debug.Log("OnEnable");
            EventMaster.AddListener<bool>(EventStrings.CUT_PROPERTY_CHANGED, SetCuttingBoolean);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<bool>(EventStrings.CUT_PROPERTY_CHANGED, SetCuttingBoolean);
        }

        private void SetCuttingBoolean(bool status)
        {
            _isCutting = status;

            if (status)
            {
                _isRotating = true;
                _currentTreePos = _treeSearcher.GetNearestAvailableTree().transform.position;
            }
        }

        private void Update()
        {
            if (!_isCutting || !_isRotating)
            {
                return;
            }

            var lookDirection = (_currentTreePos - transform.position);
            var toRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            
            if (transform.rotation == toRotation)
            {
                _isRotating = false;
                return;
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, angleSpeed * Time.deltaTime);
        }
    }
}