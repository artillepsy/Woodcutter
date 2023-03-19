using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Pooling
{
    public class Pool : MonoBehaviour
    {
        private static Pool _poolInst;

        private List<PoolableMonoBehaviour> _prefabs = new List<PoolableMonoBehaviour>();
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

            _prefabs = GetPrefabsFromFolder();
            SpawnParents();
        }

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

        public static PoolableMonoBehaviour GetInstance(PoolableMonoBehaviour instance)
        {
            _poolInst._activeInstances.Add(instance);
            _poolInst._inactiveInstances.Remove(instance);
            instance.gameObject.SetActive(true);
            return instance;
        }

        public static void Add(PoolableMonoBehaviour instance)
        {
            instance.gameObject.SetActive(false);
            _poolInst._activeInstances.Remove(instance);
            _poolInst._inactiveInstances.Add(instance);
        }

        public static List<PoolableMonoBehaviour> GetActiveByType(PoolableType type)
        {
            return _poolInst._activeInstances.Where(inst => inst.Type == type).ToList();
        }

        private List<PoolableMonoBehaviour> GetPrefabsFromFolder()
        {
            var guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/_Prefabs" });
            var prefabs = new List<PoolableMonoBehaviour>();

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = AssetDatabase.LoadAssetAtPath<PoolableMonoBehaviour>(path);
                
                if (prefab)
                {
                    prefabs.Add(prefab);
                }
            }

            return prefabs;
        }

        private PoolableMonoBehaviour GetPrefabById(int id)
        {
            foreach (var obj in _prefabs)
            {
                if (obj.ID == id)
                {
                    return obj;
                }
            }
            return null;
        }

        private PoolableMonoBehaviour GetNewInstanceById(int id)
        {
            var instance = Instantiate(GetPrefabById(id));
            if (instance.Type != PoolableType.UI)
            {
                instance.transform.SetParent(_parents[instance.Type]);
            }
            _poolInst._activeInstances.Add(instance);
            instance.ID = id;
            return instance;
        }

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

        private void SpawnParents()
        {
            for (var i = 0; i < _prefabs.Count; i++)
            {
                _prefabs[i].ID = i + 1;

                if (_parents.ContainsKey(_prefabs[i].Type))
                {
                    continue;
                }

                var instance = new GameObject($"{_prefabs[i].Type}_parent");
                instance.transform.parent = transform;
                _parents.Add(_prefabs[i].Type, instance.transform);
            }
        }
    }
}