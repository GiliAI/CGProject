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
    public List<Record> GetPredictedCluster(Vector3 pos, Vector3 rot, HashSet<Record[]> clusters)
    {
        var exchangeAxis = new ExchangeAxis();
        Record[] predicted_cluster = null;
        double maxValue=-100;
        Debug.Log("现在朝向"+rot);
        Coeffecient getCoefficient = new Coeffecient();
        foreach (var cluster in clusters)
        {
            //数据块的距离(暂时按照cluster0计算)
            Vector3 clusterCenterPos = exchangeAxis.ModelIndex_to_unityPos(cluster[0].posX,cluster[0].posY,cluster[0].posZ);
            double distance = GetDistance(pos,clusterCenterPos);
            //偏角
            double avertence = GetAvertence(pos,rot, clusterCenterPos);
            //兴趣度系数（需改进）
            double coefficient = getCoefficient.GetCoefficeent(cluster);
            double value = coefficient+distance+avertence;

            Debug.Log("=====兴趣度系数是"+value+"=====");
            //计算每个cluster的interest_value（需改进）加入数量？
            //只选取 “一个” 最优的cluster（需改进）
            if (value > maxValue)
            {
                maxValue = value;
                predicted_cluster = cluster;
            }
        }
        
        List<Record> Unitypos_predicted_cluster = new List<Record>();
        List<Record> oho = predicted_cluster.ToList();
        if (predicted_cluster!=null)
        {
            foreach (var box in predicted_cluster)
            {
                Vector3 pos1 = exchangeAxis.ModelIndex_to_unityPos(box.posX, box.posY,
                    box.posZ);
                Vector3 rot1 = new Vector3(box.rotX, box.rotY, box.rotZ);
                Unitypos_predicted_cluster.Add(new Record(pos1,rot1));
                
            }
        }
        
        return Unitypos_predicted_cluster;
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
    
}
