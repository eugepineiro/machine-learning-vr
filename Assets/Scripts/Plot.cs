using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using ClusteringKMeans;
using KMeans;
using AudioManager;

public class Plot : MonoBehaviour
{

    // Travel and Rotation Movement
    [SerializeField] private KeyCode _moveUp = KeyCode.Q;
    [SerializeField] private KeyCode _moveDown = KeyCode.E;
    [SerializeField] private KeyCode _moveLeft = KeyCode.A;
    [SerializeField] private KeyCode _moveRight = KeyCode.D;

    [SerializeField] private KeyCode _rotateUp = KeyCode.UpArrow;
    [SerializeField] private KeyCode _rotateDown = KeyCode.DownArrow;
    [SerializeField] private KeyCode _rotateLeft = KeyCode.LeftArrow;
    [SerializeField] private KeyCode _rotateRight = KeyCode.RightArrow;

    [SerializeField] private KeyCode _zoomIn = KeyCode.W;
    [SerializeField] private KeyCode _zoomOut = KeyCode.S;
    [SerializeField] private KeyCode _reset = KeyCode.R;

    [SerializeField] public string FilePath => _configFilePath;
    [SerializeField] private string _configFilePath;

    [SerializeField] public GameObject DataPointPrefab => _dataPointPrefab;
    [SerializeField] private GameObject _dataPointPrefab;

    [SerializeField] private Material quadMaterial;

    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private ParticleSystem.Particle[] _particles;
    [SerializeField] private List<ClusterPoint> _points;

    [SerializeField] private Cluster[][] _clusters;

    [SerializeField] public Material PointMaterial => _pointMaterial;
    [SerializeField] private Material _pointMaterial;
    
    private readonly int MAX_ITERATIONS = 100;

    private JsonConfig _config;

    private Vector3 _horizontalForward;
    private Vector3 _forward;
    private MovementController _movementController;
    private AudioController _audioController;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Vector3 _intialScale;
    private double _maxPosition;
    private int iterations;
 
    
    private float timer = 0; 
    private int i_iteration = 0; 
    private List<GameObject> _quads = new List<GameObject>();

    void Start()
    {
        _movementController = GetComponent<MovementController>();
        _initialPosition = transform.localPosition;
        _initialRotation = transform.rotation;
        _intialScale = transform.localScale;
        _maxPosition = transform.localPosition.y;
        _audioController = GetComponent<AudioController>();

        InitPlot();
        Debug.Log("DEBUG");
        for (int i = 0; i < iterations; i++)
        {
            int clusterId = 0;
            foreach (Cluster cluster in _clusters[i])
            {
                foreach (DataVec point in cluster.Points)
                {
                    Debug.Log($"it {i} || { clusterId} || {point.Components[0]},{point.Components[1]},{point.Components[2]}");
                }

                clusterId++;
            }
            
        }
       

        Debug.Log("START");
    }

