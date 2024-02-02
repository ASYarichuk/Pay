using System;
using System.Text;
using System.Security.Cryptography;

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

public interface IHashSystem
{
    string Hash(Order order);
}

class MD5Hesh : IHashSystem
{
    public string Hash(Order order)
    {
        string output;

        MD5 MD5Hash = MD5.Create();

        byte[] inputBytes = Encoding.ASCII.GetBytes(order.Id + order.Amount);

        byte[] hash = MD5Hash.ComputeHash(inputBytes);

        output = Convert.ToString(hash);

        return output;
    }
}

class SHA1Hesh : IHashSystem
{
    public string Hash(Order order)
    {
        string output;

        SHA1 SHA1Hash = SHA1.Create();

        byte[] inputBytes = Encoding.ASCII.GetBytes(order.Id + order.Amount);

        byte[] hash = SHA1Hash.ComputeHash(inputBytes);

        output = Convert.ToString(hash);

        return output;
    }
}

class PaySite : IPaymentSystem, MD5Hesh
{
    private string _linkSite = "pay.system1.ru/order?amount=12000RUB&hash=";

    public string GetPayingLink(Order order)
    {
        return $"{_linkSite}{Hash(order)}{order.Id}";
    }
}

class OrderSite : IPaymentSystem, MD5Hesh
{
    private string _linkSite = "order.system2.ru/pay?hash=";

    public string GetPayingLink(Order order)
    {
        return $"{_linkSite}{Hash(order)}{order.Id.GetHashCode()}/{order.Amount}";
    }
}

class SystemSite : IPaymentSystem, SHA1Hesh
{
    private string _linkSite = "system3.com/pay?amount=12000&curency=RUB&hash=";
    private string _secretKey = "SecretKey";

    public string GetPayingLink(Order order)
    {
        return $"{_linkSite}{Hash(order)}{order.Id.GetHashCode()}/{order.Id}/{_secretKey}";
    }
}