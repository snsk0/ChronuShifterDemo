using UnityEngine;

using Player.Presenter;
using Player.Structure;
using InputProviders.Player;


namespace UnityView.Player.Utils
{
    public class UnityMoveDirectionConverter : MonoBehaviour, IMoveDirectionConverter
    {
        public LookDirection ConvertMoveInputData(MoveInputData inputData)
        {
            Vector3 forward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 right = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;
            Vector3 vec = (inputData.y * forward + inputData.x * right).normalized;

            return new LookDirection(vec.x, vec.z);
        }
    }
}
