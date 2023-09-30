using System;

using UnityEngine;
using UniRx;

using Player.Behaviours;
using Animation;
using Animation.Triggerer;

using UnityView.Player.Animation;


namespace UnityView.Player.Behaviours
{

    //�n�ʂ̊p�x
    public interface IGroundNormalProvider
    {
        public Vector3 slopeNormal { get; }
    }

    //�₩��͂Ȃ��A�N�V����
    public interface ILeaveGroundAction
    {
        public IObservable<int> leaveAction { get; }
    }


    //TODO
    //���t�@�N�^�����O
    //�Ⴆ�΃X���[�v�̍X�V��isGrounded�̍X�V�͕K�������ɂ��
    //�܂��Afalse�ɂ����Ƃ��͕K�������Ă���̂�slope�A���O����None �܂���up�ɂ��邱�Ƃ�Y��Ȃ��B
    [DefaultExecutionOrder(-int.MaxValue)]  //�f�t�H���g�ň�ԍŏ��ɔ��肳����B�܂蕨�����Z�̒���ɂ͍X�V����Ă���B
    public class PlayerGroundedChecker : MonoBehaviour, IPlayerOnGrounded, IGroundNormalProvider, ILoopAnimationTriggable<PlayerAnimationType>
    {
        //�K�v�R���|�[�l���g
        [SerializeField] private new Rigidbody rigidbody;

        //�����p�p�����[�^
        [SerializeField] private float rayRange;                         //ray�̒���
        [SerializeField] private float rayRadius;                       //Ray���a
        [SerializeField] private float maxAngle;
        //[SerializeField] private float isNotGroundingYVelocityThreshold; //y�������ړ����x��臒l
        //[SerializeField] private float footHight;                        //���̍���


        //�t�B�[���h
        private bool isGrounding;                           //�ڒn���Ă��邩�ǂ��� 
        public Vector3 slopeNormal { get; private set; }    //��̖@��
        private int leaveFrame;                             //�����t���[����


        //�A�j���[�V�����p
        private Subject<PlayerAnimationType> onStopAnimationSubject = new Subject<PlayerAnimationType>();
        private Subject<AnimationParameter<PlayerAnimationType>> onTriggableAnimationSubject = new Subject<AnimationParameter<PlayerAnimationType>>();
        public IObservable<PlayerAnimationType> stopTriggableObserbable => onStopAnimationSubject;
        public IObservable<AnimationParameter<PlayerAnimationType>> triggableObservable => onTriggableAnimationSubject;



        //�֐�
        public bool IsOnGrounded()
        {
            return isGrounding;
        }



        //isGrounding�ύX
        private void SetGrounding(bool isGrounding)
        {
            //if (isGrounding == this.isGrounding) return;
            this.isGrounding = isGrounding;

            if (isGrounding) onStopAnimationSubject.OnNext(PlayerAnimationType.Airborne);
            else onTriggableAnimationSubject.OnNext(new AnimationParameter<PlayerAnimationType>(PlayerAnimationType.Airborne, 1, new BlendParameter(rigidbody.velocity.y, 0)));
        }





        //������
        private void Awake()
        {
            slopeNormal = Vector3.zero;

            //��
            foreach(ILeaveGroundAction groundAction in GetComponents<ILeaveGroundAction>())
            {
                groundAction.leaveAction.Subscribe(frame =>
                {
                    if (leaveFrame < frame) leaveFrame = frame;
                    SetGrounding(false);
                    slopeNormal = Vector3.zero;
                });
            }
        }

