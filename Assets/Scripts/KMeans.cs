using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine; 
using System;
using KMeans;

namespace ClusteringKMeans
{
  class KMeansAlgorithm  
  {
    public static (Cluster[][], int) Run(List<ClusterPoint> pointsInput, int kClusters)
    {
      List<DataVec> points = MigrateData(pointsInput);
      
      KMeansClustering cl = new KMeansClustering(points.ToArray(), kClusters); // Reference to data points and k (number of clusters)
      
      (Cluster[][] clusters, int iterations) =  cl.Compute();  // Perform clasification and return results
	 
      return (clusters, iterations);

    } 
    static List<DataVec> MigrateData(List<ClusterPoint> points)
    {
      List<DataVec> pts = new List<DataVec>();
      foreach (ClusterPoint p in points) {
        pts.Add(new DataVec(new double[] { p.X, p.Y, p.Z }));
      }
      
      return pts;
    }
  }  
} 