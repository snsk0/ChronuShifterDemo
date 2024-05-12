using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace StageGimmick {
    namespace MoveFloorGimmick {
        //���̕��̃v���O����
        public class FloorPart : MonoBehaviour {
            [SerializeField,Header("�����l")] private  float ADJUSTED_VALUE = 0f;
            /*
            private void OnCollisionEnter(Collision collision) {
                if (collision.gameObject.CompareTag("Player")) {
                    float playerPos, playerSize, thisPos, thisSize;
                    playerPos = collision.transform.position.y;
                    playerSize = collision.transform.localScale.y / 2;
                    thisPos = this.transform.position.y;
                    thisSize = this.transform.localScale.y / 2;

                    //player�������̂���ɂ��邩
                    if (playerPos - playerSize >= thisPos + thisSize - ADJUSTED_VALUE) {
                        //player�̐e��o�^
                        collision.gameObject.transform.SetParent(this.transform);
                        if (collision.gameObject.GetComponent<Rigidbody>() != null) { collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero; }
                    }
                }
            }


            private void OnCollisionExit(Collision collision) {
                if (collision.gameObject.CompareTag("Player")) {
                    collision.gameObject.transform.SetParent(null);
                }
            }
            */

            private List<Collider> colliders =new List<Collider>();
            private List<GameObject> parents = new List<GameObject>();

            private void OnTriggerEnter(Collider other) {
                //if (other.gameObject.CompareTag("Player")) {
                //player�̐e��o�^
                colliders.Add(other);
                GameObject parent = other.gameObject.transform.root.gameObject;
                parents.Add(parent);
                parent.transform.SetParent(this.transform);
                if (other.gameObject.GetComponent<Rigidbody>() != null) {
                    other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }

                //}
            }
            private void OnTriggerExit(Collider other) {
                //if (other.gameObject.CompareTag("Player")) {
                bool haveCollider = false;
                GameObject parent = null;
                for(int i=0;i<colliders.Count; i++) {
                    if (colliders[i] ==  other) {
                        haveCollider = true;
                        parent = parents[i];
                        break;
                    }
                }

                if (haveCollider) {
                    parent.transform.SetParent(null);
                    colliders.Remove(other);
                }
                //}
            }
        }
    }
}
