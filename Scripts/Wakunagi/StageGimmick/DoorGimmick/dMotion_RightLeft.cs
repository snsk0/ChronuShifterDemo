using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageGimmick {
    namespace DoorGimmick {
        //両開きのドアのモーション
        public class dMotion_RightLeft : dMotion_DoubleDoor {

            //初期位置を登録
            Vector3 old_pos_r, old_pos_l;
            private void Start() {
                old_pos_r = right_door.transform.localPosition;
                old_pos_l = left_door.transform.localPosition;
            }

            protected override void MotionPart() {
                right_door.transform.localPosition = old_pos_r + new Vector3(door_move, 0, 0);
                left_door.transform.localPosition = old_pos_l + new Vector3(-door_move, 0, 0);
            }

            protected override void LastPart() {
                right_door.transform.localPosition = old_pos_r + new Vector3(desired_value, 0, 0);
                left_door.transform.localPosition = old_pos_l + new Vector3(-desired_value, 0, 0);
            }


        }
    }
}