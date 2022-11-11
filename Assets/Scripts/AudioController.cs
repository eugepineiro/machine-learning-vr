using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private const int TOTAL_CLUSTERS = 4;

    private List<List<float>> _centroids;
    private List<AudioClip> _audioClips;
    // Factory Pattern (spawner:audios)
    [SerializeField] private Audio audioPrefab;
  
    private Spawner<Audio> _audioFactory = new Spawner<Audio>();
    public List<Audio> AudioInstances => _audioInstances;
    private List<Audio> _audioInstances = new List<Audio>();
    private Transform _audioParent;
    private Vector3 _audioInitialPosition = new Vector3(0, 0, 0);
    private AudioSource _audioSource;
    private AudioClip audio1;
    void Start()
    {
        _audioClips = getAudioClips();
        _centroids = getCentroids(); 
        _audioParent = GameObject.Find("Audios").transform;
        
        for (int i = 0; i < TOTAL_CLUSTERS; i++)
        {
            var audio = _audioFactory.Create(audioPrefab);
            audio.transform.parent = _audioParent;
            
            // Set Audio properties
            var centroid = _centroids[i];
            audio.transform.position = _audioInitialPosition + new Vector3(centroid[0],centroid[1]+12,centroid[2]);
            _audioSource = audio.GetComponent<AudioSource>();
            _audioSource.clip = _audioClips[i];
            _audioSource.Play();
            
            _audioInstances.Add(audio); // Instantiate audio
        }

    }
    
    private List<AudioClip> getAudioClips()
    {
        List<string> paths = new List<string>() //TODO dont hardcode 
            { "Audio/piano-mp3/A0", "Audio/piano-mp3/B0", "Audio/piano-mp3/C1", "Audio/piano-mp3/D1" };
        
        List<AudioClip> audioClips = new List<AudioClip>();
        for (int i = 0; i < TOTAL_CLUSTERS; i++)
        {
            audioClips.Add(Resources.Load<AudioClip>(paths[i])); 
        }

        return audioClips;

    }
    
    private List<List<float>> getCentroids()
    {

        List<List<float>> centroids = new List<List<float>>();
 
        centroids.Add(new List<float>() {-3.5F, 2, 6.5F}); //TODO dont hardcode
        centroids.Add(new List<float>() {-2.7F, 4, -7F});
        centroids.Add(new List<float>() {4.3F, 1.7F, -5.5F});
        centroids.Add(new List<float>() {6, -3, 2.7F});

        return centroids;

    }
}
