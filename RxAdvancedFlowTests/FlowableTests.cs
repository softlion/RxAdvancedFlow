using RxAdvancedFlow.subscribers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RxAdvancedFlow.Tests
{
    [TestClass()]
    public class FlowableTests
    {
        [TestMethod()]
        public void ZipTest_SameLength()
        {
            TestSubscriber<int> ts = new TestSubscriber<int>();
            
            Flowable.Zip(Flowable.Just(1), Flowable.Just(2), (a, b) => a + b).Subscribe(ts);

            ts.AssertValue(3)
                .AssertComplete()
                .AssertNoError();
        }

        [TestMethod()]
        public void ZipTest_FirstEmpty()
        {
            TestSubscriber<int> ts = new TestSubscriber<int>();

            Flowable.Zip(Flowable.Empty<int>(), Flowable.Just(2), (a, b) => a + b).Subscribe(ts);

            ts.AssertNoValues()
                .AssertComplete()
                .AssertNoError();
        }

        [TestMethod()]
        public void ZipTest_DifferentLength()
        {
            TestSubscriber<int> ts = new TestSubscriber<int>();

            Flowable.Zip(Flowable.Range(1, 3), Flowable.Range(10, 2), (a, b) => a + b).Subscribe(ts);

            ts.AssertValues(11, 13)
                .AssertComplete()
                .AssertNoError();
        }

    }
}