        //Fixed
        private void FixedUpdate()
        {
            //�ҋ@�t���[���v�Z
            if(leaveFrame > 0)
            {
                leaveFrame--;
                SetGrounding(false);
                return;
            }


            //��@���ɓ��e����(�n�ʂ���͂Ȃ��x�N�g�������邩�ǂ���
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

            //��ڒn�����肳��Ă�����
            /*
            if (isSlopeGrounding)
            {
                isSlopeGrounding = false;
                return;
            }
            */


            //���C�L���X�g
            /*
            Debug.DrawRay(transform.position, Vector3.down * rayRange, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, rayRange))
            {
                SetGrounding(true);

                //��@���̍X�V
                slopeNormal = hit.normal;
            }
            else
            {
                slopeNormal = Vector3.zero;
                SetGrounding(false);
            }*/


            //�����Ђƈ�
            //isGrounding��Ԃ�NotIsGrounding��ԂŐڒn����𕪂���
            //�ڒn���Ă����ԂȂ�ڒn������Â߂ɂ���
            //�t�ɐڒn���Ă��Ȃ���ԂȂ�ڒn������M���M���ɒ�������(����͍�ɒ��n��������velocity����̖@���ɂ��Ē��n�����Ă��邽��)
            //�܂��A�󒆂ɂ���Ƃ��̐ڒn����Ƃ��āA�X�t�B�A�L���X�g���X���[�v�p�ɂ��A����Raycast��ʂŒP���ɐڒn�������邽�߂����ɔ�΂��B


            //�g���܂킷RaycastHit
            RaycastHit hit;


            //Ray�𑫌��ɔ�΂�
            Debug.DrawRay(transform.position, Vector3.down * rayRange, Color.red);
            if (Physics.Raycast(transform.position, Vector3.down, out hit, rayRange))
            {
                //���肪��ꂽ�炻�̂܂܍X�V����
                slopeNormal = hit.normal;
                SetGrounding(true);
            }


            //���肪�Ƃ�Ȃ�������
            else
            {
                if (Physics.SphereCast(transform.position, rayRadius, Vector3.down, out hit, rayRange - rayRadius))
                {
                    //��ꂽ���������
                    Debug.DrawRay(hit.point, hit.normal * 2, Color.black);

                    //���肪��ꂽ��@���𐳂����Ƃ�&�R�ۂ���Ȃ����`�F�b�N���邽�߂�Ray�𑫌����璅�e�_�ɔ�΂�
                    Vector3 footPosition = transform.position + Vector3.down * rayRange;
                    Vector3 target = new Vector3(hit.point.x, footPosition.y, hit.point.z);
                    Debug.DrawRay(footPosition, target - footPosition, Color.black, 5f);
                    RaycastHit rayHit;
                    if (Physics.Raycast(footPosition, target - footPosition, out rayHit, (target - footPosition).magnitude))
                    {
                        /*
                        if(slopeNormal != hit.normal)
                        {
                            Vector3 vector = Vector3.up * rigidbody.velocity.y;
                            vector = Vector3.ProjectOnPlane(vector, slopeNormal);
                            rigidbody.velocity = new Vector3(rigidbody.velocity.x, vector.y, rigidbody.velocity.z);

                            Debug.Log("change");
                        }*/

                        //���x�␳(angle���ς��^�C�~���O,�p�x���󂭂Ȃ�^�C�~���O,����Ă�Ƃ�����)
                        //���R�[�h
                        //slope�̊p�x�`�F�b�N
                        /*
                        float slopeAngle = Vector3.Angle(slopeNormal, Vector3.up);
                        float hitAngle = Vector3.Angle(hit.normal, Vector3.up);
                        if (rigidbody.velocity.y > 0 && 
                            slopeAngle > hitAngle)
                        {
                            //Debug.LogError("YChange");
                            rigidbody.velocity = Vector3.Scale(new Vector3(1, 0, 1), rigidbody.velocity);
                        }
                        */

                        //�X�V����
                        slopeNormal = rayHit.normal;

                        //float angle = Vector3.Angle(Vector3.up, hit.normal);
                        float angleRay = Vector3.Angle(Vector3.up, rayHit.normal);
                        if (/*!(angle > maxAngle) &&*/ !(angleRay > maxAngle))        //�ő�p�x�𒴂��Ă��Ȃ��Ȃ�(���R�[�h)
                        {
                            SetGrounding(true);

                            //�ړ����e
                            if(rigidbody.velocity.y < 0) rigidbody.velocity = Vector3.ProjectOnPlane(rigidbody.velocity, slopeNormal);

                            //�X�ɔ��肷��
                            //SphereCast�̐ڐG�_���瑬�x�����ɂ��炵��ray���ˏo���Ă݂�
                            Vector3 root = hit.point + Vector3.up + Vector3.Scale(new Vector3(1, 0, 1), rigidbody.velocity * Time.fixedDeltaTime);
                            Debug.DrawRay(root, Vector3.down * rayRange * 1.2f, Color.magenta, 3f);
                            if (Physics.Raycast(root, Vector3.down, out rayHit, rayRange * 1.2f))
                            {
                                //��蒆�Ȃ�
                                if(rigidbody.velocity.y > 0)
                                {
                                    if(Vector3.Angle(rayHit.normal, Vector3.up) <= Vector3.Angle(slopeNormal, Vector3.up))
                                    {
                                        //�p�x���󂢂�����
                                        slopeNormal = rayHit.normal;
                                        rigidbody.velocity = Vector3.ProjectOnPlane(rigidbody.velocity, slopeNormal);
                                        Debug.Log("aaaaaNOBORI:" + rayHit.normal + ":" + rigidbody.velocity.y);
                                    }
                                }

                                //���蒆
                                else
                                {
                                    if (Vector3.Angle(rayHit.normal, Vector3.up) < Vector3.Angle(slopeNormal, Vector3.up))
                                    {
                                        //�p�x���󂢂�����
                                        Debug.Log("aaaaaKUDARI");
                                        slopeNormal = rayHit.normal;
                                        rigidbody.velocity = Vector3.ProjectOnPlane(rigidbody.velocity, slopeNormal);
                                    }
                                }
                            }
                        }
                        //�ő�p�x�𒴂��Ă��܂��Ă����ꍇ
                        else
                        {
                            SetGrounding(false);
                        }
                    }
                    else new Exception();   //sphereCast���������Ă���slope�����Ȃ����Ƃ͂Ȃ��͂��Ȃ̂ŃG���[��f���Ă���

                    //sphereCast
                    //sphereCast���s��
                    RaycastHit[] sphereHits = Physics.SphereCastAll(transform.position, rayRadius, Vector3.down, rayRange - rayRadius);
                    Debug.Log("math:" + sphereHits.Length);
                    foreach(RaycastHit point in sphereHits)
                    {
                        //Debug.Log(point.point);
                        Debug.DrawRay(point.point, point.normal * 1, Color.cyan);
                    }
                }


                //sphereCast�ł��瓖����Ȃ��ꍇ
                else
                {
                    //Debug.Log("release");
                    //Time.timeScale = 0;
                    slopeNormal = Vector3.zero;
                    SetGrounding(false);
                }
            }
        }


        void OnDrawGizmos()
        {
            //�@Capsule�̃��C���^���I�Ɏ��o��
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + Vector3.down * (rayRange - rayRadius), rayRadius);
            //Gizmos.DrawRay(transform.position, Vector3.down * 1.1f);
        }



        /*
        //�ڐG����
        private void OnCollisionEnter(Collision collision)
        {
            foreach (ContactPoint point in collision.contacts)
            {
                if (point.point.y < transform.position.y + footHight) slopeNormal = point.normal;
                isSlopeGrounding = true;
                SetGrounding(true);

                //��ɓ��������ɍ�̉������̑��x��ł�����(���R�[�h)
                //rigidbody.AddForce(-rigidbody.velocity, ForceMode.Acceleration);
                //���}����
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
