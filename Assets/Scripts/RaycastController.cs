using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    private AudioSource _activeAudioSource;
	private const float RAYCAST_MAX_DISTANCE = 50.0F; 

    void Start()
    {
  
        
    }

    void Update()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(this.transform.position, this.transform.forward, RAYCAST_MAX_DISTANCE);
        //var didHit = Physics.Raycast(this.transform.position, this.transform.forward, out var hit, RAYCAST_MAX_DISTANCE);
		if(hits.Length > 0) {
			for (int i = 0; i < hits.Length; i++)
        	{
				RaycastHit hit = hits[i];
				Debug.Log($"{hit.transform.gameObject.name}");
        		if (hit.transform.gameObject.name.Contains("Cluster") )
        		{
            		Debug.Log($"HITTTTTTTTT {hit.transform.gameObject.name}");
          
            		GameObject audio = GameObject.Find($"Audio{hit.transform.gameObject.name}");
           
            		AudioSource _audioSource = audio.GetComponent<AudioSource>();
            		if (_audioSource != _activeAudioSource) _activeAudioSource = _audioSource;
            
            		if (!_activeAudioSource.isPlaying) _activeAudioSource.Play();
        		}
        	
			}
		} else {
 			if (_activeAudioSource != null) _activeAudioSource.Stop();
		}
 
        Debug.DrawRay(this.transform.position, this.transform.forward * RAYCAST_MAX_DISTANCE, Color.green,15);
    }

}