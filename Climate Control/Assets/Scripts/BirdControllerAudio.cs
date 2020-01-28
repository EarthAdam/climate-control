using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControllerAudio : MonoBehaviour
{
    public GameObject target; //A variable to hold the bird game object
    int currentPosition; //A variable to keep track of the current transform point
    public Transform[] pointTransforms; //An array to hold the transform points for the bird to fly between
    public AudioClip[] audioClips; //An array to hold the audio clips to play (bird calling and flying)
    bool isFlying; //A variable to ensure that the coroutine is called once
    AudioSource source; //A variable to hold the audio source on the bird game object

    void Start()
    {
        currentPosition = 0;
        isFlying = false;
        source = target.GetComponent<AudioSource>();
        source.clip = audioClips[0];
        source.Play(); //Playing the calling sound initially
    }

    void Update()
    {
        Transform camera = Camera.main.transform;
        Ray ray = new Ray(camera.position, camera.rotation * Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject == target) && !isFlying)
        {
            target.transform.GetComponent<Animator>().SetTrigger("StartFlying"); //Triggering the flying state
            StartCoroutine(BirdMove());
        }
    }

    IEnumerator BirdMove()
    {
        isFlying = true;
        source.Stop();
        source.clip = audioClips[1];
        source.Play(); //Playing the flying sound

        while (true)
        {
            Vector3 startPosition = pointTransforms[currentPosition].position;
            Vector3 endPosition = pointTransforms[(currentPosition + 1) % pointTransforms.Length].position;

            //Making the bird rotated in the direction of this transform
            target.transform.rotation = pointTransforms[currentPosition].rotation; 
            for (float f = 0.0f; f < 1.0f; f += Time.deltaTime / 4.0f)
            {
                target.transform.position = Vector3.Lerp(startPosition, endPosition, f);
                yield return null;
            }

            currentPosition = (currentPosition + 1) % pointTransforms.Length;

            if (currentPosition == 0)
            {
                //Triggering the idle state when all transforms are visited
                target.transform.GetComponent<Animator>().SetTrigger("BacktoIdle"); 
                target.transform.position = pointTransforms[0].position;
                target.transform.rotation = pointTransforms[0].rotation;
                source.Stop();
                source.clip = audioClips[0];
                source.Play(); //Playing the calling sound
                break; //Once all the transforms are visited, we exit the while and the coroutine
            }
        }

        //We set the control variable to false so that the coroutine can be called again
        isFlying = false; 
    }
}

