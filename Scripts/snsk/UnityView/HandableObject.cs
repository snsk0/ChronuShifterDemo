using UnityEngine;

using Chronus;
using UnityView.Player.Behaviours;


public class HandableObject : MonoBehaviour, IHandableObject
{
    //必要コンポーネント
    private IChronusObject chronusObject;
    [SerializeField] private new Rigidbody rigidbody;
    private new Collider collider;


    private void Awake()
    {
        chronusObject = GetComponent<IChronusObject>();
        defaultParentTransform = transform.parent;
        defaultScale = transform.localScale;
        collider = GetComponent<Collider>();
    }

    public Transform defaultParentTransform { get; private set; }

    public Transform rightHandTransform => throw new System.NotImplementedException();

    public Transform leftHandTransform => throw new System.NotImplementedException();



    //通知関数
    public void OnHandle(bool isHoldUp)
    {
        //chronusObjectのトグル呼び出し
        chronusObject.ToggleCarriedState(isHoldUp);



        //RigidBody
        if (isHoldUp && rigidbody) rigidbody.isKinematic = true;
        else if (!isHoldUp && rigidbody) rigidbody.isKinematic = false;

        //collider
        if (isHoldUp && rigidbody) collider.isTrigger = true;
        else if (!isHoldUp && rigidbody) collider.isTrigger = false;


        //仮コード
        if (isHoldUp)
        {
            isSmalling = true;
            isBigging = false;
        }
        else
        {
            isSmalling = false;
            isBigging = true;
        }
    }


    //仮コード
    private bool isSmalling = false;
    private bool isBigging = false;
    private Vector3 defaultScale;

    private void Update()
    {
        if (isSmalling) 
        {
            gameObject.transform.localScale = 0.9f * (gameObject.transform.localScale);
            if(gameObject.transform.localScale.magnitude < 0.1)
            {
                isSmalling = false;
                gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                gameObject.transform.localPosition = Vector3.zero;
            }
        }
        else if (isBigging)
        {
            gameObject.transform.localScale = 1.1f * (gameObject.transform.localScale);
            if (gameObject.transform.localScale.magnitude > defaultScale.magnitude)
            {
                isBigging = false;
                gameObject.transform.localScale = defaultScale;
            }
        }
    }
}
