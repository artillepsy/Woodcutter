using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Player
{
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
            EventMaster.AddListener<bool>(EventStrings.ANIMATOR_CUT, SetCuttingBoolean);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<bool>(EventStrings.ANIMATOR_CUT, SetCuttingBoolean);
        }

        private void SetCuttingBoolean(bool status)
        {
            animator.SetBool(AnimatorStrings.IS_CUTTING, status);
        }

        private void Update()
        {
            var isMovingAtThisFrame = _rb.velocity != Vector3.zero;

            if (_isMoving != isMovingAtThisFrame)
            {
                animator.SetBool(AnimatorStrings.IS_CUTTING, isMovingAtThisFrame);
                _isMoving = isMovingAtThisFrame;
            }
        }
    }
}