    void Update()
    {
      
        timer += Time.deltaTime;
        
        if (timer > 2F && i_iteration < iterations )
        {
            Debug.Log(timer);
            UpdateColors(i_iteration++);
            timer = 0;
        } 
        
        /* Translate */
        if (Input.GetKey(_moveUp) && transform.position.y < _maxPosition * 2) _movementController.Travel(new Vector3(0, 1, 0));
        if (Input.GetKey(_moveDown) && transform.position.y > _maxPosition) _movementController.Travel(new Vector3(0, -1, 0));
        if (Input.GetKey(_moveLeft) && transform.position.x > -_maxPosition * 2) _movementController.Travel(new Vector3(-1, 0, 0));
        if (Input.GetKey(_moveRight) && transform.position.x < _maxPosition * 2) _movementController.Travel(new Vector3(1, 0, 0));

        /* Rotate */
        if (Input.GetKey(_rotateUp)) _movementController.Rotate(new Vector3(0, 1, 0));
        if (Input.GetKey(_rotateDown)) _movementController.Rotate(new Vector3(0, -1, 0));
        if (Input.GetKey(_rotateLeft)) _movementController.Rotate(new Vector3(-1, 0, 0));
        if (Input.GetKey(_rotateRight)) _movementController.Rotate(new Vector3(1, 0, 0));

        /* Zoom in/out */
        if (Input.GetKey(_zoomIn)) _movementController.Scale("IN");
        if (Input.GetKey(_zoomOut)) _movementController.Scale("OUT");

        /* Reset initial postion, rotation and scale */
        if (Input.GetKey(_reset)) _movementController.Reset(_initialPosition, _initialRotation, _intialScale);
        
    }
    private void InitPlot()
    {
        _config = JsonUtility.FromJson<JsonConfig>(File.ReadAllText(_configFilePath));

        Debug.Log(_config.K);
        Debug.Log(_config.CsvPath);
        Debug.Log(_config.ParticleSystem);

        _points = File.ReadAllLines(_config.CsvPath)
                                .Skip(1)
                                .Select(v => ClusterPoint.FromCsv(v))
                                .ToList();

        Debug.Log(_points);

        (_clusters, iterations) = KMeansAlgorithm.Run(_points, _config.K); // KMeans
        int clusterId = 1;
        List<DataVec> centroids = new List<DataVec>();
        Debug.Log($"_CLUSTERS {_clusters[1][0].Centroid}");
        
        foreach (Cluster cluster in _clusters[iterations-1])
        { // Create points for first K-Means iteration
            
            GameObject clusterGameObject = new GameObject($"Cluster{clusterId}");
            Transform clusterTransform = clusterGameObject.transform;
            clusterTransform.parent = transform;
            clusterTransform.localPosition = Vector3.zero;
            int i = 0;
            GameObject dataPoint;
            foreach (DataVec point in cluster.Points)
            {

                GameObject parentLookAt = new GameObject();

                parentLookAt.name = $"Point Parent {i}";
                dataPoint = GameObject.CreatePrimitive(PrimitiveType.Quad);
                dataPoint.GetComponent<MeshRenderer>().material = quadMaterial;
                dataPoint.transform.localRotation = Quaternion.Euler(0, 180, 0);
                dataPoint.transform.parent = parentLookAt.transform;
                dataPoint.name = $"Point Quad {i++}";

                parentLookAt.AddComponent<LookAtPlayerBehaviour>();
                parentLookAt.transform.parent = clusterTransform;
                dataPoint.transform.parent = parentLookAt.transform;
                parentLookAt.transform.localPosition = new Vector3((float)point.Components[0],
                    (float)point.Components[1], (float)point.Components[2]);
                parentLookAt.transform.localRotation = Quaternion.identity;

                dataPoint.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                dataPoint.GetComponent<Renderer>().material.color = Color.black;
                dataPoint.GetComponent<Renderer>().material
                    .SetColor("_EmissionColor", GetColorByCluster(clusterId));
                _quads.Add(dataPoint);
            }

            clusterGameObject.AddComponent<ClusterBoundsCalculator>();

            if (_config.ParticleSystem)
            {
                foreach (Transform child in clusterTransform)
                {
                    if (child.name.Contains("Point"))
                        GameObject.Destroy(child.gameObject);
                }

                gameObject.AddComponent<ParticleSystem>();
            }

            clusterId++;
            centroids.Add(cluster.Centroid);
        }
         
        _audioController.LoadAudios(_config.K, centroids);
    }

    private void UpdateColors(int i)
    {
        int clusterId = 1;
        
        foreach (Cluster cluster in _clusters[i])
        { // Create points for each K-Means iteration
            
            GameObject dataPoint;
            foreach (DataVec point in cluster.Points)
            {
                dataPoint = FindQuad(point); 
                dataPoint.GetComponent<Renderer>().material
                    .SetColor("_EmissionColor", GetColorByCluster(clusterId));
            }
            
            clusterId++; 
        }
        
    }

