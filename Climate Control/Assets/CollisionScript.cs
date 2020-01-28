using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    public AudioSource airlock;
    // Start is called before the first frame update
    void Start()
    {
        airlock = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        void OnCollisionEnter (Collision collision)
        {
            if(collision.gameObject.tag == "Target")
            {
                airlock.Play();
            }
        }
    }
}
