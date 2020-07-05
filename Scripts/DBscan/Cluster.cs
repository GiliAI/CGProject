using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Resources.Scripts.DBscan
{
    public class Cluster
    {
        private int clusterId;
        private Dictionary<int, int> destPros;
        private int number;
        private int gazeNum;
        private List<Record> _points;
        public Cluster(int id)
        {
            clusterId = id;
            number = 0;
            destPros = new Dictionary<int, int>();
            gazeNum++;
            _points = new List<Record>();
        }
        public Vector3 center
        {
            get
            {
                if (_points.Count == 0)
                {
                    return new Vector3(0, 0, 0);
                }
                double posX=0,posY=0,posZ=0;
                foreach (Record record in _points)
                {
                    posX += record.posX;
                    posY += record.posY;
                    posZ += record.posZ;
                }
                posX /= _points.Count;
                posY /= _points.Count;
                posZ /= _points.Count;
                Vector3 _center = new Vector3((float)posX, (float)posY, (float)posZ);
                return _center;
            }
        }
        public  List<Record> points
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
            }
        }
        public int gaze
        {
            get
            {
                return gazeNum;
            }
            set
            {
                gazeNum=value;
            }
        }
        public float gazePro
        {
            get
            {
                float _gaze = gazeNum;
                return _gaze/points.Count;
            }
        }
        public int id
        {
            get
            {
                return clusterId;
            }
        }
        public void Add(int clusterId)
        {
            if(destPros.Keys.Contains(clusterId))
            {
                destPros[clusterId]++;
            }
            else
            {
                destPros.Add(clusterId, 1);
            }
            number++;
        }
        public double getPro(int clusterId)
        {
            if (destPros.Keys.Contains(clusterId))
            {
                return (double)destPros[clusterId]/number;
            }
            else
            {
                return 0;
            }
        }
        public int getMaxProId()
        {
            int maxPro = 0;int maxProId=0;
            foreach(int cluterId in destPros.Keys)
            {
                if(destPros[cluterId]>maxPro)
                {
                    maxPro = destPros[cluterId];
                    maxProId = cluterId;
                }
            }
            return maxProId;
        }
        public Dictionary<int,double> getAllPro()
        {
            Dictionary<int, double> result = new Dictionary<int, double>();
            foreach (KeyValuePair<int, int> destPro in destPros)
            {
                result.Add(destPro.Key, destPro.Value / number);
            }
            return result;
        }
        public double getSilCoe(Cluster _cluster)
        {
            double avgA = 0;
            foreach(Record a in points)
            {
                double disA = 0;
                disA += (a.posX - points[0].posX) * (a.posX - points[0].posX)
                    + (a.posY - points[0].posY) * (a.posY - points[0].posY)
                    + (a.posZ - points[0].posZ) * (a.posZ - points[0].posZ);
                disA = Math.Sqrt(disA);
                avgA += disA;
            }
            avgA /= points.Count;
            double avgB = 0;
            foreach(Record b in _cluster.points)
            {
                double disB = 0;
                disB += (b.posX - points[0].posX) * (b.posX - points[0].posX)
                    + (b.posY - points[0].posY) * (b.posY - points[0].posY)
                    + (b.posZ - points[0].posZ) * (b.posZ - points[0].posZ);
                disB = Math.Sqrt(disB);
                avgB += disB;
            }
            avgB /= _cluster.points.Count;
            if (avgB == 0)
                return 0;
            double maxAB = avgA > avgB ? avgA : avgB;
            return (avgB - avgA) / maxAB;
        }
    }
}
