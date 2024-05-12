using System.Collections.Generic;
using UnityEngine;

namespace Chronus.UI.InGame.Interact {
    [CreateAssetMenu(fileName = "InteractionDatabase", menuName = "ScriptableObjects/InteractionDatabase")]
    public class InteractionDatabase : ScriptableObject {
        //アイテムの種類
        public enum ItemType {
            Box,
            Battery,
            Fire,
            Water,
            Orb,
            End,
        }
        //アイテム登録用クラス
        [System.Serializable]
        public class ItemData {
            public ItemType type;
            public string name;
        }

        [field: SerializeField] public List<ItemData> ItemDatas { private set; get; }

        public string GetItemName(int type) {
            foreach (ItemData itemData in ItemDatas) {
                if ((int)itemData.type == type) return itemData.name;
            }
            Debug.LogError("ItemType " + type + "is not found!");
            return null;
        }

        //ギミックの種類
        public enum GimmickType {
            TimeShift,
            Eagle,
            End,
        }
        //アイテム登録用クラス
        [System.Serializable]
        public class GimmickData {
            public GimmickType type;
            public string name;
        }

        [field: SerializeField] public List<GimmickData> GimmickDatas { private set; get; }

        public string GetGimmickName(int type) {
            foreach (GimmickData gimmickData in GimmickDatas) {
                if ((int)gimmickData.type == type) return gimmickData.name;
            }
            Debug.LogError("GimmickType " + type + "is not found!");
            return null;
        }

        //インタラクションの種類
        public enum InteractionType {
            Carry,
            Attach,
            Do,
            Ride,
            End,
        }
        //インタラクションの種類の登録用クラス
        [System.Serializable]
        public class InteractionData {
            public InteractionType type;
            public string name;
        }

        [field: SerializeField] public List<InteractionData> InteractionDatas { private set; get; }

        public string GetInteractionName(int type) {
            foreach (InteractionData interactionData in InteractionDatas) {
                if ((int)interactionData.type == type) return interactionData.name;
            }
            Debug.LogError("InteractionType " + type + "is not found!");
            return null;
        }
    }
}