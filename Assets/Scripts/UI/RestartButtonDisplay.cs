using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Класс перезагрузки уровня по нажатию на кнопку
    /// </summary>
    public class RestartButtonDisplay : MonoBehaviour
    {
        public void OnClickRestartLevel()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }
}