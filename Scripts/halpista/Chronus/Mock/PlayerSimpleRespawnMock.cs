using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSimpleRespawnMock : MonoBehaviour
{
    [SerializeField] WayPointDataMock wayPointDataMock;
    Transform[] wayPointData;

    [SerializeField] float lowestHeight = -20;

    [SerializeField] int respawnNumber;

    void Awake()
    {
        if(wayPointDataMock == null)
        {
            wayPointData = new Transform[] {transform};
            respawnNumber = 0;
        }
        else
        {
            wayPointData = wayPointDataMock.wayPointData;
            respawnNumber = wayPointDataMock.wayPointNumber;

            transform.SetPositionAndRotation(wayPointData[respawnNumber].position, wayPointData[respawnNumber].rotation);
        }
    }

    void Update()
    {
        if(transform.position.y < lowestHeight)
        {
            transform.SetPositionAndRotation(wayPointData[respawnNumber].position, wayPointData[respawnNumber].rotation);
        }
    }

    int cnt;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Respawn"))
        {
            cnt = 0;
            foreach(Transform t in wayPointData)
            {
                if(t.position == other.transform.position)
                {
                    respawnNumber = Mathf.Max(respawnNumber, cnt);
                }
                cnt++;
            }
        }
    }
}