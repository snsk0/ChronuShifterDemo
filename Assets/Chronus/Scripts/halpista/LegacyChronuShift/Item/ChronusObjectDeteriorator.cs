using UnityEngine;

namespace Chronus.LegacyChronuShift
{
    public class ChronusObjectDeteriorator : MonoBehaviour
    {
        [SerializeField, HeaderAttribute("�A�����鎞�Ԏ�")] ObjectChronusType defaultChronusType;
        [SerializeField] GameObject chronusAura;
        [SerializeField, HeaderAttribute("�{�̃I�u�W�F�N�g")] ChronusObject chronusObject;
        [SerializeField, HeaderAttribute("�Ή��I�u�W�F�N�g")] GameObject lockedObject;
        [SerializeField] bool fixedRotation;
        [SerializeField, HeaderAttribute("�����G�t�F�N�g")] bool spawnParticleEnabled;
        [SerializeField] ParticleSystem spawnParticle;


        // �{�̃I�u�W�F�N�g���ߋ��ɂ��邩
        bool isPast;
        // �{�̃I�u�W�F�N�g�������^�΂�Ă��邩
        bool objectCarried = false;
        // �{�̃I�u�W�F�N�g���A�����鎞�Ԏ��ɂ��邩
        bool isDefault;

        private void Awake()
        {
            if (spawnParticle == null)
            {
                spawnParticleEnabled = false;
            }
            else if (!spawnParticleEnabled)
            {
                spawnParticle.gameObject.SetActive(false);
            }

            if (defaultChronusType == ObjectChronusType.past)
            {
                this.isPast = true;
                isDefault = true;
            }
            else if (defaultChronusType == ObjectChronusType.current)
            {
                this.isPast = false;
                isDefault = true;
            }
            else
            {
                isDefault = false;
            }
        }

        public void SwitchDisplay(bool isPast)
        {
            // �����^�я�Ԃ��m�F
            objectCarried = chronusObject.GetCarriedState();

            if (spawnParticleEnabled)
            {
                spawnParticle.Stop();
            }

            if (objectCarried)
            {
                this.isPast = !this.isPast;
                isDefault = !isDefault;
            }
            else
            {
                if (this.isPast)
                {
                    // �{�̃I�u�W�F�N�g���ߋ��ɂ���ꍇ

                    if(isPast)
                    {
                        chronusObject.gameObject.SetActive(true);
                        lockedObject.SetActive(false);
                    }
                    else
                    {
                        // �{�̃I�u�W�F�N�g�̈ʒu��Ή��I�u�W�F�N�g�֔��f
                        lockedObject.transform.position = chronusObject.gameObject.transform.position;
                        if (!fixedRotation)
                            lockedObject.transform.rotation = chronusObject.gameObject.transform.rotation;

                        chronusObject.gameObject.SetActive(false);
                        lockedObject.SetActive(true);
                    }
                }
                else
                {
                    // �{�̃I�u�W�F�N�g�����݂ɂ���ꍇ

                    if(isPast)
                    {
                        chronusObject.gameObject.SetActive(false);
                        lockedObject.SetActive(false);

                        if(spawnParticleEnabled)
                        {
                            spawnParticle.transform.position = chronusObject.transform.position;   
                            spawnParticle.Play();
                        }
                    }
                    else
                    {
                        chronusObject.gameObject.SetActive(true);
                        lockedObject.SetActive(false);
                    }
                }
            }

            // �A�����Ԏ����ǂ���
            if(isDefault)
            {
                chronusAura.SetActive(false);
            }
            else
            {
                chronusAura.SetActive(true);
            }
        }
    }
}