using UnityEngine;
using System.Collections;

public class Attributes : MonoBehaviour {

	// properties with defaults
	public float transitionTime = 1;
	public bool currentSlide = false;
	public AudioClip audioClip;
	private AudioSource audioSource;
	private bool wasPlayed = false;


	// Use this for initialization
	void Start () {
		/*
		A collider is generated to trigger 
		- sound and
		(- animations) --> todo 

		when the camera enters to the slide
		*/

	   SphereCollider myCollider = gameObject.AddComponent<SphereCollider>();
	   myCollider.center = Vector3.zero; 
	   myCollider.radius = 0.5f;
	   myCollider.isTrigger = true;

	   // add audiosource for audioclip
	   audioSource = gameObject.AddComponent<AudioSource>();
	   audioSource.clip = audioClip;
	
	}
	
	// Update is called once per frame
	void Update () {
		// play sound if available on entering the slide
		if(currentSlide && !wasPlayed && audioClip) {	
				audioSource.Play();
				wasPlayed =true;
		}
		if(!currentSlide) {
			// reset wasplayed if slide is left to replay sound when its active again
			wasPlayed=false;
		}
	}


	void OnTriggerEnter(Collider other) {
        currentSlide = true;
    }

    void OnTriggerExit(Collider other) {
        currentSlide = false;
    }
}


