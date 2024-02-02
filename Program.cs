using System;

class Program
{
    static void Main(string[] args)
    {
        Random random = new Random();
        int minValue = 0;
        int maxValue = 100;
        int id = random.Next(minValue, maxValue);
        int amount = random.Next(minValue, maxValue);

        Order order = new Order(id, amount);

        IPaymentSystem paySite = new PaySite();
        Console.WriteLine(paySite.GetPayingLink(order));

        IPaymentSystem orderSite = new OrderSite();
        Console.WriteLine(orderSite.GetPayingLink(order));

        IPaymentSystem systemSite = new SystemSite();
        Console.WriteLine(systemSite.GetPayingLink(order));
    }
}

public class Order
{
    public readonly int Id;
    public readonly int Amount;

    public Order(int id, int amount) => (Id, Amount) = (id, amount);
}

public interface IPaymentSystem
{
    string GetPayingLink(Order order);
}

class PaySite : IPaymentSystem
{
    private string _linkSite = "pay.system1.ru/order?amount=12000RUB&hash=";

    public string GetPayingLink(Order order)
    {
        return $"{_linkSite}/MD5/{order.Id}";
    }
}

class OrderSite : IPaymentSystem
{
    private string _linkSite = "order.system2.ru/pay?hash=";

    public string GetPayingLink(Order order)
    {
        return $"{_linkSite}/MD5/{order.Id.GetHashCode()}/{order.Amount}";
    }
}

class SystemSite : IPaymentSystem
{
    private string _linkSite = "system3.com/pay?amount=12000&curency=RUB&hash=";
    private string _secretKey = "SecretKey";

    public string GetPayingLink(Order order)
    {
        return $"{_linkSite}/SHA-1/{order.Id.GetHashCode()}/{order.Id}/{_secretKey}";
    }
}