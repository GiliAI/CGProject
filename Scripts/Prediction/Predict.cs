using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DbscanImplementation;
using Resources.Scripts.DBscan;
using Resources.Scripts.Prediction;
using UnityEngine;

public class Predict
{
    public Cluster GetPredictedCluster(Vector3 pos, Vector3 rot,List<Cluster> clusterNet, List<Record> gazePoints)
    {
        var exchangeAxis = new ExchangeAxis();
        //Record[] predicted_cluster = null;
        Cluster predicted_cluster = null;
        double maxValue=-100;
        Debug.Log("现在朝向"+rot);
        Coeffecient getCoefficient = new Coeffecient();
        int count = 0;
        //List<Record[]> clusters= origin_clusters.ToList();
        Dictionary<int, double> _distanceDic = distanceDic(clusterNet, pos);
        var minPair = _distanceDic.Keys.Select(x => new { x, y = _distanceDic[x] }).OrderBy(x => x.y).First();
        var minId = minPair.x;
        var minDis = _distanceDic[minId];
        _distanceDic.Remove(minId);
        var secPair = _distanceDic.Keys.Select(x => new { x, y = _distanceDic[x] }).OrderBy(x => x.y).First();
        var secId = secPair.x;
        var secDis = _distanceDic[secId];
        if (Math.Abs(minDis-secDis)<10)
        {
            if(Math.Abs(GetAvertence(pos,rot,clusterNet[minId].center))<0.25)
            {
                predicted_cluster = clusterNet[minId];
            }
            else
            {
                predicted_cluster = clusterNet[secId];
            }
        }
        else
        {
            foreach (var _cluster in clusterNet)
            {
                if(_cluster.id==minId)
                {
                    continue;
                }
                double value = _cluster.gazePro + clusterNet[minId].getPro(_cluster.id)+_cluster.getSilCoe(clusterNet[minId]);
                if (value > maxValue)
                {
                    maxValue = value;
                    predicted_cluster = _cluster;
                }
            }
        }
        //foreach (var _cluster in clusterNet)
        //{
        //    //var cluster = _cluster.points;
        //    //数据块的距离(暂时按照cluster0计算)
        //    //Vector3 clusterCenterPos = exchangeAxis.ModelIndex_to_unityPos((float)cluster[0].posX, (float)cluster[0].posY, (float)cluster[0].posZ);
        //    Vector3 centerPos = _cluster.center;
        //    //Vector3 clusterCenterPos = exchangeAxis.ModelIndex_to_unityPos((float)centerPos.x, (float)centerPos.y, (float)centerPos.z);
        //    //double distance = GetDistance(pos,clusterCenterPos);
        //    double distance = GetDistance(pos, centerPos);
        //    //偏角
        //    //double avertence = GetAvertence(pos,rot, clusterCenterPos);
        //    double avertence = GetAvertence(pos, rot, centerPos);
        //    //兴趣度系数（需改进）
        //    //double coefficient = getCoefficient.GetCoefficeent(cluster);
        //    double coefficient = 0;
        //    double value = coefficient+distance+avertence + clusterNet[count].gaze;
        //    if(distance<10)
        //    {
        //        //predicted_cluster = clusters[clusterNet[count].getMaxProId()];
        //        predicted_cluster= clusterNet[clusterNet[count].getMaxProId()];
        //        break; 
        //    }
        //    //double value = clusterNet[count].gaze;

        //    count++;

        //    Debug.Log("=====兴趣度系数是"+value+"=====");
        //    //计算每个cluster的interest_value（需改进）加入数量？
        //    //只选取 “一个” 最优的cluster（需改进）
        //    if (value > maxValue)
        //    {
        //        maxValue = value;
        //        predicted_cluster = _cluster;
        //    }
        //}
        return predicted_cluster;
        //List<Record> Unitypos_predicted_cluster = new List<Record>();
        //List<Record> oho = predicted_cluster.ToList();
        //if (predicted_cluster!=null)
        //{
        //    foreach (var box in predicted_cluster)
        //    {
        //        Vector3 pos1 = exchangeAxis.ModelIndex_to_unityPos((float)box.posX, (float)box.posY,(float)box.posZ);
        //        Vector3 rot1 = new Vector3((float)box.rotX, (float)box.rotY, (float)box.rotZ);
        //        Unitypos_predicted_cluster.Add(new Record(pos1,rot1));

        //    }
        //}

        //return Unitypos_predicted_cluster;
    }

    private double GetAvertence(Vector3 pos, Vector3 rot, Vector3 clusterCenterPos)
    {
        //获取两点构成线段的方向
        var targetRotation = new Quaternion();
        Vector3 Toward = clusterCenterPos -pos;
        targetRotation.eulerAngles = Toward;
        //获取方向和摄像机朝向的夹角
        var angle = Quaternion.Angle(targetRotation, Quaternion.Euler(rot));
        //Debug.Log("目标方向"+targetRotation.eulerAngles+"   夹角是："+angle);
        //Debug.Log(Application.streamingAssetsPath + "======" + Launcher.instance.GetSceneName);
        //夹角和系数的转换关系(需改进)
        double avertence = angle / 360;
        return avertence;
    }
    
    private double GetDistance(Vector3 currentPos, Vector3 clusterCenterPos)
    {
        double distance = (clusterCenterPos.x - currentPos.x) * (clusterCenterPos.x - currentPos.x) +
                          (clusterCenterPos.y - currentPos.y) * (clusterCenterPos.y - currentPos.y) +
                          (clusterCenterPos.z - currentPos.z) * (clusterCenterPos.z - currentPos.z);
        return distance;
    }

    //public int GetNetId(List<Cluster> clusterNet,Vector3 point)
    //{
    //    int id = 0;
    //    double distance = 1000;
    //    foreach(Cluster _cluster in clusterNet)
    //    {
    //        var _distance = GetDistance(_cluster.center, point);
    //        if (distance> _distance)
    //        {
    //            id = _cluster.id;
    //            distance = _distance;
    //        }
    //    }
    //    return id;
    //}

    private Dictionary<int,double> distanceDic(List<Cluster> clusterNet, Vector3 pos)
    {
        Dictionary<int, double> _distanceDic = new Dictionary<int, double>();
        foreach(Cluster _cluster in clusterNet)
        {
            if(!_distanceDic.ContainsKey(_cluster.id))
            {
                _distanceDic.Add(_cluster.id, GetDistance(_cluster.center, pos));
            }
        }
        return _distanceDic;
    }
}
