using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPC : MonoBehaviour
{

    NavMeshAgent agent;
    float waitInPlaceTime = 2f;
    float waitInPlaceTimeEnd = 0;
    float t;
    Animator animator;
    [SerializeField] List<GameObject> models;
    bool goingToFinalPosition = true;
    float minDistanceToFireplace = 3f;
    bool waitingToDie = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        waitInPlaceTime = Random.Range(2f, 10f);
        agent.destination = transform.position;
        agent.avoidancePriority = Random.Range(10, 99);
        t = Mathf.Infinity;

        foreach (var m in models)
        {
            m.SetActive(false);
        }

        models.Random().SetActive(true);

        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (agent.enabled)
            if (FindObjectOfType<GameManager>().worldItems>0)
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
            else
            {


                if (agent.remainingDistance <= agent.stoppingDistance)
                {

                    if (Time.time - t > waitInPlaceTimeEnd )
                    {
                       // goingToFinalPosition = true;
                        t = Mathf.Infinity;
                        StartCoroutine(SetFinalDestination());

                    }
                    else if (t == Mathf.Infinity && !GetComponent<NavMeshObstacle>().enabled && !goingToFinalPosition)
                    {
                        Debug.Log("PARO");
                        GetComponent<NavMeshAgent>().enabled = false;
                        GetComponent<NavMeshObstacle>().enabled = true;
                        waitingToDie = true;
                    }

                }



            }
        else if (waitingToDie)
        {
            if (Vector3.Distance(FindObjectOfType<PlayerController>().transform.position, transform.position) < 0.5f)
            {
                Debug.Log("MUERO");
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
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

    Vector3 GetValidPointAroundFireplace(bool final = false)
    {
        float minDistance = Mathf.Lerp(1, 10, FindObjectOfType<Fireplace>().GetPower() / FindObjectOfType<Fireplace>().powerRange.y);
        float maxDistance = Random.Range(minDistance,
            FindObjectOfType<Fireplace>().mainLightDistanceRange.y / 5f);
        if (FindObjectOfType<Fireplace>().GetPower() < 20f || final)
        {
            maxDistance = 2f;
        }

        Vector2 aroundness = Random.insideUnitCircle.normalized * maxDistance;
        Vector3 startPoint = FindObjectOfType<Fireplace>().transform.position;
        startPoint.x += aroundness.x;
        startPoint.z += aroundness.y;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(startPoint, out hit, 10.0f, NavMesh.AllAreas))
        {
            waitInPlaceTime = Random.Range(1f, 5f);
            return hit.position;
        }
        waitInPlaceTime = Random.Range(0.1f, 0.2f);
        return -Vector3.one;
    }


    IEnumerator SetFinalDestination()
    {
        Debug.Log("SET FINAL DESTINRAION");
        Vector3 d = -Vector3.one;
        while (d == -Vector3.one)
        {
            Debug.Log("NEW TRY");
            d = GetValidPointAroundFireplace(true);
            if (Vector3.Distance(d, FindObjectOfType<Fireplace>().transform.position) > minDistanceToFireplace)
                d = -Vector3.one;
            else
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(d, out hit, 5f, NavMesh.AllAreas))
                {
                   
                    d = hit.position;
                    agent.SetDestination(d);
                }
            }

            yield return new WaitForEndOfFrame();
        }
        waitInPlaceTimeEnd = Mathf.Infinity;

        Debug.Log("VALID");
        yield return new WaitForSeconds(1f);
        goingToFinalPosition = false;
    }
}
