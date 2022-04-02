using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPC : MonoBehaviour
{

    NavMeshAgent agent;
    float waitInPlaceTime = 2f;
    float t;
    Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        waitInPlaceTime = Random.Range(2f, 10f);
        agent.destination = transform.position;
        agent.avoidancePriority = Random.Range(10, 99);
        t = Mathf.Infinity;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (Time.time - t > waitInPlaceTime)
            {
                t = Mathf.Infinity;
                SetNewDestination();

            }
            else if( t ==Mathf.Infinity)
            {
                t = Time.time;
            }

        }


    }

    private void LateUpdate()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void SetNewDestination()
    {
        Vector3 dest = GetValidPointAroundFireplace();
        if (dest != -Vector3.one)
        {
            agent.SetDestination(dest);

        }
    }

    Vector3 GetValidPointAroundFireplace()
    {
        float minDistance = Mathf.Lerp(1, 10, FindObjectOfType<Fireplace>().GetPower() / FindObjectOfType<Fireplace>().powerRange.y);
        float maxDistance = Mathf.Lerp(minDistance,
            FindObjectOfType<Fireplace>().mainLightDistanceRange.y / 5f,
            FindObjectOfType<Fireplace>().GetPower() / FindObjectOfType<Fireplace>().powerRange.y);

        Vector2 aroundness = Random.insideUnitCircle.normalized * maxDistance;
        Vector3 startPoint = FindObjectOfType<Fireplace>().transform.position;
        startPoint.x += aroundness.x;
        startPoint.z += aroundness.y;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(startPoint, out hit, 10.0f, NavMesh.AllAreas))
        {
            waitInPlaceTime = Random.Range(2f, 10f);
            return hit.position;
        }
        waitInPlaceTime = Random.Range(0.1f, 0.2f);
        return -Vector3.one;
    }
}
