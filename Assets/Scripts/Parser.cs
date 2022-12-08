using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Parser : MonoBehaviour
{ 
    void Start()
    {
       	using(var reader = new StreamReader("Assets/Resources/test.csv"))
    	{
        	List<List<string>> points = new List<List<string>>();
        	 
        	while (!reader.EndOfStream)
        	{
				List<string> point = new List<string>();
           		var line = reader.ReadLine();
           		var values = line.Split(',');
				
            	point.Add(values[0]);
            	point.Add(values[1]);
				point.Add(values[2]);

				points.Add(point);
				 
        	}
			Debug.Log(points);
			
    	}
    }
 
    void Update()
    {
        
    }
}
