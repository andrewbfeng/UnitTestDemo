using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Business business = new Business();
            Thread thread = new Thread(business.runningBusiness);
            Console.WriteLine("Starting Business");
            thread.Start();
            Console.ReadKey();
        }
    }
    public class Business
    {
        int quota = 100;
        public Status businessStatus = Status.Open;
        Factory factory;
        iStore seller;
#if debug
        public Business(Factory factory, iStore seller){
this.factory = factory;
this.seller = seller;
}
        public int Quota { get => quota; set => quota = value; }
        public Business() {
            factory = new Factory();
            seller = new ShoppingMall();
        }
#else
        public Business()
        {
            factory = new Factory();
            seller = new ShoppingMall();
        }
#endif
        public void runningBusiness()
        {
            businessStatus = Status.init;
            while (true)
            {
                if (seller.checkInventory() > 500)
                {
                    Console.WriteLine("Selling Goods: {0}", seller.checkInventory());
                    Debug.WriteLine("[Debug]Selling Goods: {0}", seller.checkInventory());
                    seller.sell();
                    businessStatus = Status.Closed;
                    Console.WriteLine("Goods sold out");
                    Debug.WriteLine("[Debug]Goods sold out");
                    return;
                }
                else
                {
                    if (factory.goods - quota <= 0)
                    {
                        Console.WriteLine("Produced goods: {0}", quota - factory.goods);
                        Debug.WriteLine("[Debug]Produced goods: {0}", quota - factory.goods);
                        factory.produce(quota - factory.goods);
                    }
                    seller.fetchProduct(factory.deliverGoods());
                }
            }
        }
        public enum Status
        {
            init = 0,
            Open = 1,
            Closed = 2,
        }
    }


    public class Factory
    {
        public int goods;
        public void produce(int produceQuota)
        {
            goods += produceQuota;
        }
        public int deliverGoods()
        {
            int deliveredGoods = goods;
            goods = 0;
            return deliveredGoods;
        }
    }

    class ShoppingMall : iStore
    {
        public int goods = 0;

        public int checkInventory()
        {
            return goods;
        }

        public void fetchProduct(int productedGood)
        {
            goods += productedGood;
        }
        public void sell() { goods = 0; }
    }


    public interface iStore
    {
        int checkInventory();
        void fetchProduct(int productedQuota);
        void sell();
    }
}
