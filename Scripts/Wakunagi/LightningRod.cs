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

    [SerializeField] float distance_max;    //避雷針と構造物の距離

    protected override void ChangeGimmick(bool isPast) {

        //すべて非表示
        isActive_rod = false;
        isActive_broekn_rod = false;
        isActive_struct = false;
        isActive_broken_struct = false;

        //避雷針を持っていれば
        if (lightningRod.GetCarriedState()) {
            isLightinigRod_inPast = isPast;

            isActive_broken_struct = !isPast;

            isActive_rod = true;

        }
        //設置されていれば
        else {

            //過去なら
            if (isPast) {
                if (isLightinigRod_inPast)isActive_rod = true;
            }
            //現在なら
            else {
                //避雷針が現在にある場合
                if (!isLightinigRod_inPast) {
                    isActive_rod = true;
                    isActive_broken_struct = true;
                }
                //過去にある場合
                else {
                    float distance = Vector3.SqrMagnitude(
                        lightningRod.gameObject.transform.position
                        - structure.gameObject.transform.position);

                    //壊れた避雷針を表示
                    brokenLightningRod.transform.position = lightningRod.gameObject.transform.position;
                    isActive_broekn_rod = true;

                    //避雷針が範囲外にある場合
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