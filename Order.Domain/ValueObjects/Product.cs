
using BuildBlocks.Domain.Abstractions;

namespace Order.Domain.ValueObjects;

public class Product : ValueObject
{
    public string Name { get; private set; }
    public string SkuCode { get; private set; }
    public decimal Price { get; private set; }

    public Product(string name, string skuCode,decimal price)
    {
        Name = name;
        SkuCode = skuCode;
        Price = price;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return SkuCode;
        yield return Price;
    }
}
