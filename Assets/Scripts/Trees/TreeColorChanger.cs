using Assets.Scripts.Core;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Trees
{
    /// <summary>
    /// Класс, отвечающий за изменения цвета дерева 
    /// во время удара по нему
    /// </summary>
    public class TreeColorChanger : MonoBehaviour
    {
        private const string EmissionColorName = "_EmissionColor";
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Color startEmitColor = Color.black;
        [SerializeField] private Color damagedEmitColor = Color.white;
        [SerializeField] private AnimationCurve emitCurve;
        [SerializeField] private float emitTime = 0.3f;
        [SerializeField] private TreeObject thisTree;

        private void OnEnable()
        {
            meshRenderer.material.SetColor(EmissionColorName, startEmitColor);
            EventMaster.AddListener<TreeObject>(EventStrings.TREE_KICKED, EmitMeshIfThis);
            EventMaster.AddListener<TreeObject>(EventStrings.TREE_CUTTED, EmitMeshIfThis);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<TreeObject>(EventStrings.TREE_KICKED, EmitMeshIfThis);
            EventMaster.RemoveListener<TreeObject>(EventStrings.TREE_CUTTED, EmitMeshIfThis);
        }
        /// <summary>
        /// Проверка ссылки объекта, вызвавшего ивент. Если ссылки равны,
        /// изменяется цвет модельки дерева
        /// </summary>
        private void EmitMeshIfThis(TreeObject instance)
        {
            if (instance != thisTree)
            {
                return;
            }

            StartCoroutine(EmitCo());
        }

        private IEnumerator EmitCo()
        {
            Debug.Log("Emit Co");
            var time = 0f;

            while (time < emitTime)
            {
                var lerpValue = emitCurve.Evaluate(time / emitTime);
                var color = Color.Lerp(startEmitColor, damagedEmitColor, lerpValue);
                meshRenderer.material.SetColor(EmissionColorName, color);
                meshRenderer.material.EnableKeyword("_EMISSION");

                time += Time.deltaTime;
                yield return null;
            }

            meshRenderer.material.SetColor(EmissionColorName, startEmitColor);
            meshRenderer.material.EnableKeyword("_EMISSION");
        }
    }
}