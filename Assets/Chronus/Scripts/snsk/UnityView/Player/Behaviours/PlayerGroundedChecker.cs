using System;
using UnityEngine;
using UniRx;
using Player.Behaviours;
using Animation;
using Animation.Triggerer;
using UnityView.Player.Animation;
using Chronus.KinematicPhysics;

namespace UnityView.Player.Behaviours
{

    //地面の角度
    public interface IGroundNormalProvider
    {
        public Vector3 slopeNormal { get; }
    }

    //坂からはなれるアクション
    public interface ILeaveGroundAction
    {
        public IObservable<int> leaveAction { get; }
    }

    //TODO
    //リファクタリング
    //例えばスロープの更新とisGroundedの更新は必ず同時にやる
    //また、falseにしたときは必ず浮いているのでslopeアングルをNone またはupにすることを忘れない。
    [DefaultExecutionOrder(-10)]
    public class PlayerGroundedChecker : MonoBehaviour, IPlayerOnGrounded, IGroundNormalProvider, ILoopAnimationTriggable<PlayerAnimationType>
    {
        //必要コンポーネント
        //[SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private CharacterBody _characterBody;

        //調整用パラメータ
        [SerializeField] private float rayRange;                         //rayの長さ
        [SerializeField] private float rayRadius;                       //Ray半径
        [SerializeField] private float maxAngle;
        //[SerializeField] private float isNotGroundingYVelocityThreshold; //y軸方向移動速度の閾値
        //[SerializeField] private float footHight;                        //足の高さ

        //フィールド
        private bool isGrounding;                           //接地しているかどうか 
        public Vector3 slopeNormal { get; private set; }    //坂の法線
        private int leaveFrame;                             //離れるフレーム数

        //アニメーション用
        private Subject<PlayerAnimationType> onStopAnimationSubject = new Subject<PlayerAnimationType>();
        private Subject<AnimationParameter<PlayerAnimationType>> onTriggableAnimationSubject = new Subject<AnimationParameter<PlayerAnimationType>>();
        public IObservable<PlayerAnimationType> stopTriggableObserbable => onStopAnimationSubject;
        public IObservable<AnimationParameter<PlayerAnimationType>> triggableObservable => onTriggableAnimationSubject;

        //関数
        public bool IsOnGrounded()
        {
            return _characterBody.IsGround;
        }

        //isGrounding変更
        private void SetGrounding(bool isGrounding)
        {
            //if (isGrounding == this.isGrounding) return;
            this.isGrounding = isGrounding;

            if (isGrounding) onStopAnimationSubject.OnNext(PlayerAnimationType.Airborne);
            else onTriggableAnimationSubject.OnNext(new AnimationParameter<PlayerAnimationType>(PlayerAnimationType.Airborne, 1, new BlendParameter(_characterBody.Velocity.y, 0)));
        }

        //初期化
        private void Awake()
        {
            slopeNormal = Vector3.zero;
            //仮
            //foreach(ILeaveGroundAction groundAction in GetComponents<ILeaveGroundAction>())
            //{
            //    groundAction.leaveAction.Subscribe(frame =>
            //    {
            //        if (leaveFrame < frame) leaveFrame = frame;
            //        SetGrounding(false);
            //        slopeNormal = Vector3.zero;
            //    });
            //}
        }

