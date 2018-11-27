using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var health = hit.GetComponentInParent<HealthModel>();
        if (health != null)
        {
            health.TakeDamage(10);
        }

        Destroy(gameObject);
    }
}
