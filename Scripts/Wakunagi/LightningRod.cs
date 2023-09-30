using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

using Chronus;

public class LightningRod : ChronusChangeGimmick {

    [SerializeField] ChronusObject lightningRod;
    [SerializeField] GameObject brokenLightningRod;
    [SerializeField] GameObject structure;
    [SerializeField] GameObject brokenStructure;

    bool isLightinigRod_inPast = false;
    bool isActive_rod, isActive_broekn_rod, isActive_struct, isActive_broken_struct;

    [SerializeField] float distance_max;    //�𗋐j�ƍ\�����̋���

    protected override void ChangeGimmick(bool isPast) {

        //���ׂĔ�\��
        isActive_rod = false;
        isActive_broekn_rod = false;
        isActive_struct = false;
        isActive_broken_struct = false;

        //�𗋐j�������Ă����
        if (lightningRod.GetCarriedState()) {
            isLightinigRod_inPast = isPast;

            isActive_broken_struct = !isPast;

            isActive_rod = true;

        }
        //�ݒu����Ă����
        else {

            //�ߋ��Ȃ�
            if (isPast) {
                if (isLightinigRod_inPast)isActive_rod = true;
            }
            //���݂Ȃ�
            else {
                //�𗋐j�����݂ɂ���ꍇ
                if (!isLightinigRod_inPast) {
                    isActive_rod = true;
                    isActive_broken_struct = true;
                }
                //�ߋ��ɂ���ꍇ
                else {
                    float distance = Vector3.SqrMagnitude(
                        lightningRod.gameObject.transform.position
                        - structure.gameObject.transform.position);

                    //��ꂽ�𗋐j��\��
                    brokenLightningRod.transform.position = lightningRod.gameObject.transform.position;
                    isActive_broekn_rod = true;

                    //�𗋐j���͈͊O�ɂ���ꍇ
                    if (distance > distance_max * distance_max) isActive_broken_struct = true;
                    else isActive_struct = true;
                }
            }
        }
        lightningRod.gameObject.SetActive(isActive_rod);
        brokenLightningRod.SetActive(isActive_broekn_rod);
        structure.SetActive(isActive_struct);
        brokenStructure.SetActive(isActive_broken_struct);

    }

}