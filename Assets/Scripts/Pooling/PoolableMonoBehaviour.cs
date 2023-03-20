using UnityEngine;

namespace Assets.Scripts.Pooling
{
    /// <summary>
    /// класс объекта, управляемого пулом объектов.
    /// Содержит тип и ID для удобного поиска
    /// </summary>
    public class PoolableMonoBehaviour : MonoBehaviour
    {
        [field: SerializeField] public PoolableType Type { get; private set; }
        public int ID { get; set; }
    }
}