using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Plot : MonoBehaviour
{
  
    // Travel and Rotation Movement
    [SerializeField] private KeyCode _moveUp = KeyCode.W;
    [SerializeField] private KeyCode _moveDown = KeyCode.S;
    [SerializeField] private KeyCode _moveLeft = KeyCode.A;
    [SerializeField] private KeyCode _moveRight = KeyCode.D;
    
    [SerializeField] private KeyCode _rotateUp = KeyCode.UpArrow;
    [SerializeField] private KeyCode _rotateDown = KeyCode.DownArrow;
    [SerializeField] private KeyCode _rotateLeft = KeyCode.LeftArrow;
    [SerializeField] private KeyCode _rotateRight = KeyCode.RightArrow;
    
    [SerializeField] private KeyCode _zoomIn = KeyCode.Plus;
    [SerializeField] private KeyCode _zoomOut = KeyCode.Minus;
    [SerializeField] private KeyCode _reset = KeyCode.R;

    [SerializeField] public string FilePath => _configFilePath;
    [SerializeField] private string _configFilePath;

    [SerializeField] public GameObject PointPrefab => _pointPrefab;
    [SerializeField] private GameObject _pointPrefab;

    private Color GetColorByCluster(int cluster) {
        Color[] colors = new Color[] { Color.red, Color.blue, Color.green };
        return colors[cluster - 1];
    }

    
    private Vector3 _horizontalForward;
    private Vector3 _forward;
    private MovementController _movementController;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Vector3 _intialScale;
	private double _maxPosition;

    void Start()
    {
        _movementController = GetComponent<MovementController>(); 
        _initialPosition = transform.localPosition;
        _initialRotation = transform.rotation;
        _intialScale = transform.localScale;
		_maxPosition = transform.localPosition.y;

        InitPlot();
    }

    private void InitPlot() {
        JsonConfig config = JsonUtility.FromJson<JsonConfig>(File.ReadAllText(_configFilePath));

        Debug.Log(config.K);
        Debug.Log(config.CsvPath);

        List<ClusterPoint> points = File.ReadAllLines(config.CsvPath)
                                .Skip(1)
                                .Select(v => ClusterPoint.FromCsv(v))
                                .ToList();

        Debug.Log(points);

        foreach (ClusterPoint p in points) {
            GameObject dataPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            dataPoint.transform.parent = transform;
            dataPoint.transform.localPosition = new Vector3(p.X, p.Y, p.Z);
            dataPoint.transform.localRotation = Quaternion.identity;
            dataPoint.GetComponent<Renderer>().material.color = GetColorByCluster(p.Cluster);
        }
    }

    void Update()
    {
        /* Translate */ 
        if (Input.GetKey(_moveUp) && transform.position.y < _maxPosition*2) _movementController.Travel(new Vector3(0, 1, 0));
        if (Input.GetKey(_moveDown) && transform.position.y > _maxPosition) _movementController.Travel(new Vector3(0, -1, 0));
        if (Input.GetKey(_moveLeft) && transform.position.x > -_maxPosition*2) _movementController.Travel(new Vector3(-1, 0, 0));
        if (Input.GetKey(_moveRight) && transform.position.x < _maxPosition*2) _movementController.Travel(new Vector3(1, 0, 0));
         
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
}
