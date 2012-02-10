using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuncMin;

namespace LogisticReg
{
    class LogisticReg
    {
        double[] theta;
        int yLabel;
        double lambda;

        public LogisticReg(int dim, int y, double lambda)
        {
            this.lambda = lambda;
            theta = new double[dim + 1];
            yLabel = y;
        }

        public void RandInit()
        {
            Random rand = new Random();
            for (int i = 0; i < theta.Length; i++)
            {
                theta[i] = rand.NextDouble();
            }
        }

        public void ZeroInit()
        {            
            for (int i = 0; i < theta.Length; i++)
            {
                theta[i] = 0;
            }
        }

        public void Training(List<DataItem> db)
        {
            IFuncMin funcMin = FuncMinFactory.GetFuncMin("GradDesc");
            theta = funcMin.FuncMin(theta,
                (x0) => { return CostFunc(db, x0); },
                (x0) => { return CostFuncGrad(db, x0); },
                null);
        }

        public void Testing(List<DataItem> db, out double precision, out double recall)
        {
            int TP = 0, FP = 0, TN = 0, FN = 0;
            for (int i = 0; i < db.Count; i++)
            {
                double r = Func(db[i], theta);
                if (r > 0.5)
                {
                    if (db[i].Contains(yLabel))
                    {
                        TP++;
                    }
                    else
                    {
                        FP++;
                    }
                }
                else
                {
                    if (db[i].Contains(yLabel))
                    {
                        FN++;
                    }
                    else
                    {
                        TN++;
                    }
                }
            }
            precision = (double)(TP)/(TP+FN);
            recall = (double)(TP)/(TP+FP);
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
            double regulator = 0;
            for (int i = 0; i < theta.Length - 1; i++)
            {
                regulator += theta[i] * theta[i];
            }
            regulator = lambda * regulator / (2 * db.Count);
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
            double func = 1.0 / (1.0 + Math.Exp(-sum));
            return func;
        }

        public double[] CostFuncGrad(List<DataItem> db, double[] theta)
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
                    for (int j = 0; j < db.Count; j++)
                        diff_sum += diff[j];
                }
                else
                {
                    for (int j = 0; j < db.Count; j++)
                        diff_sum += diff[j] * db[j].GetItemWeight(i);
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
