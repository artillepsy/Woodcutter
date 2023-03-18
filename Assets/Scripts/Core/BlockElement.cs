
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class BlockElement
    {
        public int Count { get; private set; }

        public bool IsBlocking => Count > 0;

        public BlockElement(int startCount = 0)
        {
            Count = startCount;
        }

        public void AddBlocker() => Count++;

        public void RemoveBlocker()
        {
            Count--;

            if (Count < 0)
            {
                Debug.LogError($"Blocker count is {Count}");
                Count = 0;
            }
        }
    }
}