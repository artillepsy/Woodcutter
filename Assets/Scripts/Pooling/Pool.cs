using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Pooling
{
    /// <summary>
    /// Контейнер заспавенных объектов для их многократного использования
    /// без уничтожения. Содержит в себе все управляемые префабы и через него происходит 
    /// спавн всех объектов такого типа на сцене.
    /// </summary>
    public class Pool : MonoBehaviour
    {
        private static Pool _poolInst;

        [SerializeField] private List<PoolableMonoBehaviour> prefabs;
        private List<PoolableMonoBehaviour> _inactiveInstances = new List<PoolableMonoBehaviour>();
        private List<PoolableMonoBehaviour> _activeInstances = new List<PoolableMonoBehaviour>();
        private Dictionary<PoolableType, Transform> _parents = new Dictionary<PoolableType, Transform>();

        private void Awake()
        {
            if (_poolInst == null)
            {
                _poolInst = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            SpawnParents();
        }

        /// <summary>
        /// Метод даёт ссылку на заспавненный или существующий объект,
        /// подходящий по id
        /// </summary>
        public static PoolableMonoBehaviour Get(int id)
        {
            if (_poolInst._inactiveInstances.Count == 0)
            {
                return _poolInst.GetNewInstanceById(id);
            }

            if (_poolInst.TryGetInstById(id, out var inst))
            {
                return inst;
            }
            return _poolInst.GetNewInstanceById(id);
        }

        /// <summary>
        /// Добавляет объект в список неактивных и деактивирует его
        /// </summary>
        public static void Add(PoolableMonoBehaviour instance)
        {
            instance.gameObject.SetActive(false);
            _poolInst._activeInstances.Remove(instance);
            _poolInst._inactiveInstances.Add(instance);
        }

        /// <summary>
        /// Возвращает все активные объекты определённого типа в виде списка
        /// </summary>
        public static List<PoolableMonoBehaviour> GetActiveByType(PoolableType type)
        {
            return _poolInst._activeInstances.Where(inst => inst.Type == type).ToList();
        }

        /// <summary>
        /// Поиск и возврат префаба, подходящего по ID
        /// </summary>
        private PoolableMonoBehaviour GetPrefabById(int id)
        {
            foreach (var obj in prefabs)
            {
                if (obj.ID == id)
                {
                    return obj;
                }
            }
            return null;
        }

        /// <summary>
        /// Метод спавна нового объекта 
        /// </summary>
        private PoolableMonoBehaviour GetNewInstanceById(int id)
        {
            var instance = Instantiate(GetPrefabById(id));
            instance.transform.SetParent(_parents[instance.Type]);
            _poolInst._activeInstances.Add(instance);
            instance.ID = id;
            return instance;
        }
        
        /// <summary>
        /// Попытка получить ссылку на заспавненный неактивный объект.
        /// В том случае, если нет таких, возвращает false
        /// </summary>
        private bool TryGetInstById(int id, out PoolableMonoBehaviour instane)
        {
            foreach (var obj in _poolInst._inactiveInstances)
            {
                if (obj.ID != id)
                {
                    continue;
                }

                _poolInst._inactiveInstances.Remove(obj);
                _poolInst._activeInstances.Add(obj);
                instane = obj;
                return true;
            }
            instane = null;
            return false;
        }

        /// <summary>
        /// спавн объектов-родителей для каждого типа управляемых объектов.
        /// Это помогает лучше организовать иерархию сцены во время выполнения
        /// </summary>
        private void SpawnParents()
        {
            for (var i = 0; i < prefabs.Count; i++)
            {
                prefabs[i].ID = i + 1;

                if (_parents.ContainsKey(prefabs[i].Type))
                {
                    continue;
                }

                var instance = new GameObject($"{prefabs[i].Type}_parent");
                instance.transform.parent = transform;
                _parents.Add(prefabs[i].Type, instance.transform);
            }
        }
    }
}