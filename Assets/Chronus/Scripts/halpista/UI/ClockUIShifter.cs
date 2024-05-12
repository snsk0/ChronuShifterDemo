using Chronus.ChronuShift;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Chronus.HeadUpDisplay
{
    public class ClockUIShifter : MonoBehaviour, IChronusTarget
    {
        [SerializeField] Image fillerCurrent;
        [SerializeField] TextMeshProUGUI currentText;
        [SerializeField] TextMeshProUGUI pastText;

        private Vector2 standbyPosition = new Vector2(50f, 0f);
        private Vector2 targetPosition = new Vector2(-65f, 0f);

        private Sequence forwardSequence;
        private Sequence backwardSequence;

        private float shiftDuration;

        private void Awake()
        {
            shiftDuration = ChronusStateManager.Instance.shiftDuration;

            forwardSequence = DOTween.Sequence()
                .Append(fillerCurrent.DOFillAmount(1f, shiftDuration)).SetEase(Ease.OutSine)
                .Join(pastText.rectTransform.DOLocalMove(standbyPosition, shiftDuration).SetEase(Ease.OutSine))
                .Join(currentText.rectTransform.DOLocalMove(targetPosition, shiftDuration).SetEase(Ease.OutSine))
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject);

            backwardSequence = DOTween.Sequence()
                .Append(fillerCurrent.DOFillAmount(0f, shiftDuration)).SetEase(Ease.InSine)
                .Join(pastText.rectTransform.DOLocalMove(targetPosition, shiftDuration).SetEase(Ease.OutSine))
                .Join(currentText.rectTransform.DOLocalMove(standbyPosition, shiftDuration).SetEase(Ease.OutSine))
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject);
        }

        public void OnShift(ChronusState state)
        {
            if (state == ChronusState.Past)
            {
                pastText.rectTransform.anchoredPosition = targetPosition;
                currentText.rectTransform.anchoredPosition = standbyPosition;
            }
            else if (state == ChronusState.Forward)
            {
                forwardSequence.Restart();
            }
            else if (state == ChronusState.Current)
            {
                pastText.rectTransform.anchoredPosition = standbyPosition;
                currentText.rectTransform.anchoredPosition = targetPosition;
            }
            else
            {
                backwardSequence.Restart();
            }
        }
    }
}