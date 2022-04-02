using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Monster : MonoBehaviour
{

    NavMeshAgent agent;
    float waitInPlaceTime = 2f;
    float t;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        waitInPlaceTime = Random.Range(2f, 10f);
        agent.destination = transform.position;
        agent.avoidancePriority = Random.Range(10, 99);
        t = Mathf.Infinity;
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
            else if (t == Mathf.Infinity)
            {
                t = Time.time;
            }

        }


    }

    void SetNewDestination()
    {
        Vector3 dest = -Vector3.one;
        if (!FindObjectOfType<PlayerController>().IsInsideLight())
        {
            dest = GetValidPointAroundPlayer();
        }
        else
        {
             dest = GetValidPointAroundFireplace();
          
        }

        if (dest != -Vector3.one)
        {
            //    Debug.Log(Vector3.Distance(FindObjectOfType<Fireplace>().transform.position, dest));
            agent.SetDestination(dest);

        }
    }

    Vector3 GetValidPointAroundFireplace()
    {
        float minDistance = Mathf.Lerp(1, 10, FindObjectOfType<Fireplace>().GetPower() / FindObjectOfType<Fireplace>().powerRange.y);
        float maxDistance = Mathf.Lerp(minDistance,
            FindObjectOfType<Fireplace>().mainLightDistanceRange.y / 5f,
            FindObjectOfType<Fireplace>().GetPower() / FindObjectOfType<Fireplace>().powerRange.y);
        // Debug.Log(maxDistance);
        float maxMaxDistance = Random.Range(maxDistance, 100f);
        Vector2 aroundness = Random.insideUnitCircle.normalized * maxMaxDistance;
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


    Vector3 GetValidPointAroundPlayer()
    {
      
        Vector2 aroundness = Random.insideUnitCircle.normalized * Random.Range(0.25f, 0.5f);
        Vector3 startPoint = FindObjectOfType<PlayerController>().transform.position;
        startPoint.x += aroundness.x;
        startPoint.z += aroundness.y;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(startPoint, out hit, 10.0f, NavMesh.AllAreas))
        {
            waitInPlaceTime = Random.Range(0.5f, 1f);
            return hit.position;
        }
        waitInPlaceTime = Random.Range(0.1f, 0.2f);
        return -Vector3.one;
    }
}
