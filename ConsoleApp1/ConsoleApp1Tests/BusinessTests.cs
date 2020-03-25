using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp1.Tests
{
    [TestClass()]
    public class BusinessTests
    {
        [TestMethod()]
        public void UnitTest01_storeGoodsGreaterThan500WillTriggerSellAction()
        {
            //Arrange
            CallChecker isCalledSell = new CallChecker();
            CallChecker isCalledProduce = new CallChecker();
            iStoreManagement sellerMock = new SellerMock(isCalledSell,501);
            iFactoryManagement factoryMock = new FactoryMock(isCalledProduce);
            Business testObject = new Business(factoryMock, sellerMock);

            //Act
            testObject.runningBusiness();

            //Assert
            Assert.IsTrue(isCalledSell.isCalled);
        }

        [TestMethod()]
        public void UnitTest02_storeGoodsLessThan500WillProduceSixTimes()
        {
            //Arrange
            CallChecker isCalledFetchProduct = new CallChecker();
            CallChecker isCalledProduce = new CallChecker();
            iStoreManagement sellerMock = new SellerMock(isCalledFetchProduct);
            iFactoryManagement factoryMock = new FactoryMock(isCalledProduce);
            Business testObject = new Business(factoryMock, sellerMock);

            //Act
            testObject.runningBusiness();
            
            //Assert
            Assert.IsTrue(isCalledFetchProduct.isCalled);
            Assert.AreEqual(6, ((FactoryMock)factoryMock).produceTime);
        }

        public class SellerMock : iStoreManagement
        {
            CallChecker isCalled;
            public SellerMock(CallChecker isCalled, int inventory = 0)
            {
                this.isCalled = isCalled;
                this.inventory = inventory;
            }

            public int inventory = 0;

            public int checkInventory()
            {
                isCalled.isCalled = true;
                return inventory;
            }

            public void stockIn(int productedQuota)
            {
                inventory += productedQuota;
                isCalled.isCalled = true;
            }
            public void sell() { isCalled.isCalled = true; inventory = 0; }
        }

        public class FactoryMock : iFactoryManagement
        {
            public int inventory;
            public int produceTime = 0;
            CallChecker callChecker;
            public FactoryMock(CallChecker callChecker, int inventory = 0)
            {
                this.callChecker = callChecker;
                this.inventory = inventory;
            }
            public void produce(int produceQuota)
            {
                produceTime++;
                inventory += produceQuota;
                callChecker.isCalled = true;
            }
            public int deliverGoods()
            {
                int deliveredGoods = inventory;
                inventory = 0;
                callChecker.isCalled = true;
                return deliveredGoods;
            }

            public int checkInventory()
            {
                callChecker.isCalled = true;
                return inventory;
            }
        }

        public class CallChecker
        {
            public bool isCalled = false;
        }
    }
}