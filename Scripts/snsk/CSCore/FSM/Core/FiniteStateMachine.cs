using System.Collections.Generic;

using FSM.Parameter;

namespace FSM.Core
{
    public class FiniteStateMachine<TOwner, TEvent>
    {
        /*
         * �t�B�[���h
         */
        //�t���O
        public bool isRunning => currentState != null;  //�X�e�[�g�}�V�������s����Ă��邩�ǂ���
        private bool isAcceptableEvent;                 //�C�x���g���󂯓�����邩�ǂ����̃t���O

        //�R���e�L�X�g,�X�e�[�g
        public readonly TOwner owner;                                       //�X�e�[�g�}�V���̃I�[�i�[
        private StateBase<TOwner, TEvent> initialState;                     //�����X�e�[�g
        public StateBase<TOwner, TEvent> currentState { get; private set; } //���݃X�e�[�g
        public StateBase<TOwner, TEvent> lastState { get; private set; }    //���s���ŏI�X�e�[�g

        //�R���N�V����
        private readonly List<StateBase<TOwner, TEvent>> stateList;                                                                     //�X�e�[�g���X�g
        private readonly Dictionary<StateBase<TOwner, TEvent>, Dictionary<TEvent, StateBase<TOwner, TEvent>>> transitonTableDictionary; //�J�ڃ��X�g

        //�C�x���g����
        private readonly List<TEvent> processableEventList;                         //�����\�ȃC�x���g���X�g
        private readonly Queue<TEvent> stateEventQueue;                             //�X�e�[�g�ɂ���đ��M���ꂽ�����҂��C�x���g
        private readonly Queue<TEvent> eventQueue;                                  //�����҂��C�x���g

        //�p�����[�^
        public readonly IParameterContainer globalParameterContainer;   //�Q�[�������L�ϐ����p
        private readonly IParameterContainer localParameterContainer;   //�O���t���ł̂ݗL��,�ǂݍ��݂͊O������\
        public IParameterContainer readLocalParameterContainer => localParameterContainer;  //�ǂݎ���p


        //�R���X�g���N�^
        public FiniteStateMachine(TOwner owner, IParameterContainer globalParameterContainer, IParameterContainer localParameterContainer)
        {
            this.owner = owner;
            this.globalParameterContainer = globalParameterContainer;
            this.localParameterContainer = localParameterContainer;

            //�t���O�̏�����
            isAcceptableEvent = true;

            //�e�R���N�V�����̏�����
            processableEventList = new List<TEvent>();
            stateList = new List<StateBase<TOwner, TEvent>>();
            transitonTableDictionary = new Dictionary<StateBase<TOwner, TEvent>, Dictionary<TEvent, StateBase<TOwner, TEvent>>>();
            stateEventQueue = new Queue<TEvent>();
            eventQueue = new Queue<TEvent>();
        }





        //�������X�e�[�g�̐ݒ�
        public bool SetInitialState(StateBase<TOwner, TEvent> state)
        {
            //���s���łȂ��Ƃ������X�e�[�g�̐ݒ�\
            if (!isRunning)
            {
                initialState = state;
                return true;
            }
            return false;
        }




        //�����N�����\�b�h
        public void StartUp()
        {
            //����������start�Ăяo��
            lastState = null;
            currentState = initialState;
            currentState.OnStart();
        }


        //�X�V���\�b�h
        public void Tick()
        {
            //���݃X�e�[�g��null�̏ꍇ
            if (!isRunning)
            {
                StartUp();
            }


            //Update�̌Ăяo��
            currentState.OnUpdate();


            //�C�x���g��������������
            while (eventQueue.Count > 0�@|| stateEventQueue.Count > 0)
            {
                //���X�e�[�g���擾����
                StateBase<TOwner, TEvent> nextState = GetNextState(stateEventQueue);    //�X�e�[�g����̃C�x���g���ɏ���

                if (nextState == null) nextState = GetNextState(eventQueue);            //�X�e�[�g����̃C�x���g�őJ�ڂł��Ȃ���ΊO���C�x���g������
                else stateEventQueue.Clear();                                           //�X�e�[�g�C�x���g�ɂ���đJ�ڂ����܂����ꍇ�c��̃X�e�[�g�C�x���g�L���[���N���A

                if (nextState != null) ChangeState(nextState);                          //�ǂ��炩�ŃX�e�[�g���擾�ł��Ă���΃X�e�[�g��ύX����
            }
        }


