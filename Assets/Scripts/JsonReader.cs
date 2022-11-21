using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

public class JsonReader : MonoBehaviour
{ 
    private class JsonConfig {
        public int K;
        public string CsvPath;

    }

    private class ClusterPoint {
        public float X;
        public float Y;
        public float Z;

        public int Cluster;

        public static ClusterPoint FromCsv(string csvLine) {
            string[] values = csvLine.Split(',');
            ClusterPoint point = new ClusterPoint();
            point.X = (float) Convert.ToDouble(values[0]);
            point.Y = (float) Convert.ToDouble(values[1]);
            point.Z = (float) Convert.ToDouble(values[2]);
            point.Cluster = Convert.ToInt32(values[3]);
            return point;
        }

        public override string ToString() {
            return String.Format("X: {0}, Y: {1}, Z:{2}, Cluster: {3}", X, Y, Z, Cluster);
        }
    }

    [SerializeField] public string FilePath => _configFilePath;
    [SerializeField] private string _configFilePath;

    [SerializeField] public GameObject PointPrefab => _pointPrefab;
    [SerializeField] private GameObject _pointPrefab;

    private Color GetColorByCluster(int cluster) {
        Color[] colors = new Color[] { Color.red, Color.blue, Color.green };
        return colors[cluster - 1];
    }

    void Start()
    {

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
        
    }
}


