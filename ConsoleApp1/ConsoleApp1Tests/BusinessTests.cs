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
            Factory factoryFake = new Factory();
            iStore sellerMock = new SellerMock();
            Business testObject = new Business(factoryFake, sellerMock);

            //Act
            testObject.runningBusiness();
            

            //Assert
            Assert.IsTrue(((SellerMock)sellerMock).isCalled);
            Assert.AreEqual(2, (int)testObject.businessStatus);

        }

        public class SellerMock : iStore
        {
            public int goods = 0;
            public bool isCalled = false;

            public int checkInventory()
            {
                return 501;
            }

            public void fetchProduct(int productedQuota)
            {
                goods += productedQuota;
            }
            public void sell() { isCalled = true; goods = 0; }
        }
    }
}