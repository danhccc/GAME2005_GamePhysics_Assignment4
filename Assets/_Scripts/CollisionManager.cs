using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class CollisionManager : MonoBehaviour
{
    public CubeBehaviour[] cubes;
    public BulletBehaviour[] spheres;
    public PlayerBehaviour player;

    private static Vector3[] faces;

    // Start is called before the first frame update
    void Start()
    {
        cubes = FindObjectsOfType<CubeBehaviour>();

        faces = new Vector3[]
        {
            Vector3.left, Vector3.right,
            Vector3.down, Vector3.up,
            Vector3.back , Vector3.forward
        };
    }

    // Update is called once per frame
    void Update()
    {
        spheres = FindObjectsOfType<BulletBehaviour>();

        // Checking AABB with every other AABB in the scene
        for (int i = 0; i < cubes.Length; i++)
        {
            for (int j = 0; j < cubes.Length; j++)
            {
                if (i != j)
                {
                    CheckAABBs(cubes[i], cubes[j]);
                }
            }
        }


        // Check each sphere against each AABB in the scene
        foreach (var sphere in spheres)
        {
            foreach (var cube in cubes)
            {
                if (cube.name != "Player")
                {
                    CheckSphereAABB(sphere, cube);
                }

            }
        }
    }

    public static void CheckAABBs(CubeBehaviour a, CubeBehaviour b)
    {
        if ((a.min.x <= b.max.x && a.max.x >= b.min.x) &&
            (a.min.y <= b.max.y && a.max.y >= b.min.y) &&
            (a.min.z <= b.max.z && a.max.z >= b.min.z))
        {
            if (!a.contacts.Contains(b))
            {
                a.contacts.Add(b);
                a.isColliding = true;
                b.isColliding = true;

                b.speed = a.force;
                b.direction = a.direction;
                a.direction *= -1;


                //a.velocity = Vector3.zero;

            }
        }
        else
        {
            if (a.contacts.Contains(b))
            {
                a.contacts.Remove(b);
                a.isColliding = false;
                b.isColliding = false;
            }
           
        }
    }

    public static void CheckSphereAABB(BulletBehaviour bullet, CubeBehaviour cube)
    {
        // get box closest point to sphere center by clamping
        var x = Mathf.Max(cube.min.x, Mathf.Min(bullet.transform.position.x, cube.max.x));
        var y = Mathf.Max(cube.min.y, Mathf.Min(bullet.transform.position.y, cube.max.y));
        var z = Mathf.Max(cube.min.z, Mathf.Min(bullet.transform.position.z, cube.max.z));

        var distance = Math.Sqrt((x - bullet.transform.position.x) * (x - bullet.transform.position.x) +
                                 (y - bullet.transform.position.y) * (y - bullet.transform.position.y) +
                                 (z - bullet.transform.position.z) * (z - bullet.transform.position.z));

        if ((distance < bullet.radius) && (!bullet.isColliding))
        {
            // determine the distances between the contact extents
            float[] distances = {
                (cube.max.x - bullet.transform.position.x),
                (bullet.transform.position.x - cube.min.x),
                (cube.max.y - bullet.transform.position.y),
                (bullet.transform.position.y - cube.min.y),
                (cube.max.z - bullet.transform.position.z),
                (bullet.transform.position.z - cube.min.z)
            };

            float penetration = float.MaxValue;
            Vector3 face = Vector3.zero;

            // check each face to see if it is the one that connected
            for (int i = 0; i < 6; i++)
            {
                if (distances[i] < penetration)
                {
                    // determine the penetration distance
                    penetration = distances[i];
                    face = faces[i];
                }
            }

            bullet.penetration = penetration;
            bullet.collisionNormal = face;

            if (cube.hitByBullet == false)
            {
                cube.direction = new Vector3(0.0f, -1.0f, 0.0f);
                cube.speed = bullet.force;
                cube.hitByBullet = true;
            }
            else if (cube.isColliding)
            {
                cube.direction = bullet.collisionNormal;
                cube.speed = bullet.force;

            }

            bullet.speed *= 0.6f;

            Reflect(bullet);
        }

    }

    // Use this to bounce the bullet back
    private static void Reflect(BulletBehaviour bullet)
    {
        if ((bullet.collisionNormal == Vector3.forward) || (bullet.collisionNormal == Vector3.back))
        {
            bullet.direction = new Vector3(bullet.direction.x, bullet.direction.y, -bullet.direction.z);
        }
        else if ((bullet.collisionNormal == Vector3.right) || (bullet.collisionNormal == Vector3.left))
        {
            bullet.direction = new Vector3(-bullet.direction.x, bullet.direction.y, bullet.direction.z);
        }
        else if ((bullet.collisionNormal == Vector3.up) || (bullet.collisionNormal == Vector3.down))
        {
            bullet.direction = new Vector3(bullet.direction.x, -bullet.direction.y, bullet.direction.z);
        }
    }


}
