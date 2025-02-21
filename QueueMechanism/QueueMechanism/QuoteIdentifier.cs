namespace QueueMechanism;

public class QuoteIdentifier
{
    public QuoteIdentifier()
    {
    }

    public QuoteIdentifier(int opportunityId, string? productCode, DateTime endDate, int priceVersion)
    {
        OpportunityId = opportunityId;
        ProductCode = productCode;
        EndDate = endDate;
        PriceVersion = priceVersion;
    }

    public int OpportunityId { get; set; }
    public string? ProductCode { get; set; }
    public DateTime EndDate { get; set; }
    public int PriceVersion { get; set; }

    public override int GetHashCode()
    {
        int productHasCode = string.IsNullOrEmpty(ProductCode) ? 0 : ProductCode.GetHashCode();
        return (OpportunityId.GetHashCode() + productHasCode + EndDate.GetHashCode() + PriceVersion.GetHashCode());
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || GetType() != obj.GetType())
            return false;

        QuoteIdentifier quoteIdentifier = (QuoteIdentifier)obj;

        int productHasCode = string.IsNullOrEmpty(ProductCode) ? 0 : ProductCode.GetHashCode();
        int productObjHasCode = string.IsNullOrEmpty(quoteIdentifier.ProductCode) ? 0 : quoteIdentifier.ProductCode.GetHashCode();

        bool result = (OpportunityId.GetHashCode() +
                        productHasCode +
                        EndDate.GetHashCode() +
                        PriceVersion.GetHashCode()) == (quoteIdentifier.OpportunityId.GetHashCode() +
                                                        productObjHasCode +
                                                        quoteIdentifier.EndDate.GetHashCode() +
                                                        quoteIdentifier.PriceVersion.GetHashCode());

        return result;
    }

    public static bool operator ==(QuoteIdentifier obj1, QuoteIdentifier obj2)
    {
        return (obj1.Equals(obj2));
    }

    public static bool operator !=(QuoteIdentifier obj1, QuoteIdentifier obj2)
    {
        return !(obj1.Equals(obj2));
    }
}
