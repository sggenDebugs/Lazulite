using Lavelle.Lazulite;
using Lavelle.Stats32;
using Lavelle.Linalg32;

var lctx = new LazuliteContext().EnableLinalg32();
var sctx = new StatsContext(lctx);

/* One-sample T-test */
var vec1 = new RemoteVector[]
{
    lctx.GetVector(3).Set([1, 2, 3]).AsVector(),
    lctx.GetVector(3).Set([4, 5, 6]).AsVector(),
    lctx.GetVector(3).Set([7, 7, 7]).AsVector(),
    lctx.GetVector(1).Set([8]).AsVector(),
    lctx.GetVector(3).Set([-10, -20, -30]).AsVector()
};

float mu1 = 2;
float mu2 = -20;
// mu is inside vector
float one_sample_t_test_vec0 = sctx.OneSampleTTest(vec1[0], mu1).Get();
Console.WriteLine($"One-sample t-test for vec1[0]: {one_sample_t_test_vec0}");

// mu is outside vector
float one_sample_t_test_vec1 = sctx.OneSampleTTest(vec1[1], mu1).Get();
Console.WriteLine($"One-sample t-test for vec1[1]: {one_sample_t_test_vec1}");

// constant vector (variance = 0) (throws InvalidCastException when enabled)
// float one_sample_t_test_vec2 = sctx.OneSampleTTest(vec1[2], mu1).Get();
// Console.WriteLine($"One-sample t-test for vec1[2]: {one_sample_t_test_vec2}");

// sample size is 1 (throws InvalidCastException when enabled)
// float one_sample_t_test_vec3 = sctx.OneSampleTTest(vec1[3], mu1).Get();
// Console.WriteLine($"One-sample t-test for vec1[3]: {one_sample_t_test_vec3}");

// input values are negative
float one_sample_t_test_vec4 = sctx.OneSampleTTest(vec1[4], mu2).Get();
Console.WriteLine($"One-sample t-test for vec1[4]: {one_sample_t_test_vec4}");

/* Two-sample T-test */
var vec2 = new RemoteVector[]
{
    lctx.GetVector(3).Set([10, 11, 12]).AsVector(),
    lctx.GetVector(3).Set([20, 30, 50]).AsVector(),
    lctx.GetVector(3).Set([67, 67, 67]).AsVector(),
    lctx.GetVector(1).Set([88]).AsVector(),
    lctx.GetVector(3).Set([-100, -200, -300]).AsVector()
};

// two vectors with different variance
float two_sample_t_test_vec0 = sctx.TwoSampleTTest(vec2[0], vec2[1]).Get();
Console.WriteLine($"One-sample t-test for vec2[0] and vec2[1]: {two_sample_t_test_vec0}");

// one vector has zero variance
float two_sample_t_test_vec1 = sctx.TwoSampleTTest(vec2[0], vec2[2]).Get();
Console.WriteLine($"One-sample t-test for vec2[0] and vec2[2]: {two_sample_t_test_vec1}");

// one vector has one sample size (throws InvalidCastException when enabled)
// float two_sample_t_test_vec2 = sctx.TwoSampleTTest(vec2[1], vec2[3]).Get();
// Console.WriteLine($"One-sample t-test for vec2[1] and vec2[3]: {two_sample_t_test_vec2}");

// one vector has negative values
float two_sample_t_test_vec3 = sctx.TwoSampleTTest(vec2[0], vec2[4]).Get();
Console.WriteLine($"One-sample t-test for vec2[0] and vec2[4]: {two_sample_t_test_vec3}");
float two_sample_t_test_vec4 = sctx.TwoSampleTTest(vec2[1], vec2[4]).Get();
Console.WriteLine($"One-sample t-test for vec2[1] and vec2[4]: {two_sample_t_test_vec4}");