public record OrderByRequest(
    bool byAmount = false,
    bool byDate = false,
    bool byAmountDesc = false,
    bool byDateDesc = false)
{
    public bool byAmount { get; } = byAmount;
    public bool byDate { get; } = byDate;
    public bool byAmountDesc { get; } = byAmountDesc;
    public bool byDateDesc { get; } = byDateDesc;
    private readonly bool[] All = { byAmount, byDate, byAmountDesc, byDateDesc }; 
    private static bool IsTrue(OrderByRequest request)
    {
        if (request.All.All(x => x == false))
            return false;
        return true;
    }
    public string GetOrderByStatement()
    {
        if (!IsTrue(this))
            return "";
        string statement = @" order by ";
        if ((byAmountDesc || byAmount) && (byDate || byDateDesc))
            statement += (byAmountDesc ? "amount desc, " : byAmount ? "amount, " : "") +
                (byDateDesc ? "date desc" : byDate ? "date" : "");
        else
            statement += byAmountDesc ? "amount desc" : byAmount ? "amount" : byDateDesc ? "date desc" : byDate ? "date" : "";
        return statement;
    }
}