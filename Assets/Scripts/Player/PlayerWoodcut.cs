using Assets.Scripts.Core;
using Assets.Scripts.Settings;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerWoodcut : MonoBehaviour
    {
        [SerializeField] private Collider searchTrigger;
        private PlayerTreeSearcher _treeSearcher;
        private Rigidbody _rb;
        private bool _isCutting = false;
        private float _timeBetweenKicks;
        private float _timeSinceLastKick;
        private int _kickPoints;
        // таймер сбрасывается при каждом движении игрока

        private void Awake()
        {
            _treeSearcher = GetComponent<PlayerTreeSearcher>();
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _timeBetweenKicks = LevelSettings.Inst.PlayerStatsSettings.TimeBetweenKiks;
        }

        private void OnEnable()
        {
            EventMaster.AddListener(EventStrings.TREE_CUTTED, CheckTreeCountInRadius);
            EventMaster.AddListener<int>(EventStrings.LEVEL_UP, UpdateKickPoints);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener(EventStrings.TREE_CUTTED, CheckTreeCountInRadius);
            EventMaster.RemoveListener<int>(EventStrings.LEVEL_UP, UpdateKickPoints);
        }

        private void Update()
        {
            if (_rb.velocity == Vector3.zero && !_isCutting && _treeSearcher.TreeCount > 0)
            {
                SetCutPropertyBoolean(true);
            }
            else if (_rb.velocity != Vector3.zero && _isCutting)
            {
                SetCutPropertyBoolean(false);
            }
            UpdateKickTimer();
        }

        private void UpdateKickPoints(int level)
        {
            _kickPoints = LevelSettings.Inst.PlayerStatsSettings.GetPointsForLevel(level);
        }

        private void CheckTreeCountInRadius()
        {
            if (_treeSearcher.TreeCount == 0)
            {
                SetCutPropertyBoolean(false);
            }
        }

        private void SetCutPropertyBoolean(bool status)
        {
            _isCutting = status;
            EventMaster.PushEvent(EventStrings.CUT_PROPERTY_CHANGED, status);

            if (!_isCutting)
            {
                _timeSinceLastKick = 0f;
            }
        }

        private void UpdateKickTimer()
        {
            if (!_isCutting)
            {
                return;
            }

            _timeSinceLastKick += Time.deltaTime;

            if (_timeSinceLastKick < _timeBetweenKicks)
            {
                return;
            }
            _timeSinceLastKick = 0f;
            var tree = _treeSearcher.GetNearestAvailableTree();
            tree.Health.TakeDamage(_kickPoints);
            EventMaster.PushEvent(EventStrings.TREE_KICKED);
        }

        private void OnDrawGizmosSelected()
        {
            if (!searchTrigger)
            {
                return;
            }

            Handles.color = new Color(1, 0, 0, 0.1f);
            Handles.DrawSolidDisc(transform.position + Vector3.up * searchTrigger.bounds.center.y,
                Vector3.up, searchTrigger.bounds.extents.x);
        }
    }
}