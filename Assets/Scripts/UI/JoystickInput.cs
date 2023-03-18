using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Класс ввода при помощи джойстика
    /// </summary>
    public class JoystickInput : MonoBehaviour
    {
        [SerializeField] private Joystick joystick;

        /// <summary>
        /// Проверка ввода
        /// </summary>
        private void Update()
        {
            EventMaster.PushEvent(EventStrings.JOYSTICK_INPUT, joystick.Direction);
        }
    }
}