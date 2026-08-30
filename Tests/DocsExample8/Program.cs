using Lavelle.Lazulite;
using Lavelle.Stats32;
using Lavelle.Linalg32;

var lctx = new LazuliteContext().EnableLinalg32();
var sctx = new StatsContext(lctx);

var vec1 = new RemoteVector[]
{
    lctx.GetVector(3).Set([1, 2, 3]).AsVector(),
    lctx.GetVector(3).Set([4, 5, 6]).AsVector()
};

float mu = 2;

float one_sample_t_test = sctx.OneSampleTTest(vec1[1], mu).Get();
Console.WriteLine($"One-sample t-test for vec1[1]: {one_sample_t_test}");

