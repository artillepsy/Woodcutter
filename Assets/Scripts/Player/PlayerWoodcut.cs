using Assets.Scripts.Core;
using Assets.Scripts.Settings;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerWoodcut : MonoBehaviour
    {
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
            EventMaster.AddListener(EventStrings.TREE_CUTTED, () => SetCutPropertyBoolean(false));
            EventMaster.AddListener<int>(EventStrings.LEVEL_UP, UpdateKickPoints);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener(EventStrings.TREE_CUTTED, () => SetCutPropertyBoolean(false));
            EventMaster.RemoveListener<int>(EventStrings.LEVEL_UP, UpdateKickPoints);
        }

        private void Update()
        {
            if (_rb.velocity == Vector3.zero && !_isCutting && _treeSearcher.NearestTree)
            {
                SetCutPropertyBoolean(true);
            }
            else if (_rb.velocity != Vector3.zero && _isCutting || _isCutting && !_treeSearcher.NearestTree)
            {
                SetCutPropertyBoolean(false);
            }
            UpdateKickTimer();
        }

        private void UpdateKickPoints(int level)
        {
            _kickPoints = LevelSettings.Inst.PlayerStatsSettings.GetPointsForLevel(level);
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
            var tree = _treeSearcher.NearestTree;
            
            if (!tree || !tree.Health.Active)
            {
                return;
            }

            tree.Health.TakeDamage(_kickPoints, transform.position);
        }
    }
}