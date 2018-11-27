using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyBot : NetworkBehaviour
{
    public Vector3 flightRange;
    public float timeSinceMovement = 0.0f;
    public float movementTime = 5.0f;

    public Vector3 nextPosition;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    public float standStillTime = 5.0f;
    public GameObject enemyFirePrefab;
    public GameObject currentTarget;

    public float fireRate = 2.0f;
    float timeSinceFire = 0.0f;
 
    // Use this for initialization
    void Start () {
        if (isServer)
        {
            HealthModel health = transform.GetComponent<HealthModel>();
            health.Revive(100);
            health.OnHealthChanged.AddListener(OnHealthChange);
            nextPosition = new Vector3(Random.Range(-flightRange.x, flightRange.x), Random.Range(-flightRange.y, flightRange.y), Random.Range(-flightRange.y, flightRange.y));
        }
    }

    // Update is called once per frame
    void Update () {
        if (isServer)
        {

            if (currentTarget != null)
            {
                transform.LookAt(currentTarget.transform.position);
            }

            transform.position = Vector3.SmoothDamp(transform.position, nextPosition, ref velocity, smoothTime);

            if (Vector3.Distance(transform.position, nextPosition) < 1.0f)
            {
                timeSinceMovement += Time.fixedDeltaTime * 1.0f;
                if (timeSinceMovement > movementTime)
                {
                    movementTime = Random.Range(standStillTime / 2, standStillTime);
                    nextPosition = new Vector3(Random.Range(-flightRange.x, flightRange.x), Random.Range(-flightRange.y, flightRange.y), Random.Range(-flightRange.y, flightRange.y));
                    timeSinceMovement = 0.0f;

                    LookAtTarget();
                }
                if (timeSinceFire > fireRate)
                    CmdShoot();

            }
            else
            {


            }
            timeSinceFire += Time.deltaTime;
        }
    }

    void LookAtTarget()
    {
        GameObject[] targets;
        targets = GameObject.FindGameObjectsWithTag("Player");
        float closestDist = 100.0f;
        for (int i = 0; i < targets.Length; i++)
        {
            float dist = Vector3.Distance(targets[i].transform.position, transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                currentTarget = targets[i];
            }
        }
    }

    [Command]
    void CmdShoot()
    {
        Vector3 offset = transform.TransformPoint(0, 0, 1.0f);
        GameObject enemyFire;
        enemyFire = Instantiate(enemyFirePrefab,  offset, transform.rotation);

        enemyFire.GetComponent<Rigidbody>().velocity = 
            transform.TransformDirection(Vector3.forward * 5.0f);
        timeSinceFire = 0.0f;

        NetworkServer.Spawn(enemyFire);

        // Destroy the bullet after 10 seconds
        Destroy(enemyFire, 10.0f);
    }

    private void OnHealthChange(int health)
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
