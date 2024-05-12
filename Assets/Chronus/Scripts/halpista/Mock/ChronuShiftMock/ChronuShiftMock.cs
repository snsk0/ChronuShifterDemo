using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;

namespace Chronus.ChronuShift.Mock
{
    public class ChronuShiftMock : MonoBehaviour
    {
        private ChronusState beforeState;

        [SerializeField] TextMeshProUGUI text;

        public void ChronuShiftDemo()
        {
            ChronusStateManager.Instance.ChronuShift();
        }

        private void Awake()
        {
            AutoChronuShiftAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTask AutoChronuShiftAsync(CancellationToken token)
        {
            while(true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(ChronusStateManager.Instance.shiftDuration + 1.5f));
                ChronuShiftDemo();
            }
        }

        private void Update()
        {
            if(beforeState != ChronusStateManager.Instance.chronusState.Value)
            {
                beforeState = ChronusStateManager.Instance.chronusState.Value;
                if(text != null) text.text = beforeState.ToString();
            }
        }
    }
}