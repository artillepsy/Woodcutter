using UnityEngine;

namespace Assets.Scripts.Pooling
{
    public class PoolableMonoBehaviour : MonoBehaviour
    {
        [field: SerializeField] public PoolableType Type { get; private set; }
        public int ID { get; set; }
    }
}