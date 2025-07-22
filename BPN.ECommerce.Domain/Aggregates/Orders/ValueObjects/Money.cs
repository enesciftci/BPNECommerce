
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using BPN.ECommerce.Domain.Base;

namespace BPN.ECommerce.Domain.Aggregates.Orders.ValueObjects;


public sealed class Money 
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [ExcludeFromCodeCoverage]
    [JsonConstructor]
    private Money()
    {
    }

    private Money(decimal amount)
    {
        if (decimal.IsNegative(amount))
        {
            throw new Exception("Negative amount.");
        }

        Amount = amount;
        Currency = "TRY";
    }
    
    private Money(int amount)
    {
        if (int.IsNegative(amount))
        {
            throw new Exception("Negative amount.");
        }

        AmountInt = amount;
        Currency = "TRY";
    }

    public decimal Amount { get; init; }
    public int AmountInt { get; set; }
    public string Currency { get; init; }

    public static Money Create(int amount)
    {
        return new Money(ConvertToDecimalAmount(amount));
    }
    
    public static Money Create(long amount)
    {
        return new Money(ConvertToDecimalAmount(amount));
    }
    
    public static Money Create(decimal amount)
    {
        return new Money(ConvertToIntAmount(amount));
    }
    
    private static int ConvertToIntAmount(decimal amount)
    {
        return (int)Math.Round(amount * 100, MidpointRounding.AwayFromZero);
    }

    private static decimal ConvertToDecimalAmount(int amount)
    {
        return (decimal)amount / 100;
    }
    
    private static decimal ConvertToDecimalAmount(long amount)
    {
        return (decimal)amount / 100;
    }
}