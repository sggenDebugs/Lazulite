using Lavelle.Linalg32;

namespace Lavelle.Stats32
{
    public partial class StatsContext
    {
        protected RemoteScalar PooledStddev(RemoteVector data1, RemoteVector data2)
        {
            using var stddev1 = Stddev(data1, sample: true);
            using var squaredStddev1 = LContext.GetScalar(true);
            LContext.PowScalar(stddev1, 2, r: squaredStddev1);

            using var dfStddev1Prod = LContext.GetScalar(true);
            LContext.MultiplyScalar(squaredStddev1, data1.Length - 1, r: dfStddev1Prod);

            using var stddev2 = Stddev(data2, sample: true);
            using var squaredStddev2 = LContext.GetScalar(true);
            LContext.PowScalar(stddev2, 2, r: squaredStddev2);

            using var dfStddev2Prod = LContext.GetScalar(true);
            LContext.MultiplyScalar(squaredStddev2, data2.Length - 1, r: dfStddev2Prod);

            using var pooledVarianceNumer = LContext.GetScalar(true);
            LContext.Add(dfStddev1Prod, dfStddev2Prod, r: pooledVarianceNumer);
            int lengthSum = data1.Length + data2.Length - 2;

            using var pooledVariance = LContext.GetScalar(true);
            LContext.DivideScalar(pooledVarianceNumer, lengthSum, r: pooledVariance);
            LContext.Synchronize();

            var result = LContext.GetScalar(true);
            LContext.Sqrt(pooledVariance, r: result);
            LContext.Synchronize();
            return result;
        }
        // one sample t-test, two sample t-test
        public RemoteScalar OneSampleTTest(RemoteVector data, float mu)
        {
            if (data.Length < 2)
            {
                throw new ArgumentException("Sample size must be at least 2.");
            }
            float sqrtN = MathF.Sqrt(data.Length);

            using var stddev = Stddev(data, sample: true);
            if(stddev.Get() <= 0)
            {
                throw new ArgumentException("Standard deviation must not be zero.");
            }
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
            if ((data1.Length < 2) || (data2.Length < 2))
            {
                throw new ArgumentException("Sample size must be at least 2.");
            }
            int n1 = data1.Length;
            int n2 = data2.Length;
            float sqrtRecipSampleSize = (float) MathF.Sqrt((1f / n1) + (1f / n2));

            using var pooledStddev = PooledStddev(data1, data2);
            using var mean1 = Mean(data1);
            using var mean2 = Mean(data2);
            
            using var meanDiff = LContext.GetScalar(true);
            LContext.Subtract(mean1, mean2, r: meanDiff);
            using var denom = LContext.GetScalar(true);
            LContext.MultiplyScalar(pooledStddev, sqrtRecipSampleSize, r: denom);
            LContext.Synchronize();

            var result = LContext.GetScalar(true);
            LContext.Divide(meanDiff, denom, r: result);

            LContext.Synchronize();
            return result;
        }
        // welch's t-test, ANOVA, chi-squared
        public RemoteScalar WelchTTest(RemoteVector data1, RemoteVector data2, float mu1, float mu2)
        {
            if ((data1.Length < 2)||(data2.Length < 2))
            {
                throw new ArgumentException("Sample size must be at least 2.");
            }

            using var mean1 = Mean(data1);
            using var mean2 = Mean(data2);

            using var meanDiff = LContext.GetScalar(true);
            LContext.Subtract(mean1, mean2, r: meanDiff);
            
            using var numer = LContext.GetScalar(true);
            LContext.SubtractScalar(meanDiff, mu1 - mu2, r: numer);

            using var variance1 = Variance(data1, true);
            using var variance2 = Variance(data2, true);

            using var dividedVarianceBySize1 = LContext.GetScalar(true);
            LContext.DivideScalar(variance1, data1.Length, r: dividedVarianceBySize1);

            using var dividedVarianceBySize2 = LContext.GetScalar(true);
            LContext.DivideScalar(variance2, data2.Length, r: dividedVarianceBySize2);

            using var sumVarianceBySize = LContext.GetScalar(true);
            LContext.Add(dividedVarianceBySize1, dividedVarianceBySize2, r: sumVarianceBySize);

            using var denom = LContext.GetScalar(true);
            LContext.Sqrt(sumVarianceBySize, r: denom);
            LContext.Synchronize();

            var result = LContext.GetScalar(true);
            LContext.Divide(numer, denom, r: result);

            LContext.Synchronize();
            return result;
        }
    }
}