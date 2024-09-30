/* 
This file is for the start and end points. It destroys the edge on collision to make the startpoint
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWallOnCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(collision.gameObject);
        }
    }
}