        //���f���\�b�h
        public void Abort()
        {
            //���݃X�e�[�g�̏I�����Ăяo��
            isAcceptableEvent = false;
            currentState.OnEnd();
            isAcceptableEvent = true;

            //���݃X�e�[�g��null�ɂ���
            lastState = currentState;
            currentState = null;
        }



        //�C�x���g���M(�O��)
        public bool SendEvent(TEvent triggerEvent)
        {
            //�C�x���g���󂯓���\���ǂ���
            if (!isAcceptableEvent || !isRunning) return false;

            //�C�x���g�̏�������(�����ł��Ă����炻�̂܂�true��Ԃ�)
            if (currentState.ReceiveEvent(triggerEvent)) return true;


            //�C�x���g�������\�����肷��
            if (!processableEventList.Contains(triggerEvent)) return false;

            //�L���[�ɐς�
            eventQueue.Enqueue(triggerEvent);
            return true;
        }

        
        //�X�e�[�g���ɂ��C�x���g���M
        private bool SendEventState(TEvent triggerEvent)
        {
            //�C�x���g���󂯓���\���ǂ���
            if (!isAcceptableEvent || !isRunning) return false;

            //�L���[�ɒǉ�
            stateEventQueue.Enqueue(triggerEvent);
            return true;
        }




        //�X�e�[�g�ǉ�
        public void AddState(StateBase<TOwner, TEvent> state)
        {
            if (!stateList.Contains(state))
            {
                //���X�g�ɒǉ�
                stateList.Add(state);

                //������
                state.Initialize(owner, SendEventState, globalParameterContainer, localParameterContainer);

                //�V�����e�[�u���𐶐�����
                Dictionary<TEvent, StateBase<TOwner, TEvent>> transitionTable = new Dictionary<TEvent, StateBase<TOwner, TEvent>>();

                //�e�[�u���̓o�^����
                transitonTableDictionary.Add(state, transitionTable);
            }
        }


        //�J�ڒǉ�
        public bool AddTransition(StateBase<TOwner, TEvent> state, StateBase<TOwner, TEvent> nextState, TEvent triggerEvent)
        {
            //�J�ڌ��Ɛ悪�Ǘ��ς݂�
            if (!stateList.Contains(state) || !stateList.Contains(nextState)) return false;


            //Transition�\�z
            Dictionary<TEvent, StateBase<TOwner, TEvent>> transitionTable = transitonTableDictionary[state];

            //�C�x���g�����łɓo�^����Ă���ꍇ���s
            if (transitionTable.ContainsKey(triggerEvent)) return false;

            //�C�x���g��o�^����
            transitionTable.Add(triggerEvent, nextState);
            processableEventList.Add(triggerEvent);
            return true;
        }

        
        //���݃X�e�[�g�̕ύX
        private void ChangeState(StateBase<TOwner, TEvent> nextState)
        {
            //�I�����Ăяo��
            isAcceptableEvent = false;
            currentState.OnEnd();
            isAcceptableEvent = true;

            //�X�e�[�g��ύX����
            currentState = nextState;

            //�X�^�[�g��Update���Ăяo��
            currentState.OnStart();
            currentState.OnUpdate();
        }


        //���̑J�ڃX�e�[�g���擾(�Ȃ��ꍇ��null��Ԃ�)
        private StateBase<TOwner, TEvent> GetNextState(Queue<TEvent> events)
        {
            //����e�[�u�����擾
            Dictionary<TEvent, StateBase<TOwner, TEvent>> currentStateTable = transitonTableDictionary[currentState];

            //�X�^�b�N���ꂽ�C�x���g����������
            while (events.Count > 0)
            {
                //�C�x���g���|�b�v����
                TEvent triggerEvent = events.Dequeue();

                //�J�ڃ`�F�b�N
                if (currentStateTable.ContainsKey(triggerEvent))
                {
                    //�J�ڃe�[�u���ɂ���ꍇ���݃X�e�[�g�̃K�[�h���`�F�b�N
                    if (!currentState.GuardEvent(triggerEvent)) continue;        //�K�[�h���ꂽ���΂�

                    //�C�x���g����擾����
                    StateBase<TOwner, TEvent> nextState = currentStateTable[triggerEvent];
                    return nextState;
                }
            }
            //�J�ڐ悪������Ȃ������ꍇ
            return null;
        }
    }
}

