using System;
using System.Collections.Generic;
using System.Text;
using Lavelle.Linalg32;
using Microsoft.VisualBasic;

namespace Lavelle.Stats32
{
    public partial class StatsContext
    {
        protected RemoteScalar PooledStddev(RemoteVector data1, RemoteVector data2)
        {
            using var dfStddev1Prod = LContext.MultiplyScalar(
                LContext.PowScalar(Stddev(data1), 2), 
                data1.Length - 1
                );

            using var dfStddev2Prod = LContext.MultiplyScalar(
                LContext.PowScalar(Stddev(data2), 2),
                data2.Length - 1
                );

            using var pooledStddevNumer = LContext.Add(dfStddev1Prod, dfStddev2Prod);
            int lengthSum = data1.Length + data2.Length - 2;

            using var result = LContext.GetScalar(true);
            LContext.DivideScalar(pooledStddevNumer, lengthSum, r: result);

            LContext.Synchronize();
            return result;
        }
        // one sample t-test, two sample t-test
        public RemoteScalar OneSampleTTest(RemoteVector data, float mu)
        {
            float sqrtN = MathF.Sqrt(data.Length);

            using var stddev = Stddev(data, sample: true);
            using var mean = Mean(data);

            using var denom = LContext.GetScalar(true);
            LContext.DivideScalar(stddev, sqrtN, r: denom);
            using var numer = LContext.GetScalar(true);
            LContext.SubtractScalar(mean, mu, r: numer);

            var result = LContext.GetScalar(true);
            LContext.Divide(numer, denom, r: result);

            LContext.Synchronize();
            return result;
        }

        public RemoteScalar TwoSampleTTest(RemoteVector data1, RemoteVector data2)
        {
            using var pooledStddev = PooledStddev(data1, data2);
            using var meanDiff = LContext.Subtract(Mean(data1), Mean(data2));
            using var reciprocalSampleSum = LContext.Add(
                LContext.PowScalar(data1, -1), 
                LContext.PowScalar(data2, -1)
                ).AsScalar();
            using var sqrtReciprocalSampleSum = LContext.Sqrt(
                LContext.Add(
                    LContext.PowScalar(data1, -1), 
                    LContext.PowScalar(data2, -1)
                    )
                ).AsScalar();

            var result = LContext.GetScalar(true);

            LContext.Divide(meanDiff, LContext.Multiply(sqrtReciprocalSampleSum, pooledStddev), r: result).AsScalar();

            LContext.Synchronize();
            return result;
        }
        // welch's t-test, ANOVA, chi-squared
    }
}