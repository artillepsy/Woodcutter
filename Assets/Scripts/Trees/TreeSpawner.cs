using Assets.Scripts.Core;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    public class TreeSpawner : MonoBehaviour
    {
        [SerializeField] private GridData gridData;

        [SerializeField, Space] private bool toggleGizmos = true;
        [SerializeField] private float gizmoSphereRadius = 1f;

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