        //Fixed
        private void FixedUpdate()
        {
            SetGrounding(IsOnGrounded());

            ////待機フレーム計算
            //if(leaveFrame > 0)
            //{
            //    leaveFrame--;
            //    SetGrounding(false);
            //    return;
            //}


            //坂法線に投影する(地面からはなれるベクトルがあるかどうか
            /*
            if(slopeNormal != Vector3.zero)
            {
                float angle = Vector3.Angle(rigidbody.velocity, slopeNormal);
                if (angle < 90)
                {
                    Vector3 verticalVelocity = Vector3.Project(rigidbody.velocity, slopeNormal);
                    //Debug.LogError("V:" + verticalVelocity.magnitude);
                    if (verticalVelocity.magnitude > isNotGroundingYVelocityThreshold)
                    {
                        slopeNormal = Vector3.zero;
                        Debug.LogError("velocityRelease");
                        SetGrounding(false);
                        return;
                    }
                }
            }
            */

            //坂接地が判定されていたら
            /*
            if (isSlopeGrounding)
            {
                isSlopeGrounding = false;
                return;
            }
            */


            //レイキャスト
            /*
            Debug.DrawRay(transform.position, Vector3.down * rayRange, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, rayRange))
            {
                SetGrounding(true);

                //坂法線の更新
                slopeNormal = hit.normal;
            }
            else
            {
                slopeNormal = Vector3.zero;
                SetGrounding(false);
            }*/


            //もうひと案
            //isGrounding状態とNotIsGrounding状態で接地判定を分ける
            //接地している状態なら接地判定を甘めにする
            //逆に接地していない状態なら接地判定をギリギリに調整する(これは坂に着地した時にvelocityを坂の法線にして着地させているため)
            //また、空中にいるときの接地判定として、スフィアキャストをスロープ用にし、足元Raycastを別で単純に接地判定を取るためだけに飛ばす。


            //使いまわすRaycastHit
            //RaycastHit hit;


            ////Rayを足元に飛ばす
            //Debug.DrawRay(transform.position, Vector3.down * rayRange, Color.red);
            //if (Physics.Raycast(transform.position, Vector3.down, out hit, rayRange))
            //{
            //    //判定が取れたらそのまま更新する
            //    slopeNormal = hit.normal;
            //    SetGrounding(true);
            //}


            ////判定がとれなかったら
            //else
            //{
            //    if (Physics.SphereCast(transform.position, rayRadius, Vector3.down, out hit, rayRange - rayRadius))
            //    {
            //        //取れた判定を見る
            //        Debug.DrawRay(hit.point, hit.normal * 2, Color.black);

            //        //判定が取れたら法線を正しくとる&崖際じゃないかチェックするためのRayを足元から着弾点に飛ばす
            //        Vector3 footPosition = transform.position + Vector3.down * rayRange;
            //        Vector3 target = new Vector3(hit.point.x, footPosition.y, hit.point.z);
            //        Debug.DrawRay(footPosition, target - footPosition, Color.black, 5f);
            //        RaycastHit rayHit;
            //        if (Physics.Raycast(footPosition, target - footPosition, out rayHit, (target - footPosition).magnitude))
            //        {
            //            /*
            //            if(slopeNormal != hit.normal)
            //            {
            //                Vector3 vector = Vector3.up * rigidbody.velocity.y;
            //                vector = Vector3.ProjectOnPlane(vector, slopeNormal);
            //                rigidbody.velocity = new Vector3(rigidbody.velocity.x, vector.y, rigidbody.velocity.z);

            //                Debug.Log("change");
            //            }*/

            //            //速度補正(angleが変わるタイミング,角度が浅くなるタイミング,上ってるとき限定)
            //            //仮コード
            //            //slopeの角度チェック
            //            /*
            //            float slopeAngle = Vector3.Angle(slopeNormal, Vector3.up);
            //            float hitAngle = Vector3.Angle(hit.normal, Vector3.up);
            //            if (rigidbody.velocity.y > 0 && 
            //                slopeAngle > hitAngle)
            //            {
            //                //Debug.LogError("YChange");
            //                rigidbody.velocity = Vector3.Scale(new Vector3(1, 0, 1), rigidbody.velocity);
            //            }
            //            */

            //            //更新する
            //            slopeNormal = rayHit.normal;

            //            //float angle = Vector3.Angle(Vector3.up, hit.normal);
            //            float angleRay = Vector3.Angle(Vector3.up, rayHit.normal);
            //            if (/*!(angle > maxAngle) &&*/ !(angleRay > maxAngle))        //最大角度を超えていないなら(仮コード)
            //            {
            //                SetGrounding(true);

            //                //移動投影
            //                if(rigidbody.velocity.y < 0) rigidbody.velocity = Vector3.ProjectOnPlane(rigidbody.velocity, slopeNormal);

            //                //更に判定する
            //                //SphereCastの接触点から速度方向にずらしたrayを射出してみる
            //                Vector3 root = hit.point + Vector3.up + Vector3.Scale(new Vector3(1, 0, 1), rigidbody.velocity * Time.fixedDeltaTime);
            //                Debug.DrawRay(root, Vector3.down * rayRange * 1.2f, Color.magenta, 3f);
            //                if (Physics.Raycast(root, Vector3.down, out rayHit, rayRange * 1.2f))
            //                {
            //                    //上り中なら
            //                    if(rigidbody.velocity.y > 0)
            //                    {
            //                        if(Vector3.Angle(rayHit.normal, Vector3.up) <= Vector3.Angle(slopeNormal, Vector3.up))
            //                        {
            //                            //角度が浅いか判定
            //                            slopeNormal = rayHit.normal;
            //                            rigidbody.velocity = Vector3.ProjectOnPlane(rigidbody.velocity, slopeNormal);
            //                            Debug.Log("aaaaaNOBORI:" + rayHit.normal + ":" + rigidbody.velocity.y);
            //                        }
            //                    }

            //                    //下り中
            //                    else
            //                    {
            //                        if (Vector3.Angle(rayHit.normal, Vector3.up) < Vector3.Angle(slopeNormal, Vector3.up))
            //                        {
            //                            //角度が浅いか判定
            //                            Debug.Log("aaaaaKUDARI");
            //                            slopeNormal = rayHit.normal;
            //                            rigidbody.velocity = Vector3.ProjectOnPlane(rigidbody.velocity, slopeNormal);
            //                        }
            //                    }
            //                }
            //            }
            //            //最大角度を超えてしまっていた場合
            //            else
            //            {
            //                SetGrounding(false);
            //            }
            //        }
            //        else new Exception();   //sphereCastが当たっていてslopeが取れないことはないはずなのでエラーを吐いておく

            //        //sphereCast
            //        //sphereCastを行う
            //        RaycastHit[] sphereHits = Physics.SphereCastAll(transform.position, rayRadius, Vector3.down, rayRange - rayRadius);
            //        Debug.Log("math:" + sphereHits.Length);
            //        foreach(RaycastHit point in sphereHits)
            //        {
            //            //Debug.Log(point.point);
            //            Debug.DrawRay(point.point, point.normal * 1, Color.cyan);
            //        }
            //    }


            //    //sphereCastですら当たらない場合
            //    else
            //    {
            //        //Debug.Log("release");
            //        //Time.timeScale = 0;
            //        slopeNormal = Vector3.zero;
            //        SetGrounding(false);
            //    }
            //}
        }

