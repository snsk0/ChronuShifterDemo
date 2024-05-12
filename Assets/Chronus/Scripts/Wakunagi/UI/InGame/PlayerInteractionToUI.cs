using Chronus.LegacyChronuShift;
using Chronus.UI.InGame.ToOut;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityView.Player.Behaviours;
using UnityView.Player.Utils;

namespace Chronus.UI.InGame.Interact {
    public class PlayerInteractionToUI : MonoBehaviour {
        const int ITEM_LAYER = 20;

        //(�V���O���g���N���X)������
        [SerializeField]InGameUI inGameUI;

        //�C���Q�[���n
        private List<InteractionObjHolder> interactionObjList = new List<InteractionObjHolder>();
        private List<InteractionGimmickHolder> interactionGimmickList = new List<InteractionGimmickHolder>();
        private List<GameObject> elseObjcts = new List<GameObject>();
        GameObject insideObj = null;
        GameObject obj_UsedByUI = null;
        int itemType = (int)InteractionDatabase.ItemType.End;
        [SerializeField] Camera camera_UsedByUI;
        [SerializeField]Vector3 camera_pos = Vector3.zero;
        [SerializeField] float camera_adjustment_hight = -0.5f;


        //Player�Q��
        [SerializeField] PlayerItemHandler playerInteract = null;
        [SerializeField] PlayerContainerReader reader; //�R���|�[�l���g�Ȃ̂�Player����Ƃ��Ă���

        private void Start() {
            //inGameUI = InGameUI.Instance;
            camera_UsedByUI.gameObject.transform.position = camera_pos;
        }

        // Update is called once per frame
        void Update() {
            CanInteraction();

            GetItem();
        }

        //�A�C�e������������
        void GetItem() {
            GameObject obj = playerInteract.GetHandlingObject();

            //�����Ă�����
            if (obj != null) {

                if (obj_UsedByUI == null) {

                    //�����Ă���A�C�e����UI�ŕ\������

                    obj_UsedByUI = Instantiate(obj);
                    obj_UsedByUI.SetActive(false);
                    //obj_UsedByUI.layer = ITEM_LAYER;
                    SetChildrenLayer(obj_UsedByUI, ITEM_LAYER);
                    DestroyComponent(obj_UsedByUI);

                    //����Ȃ��q�G�����L�[���폜���Ă���
                    for (int i = 0; i < obj_UsedByUI.transform.childCount; i++) {
                        GameObject childObj = obj_UsedByUI.transform.GetChild(i).gameObject;
                        DestroyComponent(childObj);
                    }
                    void DestroyComponent(GameObject destroyObj) {
                        destroyObj.layer = ITEM_LAYER;
                        destroyObj.SetActive(true);
                        Destroy(destroyObj.GetComponent<Rigidbody>());
                        Destroy(destroyObj.GetComponent<ChronusObject>());
                        Destroy(destroyObj.GetComponent<HandableObject>());
                        Destroy(destroyObj.GetComponent<InteractionItem>());
                    }

                    //�\��
                    obj_UsedByUI.transform.position = Vector3.zero;

                    float hight = obj_UsedByUI.transform.localScale.y + camera_adjustment_hight;
                    camera_UsedByUI.gameObject.transform.position = new Vector3(0, hight / 2, camera_UsedByUI.gameObject.transform.position.z);

                    obj_UsedByUI.SetActive(true);

                    inGameUI.DisplayItemUI(itemType);
                }
            }

            //�����Ă��Ȃ����
            else {
                if (obj_UsedByUI != null) {

                    Destroy(obj_UsedByUI.gameObject);
                    obj_UsedByUI = null;

                    inGameUI.HiddenItemUI();
                }
            }

        }


        GameObject GetObjInRay() {

            bool isExit = reader.GetParameter<bool>("IsExitObject");

            if (isExit) {
                var tempObject = reader.GetParameter<ITempObject>("SearchedObject");

                if (tempObject.IsItem) {
                    var item = tempObject as ItemTemp;
                    return item.GameObject;//�Q�[���I�u�W�F�N�g�̎擾
                }
                else {
                    var gimmick = tempObject as GimmickTemp;
                    return gimmick.GameObject;//�Q�[���I�u�W�F�N�g�̎擾
                }
            }

            return null;
        }

