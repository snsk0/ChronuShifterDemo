using System.Collections.Generic;

using UnityEngine;

using Player;
using Player.Presenter;
using Player.States;
using Animation.Player;
using Animation.Triggerer;
using Animation.Presenter;
using InputProviders.Player;
using InputProviders.LifeCycle;
using FSM.Parameter;
using FSMUtils;

using UnityView.Player.Animation;


namespace UnityView.Player
{
    public class PlayerCore : MonoBehaviour
    {
        //�R���|�[�l���g���X�g(�I�u�W�F�N�g�R���e�i�i�[����
        [SerializeField] private List<MonoBehaviour> playerBehaviourList;

        //�X�e�[�g�}�V���R�A
        private PlayerFSMOwner fsmCore;



        //������
        private void Awake()
        {
            //�A�j���[�V������Presenter�R�Â�
            AnimationPresenter<PlayerAnimationType> presenter = new AnimationPresenter<PlayerAnimationType>(GetComponent<IAnimationPlayer<PlayerAnimationType>>());
            foreach (INonLoopAnimationTriggable<PlayerAnimationType> triggable in GetComponents<INonLoopAnimationTriggable<PlayerAnimationType>>()) presenter.RegisterAnimationTriggable(triggable);
            foreach (ILoopAnimationTriggable<PlayerAnimationType> triggable in GetComponents<ILoopAnimationTriggable<PlayerAnimationType>>()) presenter.RegisterAnimationTriggable(triggable);


            //FSMUpdator��������
            FSMUpdateManager.Initialize(GetComponent<IInputUpdateProvider>());


            //�R���e�i�𐶐�
            ObjectContainer objectContainer = new ObjectContainer();
            foreach (object obj in playerBehaviourList) objectContainer.AddObject(obj);


            //�p�����[�^�R���e�i�̐���
            IParameterContainer globalParameterContainer = new DefaultParameterContainer();
            globalParameterContainer.InitializePlayerGlobalParameter();
            IParameterContainer localParameterContainer = new DefaultParameterContainer();
            localParameterContainer.InitializePlayerLocalParameter();



            //fsm�R�A�𐶐�
            fsmCore = new PlayerFSMOwner(objectContainer, FSMUpdateManager.instance, PlayerFSMBuilder.builder, globalParameterContainer, localParameterContainer);


            //�v���C���[�̃v���[���^�[�𐶐�
            new PlayerEventPresenter(fsmCore, fsmCore, GetComponent<IPlayerInputProvider>(), GetComponent<IDamagableEventSender>(), GetComponent<IMoveDirectionConverter>());
        }


        //�J�n
        private void Start()
        {
            fsmCore.StartFSM();
        }


        //�X�V����(Debug)
        void Update()
        {
            //Debug.Log(fsmCore.playerLocalParameterContainer.GetParameter<bool>(PlayerParameterKey.isSprint));
            //Debug.Log(fsmCore.currentLeafState.GetType().Name);
        }
    }
}
