using System;

internal class ClusterPoint {
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