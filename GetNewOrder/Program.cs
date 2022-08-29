namespace GetNewOrder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logic.GetNewOrders.GetOrders();
            System.Threading.Thread.Sleep(60000);
        }
    }
}
