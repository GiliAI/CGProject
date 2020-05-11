using System;
using Resources.Scripts.DBscan;
using Resources.Scripts.Prediction;
using UnityEngine;

namespace DbscanImplementation
{
    public class MyCustomDatasetItem : DatasetItemBase
    {
        public float X;
        public float Y;
        public float Z;

        //？？循环调用构造函数
        /*
        public MyCustomDatasetItem(float x, float y, float z)
        {
            var exchangeAxis = new ExchangeAxis();
            MyCustomDatasetItem modelIndex = exchangeAxis.UnityPos_to_modelIndex(new Vector3(x,y,z));
            X = modelIndex.X;
            Y = modelIndex.Y;
            Z = modelIndex.Z;
            Debug.Log(X+" , "+Y+" , "+Z);
        }
        */
        public MyCustomDatasetItem(float x, float y, float z)
        {
            X = (float) Math.Ceiling((x + 12151.7) / 217.3);
            Y = (float) Math.Ceiling((z + 298.019) / 217.3);
            Z = (float) Math.Ceiling((y + 12574.8) / 217.3);
            Debug.Log(X+" , "+Y+" , "+Z);
        }
        
    }
}