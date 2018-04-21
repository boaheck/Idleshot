using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent nma;

    bool hasTarget = true;
    bool active = false;
    bool searching = false;
    Vector3 targetPos;
    Coroutine playerSearcher;
    float sightDistance = 10f;
    float shoutDistance = 5f;
    int confidence = 0;
    int maxConfidence = 3;
    Transform closeFriend;

    void Start()
    {
        nma = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        FindTarget();

        GoToTarget();
    }

    void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightDistance);
        bool playerFound = false;
        List<EnemyAI> friends = new List<EnemyAI>();
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                Vector3 direction = col.transform.position - transform.position;
                float distance = Vector3.Magnitude(direction);
                Ray ray = new Ray(transform.position, Vector3.Normalize(direction));
                RaycastHit raycastHit;
                if (Physics.Raycast(ray.origin, ray.direction, out raycastHit, distance))
                {
                    if (raycastHit.collider == col)
                    {
                        SetTarget(col.transform.position, 3);
                        playerFound = true;
                    }
                }
            }
            if (col.GetComponentInParent<EnemyAI>() && col.transform.parent != this)
            {
                Vector3 direction = col.transform.position - transform.position;
                float distance = Vector3.Magnitude(direction);
                Ray ray = new Ray(transform.position, Vector3.Normalize(direction));
                RaycastHit raycastHit;
                if (Physics.Raycast(ray.origin, ray.direction, out raycastHit, distance))
                {
                    Debug.DrawRay(ray.origin, ray.direction * distance, Color.blue, 0.1f);
                    if (raycastHit.collider == col)
                    {
                        friends.Add(col.GetComponentInParent<EnemyAI>());
                    }
                }
            }
        }
        foreach (EnemyAI friend in friends)
        {
            friend.SetTarget(targetPos, confidence - 1); //Tell nearby friends where the enemy is, but lose a bit of confidence doing so.
            if (closeFriend == null)
            {
                closeFriend = friend.transform;
            }
            float closeFriendDistance = Vector3.Magnitude(transform.position - closeFriend.transform.position);
            float friendDistance = Vector3.Magnitude(transform.position - friend.transform.position);
            if (friendDistance < closeFriendDistance)
            {
                closeFriend = friend.transform;
            }
        }
        if (friends.Count == 0)
        {
            closeFriend = null;
        }
        if (!playerFound && !searching)
        {
            hasTarget = false;
            StartPlayerSearch();
        }
    }

    public void SetTarget(Vector3 target, int confidence)
    {
        if (confidence > 0 && confidence >= this.confidence)
        {
            targetPos = target;
            active = true;
            this.confidence = confidence;
            if (playerSearcher != null && confidence == maxConfidence)
            {
                searching = false;
                StopCoroutine(playerSearcher);
            }
        }
    }

    void GoToTarget()
    {
        if (active)
        {
            nma.SetDestination(targetPos);
            Debug.DrawLine(transform.position, targetPos, Color.red, 0.1f);
        }
        else
        {
            if (closeFriend != null)
            {
                Vector3 target = closeFriend.position;
                nma.SetDestination(target);
                Debug.DrawLine(transform.position, target, Color.green, 0.1f);
            }
        }
    }

    void StartPlayerSearch()
    {
        if (playerSearcher != null)
        {
            StopCoroutine(playerSearcher);
        }
        playerSearcher = StartCoroutine(SearchForPlayer());
    }

    IEnumerator SearchForPlayer()
    {
        searching = true;
        yield return new WaitForSeconds(2f);
        confidence--;
        yield return new WaitForSeconds(2f);
        confidence--;
        yield return new WaitForSeconds(2f);
        active = false;
        searching = false;
        confidence = 0;
    }
}
