using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    private List<AudioSource> _activeAudioSources;
	private const float RAYCAST_MAX_DISTANCE = 50.0F; 

	void Start() 
	{ 
		_activeAudioSources = new List<AudioSource>();
	}

    void Update()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(this.transform.position, this.transform.forward, RAYCAST_MAX_DISTANCE);
      
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
            		if (!_activeAudioSources.Contains(_audioSource)) _activeAudioSources.Add(_audioSource);
            
            		if (!_audioSource.isPlaying) _audioSource.Play();
        		}
        	
			}
		} else {
			foreach (AudioSource audio in _activeAudioSources) { 
				audio.Stop();
			} 
			_activeAudioSources.Clear();
 
		}
 
        Debug.DrawRay(this.transform.position, this.transform.forward * RAYCAST_MAX_DISTANCE, Color.green,15);
    }

}