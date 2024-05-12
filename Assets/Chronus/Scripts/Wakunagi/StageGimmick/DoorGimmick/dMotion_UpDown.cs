using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageGimmick {
    namespace DoorGimmick {
        //上下開きのドアのモーション
        public class dMotion_UpDown : dMotion_DoubleDoor {

            //right_door = up_door , left_door = down_door

            //初期位置を登録
            Vector3 old_pos_u, old_pos_d;
            private void Start() {
                old_pos_u = right_door.transform.localPosition;
                old_pos_d = left_door.transform.localPosition;
            }

            protected override void MotionPart() {
                right_door.transform.localPosition = old_pos_u + new Vector3(0, door_move, 0);
                left_door.transform.localPosition = old_pos_d + new Vector3(0, -door_move, 0);
            }

            protected override void LastPart() {
                right_door.transform.localPosition = old_pos_u + new Vector3(0, desired_value, 0);
                left_door.transform.localPosition = old_pos_d + new Vector3(0, -desired_value, 0);
            }


        }
    }
}