using Chronus.ChronuShift;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class MaterialSkyColorShifter : MonoBehaviour, IChronusTarget
{
    // Typhon.SkyBox1.1のDaytimeColorを変更する

    // Skybox
    [SerializeField] private Material skyboxMaterial; 

    // SkyboxMaterialのインスタンス
    private Material typhonSkyboxMaterial;

    // Materialのプロパティ名
    private string daySky = "_DaySkyColor";
    private string dayHorizon = "_DayHorizonColor";
    private string dayGround = "_DayGroundColor";
    private string dayCloudOuter = "_DayCloudColor";
    private string dayCloudInner = "_DayCloudColor2";

    // 遷移時の色変化設定
    [SerializeField] Gradient skyGradient;
    [SerializeField] Gradient horizonGradient;
    [SerializeField] Gradient groundGradient;
    [SerializeField] Gradient cloudOuterGradient;
    [SerializeField] Gradient cloudInnerGradient;

    private float shiftDuration;

    private CancellationToken token;

    private void Awake()
    {
        token = this.GetCancellationTokenOnDestroy();
        
        typhonSkyboxMaterial = new Material(skyboxMaterial);
        RenderSettings.skybox = typhonSkyboxMaterial;

        shiftDuration = ChronusStateManager.Instance.shiftDuration;
    }

    public void OnShift(ChronusState state)
    {
        if(state == ChronusState.Forward)
        {
            ShiftForwardAsync(token).Forget();
        }
        else if(state == ChronusState.Backward)
        {
            ShiftBackWardAsync(token).Forget();
        }
    }

    private async UniTask ShiftForwardAsync(CancellationToken token)
    {
        float time = 0;

        while(time < shiftDuration)
        {
            time += Time.deltaTime;
            
            typhonSkyboxMaterial.SetColor(daySky, skyGradient.Evaluate(time / shiftDuration));
            typhonSkyboxMaterial.SetColor(dayHorizon, horizonGradient.Evaluate(time / shiftDuration));
            typhonSkyboxMaterial.SetColor(dayGround, groundGradient.Evaluate(time / shiftDuration));
            typhonSkyboxMaterial.SetColor(dayCloudOuter, cloudOuterGradient.Evaluate(time / shiftDuration));
            typhonSkyboxMaterial.SetColor(dayCloudInner, cloudInnerGradient.Evaluate(time / shiftDuration));

            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }

    private async UniTask ShiftBackWardAsync(CancellationToken token)
    {
        float time = 0;

        while (time < shiftDuration)
        {
            time += Time.deltaTime;

            typhonSkyboxMaterial.SetColor(daySky, skyGradient.Evaluate(1f - (time / shiftDuration)));
            typhonSkyboxMaterial.SetColor(dayHorizon, horizonGradient.Evaluate(1f - (time / shiftDuration)));
            typhonSkyboxMaterial.SetColor(dayGround, groundGradient.Evaluate(1f - (time / shiftDuration)));
            typhonSkyboxMaterial.SetColor(dayCloudOuter, cloudOuterGradient.Evaluate(1f - (time / shiftDuration)));
            typhonSkyboxMaterial.SetColor(dayCloudInner, cloudInnerGradient.Evaluate(1f - (time / shiftDuration)));

            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }

    private void OnDestroy()
    {
        Destroy(typhonSkyboxMaterial);
    }
}
