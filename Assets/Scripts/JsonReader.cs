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
        public decimal X;
        public decimal Y;
        public decimal Z;

        public int Cluster;

        public static ClusterPoint FromCsv(string csvLine) {
            string[] values = csvLine.Split(',');
            ClusterPoint point = new ClusterPoint();
            point.X = Convert.ToDecimal(values[0]);
            point.Y = Convert.ToDecimal(values[1]);
            point.Z = Convert.ToDecimal(values[2]);
            point.Cluster = Convert.ToInt32(values[3]);
            return point;
        }

        public override string ToString() {
            return String.Format("X: {0}, Y: {1}, Z:{2}, Cluster: {3}", X, Y, Z, Cluster);
        }
    }

    [SerializeField] public string FilePath => _configFilePath;
    [SerializeField] private string _configFilePath;


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

        foreach (ClusterPoint point in points) {
            Debug.Log(point);
        }
    }
 
    void Update()
    {
        
    }
}


