using Assets.Scripts.Core;
using Assets.Scripts.Pooling;
using Assets.Scripts.Settings;
using Assets.Scripts.Stumps;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    /// <summary>
    /// Спавнер деревьев. При старте сцены заполняет 
    /// участок игрового поля деревьями 
    /// </summary>
    public class TreeSpawner : MonoBehaviour
    {
        [SerializeField] private GridData gridData;

        [SerializeField, Space] private bool toggleGizmos = true;
        [SerializeField] private float gizmoSphereRadius = 1f;

        private Quaternion RandomRotation => Quaternion.Euler(0, Random.Range(0, 360), 0);

        private void OnEnable()
        {
            EventMaster.AddListener<Stump>(EventStrings.STUMP_GROWED, RespawnTree);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<Stump>(EventStrings.STUMP_GROWED, RespawnTree);
        }

        private void Start()
        {
            SpawnTrees();
        }

        /// <summary>
        /// Метод спавна деревьев в пределах сетки в начале уровня
        /// </summary>
        private void SpawnTrees()
        {
            var settings = LevelSettings.Inst.TreeSettings;
            var spawnCount = settings.RandomSpawnCount;
            var spawnPoints = gridData.GetSpawnPoints();

            for(var i = 0; i < spawnCount; i++)
            {
                var spawnPoint = GetSpawnPointWithExclude(spawnPoints);
                var growDelay = settings.RandomStartGrowTime;
                var randomPrefab = settings.GetRandomPrefab();
                SpawnTree(spawnPoint, growDelay, randomPrefab);
            }
        }

        /// <summary>
        /// Метод, спавнящий дерево на месте пня
        /// </summary>
        private void RespawnTree(Stump stump)
        {
            var inst = Pool.Get(stump.TreeID) as TreeObject;
            inst.transform.position = stump.transform.position;
            inst.transform.rotation = stump.transform.rotation;
            inst.gameObject.SetActive(true);
            inst.StartGrow(0f);
        }

        /// <summary>
        /// Спавн дерева 
        /// </summary>
        private void SpawnTree(Vector3 spawnPoint, float growDelay, TreeObject prefab)
        {
            var instance = Pool.Get(prefab.ID) as TreeObject;
            instance.transform.position = spawnPoint;
            instance.transform.rotation = RandomRotation;
            instance.StartGrow(growDelay);
        }

        /// <summary>
        /// Берёт точки из списка и убирает их оттуда для исключения повтора
        /// позиции при спавне
        /// </summary>
        private Vector3 GetSpawnPointWithExclude(List<Vector3> spawnPoints)
        {
            if (spawnPoints.Count == 0)
            {
                Debug.LogError("No available points to spawn");
                return Vector3.zero;
            }
            
            var randomPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            spawnPoints.Remove(randomPoint);
            return randomPoint;
        }

        private void OnDrawGizmos()
        {
            if (!toggleGizmos || !gridData.spawnCenter)
            {
                return;
            }

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(gridData.spawnCenter.position, gizmoSphereRadius);
            Gizmos.color = Color.green;

            foreach(var p in gridData.GetSpawnPoints())
            {
                Gizmos.DrawSphere(p, gizmoSphereRadius);
            }
        }
    }
}