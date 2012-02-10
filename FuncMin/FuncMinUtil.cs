using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncMin
{
    class FuncMinUtil
    {
        public static double mu = 0.2;
        
        public static bool SuffDesc(double[] x0, double[] p,
            double alpha, Func func, double[] grad)
        {
            double[] x1 = Add(x0, Multiply(p, alpha));
            double y_x1 = func(x1);
            double y_x0 = func(x0);
            if (y_x1 > y_x0) return false;            
            double est = mu * alpha * Multiply(p, grad);
            if (y_x1 < y_x0 + est) return true;
            return false;
        }

        public static double[] Add(double[] x, double[] y)
        {
            if (x.Length != y.Length)
            {
                throw new Exception("dimension does not agree");
            }
            double[] r = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                r[i] = x[i] + y[i];
            }
            return r;
        }

        public static double[] Multiply(double[] x, double ratio)
        {
            double[] r = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                r[i] = x[i] * ratio;
            }
            return r;
        }

        public static double Multiply(double[] x, double[] y)
        {
            if (x.Length != y.Length)
            {
                throw new Exception("dimension does not agree");
            }
            double r = 0;
            for (int i = 0; i < x.Length; i++)
            {
                r += x[i] * y[i];
            }
            return r;
        }        
    }
}
