using System;

using UnityEngine;
using UniRx;

using InputProviders.Player;


namespace UnityView.InputProviders
{
    /*
    public class UnityInputManagerPlayerInputProvider : MonoBehaviour, IPlayerInputProvider
    {

        //�T�u�W�F�N�g
        private Subject<MoveInputData> moveSubject = new Subject<MoveInputData>();
        private Subject<JumpInputData> jumpSubject = new Subject<JumpInputData>();
        private Subject<SprintInputData> sprintSubject = new Subject<SprintInputData>();
        private Subject<bool> interactSubject = new Subject<bool>();


        //�C�x���g�w�Ǘp
        public IObservable<MoveInputData> onMoveInput => moveSubject;
        public IObservable<JumpInputData> onJumpInput => jumpSubject;
        public IObservable<SprintInputData> onSprintInput => sprintSubject;
        public IObservable<bool> onInteractInput => interactSubject;


        //�p�����[�^
        [SerializeField] private bool isSprintHold;

        //�ړ��L���b�V��
        private int lastHorizontal = 0;
        private int lastVertical = 0;





        //Destory���ɔj��
        private void OnDestroy()
        {
            moveSubject.Dispose();
            jumpSubject.Dispose();
            sprintSubject.Dispose();
        }




        //Update
        private void Update()
        {
            float x = 0;
            bool right = Input.GetKey(KeyCode.D);
            bool left = Input.GetKey(KeyCode.A);

            //��������
            if (right && left)
            {
                //Down�ɕύX
                right = Input.GetKeyDown(KeyCode.D);
                left = Input.GetKeyDown(KeyCode.A);

                //down�����ق��ɕύX
                if (right && !left) { x = 1; lastHorizontal = 1; }
                else if (left && !right) { x = -1; lastHorizontal = -1; }
                else
                {
                    //���������łǂ����Down����Ȃ��Ƃ��o�O��̂ŉ������ɃL���b�V��������
                    x = lastHorizontal;
                }

                //���S�ɓ����^�C�~���O�Ń_�E�������ꍇ 0�̂܂܂ɂ��Ă���
            }

            //������������Ȃ��Ȃ炻�̂܂�
            else
            {
                lastHorizontal = 0; //�L���b�V����j��
                if (right && !left) x = 1;
                else if (left && !right) x = -1;
            }


            float y = 0;
            bool front = Input.GetKey(KeyCode.W);
            bool back = Input.GetKey(KeyCode.S);

            //��������
            if (front && back)
            {
                //Down�ɕύX
                front = Input.GetKeyDown(KeyCode.W);
                back = Input.GetKeyDown(KeyCode.S);

                //down�����ق��ɕύX
                if (front && !back) { y = 1; lastVertical = 1; }
                else if (back && !front) { y = -1; lastVertical = -1; }

                else
                {
                    y = lastVertical;
                }
                //���S�ɓ����^�C�~���O�Ń_�E�������ꍇ 0�̂܂܂ɂ��Ă���
            }

            //������������Ȃ��Ȃ炻�̂܂�
            else
            {
                lastVertical = 0;
                if (front && !back) y = 1;
                else if (back && !front) y = -1;
            }


            //�������Ĕ��s
            MoveInputData moveInputData = new MoveInputData(x, y);
            moveSubject.OnNext(moveInputData);


            //Sprint�𔭍s
            bool isSprint;
            if (isSprintHold) isSprint = Input.GetKey(KeyCode.LeftShift);
            else isSprint = Input.GetKeyDown(KeyCode.LeftShift);
            sprintSubject.OnNext(new SprintInputData(isSprint, isSprintHold));


            //�W�����v�𔭍s
            if (Input.GetKeyDown(KeyCode.Space)) jumpSubject.OnNext(new JumpInputData(1.0f));

            //�C���^���N�g���s
            if (Input.GetKeyDown(KeyCode.F)) interactSubject.OnNext(true);
        }
    }*/
    

}