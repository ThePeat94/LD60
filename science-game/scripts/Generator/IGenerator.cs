using Nidavellir.IdleNumbers;

namespace Nidavellir.Generator;

public interface IGenerator
{
    public void Generate(IdleNumber power);
}