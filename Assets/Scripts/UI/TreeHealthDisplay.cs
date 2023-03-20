using Assets.Scripts.Core;
using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Отображает количество очков здоровья дерева в момент рубки
    /// для удобства отслеживания процесса
    /// </summary>
    public class TreeHealthDisplay : MonoBehaviour
    {
        [SerializeField] private Transform imageParent;
        [SerializeField] private Image imageSlider;
        [SerializeField] private PlayerTreeSearcher treeSearcher;
        private Camera _cam;
        private bool _isCutting = false;

        private void OnEnable()
        {
            EventMaster.AddListener<bool>(EventStrings.CUT_PROPERTY_CHANGED, SetCuttingStatus);
        }

        private void OnDisable()
        {
            EventMaster.RemoveListener<bool>(EventStrings.CUT_PROPERTY_CHANGED, SetCuttingStatus);
        }

        private void Start()
        {
            _cam = Camera.main;
            SetCuttingStatus(false);
        }

        /// <summary>
        /// Метод закрепляет здоровье дерева к позиции игрока
        /// и обновляет слайдер здоровья дерева
        /// </summary>
        private void LateUpdate()
        {
            if (_isCutting)
            {
                UpdateFillImage();
                imageParent.transform.position = _cam.WorldToScreenPoint(treeSearcher.transform.position);
            }
        }

        private void UpdateFillImage()
        {
            if (!treeSearcher.NearestTree)
            {
                return;
            }

            var health = treeSearcher.NearestTree.Health;
            
            imageSlider.fillAmount =  1f - (float)health.HealthPoints / health.MaxHealth;
        }

        private void SetCuttingStatus(bool status)
        {
            imageParent.gameObject.SetActive(status);
            _isCutting = status;

            if (!status)
            {
                return;
            }

            UpdateFillImage();
        }
    }
}