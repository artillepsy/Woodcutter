using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 3f;
        [SerializeField] private float angleSpeed = 120f;
        private Vector3 _direction;
        private Rigidbody _rb;
        private BlockElement _movementBlocker = new BlockElement();

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            EventMaster.AddListener<Vector2>(EventStrings.JOYSTICK_INPUT, CacheDirection);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<Vector2>(EventStrings.JOYSTICK_INPUT, CacheDirection);
        }

        private void CacheDirection(Vector2 joystickData)
        {
            _direction = new Vector3(joystickData.x, 0, joystickData.y).normalized;
        }

        private void FixedUpdate()
        {
            if (_movementBlocker.IsBlocking || _direction == Vector3.zero)
            {
                _rb.velocity = Vector3.zero;
                return;
            }

            var toRotation = Quaternion.LookRotation(_direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, angleSpeed * Time.fixedDeltaTime);

            _rb.velocity = transform.forward * speed;
        }
    }
}
