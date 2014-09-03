using NUnit.Framework;
using TinyIoC;

namespace Church.UnitTests.Church.Common
{
    
    public class TinyIocTests
    {
        [TestFixture]
        public class AutoRegisterTests : TinyIocTests
        {

            private interface IFoo
            {
                string FooProperty { get; set; }
            }

            private class Foo : IFoo
            {
                public string FooProperty { get; set; }
            }


            [Test]
            public void CanResolveAfterAutoRegistration()
            {
                //arrange
                var container = new TinyIoCContainer();

                //act
                container.AutoRegister<Foo>();

                //Assert
                var foo = container.Resolve<IFoo>();
                Assert.IsNotNull(foo, "expected instance of Foo");
                Assert.IsTrue(foo is Foo, "expected foo variable to be of type Foo");
            }
            
        }
    }
}