        void OnDrawGizmos()
        {
            ////　Capsuleのレイを疑似的に視覚化
            //Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(transform.position + Vector3.down * (rayRange - rayRadius), rayRadius);
            ////Gizmos.DrawRay(transform.position, Vector3.down * 1.1f);
        }

        /*
        //接触判定
        private void OnCollisionEnter(Collision collision)
        {
            foreach (ContactPoint point in collision.contacts)
            {
                if (point.point.y < transform.position.y + footHight) slopeNormal = point.normal;
                isSlopeGrounding = true;
                SetGrounding(true);

                //坂に入った時に坂の下方向の速度を打ち消す(仮コード)
                //rigidbody.AddForce(-rigidbody.velocity, ForceMode.Acceleration);
                //応急処理
                /*
                Vector3 vector = Vector3.ProjectOnPlane(rigidbody.velocity, slopeNormal);
                transform.position = transform.position - vector * Time.fixedDeltaTime;
                rigidbody.velocity = Vector3.zero;
                //rigidbody.velocity = Vector3.zero;
                */
        //rigidbody.AddForce(-slopeNormal.normalized * Physics.gravity.magnitude, ForceMode.Acceleration);
        //break;
        /*}
    }
    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint point in collision.contacts)
        {
            if (point.point.y < transform.position.y + footHight) slopeNormal = point.normal;
            isSlopeGrounding = true;
            SetGrounding(true);
            break;
        }
    }*/

    }
}
