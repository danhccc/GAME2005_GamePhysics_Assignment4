using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletBehaviour : MonoBehaviour
{
    public float speed;
    public float mass;
    public Vector3 direction;
    public float range;
    public float radius;
    public bool debug;
    public bool isColliding;
    public Vector3 collisionNormal;
    public float penetration;
    public BulletManager bulletManager;
    public Vector3 velocity;
    public float force;

    // Start is called before the first frame update
    void Start()
    {
        isColliding = false;
        radius = Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z) * 0.5f;
        bulletManager = FindObjectOfType<BulletManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
        _CheckBounds();
    }

    private void _Move()
    {
        force = mass * speed;
        velocity = direction * speed * Time.deltaTime;
        //velocity = direction * speed * Time.deltaTime;
        transform.position += velocity;
    }

    private void _CheckBounds()
    {
        if (Vector3.Distance(transform.position, Vector3.zero) > range)
        {
            Destroy(gameObject);
        }
    }
}
