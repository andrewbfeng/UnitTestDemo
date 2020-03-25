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
        int factoryQuota = 100;
        Status businessStatus = Status.Open;
        iFactoryManagement factory;
        iStoreManagement store;
#if debug
        public Business(iFactoryManagement factory, iStoreManagement seller, int factoryQuota = 100)
        {
            this.factory = factory;
            this.store = seller;
            this.factoryQuota = factoryQuota;
        }

        public Business() {
            factory = new Factory();
            store = new ShoppingMall();
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
            businessStatus = Status.Open;
            while (true)
            {
                if (store.checkInventory() > 500)
                {
                    Console.WriteLine("Selling Goods: {0}", store.checkInventory());
                    Debug.WriteLine("[Debug]Selling Goods: {0}", store.checkInventory());
                    store.sell();
                    businessStatus = Status.Closed;
                    Console.WriteLine("Goods sold out");
                    Debug.WriteLine("[Debug]Goods sold out");
                    return;
                }
                else
                {
                    if (factory.checkInventory() - factoryQuota <= 0)
                    {
                        Console.WriteLine("Produced goods: {0}", factoryQuota - factory.checkInventory());
                        Debug.WriteLine("[Debug]Produced goods: {0}", factoryQuota - factory.checkInventory());
                        factory.produce(factoryQuota - factory.checkInventory());
                    }
                    store.stockIn(factory.deliverGoods());
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


    public class Factory : iFactoryManagement
    {
        int inventory;
        public void produce(int produceQuota)
        {
            inventory += produceQuota;
        }
        public int deliverGoods()
        {
            int deliveredGoods = inventory;
            inventory = 0;
            return deliveredGoods;
        }

        public int checkInventory()
        {
            return inventory;
        }
    }

    class ShoppingMall : iStoreManagement
    {
        int goods = 0;

        public int checkInventory()
        {
            return goods;
        }

        public void stockIn(int productedGood)
        {
            goods += productedGood;
        }
        public void sell() { goods = 0; }
    }


    public interface iStoreManagement
    {
        int checkInventory();
        void stockIn(int stocks);
        void sell();
    }

    public interface iFactoryManagement
    {
        void produce(int quota);
        int checkInventory();
        int deliverGoods();
    }
}
