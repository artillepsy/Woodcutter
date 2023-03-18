using Assets.Scripts.Core;
using Assets.Scripts.Settings;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerWoodcut : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float damageTimeValueAtInterval = 0.5f;
        private PlayerTreeSearcher _treeSearcher;
        private Rigidbody _rb;
        private bool _isCutting = false;
        private float _timeSinceLastTick;
        private float _tickTime;
        private bool _damagedAtThisInterval = false;
        private bool _kickEventWasPushed = false;
        private int _kickPoints;
        // таймер сбрасывается при каждом движении игрока

        private void Awake()
        {
            _treeSearcher = GetComponent<PlayerTreeSearcher>();
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _tickTime = LevelSettings.Inst.PlayerStatsSettings.TimeBetweenKiks;
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
            UpdateTickTimer();
        }

        private void UpdateKickPoints(int level)
        {
            _kickPoints = LevelSettings.Inst.PlayerStatsSettings.GetPointsForLevel(level);
        }

        private void SetCutPropertyBoolean(bool status)
        {
            _isCutting = status;
            _timeSinceLastTick = 0f;
            _damagedAtThisInterval = false;
            _kickEventWasPushed = false;

            EventMaster.PushEvent(EventStrings.CUT_PROPERTY_CHANGED, status);
        }

        private void UpdateTickTimer()
        {
            if (!_isCutting)
            {
                return;
            }
            var tree = _treeSearcher.NearestTree;
            
            if (!tree)
            {
                return;
            }
            if (!_kickEventWasPushed)
            {
                EventMaster.PushEvent(EventStrings.START_KICK);
                _kickEventWasPushed = true;
            }

            _timeSinceLastTick += Time.deltaTime;

            if (_timeSinceLastTick >= damageTimeValueAtInterval && !_damagedAtThisInterval)
            {
                tree.Health.TakeDamage(_kickPoints, transform.position);
                _damagedAtThisInterval = true;
                return;
            }

            if (_timeSinceLastTick < _tickTime)
            {
                return;
            }
            _timeSinceLastTick = 0f;
            _damagedAtThisInterval = false;
            _kickEventWasPushed = false;
        }
    }
}