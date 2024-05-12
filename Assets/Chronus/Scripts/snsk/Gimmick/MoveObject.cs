using UnityEngine;
using StageGimmick;
using Chronus.KinematicPhysics;

namespace Chronus.OrbGimmick
{
    public class MoveObject : GimmickMotion
    {
        [SerializeField] private PhysicsMover _physicsBehaviour;

        public override void OpenMotion()
        {
            _physicsBehaviour.enabled = true;
        }

        public override void CloseMotion()
        {
            _physicsBehaviour.enabled = false;
        }
    }
}
