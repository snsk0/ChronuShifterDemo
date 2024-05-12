using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageGimmick {
    namespace DoorGimmick {
        //両開きのドアのモーション
        public class dMotion_Open : dMotion_DoubleDoor {

            protected override void MotionPart() {
                right_door.transform.localEulerAngles = new Vector3(0, door_move, 0);
                left_door.transform.localEulerAngles = new Vector3(0, -door_move, 0);
            }

            protected override void LastPart() {
                right_door.transform.localEulerAngles = new Vector3(0, desired_value, 0);
                left_door.transform.localEulerAngles = new Vector3(0, -desired_value, 0);
            }


        }
    }
}