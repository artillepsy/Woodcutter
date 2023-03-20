using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Класс для управления анимациями игрока через 
    /// установку полей аниматора
    /// </summary>
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private bool _isMoving = false;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponentInParent<Rigidbody>();
        }

        private void OnEnable()
        {
            EventMaster.AddListener(EventStrings.START_KICK, SetKickTrigger);
            EventMaster.AddListener<bool>(EventStrings.CUT_PROPERTY_CHANGED, SetCuttingBoolean);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener(EventStrings.START_KICK, SetKickTrigger);
            EventMaster.RemoveListener<bool>(EventStrings.CUT_PROPERTY_CHANGED, SetCuttingBoolean);

        }
        private void SetCuttingBoolean(bool status)
        {
            animator.SetBool(AnimatorStrings.IS_CUTTING, status);
        }

        private void SetKickTrigger()
        {
            animator.SetTrigger(AnimatorStrings.KICK);
        }

        private void Update()
        {
            var isMovingAtThisFrame = _rb.velocity != Vector3.zero;

            if (_isMoving != isMovingAtThisFrame)
            {
                animator.SetBool(AnimatorStrings.IS_MOVING, isMovingAtThisFrame);
                _isMoving = isMovingAtThisFrame;
            }
        }
    }
}