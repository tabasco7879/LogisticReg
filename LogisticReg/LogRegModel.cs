using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogReg
{
    class LogRegModel
    {
        double[] theta;
        int yLabel;
        double alpha, lambda;        

        public LogRegModel(int dim, int y, double alpha, double lambda)
        {
            this.alpha = alpha;
            this.lambda = lambda;
            theta = new double[dim + 1];
            yLabel = y;
        }

        public void InitTheta()
        {
            Random rand = new Random();
            for (int i = 0; i < theta.Length; i++)
            {
                theta[i] = rand.NextDouble();
            }
        }

        public void UpdateTheta(List<DataItem> db)
        {
            
        }        
        
        public double CostFunc(List<DataItem> db, double[] theta)
        {
            double total = 0;
            for (int i = 0; i < db.Count; i++)
            {
                double func = Func(db[i], theta);
                if (db[i].Contains(yLabel))
                {
                    total += Math.Log(func);
                }
                else
                {
                    total += Math.Log(1 - func);
                }                
            }
            total = -total / db.Count;
            return total;
        }

        public double Func(DataItem item, double[] theta)
        {
            double sum = 0;
            for (int j = 0; j < theta.Length; j++)
            {
                if (j == theta.Length - 1)
                {
                    sum += theta[j];
                }
                else
                {
                    sum += item.GetItemWeight(j) * theta[j];
                }
            }
            double func = 1.0/(1.0 + Math.Exp(-sum));
            return func;
        }

        public double[] CostFunc_gradient(List<DataItem> db, double[] theta)
        {
            double[] gradient = new double[theta.Length];
            double[] diff = new double[db.Count];
            for (int i = 0; i < db.Count; i++)
            {
                double y = 0;
                if (db[i].Contains(yLabel)) y = 1;
                diff[i] = Func(db[i], theta) - y;
            }

            for (int i = 0; i < theta.Length; i++)
            {
                double diff_sum = 0;
                if (i == theta.Length - 1)
                {
                    diff_sum += diff[i];
                }
                else
                {
                    for (int j = 0; j < db.Count; j++)
                    {
                        diff_sum += diff[i] * db[j].GetItemWeight(i);
                    }
                }

                if (i == theta.Length - 1)
                {
                    gradient[i] = diff_sum / db.Count;
                }
                else
                {
                    gradient[i] = diff_sum / db.Count + lambda * theta[i] / db.Count;
                }
            }
            return gradient;
        }
    }
}
