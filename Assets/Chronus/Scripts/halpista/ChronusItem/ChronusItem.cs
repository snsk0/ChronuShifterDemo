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
        // Awake���̃A�C�e�������݂��鎞�Ԏ�
        [SerializeField] ItemChronusType _defaultChronusType = ItemChronusType.Current;
        public ItemChronusType defaultChronusType => _defaultChronusType;
       
        // �A�C�e�������݂��鎞�Ԏ�
        public ItemChronusType currentChronusType { private set; get; }

        // �����^�я��
        private bool carried;
        // �����^�ю��̗v�؂�ւ��R���C�_
        private Collider[] itemColliderArray;

        // Dissolve�\��Timeline
        [SerializeField] private PlayableDirector director;

        [SerializeField] private TimelineAsset appearanceTimeline;
        [SerializeField] private TimelineAsset disappearanceTimeline;

        // Dissolve�F�w��
        private string disolveEdgeColorProperty = "_EdgeColor";
        private Color pastColor = new Color(1f, 0.6569861f, 0f);
        private Color currentColor = new Color(0.3058823f, 0.7318059f, 0.7318059f);
        [SerializeField] private List<MeshRenderer> renderers;
        private List<Material> materials = new List<Material>();

        // �i�����i���ԑJ�ڂ̉e�����󂯂Ȃ��j
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

        // �����^�я�Ԃ̏�������
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

        // �����^�я�Ԃ̎擾
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

        // ���ԑJ�ڎ��̋���
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

            // �o��Dissolve�\��
            if (director != null)
            { 
                director.playableAsset = appearanceTimeline;
                TimelinePlayer.SetTimeline(director);
            }
        }

        private void DisappearItem()
        {
            // ����Dissolve�\��
            if (director != null)
            {
                director.playableAsset = disappearanceTimeline;
                TimelinePlayer.SetTimeline(director);
            }
        }

        // Dissolve�F�̐ݒ�
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

        // �i�����i���ԑJ�ڂ̉e�����󂯂Ȃ��j�̏�������
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