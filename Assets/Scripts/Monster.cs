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
        waitInPlaceTime = Random.Range(1f, 2);
        agent.destination = transform.position;
        agent.avoidancePriority = Random.Range(10, 99);
        t = Mathf.Infinity;
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance || followingPlayer)
        {


            if (Time.time - t > waitInPlaceTime)
            {
                t = Mathf.Infinity;
                StartCoroutine(SetNewDestination());

            }
            else if (t == Mathf.Infinity)
            {
                t = Time.time;
            }

        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        //  Vector3 localPos = player.transform.TransformPoint(transform.position);
        //   Debug.Log();
        if (distanceToPlayer < 5f &&
            Vector3.Angle(player.transform.forward, transform.position - player.transform.position) < player.lanternAngle 
            && !avoidingPlayer)
        {
            StartCoroutine(AvoidingPlayerForAWhile());
            //Debug.Log("RUN FROM PLAYER");
            t = Mathf.Infinity;
            StartCoroutine(SetNewDestination(true));

        }


        if (distanceToPlayer < 1f)
            FindObjectOfType<GameManager>().GameOver();
    }


    IEnumerator SetNewDestination(bool runFromPlayer = false)
    {
        Vector3 dest = -Vector3.one;
        if (!player.IsInsideLight() && !runFromPlayer)
        {
            dest = GetValidPointAroundPlayer();
            if(dest != -Vector3.one)
            {
                agent.speed = 4f;
                Debug.DrawLine(transform.position + Vector3.up * 2f, dest + Vector3.up * 2f, Color.red, 5f);

            }

        }
        else if (runFromPlayer)
        {
            Vector3 d = -Vector3.one;
            while (d == -Vector3.one)
            {
                /*  d = GetValidPointAroundFireplace();
                Debug.Log("DISTANCE:  " + Vector3.Distance(d, player.transform.position));
                Debug.Log("ANGLE:  " + Vector3.Angle((player.transform.position - transform.position), (d - transform.position))); */
                d = transform.position - player.transform.position;
                d.y = 0f;
                d = player.transform.position+  d * Random.Range(20f,40f);
                /*   if (Vector3.Distance(d, player.transform.position) < 30f && 
                       Vector3.Angle((transform.position- player.transform.position), (d-transform.position)) > 5f)
                       d = -Vector3.one;
                   */
                NavMeshHit hit;
                if (NavMesh.SamplePosition(d, out hit, 10.0f, NavMesh.AllAreas))
                {
                    d = hit.position;
                }
                else
                {
                    d = -Vector3.one;
                }
             

                //    GameObject.Find("aae").transform.position = d;
                yield return new WaitForEndOfFrame();
            }
            agent.speed = 8f;
            Debug.DrawLine(transform.position + Vector3.up * 2f, d + Vector3.up * 2f, Color.blue, 5f);

            dest = d;
        }
        else
        {
            dest = GetValidPointAroundFireplace();
            if (dest != -Vector3.one)
            {
                agent.speed = 2f;
                Debug.DrawLine(transform.position + Vector3.up * 2f, dest + Vector3.up * 2f, Color.green, 5f);

            }
        }

        if (dest != -Vector3.one)
        {
            //    Debug.Log(Vector3.Distance(FindObjectOfType<Fireplace>().transform.position, dest));
            agent.SetDestination(dest);

        }
    }

    Vector3 GetValidPointAroundFireplace()
    {
        /*  float minDistance = Mathf.Lerp(1, 10, FindObjectOfType<Fireplace>().GetPower() / FindObjectOfType<Fireplace>().powerRange.y);
          float maxDistance = Mathf.Lerp(minDistance,
              FindObjectOfType<Fireplace>().mainLightDistanceRange.y / 5f,
              FindObjectOfType<Fireplace>().GetPower() / FindObjectOfType<Fireplace>().powerRange.y);     */
        // Debug.Log(maxDistance);
        float maxMaxDistance = Random.Range(FindObjectOfType<Fireplace>().GetLightDistance(true), 75f);
        // Debug.Log(FindObjectOfType<Fireplace>().GetLightDistance());
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
            StartCoroutine(FollowingPlayer());
            return hit.position;
        }
        waitInPlaceTime = Random.Range(0.1f, 0.2f);
        return -Vector3.one;
    }

    IEnumerator AvoidingPlayerForAWhile()
    {
        avoidingPlayer = true;

        yield return new WaitForSeconds(2f);

        avoidingPlayer = false;
    }

    IEnumerator FollowingPlayer()
    {
        while (followingPlayer)
        {

            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            Vector3 v = GetValidPointAroundPlayer();
            if(v != -Vector3.one)
            agent.SetDestination(v);

        }
    }
}
