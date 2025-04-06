using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    public Transform LeftPatrolPoint;
    public Transform RightPatrolPoint;

    [Header("The body of the enemy that is moving")]
    public Transform Body;

    public int MovementSpeed;

    private bool isPatrollingLeft = true;

    private void Update()
    {
        if (isPatrollingLeft)
        {
            Body.transform.position += Vector3.left * Time.deltaTime * MovementSpeed;
            if (Body.transform.position.x <= LeftPatrolPoint.position.x)
            {
                isPatrollingLeft = false;
            }
        }
        else
        {
            Body.transform.position += Vector3.right * Time.deltaTime * MovementSpeed;
            if (Body.transform.position.x >= RightPatrolPoint.position.x)
            {
                isPatrollingLeft = true;
            }
        }
    }
}
