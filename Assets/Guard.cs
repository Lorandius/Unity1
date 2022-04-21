using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public static event System.Action OnGuardHasSpottenPlayer;
    [SerializeField] private float speed = 5;
    [SerializeField] private float turnSpeed = 5;
    [SerializeField] private float waitTime = .3f;
    [SerializeField] private float turnTime = .3f;
    [SerializeField] private float timeToSpotPlayer = .5f;
    [SerializeField] private Transform pathHolder;

    [SerializeField] private Light spotLight;

    [SerializeField] private float viewDistance;
    [SerializeField] private LayerMask viewMask;

    private float viewAngle;
    float playerVisibleTimer;
    private Transform player;

    private Color originalSpotlightColor;
    // Start is called before the first frame update
    void Start()
    {
        viewAngle = spotLight.spotAngle;
        originalSpotlightColor = spotLight.color;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }

        StartCoroutine(FollowPath(waypoints));
    }

    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];
        int targetWaypointsIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointsIndex];
        transform.LookAt(targetWaypoint);
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint,
                speed * Time.deltaTime);
            if (transform.position == targetWaypoint)
            {
                targetWaypointsIndex = (targetWaypointsIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointsIndex];
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }
            yield return null;
        }
        

        }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 -Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;
        while (Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle,
                turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    bool CanSeePlayer()
    {
        
        if (Vector3.Distance(transform.position, player.position) < viewDistance )
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position,  viewMask))
                {
                    return true;
                }
            }
            
        }

        return false;
    }
    // Update is called once per frame
    void Update()
    {
        if (CanSeePlayer())
        {
            playerVisibleTimer += Time.deltaTime;
        }
        else
        {
            playerVisibleTimer -= Time.deltaTime;
        }

        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
        spotLight.color = Color.Lerp(originalSpotlightColor, Color.red, playerVisibleTimer / timeToSpotPlayer);
        if (playerVisibleTimer >= timeToSpotPlayer)
        {
            if (OnGuardHasSpottenPlayer != null)
            {
                OnGuardHasSpottenPlayer();
            }
        }
    }
}
