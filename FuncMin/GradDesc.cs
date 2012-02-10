using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncMin
{
    class GradDesc: IFuncMin
    {
        public int MaxIter = 1000;
        public double Epsilon = 1e-6;

        public double[] FuncMin(double[] x0, Func func, FuncGrad funcGrad, FuncHess funcHess)
        {
            double[] grad = funcGrad(x0);
            double e = Math.Sqrt(FuncMinUtil.Multiply(grad, grad)) / (1 + Math.Abs(func(x0)));
            int step = 0;
            while (e > Epsilon && step < MaxIter)
            {
                step++;
                double[] p = FuncMinUtil.Multiply(x0, -1);
                double alpha = 1;
                while (!FuncMinUtil.SuffDesc(x0, p, alpha, func, grad))
                {
                    alpha = alpha / 2;
                    if (alpha < Epsilon)
                    {
                        break;
                    }
                }
                double y_0 = func(x0);
                // updated x0
                x0 = FuncMinUtil.Add(x0, FuncMinUtil.Multiply(p, alpha));
                double y_1 = func(x0);
                if (y_0 < y_1)
                {
                    break;
                }
                grad = funcGrad(x0);
                e = Math.Sqrt(FuncMinUtil.Multiply(grad, grad)) / (1 + Math.Abs(y_1));
            }
            return x0;
        }
    }
}
