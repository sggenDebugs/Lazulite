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

Console.WriteLine("Vec1 [0]: " + string.Join(", ", vec1[0].Get()));
Console.WriteLine("Vec1 [1]: " + string.Join(", ", vec1[1].Get()));