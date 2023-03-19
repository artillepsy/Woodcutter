using Assets.Scripts.Core;
using Assets.Scripts.Settings;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerWoodcut : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float damageTimeOffset = 0.5f;
        private PlayerTreeSearcher _treeSearcher;
        private Rigidbody _rb;
        private bool _isCutting = false;
        private int _kickPoints;

        private float _animInterval;
        private float _animTime;

        // таймер сбрасывается при каждом движении игрока

        private void Awake()
        {
            _treeSearcher = GetComponent<PlayerTreeSearcher>();
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _animInterval = LevelSettings.Inst.PlayerStatsSettings.TimeBetweenKiks;
            _animTime = _animInterval;
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
            UpdateAnimationTimer();
        }

        private void UpdateKickPoints(int level)
        {
            _kickPoints = LevelSettings.Inst.PlayerStatsSettings.GetPointsForLevel(level);
        }

        private void SetCutPropertyBoolean(bool status)
        {
            _isCutting = status;
            EventMaster.PushEvent(EventStrings.CUT_PROPERTY_CHANGED, status);
        }
        private void Damage()
        {
            _treeSearcher.NearestTree.Health.TakeDamage(_kickPoints, transform.position);
        }

        private void UpdateAnimationTimer()
        {
            if (_isCutting && _animTime >= _animInterval)
            {
                EventMaster.PushEvent(EventStrings.START_KICK);
                Invoke(nameof(Damage), damageTimeOffset);
                _animTime = 0f;
            }
            _animTime += Time.deltaTime;
        }
    }
}