        void CanInteraction() {
            GameObject hit = null;
            bool isGimmick = false;
            bool isExit = reader.GetParameter<bool>("IsExitObject");

            if (isExit) {
                var tempObject = reader.GetParameter<ITempObject>("SearchedObject");

                if (tempObject.IsItem) {
                    var item = tempObject as ItemTemp;
                    hit = item.GameObject;//�Q�[���I�u�W�F�N�g�̎擾
                }
                else {
                    var gimmick = tempObject as GimmickTemp;
                    hit= gimmick.GameObject;//�Q�[���I�u�W�F�N�g�̎擾
                    isGimmick = true;
                }
            }
            else {
                hit = null; 
            }

            //�������Ă�����
            if (hit != null) {
                if (isGimmick) {
                    IntoRay(hit,isGimmick);
                }
                else {
                    //���������Ă��Ȃ���ԂȂ�
                    if (obj_UsedByUI == null) {
                        IntoRay(hit, isGimmick);
                    }
                }
            }

            //�������Ă��Ȃ����
            else {
                OuttoRay();
            }

        }


        //Ray�ɓ���������
        void IntoRay(GameObject hitObj , bool isGimmick) {

            //���߂ē��������Ƃ��̂�
            if (insideObj == hitObj) return;
            insideObj = hitObj;

            if(elseObjcts.Contains(hitObj)) return;

            if(isGimmick) {
                HitGimmick(hitObj);
            }
            else {
                HitItem(hitObj);
            }
        }

        //Item�ɓ������Ă�����
        void HitItem(GameObject hitObj) {

            InteractionItem interactionObj = SearchInteractionObj(hitObj, interactionObjList);

            //�����������Ƃ��Ȃ��Ȃ�
            if (interactionObj == null) {

                interactionObj = hitObj.GetComponent<InteractionItem>();
                if (interactionObj == null) { elseObjcts.Add(hitObj); return; }

                InteractionObjHolder tmp = new InteractionObjHolder();
                tmp.interactionObj = interactionObj;
                tmp.obj = hitObj;
                interactionObjList.Add(tmp);
            }

            itemType = (int)interactionObj.itemType;
            int interactionType = (int)interactionObj.interactionType;

            inGameUI.DisplayInteractionUI(itemType, interactionType);
        }

        //Gimmick�ɓ������Ă�����
        void HitGimmick(GameObject hitObj) {

            InteractionGimmick interactionObj = SearchInteractionObj(hitObj, interactionGimmickList);

            //�����������Ƃ��Ȃ��Ȃ�
            if (interactionObj == null) {

                interactionObj = hitObj.GetComponent<InteractionGimmick>();
                if (interactionObj == null) { elseObjcts.Add(hitObj); return; }

                InteractionGimmickHolder tmp = new InteractionGimmickHolder();
                tmp.interactionObj = interactionObj;
                tmp.obj = hitObj;
                interactionGimmickList.Add(tmp);
            }

            itemType = (int)interactionObj.itemType;
            int interactionType = (int)interactionObj.interactionType;

            inGameUI.DisplayInteractionUI_Gimmick(itemType, interactionType);
        }

            //Ray����O�ꂽ��
            void OuttoRay() {
            if (insideObj == null) return;
            insideObj = null;

            itemType = (int)InteractionDatabase.ItemType.End;

            inGameUI.HiddenInteractionUI();
        }

        //�o�^���Ă��邩������
        InteractionItem SearchInteractionObj(GameObject obj, List<InteractionObjHolder> list) {
            if (list.Count == 0) return null;
            foreach (InteractionObjHolder objHolder in list) {
                if (objHolder.obj == obj) return objHolder.interactionObj;
            }
            return null;
        }

        //�o�^���Ă��邩������
        InteractionGimmick SearchInteractionObj(GameObject obj, List<InteractionGimmickHolder> list) {
            if (list.Count == 0) return null;
            foreach (InteractionGimmickHolder objHolder in list) {
                if (objHolder.obj == obj) return objHolder.interactionObj;
            }
            return null;
        }

        // �I�u�W�F�N�g�Ǝq�v�f���ׂẴ��C���[��ݒ�
        void SetChildrenLayer(GameObject parent, int layer)
        {
            parent.layer = layer;

            foreach (Transform objTransform in parent.transform)
            {
                SetChildrenLayer(objTransform.gameObject, layer);
            }
        }
    }


    public class InteractionObjHolder {
        public GameObject obj;
        public InteractionItem interactionObj;
    }

    public class InteractionGimmickHolder {
        public GameObject obj;
        public InteractionGimmick interactionObj;
    }
}