using Chronus.ChronuShift;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Chronus.ChronusDecoration
{
    public class MaterialMossShifter : MonoBehaviour, IChronusTarget
    {
        // HexTileアセットシェーダーのMossHeightを変更する

        private string mossHeight = "Vector1_F0921479";
        private float pastHeight = -15f;
        private float currentHeight = 3.1f;

        [SerializeField] private float shiftDuration = 1f;

        private CancellationToken token;

        private void Awake()
        {
            token = this.GetCancellationTokenOnDestroy();

#if UNITY_EDITOR
            // PlayMode終了時に呼び出し
            EditorApplication.playModeStateChanged += OnExitingPlayMode;
#endif
        }

        public void OnShift(ChronusState state)
        {
            if(state == ChronusState.Past)
            {
                Shader.SetGlobalFloat(mossHeight, pastHeight);
            }
            else if(state == ChronusState.Forward)
            {
                ShiftForwardAsync(token).Forget();
            }
            else if(state == ChronusState.Current)
            {
                Shader.SetGlobalFloat(mossHeight, currentHeight);
            }
            else
            {
                ShiftBackwardAsync(token).Forget();
            }
        }

        private async UniTask ShiftForwardAsync(CancellationToken token)
        {
            float time = 0;

            while(time < shiftDuration)
            {
                time += Time.deltaTime;
                Shader.SetGlobalFloat(mossHeight, Mathf.Lerp(pastHeight, currentHeight, time / shiftDuration));
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }

        private async UniTask ShiftBackwardAsync(CancellationToken token)
        {
            float time = 0;

            while (time < shiftDuration)
            {
                time += Time.deltaTime;
                Shader.SetGlobalFloat(mossHeight, Mathf.Lerp(currentHeight, pastHeight, time / shiftDuration));
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }

#if UNITY_EDITOR
        // マテリアルのMossHeight値を実行前の値に戻す
        private void OnExitingPlayMode(PlayModeStateChange state)
        {
            if(state == PlayModeStateChange.ExitingPlayMode)
            {
                Shader.SetGlobalFloat(mossHeight, pastHeight);
            }
        }
#endif
    }
}