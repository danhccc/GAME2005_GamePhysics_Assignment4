using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Color = UnityEngine.Color;


[System.Serializable]
public class CubeBehaviour : MonoBehaviour
{
    public float mass;
    public Vector3 direction;
    public Vector3 size;
    public Vector3 max;
    public Vector3 min;
    public bool isColliding;
    public bool hitByBullet;
    public bool debug;
    public Vector3 velocity;
    //public Vector3 acceleration;
    private float gravity;
    public float gravityScale;
    public float timer;
    public float force;
    public float speed;
    public bool isStatic;

    public List<CubeBehaviour> contacts;

    private MeshFilter meshFilter;
    private Bounds bounds;

    // Start is called before the first frame update
    void Start()
    {
        debug = false;
        meshFilter = GetComponent<MeshFilter>();

        bounds = meshFilter.mesh.bounds;
        size = bounds.size;
        hitByBullet = false;

        timer = 0.0f;
        gravity = -0.001f;
        velocity = Vector3.zero;
        //acceleration = new Vector3(0.0f, , 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        max = Vector3.Scale(bounds.max, transform.localScale) + transform.position;
        min = Vector3.Scale(bounds.min, transform.localScale) + transform.position;

        if(!isStatic)
        {
            _Move();
        }

    }

    void FixedUpdate()
    {
        // physics related calculations

        //if (hitByBullet == true)
        //{
        //    if (transform.position.y > 0)
        //    {
        //        velocity += acceleration * 0.5f * timer * timer;
        //        transform.position += velocity;
        //    }
        //}
    }

    //private void _Move()
    //{
    //    velocity = acceleration;
    //    transform.position += velocity;
    //}
    private void _Move()
    {
        force = mass * speed;
        velocity = direction * speed * Time.deltaTime;
        //velocity = direction * speed * Time.deltaTime;
        transform.position += velocity;
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.magenta;

            Gizmos.DrawWireCube(transform.position, Vector3.Scale(new Vector3(1.0f, 1.0f, 1.0f), transform.localScale));
        }
    }

}
