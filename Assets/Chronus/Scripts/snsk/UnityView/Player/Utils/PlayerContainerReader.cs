using UnityEngine;

namespace UnityView.Player.Utils
{
    /// <summary>
    /// 外部からパラメータコンテナを参照するための依存性逆転用クラス
    /// </summary>
    [RequireComponent(typeof(PlayerCore))]
    public class PlayerContainerReader : MonoBehaviour
    {
        private PlayerCore core;

        private void Awake()
        {
            core = GetComponent<PlayerCore>();
        }

        public T GetParameter<T>(string name)
        {
            return core.Container.GetParameter<T>(name);
        }
    }
}
