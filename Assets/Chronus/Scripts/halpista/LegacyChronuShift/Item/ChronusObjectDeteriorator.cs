using UnityEngine;

namespace Chronus.LegacyChronuShift
{
    public class ChronusObjectDeteriorator : MonoBehaviour
    {
        [SerializeField, HeaderAttribute("帰属する時間軸")] ObjectChronusType defaultChronusType;
        [SerializeField] GameObject chronusAura;
        [SerializeField, HeaderAttribute("本体オブジェクト")] ChronusObject chronusObject;
        [SerializeField, HeaderAttribute("対応オブジェクト")] GameObject lockedObject;
        [SerializeField] bool fixedRotation;
        [SerializeField, HeaderAttribute("発生エフェクト")] bool spawnParticleEnabled;
        [SerializeField] ParticleSystem spawnParticle;


        // 本体オブジェクトが過去にあるか
        bool isPast;
        // 本体オブジェクトが持ち運ばれているか
        bool objectCarried = false;
        // 本体オブジェクトが帰属する時間軸にあるか
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
            // 持ち運び状態を確認
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
                    // 本体オブジェクトが過去にある場合

                    if(isPast)
                    {
                        chronusObject.gameObject.SetActive(true);
                        lockedObject.SetActive(false);
                    }
                    else
                    {
                        // 本体オブジェクトの位置を対応オブジェクトへ反映
                        lockedObject.transform.position = chronusObject.gameObject.transform.position;
                        if (!fixedRotation)
                            lockedObject.transform.rotation = chronusObject.gameObject.transform.rotation;

                        chronusObject.gameObject.SetActive(false);
                        lockedObject.SetActive(true);
                    }
                }
                else
                {
                    // 本体オブジェクトが現在にある場合

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

            // 帰属時間軸かどうか
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