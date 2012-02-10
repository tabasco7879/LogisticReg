using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogisticReg
{
    class DataItem
    {
        Dictionary<int, double> itemWeight;       
        HashSet<int> classLabels;

        public DataItem()
        {
            itemWeight = new Dictionary<int,double>();           
            classLabels = new HashSet<int>();
        }

        public void AddClassLabel(int cl)
        {
            classLabels.Add(cl);
        }

        public bool Contains(int cl)
        {
            return classLabels.Contains(cl);
        }

        public void AddItem(int item, double weight)
        {
            double w = 0;
            if (!itemWeight.TryGetValue(item, out w))
            {
                itemWeight.Add(item, weight);
            }
            else
            {
                itemWeight[item] = w + weight;
            }
        }

        public void Normalize()
        {
            int[] keys = itemWeight.Keys.ToArray();
            double total = 0;
            for (int i = 0; i < keys.Length; i++)
            {
                total += itemWeight[keys[i]];
            }
            for (int i = 0; i < keys.Length; i++)
            {
                itemWeight[keys[i]] = itemWeight[keys[i]] / total;
            }
        }

        public string DataItemId
        {
            get;
            set;
        }

        public double GetItemWeight(int item)
        {
            double w = 0;
            itemWeight.TryGetValue(item, out w);
            return w;            
        }
    }
}
