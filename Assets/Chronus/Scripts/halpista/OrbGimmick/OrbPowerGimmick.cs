using StageGimmick;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chronus.OrbGimmick
{
    public class OrbPowerGimmick : MonoBehaviour
    {
        // ギミック起動状態をスイッチによって切り替える

        // 起動状態
        private bool isMove;
        private bool prevState;

        // 起動状態を切り替えるorbSwitch
        [SerializeField] private List<OrbSwitch> orbSwitches;

        // GimmickMotionを実装するオブジェクト（具体的なギミック）
        [SerializeField] private GimmickMotion gimmickMotion;

        private void Awake()
        {
            if(gimmickMotion == null)
            {
                Debug.Log("GimmickMotion is not registered.");
            }

            if (CheckSwitch())
            {
                gimmickMotion.OpenMotion();
            }
            else
            {
                gimmickMotion.CloseMotion();
            }
        }

        private void Update()
        {
            prevState = isMove;
            isMove = CheckSwitch();
            
            if(isMove != prevState)
            {
                if(isMove)
                {
                    gimmickMotion.OpenMotion();
                }
                else
                    gimmickMotion.CloseMotion();
            }
        }

        private bool CheckSwitch()
        {
            return orbSwitches.All(orbSwitch => orbSwitch.isOrbExist == true);
        }    }
}