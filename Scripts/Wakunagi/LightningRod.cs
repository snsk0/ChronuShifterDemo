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

    [SerializeField] float distance_max;    //”ð—‹j‚Æ\‘¢•¨‚Ì‹——£

    protected override void ChangeGimmick(bool isPast) {

        //‚·‚×‚Ä”ñ•\Ž¦
        isActive_rod = false;
        isActive_broekn_rod = false;
        isActive_struct = false;
        isActive_broken_struct = false;

        //”ð—‹j‚ðŽ‚Á‚Ä‚¢‚ê‚Î
        if (lightningRod.GetCarriedState()) {
            isLightinigRod_inPast = isPast;

            isActive_broken_struct = !isPast;

            isActive_rod = true;

        }
        //Ý’u‚³‚ê‚Ä‚¢‚ê‚Î
        else {

            //‰ß‹Ž‚È‚ç
            if (isPast) {
                if (isLightinigRod_inPast)isActive_rod = true;
            }
            //Œ»Ý‚È‚ç
            else {
                //”ð—‹j‚ªŒ»Ý‚É‚ ‚éê‡
                if (!isLightinigRod_inPast) {
                    isActive_rod = true;
                    isActive_broken_struct = true;
                }
                //‰ß‹Ž‚É‚ ‚éê‡
                else {
                    float distance = Vector3.SqrMagnitude(
                        lightningRod.gameObject.transform.position
                        - structure.gameObject.transform.position);

                    //‰ó‚ê‚½”ð—‹j‚ð•\Ž¦
                    brokenLightningRod.transform.position = lightningRod.gameObject.transform.position;
                    isActive_broekn_rod = true;

                    //”ð—‹j‚ª”ÍˆÍŠO‚É‚ ‚éê‡
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