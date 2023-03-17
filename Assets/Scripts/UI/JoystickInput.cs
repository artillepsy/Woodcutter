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
            var x = joystick.Horizontal;
            var y = joystick.Vertical;

            // положение джойстика относительно центра. Значения от [-1; 1]
            var inputData = new Vector2(x, y); 
            EventMaster.PushEvent(EventStrings.JOYSTICK_INPUT, inputData);

            Debug.Log("Input");
        }
    }
}