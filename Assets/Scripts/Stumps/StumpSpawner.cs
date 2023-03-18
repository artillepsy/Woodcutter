using Assets.Scripts.Core;
using Assets.Scripts.Pooling;
using Assets.Scripts.Settings;
using Assets.Scripts.Trees;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Stumps
{
    public class StumpSpawner : MonoBehaviour
    {
        [SerializeField] private Stump stumpPrefab;
        private void OnEnable()
        {
            EventMaster.AddListener<TreeObject>(EventStrings.TREE_CUTTED, SpawnStump);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<TreeObject>(EventStrings.TREE_CUTTED, SpawnStump);
        }

        private void SpawnStump(TreeObject tree)
        {
            var instance = Pool.Get(stumpPrefab.ID) as Stump;
            instance.transform.position = tree.transform.position;
            instance.transform.rotation = tree.transform.rotation;
            instance.gameObject.SetActive(true);
            instance.StartSpawnTimer(tree, LevelSettings.Inst.TreeSettings.RandomGrowTime);
        }
    }
}