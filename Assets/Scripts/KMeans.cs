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
    public static Cluster[] Run(List<ClusterPoint> pointsInput, int kClusters)
    {
      Debug.Log("Begin k-means clustering");
       
      List<DataVec> points = MigrateData(pointsInput);

      // First argument of the constructor is a reference to data points. Second argument is k (number of clusters)
      KMeansClustering cl = new KMeansClustering(points.ToArray(), kClusters);

      // Perform clasification and return results
      Cluster[] clusters =  cl.Compute(); 

      return clusters;

    } 
    static List<DataVec> MigrateData(List<ClusterPoint> points)
    {
      List<DataVec> pts = new List<DataVec>();
      foreach (ClusterPoint p in points) {
        pts.Add(new DataVec(new double[] { p.X, p.Y, p.Z }));
      }
      
      return pts;
    }
  } // Program
} // ns