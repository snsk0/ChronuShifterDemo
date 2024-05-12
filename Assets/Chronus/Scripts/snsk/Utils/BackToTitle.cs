using UnityEngine;
using UnityEngine.UI;

namespace Chronus.Utils
{
    public class BackToTitle : MonoBehaviour
    {
        [SerializeField] private Button _backButton;

        private void Awake()
        {
            _backButton.onClick.AddListener(() =>
            {
                FadeManager.Instance.LoadScene("Title", 0.5f);
                SESingleton.instance.ClickSE();
            });
        }
    }
}
