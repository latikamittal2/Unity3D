using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider bx;
    bool disableRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        bx = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if(!disableRotation)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            disableRotation = true;
            rb.isKinematic = true;
            bx.isTrigger = true;
        }
        
        if(collision.gameObject.tag == "animal")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject , 0.5f);
        }
    }
}