    private IEnumerator WaitSeconds()
    {
        yield return new WaitForSecondsRealtime(4);
    }
    private GameObject FindQuad(DataVec point)
    {
        foreach (GameObject quad in _quads)
        {
            float distance = Vector3.Distance(quad.transform.parent.transform.localPosition, 
                new Vector3((float)point.Components[0], (float)point.Components[1], (float)point.Components[2]));
            if (distance < 0.001F)
            {
                return quad;
            }
        }

        return null;
    }
    
    private Color GetColorByCluster(int cluster)
    {
        Color[] colors = new Color[] {
            new Color(0.902F,0.098F,0.294F,1F),
            new Color(0.96F,0.51F,0.188F,1F),
            new Color(1F,1F,0.294F,1F),
            new Color(0.824F,0.96F,0.235F,1F),
            new Color(0.235F,0.706F,0.294F,1F),
            new Color(0.275F,0.941F,0.941F,1F),
            new Color(0F,0.51F,0.784F,1F),
            new Color(0.568F,0.118F,0.706F,1F),
            new Color(0.941F,0.196F,0.902F,1F),
            new Color(0.502F,0.502F,0.502F,1F),
            new Color(0.98F,0.745F,0.831F,1F),
            new Color(1F,0.843F,0.706F,1F),
            new Color(1F,0.98F,0.784F,1F),
            new Color(0.667F,1F,0.765F,1F),
            new Color(0.863F,0.745F,1F,1F),
            new Color(1F,1F,1F,1F),
            new Color(0.502F,0F,0F,1),
            new Color(0.667F,0.431F,0.157F,1F),
            new Color(0.502F,0.502F,0F,1F),
            new Color(0F,0.502F,0.502F,1F),
            new Color(0F,0F,0.502F,1F),
            new Color(0.48F, 0F, 1, 1), // violeta
			new Color(0.48F, 0F, 1, 1),
            new Color(1, 0, 0, 1), // rojo
			new Color(0, 1, 0, 1), // verde
			new Color(0, 0, 1, 1), // azul
			new Color(1, 0, 0.75F, 1), // rosa
			new Color(0.004F, 1, 0.996F, 1), // celeste
			new Color(1, 0.86F, 0.4F, 1), // amarillo 
			new Color(1, 0.39F, 0.004F, 1), // verde oscuro
			new Color(1, 0.39F, 0.004F, 1), // bordo  
			 

			
		 };
        return colors[cluster - 1];
    }

    void LateUpdate()
    {
        if (_config.ParticleSystem)
        {
            InitializeIfNeeded();

            _particles = new ParticleSystem.Particle[_points.Count];
            int aliveParticles = _particleSystem.GetParticles(_particles);
            int index = 0;
            for (int i = 0; i < _clusters.Count(); i++)
            {
                foreach (DataVec p in _clusters[MAX_ITERATIONS-1][i].Points)
                {
                    _particles[index].position = new Vector3((float)p.Components[0], (float)p.Components[1], (float)p.Components[2]);
                    _particles[index].velocity = Vector3.zero;
                    _particles[index].startColor = GetColorByCluster(i + 1);
                    index++;
                }
            }

            _particleSystem.SetParticles(_particles, aliveParticles);
        }
    }

    void InitializeIfNeeded()
    {
        if (_particleSystem == null)
            _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ParticleSystem.MainModule configModule = _particleSystem.main;
        configModule.duration = 1F;
        configModule.prewarm = true;
        configModule.startLifetime = 1F;
        configModule.maxParticles = _points.Count;
        configModule.startSize = 0.5F;
        configModule.startSpeed = 0F;

        ParticleSystem.EmissionModule emissionModule = _particleSystem.emission;
        emissionModule.rateOverTime = _points.Count;

        ParticleSystemRenderer rendererModule = GetComponent<ParticleSystemRenderer>();
        rendererModule.renderMode = ParticleSystemRenderMode.Mesh;
        rendererModule.SetMeshes(new Mesh[] { Resources.GetBuiltinResource<Mesh>("Sphere.fbx") });
        rendererModule.material = _pointMaterial;
        _particleSystem.Play();

        if (_particles == null || _particles.Length < _points.Count)
            _particles = new ParticleSystem.Particle[_points.Count];
    }

}
