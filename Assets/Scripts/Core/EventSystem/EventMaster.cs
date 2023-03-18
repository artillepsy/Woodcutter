using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core
{
    /// <summary>
    /// Класс для обращения при вызове ивента или подписки на ивент
    /// </summary>
    public class EventMaster : MonoBehaviour
    {
        [SerializeField] private bool debug;
        private static bool _debug;
        /// <summary>
        /// список слушателей, где к каждому имени ивента привязаны свои обработчики
        /// </summary>
        private static Dictionary<string, List<Delegate>> _listeners = new Dictionary<string, List<Delegate>>();

        private void Awake()
        {
            _listeners.Clear();
            _debug = debug;
        }

        /// <summary>
        /// Отправка ивента всем слушателям. Вызывается в случае отсутствия аргументов
        /// </summary>
        /// <param name="eventName">Название ивента (из  EventStrings.cs)</param>
        public static void PushEvent(string eventName)
        {
            if (!_listeners.ContainsKey(eventName))
            {
                return;
            }

            var handlersToDelete = new List<Delegate>();
            var listenerCount = 0;

            foreach (var handler in _listeners[eventName])
            {
                if (handler == null)
                {
                    handlersToDelete.Add(handler);
                }
                else if (handler is Action action)
                {
                    action?.Invoke();
                    listenerCount++;
                }
            }

            foreach(var handler in handlersToDelete)
            {
                _listeners[eventName].Remove(handler);
            }

            if (_listeners[eventName].Count == 0)
            {
                _listeners.Remove(eventName);
            }

            if (_debug)
            {
                Debug.Log($"Push event [{eventName}] to {listenerCount} listeners");
            }
        }

        /// <summary>
        /// Отправка ивента всем слушателям. При вызове передаёт аргумент в функцию-обработчик. 
        /// В случае отсутствия у обработчика параметров, вызывает его функцию без них
        /// </summary>
        /// <param name="eventName">Название ивента (из  EventStrings.cs)</param>
        /// <param name="data">Данные для передачи</param>
        public static void PushEvent<T>(string eventName, T data)
        {
            if (!_listeners.ContainsKey(eventName))
            {
                return;
            }
            var listenerCount = 0;
            var handlerCount = 0;
            var handlersToDelete = new List<Delegate>();
            foreach (var handler in _listeners[eventName])
            {
                handlerCount++;
                if (handler == null)
                {
                    handlersToDelete.Add(handler);
                }
                else if (handler is Action<T> action)
                {
                    action?.Invoke(data);
                    listenerCount++;
                }
                else if (handler is Action action1)
                {
                    action1?.Invoke();
                    listenerCount++;
                }
            }

            foreach (var handler in handlersToDelete)
            {
                _listeners[eventName].Remove(handler);
            }

            if (_listeners[eventName].Count == 0)
            {
                _listeners.Remove(eventName);
            }

            if (_debug)
            {
                Debug.Log($"Push event [{eventName}] to {listenerCount} listeners, handlers: {handlerCount}");
            }
        }

        /// <summary>
        /// Добавление обработчика без параметров на ивент
        /// </summary>
        /// <param name="eventName">Название ивента (из  EventStrings.cs)</param>
        /// <param name="handler">Функция-обработчик без параметров</param>
        public static void AddListener(string eventName, Action handler)
        {
            if (!_listeners.ContainsKey(eventName))
            {
                _listeners[eventName] = new List<Delegate>();
            }

            _listeners[eventName].Add(handler);

            if (_debug)
            {
                Debug.Log($"Key existance: {_listeners[eventName].Contains(handler)}," +
                    $" handler: {handler.Method.Name}, class: {handler.Method.DeclaringType}");
            }
        }

        /// <summary>
        /// Добавление обработчика с параметрами на ивент
        /// </summary>
        /// <param name="eventName">Название ивента (из  EventStrings.cs)</param>
        /// <param name="handler">Функция-обработчик без параметров</param>
        public static void AddListener<T>(string eventName, Action<T> handler)
        {
            if (!_listeners.ContainsKey(eventName))
            {
                _listeners[eventName] = new List<Delegate>();
            }

            _listeners[eventName].Add(handler);
            
            if (_debug)
            {
                Debug.Log($"Key existance: {_listeners[eventName].Contains(handler)}," +
                    $" handler: {handler.Method.Name}, class: {handler.Method.DeclaringType}");
            }
        }

        /// <summary>
        /// Отписка обработчика без параметров от ивента
        /// </summary>
        /// <param name="eventName">Название ивента (из  EventStrings.cs)</param>
        /// <param name="handlerToRemove">Функция-обработчик без параметров</param>
        public static void RemoveListener(string eventName, Action handlerToRemove)
        {
            if (!_listeners.ContainsKey(eventName))
            {
                return;
            }

            foreach(var handler in _listeners[eventName])
            {
                if (handler.Method.Equals(handlerToRemove.Method))
                {
                    continue;
                }
                _listeners[eventName].Remove(handler);
                
                if (_listeners[eventName].Count == 0)
                {
                    _listeners.Remove(eventName);
                }

                break;
            }
        }

        /// <summary>
        /// Отписка обработчика с параметрами от ивента
        /// </summary>
        /// <param name="eventName">Название ивента (из  EventStrings.cs)</param>
        /// <param name="handlerToRemove">Функция-обработчик без параметров</param>
        public static void RemoveListener<T>(string eventName, Action<T> handlerToRemove)
        {
            if (!_listeners.ContainsKey(eventName))
            {
                return;
            }

            foreach (var handler in _listeners[eventName])
            {
                if (handler.Method.Equals(handlerToRemove.Method))
                {
                    continue;
                }
                _listeners[eventName].Remove(handler);

                if (_listeners[eventName].Count == 0)
                {
                    _listeners.Remove(eventName);
                }

                break;
            }
        }
    }
}