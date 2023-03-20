using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Класс, поворачивающий персонажа игрока к ближайшему (активному)
    /// дереву во время рубки
    /// </summary>
    public class PlayerRotationToTreeSetter : MonoBehaviour
    {
        [SerializeField] private float angleSpeed = 500f;
        private PlayerTreeSearcher _treeSearcher;
        private bool _isCutting = false;

        private void Awake()
        {
            _treeSearcher = GetComponent<PlayerTreeSearcher>();
        }

        private void OnEnable()
        {
            EventMaster.AddListener<bool>(EventStrings.CUT_PROPERTY_CHANGED, SetCuttingBoolean);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<bool>(EventStrings.CUT_PROPERTY_CHANGED, SetCuttingBoolean);
        }

        private void SetCuttingBoolean(bool status) => _isCutting = status;

        private void Update()
        {
            if (!_isCutting || !_treeSearcher.NearestTree)
            {
                return;
            }

            var treePos = _treeSearcher.NearestTree.transform.position;
            var lookDirection = (treePos - transform.position);
            var toRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            
            if (transform.rotation == toRotation)
            {
                return;
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, angleSpeed * Time.deltaTime);
        }
    }
}