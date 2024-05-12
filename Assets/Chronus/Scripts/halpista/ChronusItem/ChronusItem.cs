using Chronus.ChronuShift;
using Chronus.Direction;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Chronus.ChronusItem
{
    public class ChronusItem : MonoBehaviour, IChronusItem, IChronusTarget
    {
        // Awake時のアイテムが存在する時間軸
        [SerializeField] ItemChronusType _defaultChronusType = ItemChronusType.Current;
        public ItemChronusType defaultChronusType => _defaultChronusType;
       
        // アイテムが存在する時間軸
        public ItemChronusType currentChronusType { private set; get; }

        // 持ち運び状態
        private bool carried;
        // 持ち運び時の要切り替えコライダ
        private Collider[] itemColliderArray;

        // Dissolve表現Timeline
        [SerializeField] private PlayableDirector director;

        [SerializeField] private TimelineAsset appearanceTimeline;
        [SerializeField] private TimelineAsset disappearanceTimeline;

        // Dissolve色指定
        private string disolveEdgeColorProperty = "_EdgeColor";
        private Color pastColor = new Color(1f, 0.6569861f, 0f);
        private Color currentColor = new Color(0.3058823f, 0.7318059f, 0.7318059f);
        [SerializeField] private List<MeshRenderer> renderers;
        private List<Material> materials = new List<Material>();

        // 永続性（時間遷移の影響を受けない）
        private bool permanence = false;

        private void Awake()
        {
            itemColliderArray = gameObject.GetComponentsInChildren<Collider>();
            currentChronusType = defaultChronusType;

            foreach (var renderer in renderers)
            {
                materials.AddRange(renderer.materials);
            }
        }

        private void OnDestroy()
        {
            foreach(var material in materials)
            {
                Destroy(material);
            }
            materials.Clear();
        }

        // 持ち運び状態の書き換え
        public bool ToggleCarriedState()
        {
            carried = !carried;
            SetCollidersActive(!carried);
            UpdateCurrentChronusType();

            return carried;
        }

        public void ToggleCarriedState(bool carried)
        {
            this.carried = carried;
            SetCollidersActive(!carried);
            UpdateCurrentChronusType();
        }

        // 持ち運び状態の取得
        public bool GetCarriedState()
        {
            return carried;
        }

        private void SetCollidersActive(bool enabled)
        {
            foreach (Collider collider in itemColliderArray) 
            {
                collider.enabled = enabled;
            }
        }

        private void SetItemActive(bool enabled)
        {
            gameObject.SetActive(enabled);
        }

        // 時間遷移時の挙動
        public void OnShift(ChronusState state)
        {
            if (carried) return;
            if(permanence) return;

            if(currentChronusType == ItemChronusType.Current)
            {
                if (state == ChronusState.Past)
                {
                    SetItemActive(false);
                }
                else if (state == ChronusState.Forward)
                {
                    SetDissolveEdgeColor(currentColor);
                    AppearItem();
                }
                else if (state == ChronusState.Current)
                {
                    SetItemActive(true);
                }
                else
                {
                    SetDissolveEdgeColor(currentColor);
                    DisappearItem();
                }
            }
            else
            {
                if (state == ChronusState.Past)
                {
                    SetItemActive(true);
                }
                else if (state == ChronusState.Forward)
                {
                    SetDissolveEdgeColor(pastColor);
                    DisappearItem();
                }
                else if (state == ChronusState.Current)
                {
                    SetItemActive(false);
                }
                else
                {
                    SetDissolveEdgeColor(pastColor);
                    AppearItem();
                }
            }
        }

        private void AppearItem()
        {
            SetItemActive(true);

            // 出現Dissolve表現
            if (director != null)
            { 
                director.playableAsset = appearanceTimeline;
                TimelinePlayer.SetTimeline(director);
            }
        }

        private void DisappearItem()
        {
            // 消滅Dissolve表現
            if (director != null)
            {
                director.playableAsset = disappearanceTimeline;
                TimelinePlayer.SetTimeline(director);
            }
        }

        // Dissolve色の設定
        private void SetDissolveEdgeColor(Color color)
        {
            foreach(var material in materials)
            {
                material.SetColor(disolveEdgeColorProperty, color);
            }
        }

        private void UpdateCurrentChronusType()
        {
            if(ChronusStateManager.Instance.chronusState.Value == ChronusState.Past)
            {
                currentChronusType = ItemChronusType.Past;
            }
            else if(ChronusStateManager.Instance.chronusState.Value == ChronusState.Current)
            {
                currentChronusType = ItemChronusType.Current;
            }
        }

        // 永続性（時間遷移の影響を受けない）の書き換え
        public void SetPermanence(bool permanence)
        {
            this.permanence = permanence;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (EditorApplication.isPlaying)
            {
                if (currentChronusType == ItemChronusType.Past)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.cyan;
                }
            }
            else
            {
                if(defaultChronusType == ItemChronusType.Past)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.cyan;
                }
            }

            Gizmos.DrawCube(transform.position, Vector3.one * 0.3f);
            Gizmos.DrawWireCube(transform.position, Vector3.one * 0.3f);
        }
#endif
    }
}