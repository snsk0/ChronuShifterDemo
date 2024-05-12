using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

using Chronus.LegacyChronuShift;


public class BoxOnTree : ChronusChangeGimmick {

    [field: SerializeField] public ChronusGimmickState boxOnTreeState { private set; get; } = ChronusGimmickState.current;


    [SerializeField] LegacyWaterFeature waterFeature;

    [SerializeField] GameObject pastSeedlings;
    [field: SerializeField] public GameObject currentTree { private set; get; }
    [SerializeField] GameObject currentDeadTree;


    bool isActive_pastSeedlings, isActive_currentTree, isActive_currentDeadTree;

    protected override void ChangeGimmick(bool isPast) {
        //��񂷂ׂĔ�\��
        isActive_pastSeedlings = false;
        isActive_currentTree = false;
        isActive_currentDeadTree = false;


        //���݂̕ω��̏�������
        if (isPast)
            boxOnTreeState = ChronusGimmickState.past;
        else {
            if (waterFeature.isWatered) boxOnTreeState = ChronusGimmickState.changed_current;
            else boxOnTreeState = ChronusGimmickState.current;
        }

        //�������򂲂Ƃ̏���
        if (boxOnTreeState == ChronusGimmickState.past) {
            isActive_pastSeedlings = true;
        }
        else if (boxOnTreeState != ChronusGimmickState.END) {

            if (boxOnTreeState == ChronusGimmickState.current)
                isActive_currentDeadTree = true;

            if (boxOnTreeState == ChronusGimmickState.changed_current)
                isActive_currentTree = true;

        }
        else {
            Debug.LogError("Error:ChronusGimmickState is unexpected!");
        }


        pastSeedlings.SetActive(isActive_pastSeedlings);
        currentTree.SetActive(isActive_currentTree);
        currentDeadTree.SetActive(isActive_currentDeadTree);
    }
}