using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Monster : MonoBehaviour
{

    NavMeshAgent agent;
    float waitInPlaceTime = 2f;
    float t;
    bool followingPlayer = false;
    bool avoidingPlayer = false;
    PlayerController player;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
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
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < 10f && !followingPlayer && !avoidingPlayer)
        {
            StartCoroutine(AvoidingPlayerForAWhile());
            //if (player.GetComponent<PlayerLight>().GetPower() > 0)
           // {
                t = Mathf.Infinity;
                SetNewDestination(true);
            //}
        }


        if (distanceToPlayer < 1f)
            FindObjectOfType<GameManager>().GameOver();
    }


    void SetNewDestination(bool runFromPlayer = false)
    {
        Vector3 dest = -Vector3.one;
        if (!player.IsInsideLight() && !runFromPlayer)
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
            followingPlayer = false;
            return hit.position;
        }
        waitInPlaceTime = Random.Range(0.1f, 0.2f);
        return -Vector3.one;
    }


    Vector3 GetValidPointAroundPlayer()
    {
      
        Vector2 aroundness = Random.insideUnitCircle.normalized * 0.25f;
        Vector3 startPoint = player.transform.position;
        startPoint.x += aroundness.x;
        startPoint.z += aroundness.y;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(startPoint, out hit, 10.0f, NavMesh.AllAreas))
        {
            waitInPlaceTime = Random.Range(0.5f, 1f);
            followingPlayer = true;
            return hit.position;
        }
        waitInPlaceTime = Random.Range(0.1f, 0.2f);
        return -Vector3.one;
    }

    IEnumerator AvoidingPlayerForAWhile()
    {
        avoidingPlayer = true;

        yield return new WaitForSeconds(5f);

        avoidingPlayer = false;
    }
}
