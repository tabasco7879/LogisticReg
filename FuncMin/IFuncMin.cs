using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncMin
{
    public delegate double Func(double[] x);
    public delegate double[] FuncGrad(double[] x);
    public delegate double[,] FuncHess(double[] x);

    public interface IFuncMin
    {
        double[] FuncMin(double[] x0, Func func, FuncGrad funcGrad, FuncHess funcHess); 
    }

    public class FuncMinFactory
    {
        public static IFuncMin GetFuncMin(string method)
        {
            if (method == "GradDesc")
            {
                return new GradDesc();
            }
            return null;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IFuncMin funcMin = FuncMinFactory.GetFuncMin("GradDesc");
            double[] x0={100};
            double[] x1 = funcMin.FuncMin(x0, test1, test1Grad, null);            
        }

        static double test1(double[] x0)
        {
            return x0[0] * x0[0];
        }

        static double[] test1Grad(double[] x0)
        {
            double[] r = { 2 * x0[0] };
            return r;
        }
    }
}
