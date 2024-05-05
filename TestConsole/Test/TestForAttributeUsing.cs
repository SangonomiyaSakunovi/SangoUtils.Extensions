namespace TestConsole.Test
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    internal class TestInterfaceAttribute : Attribute
    {
        public int Value { get; set; }

        public TestInterfaceAttribute(int value = 0)
        {
            this.Value = value;
        }
    }

    [TestInterface(1)]
    internal interface IMainInterface
    {
        public void TestMethod(int value)
        {
            Console.WriteLine("这是父接口定义的方法" + value);
        }

        public void TestMethod();
        
    }

    [TestInterface(2)]
    internal interface ITest1Interface : IMainInterface
    {
        public new void TestMethod(int value)
        {
            Console.WriteLine("这是子接口重写的方法" + value);
        }
    }

    [TestInterface(3)]
    internal interface ITest2Interface : IMainInterface
    {
        public void TestAttributeMethod(int value)
        {
            Type type = typeof(ITest2Interface);
            TestInterfaceAttribute attribute = (TestInterfaceAttribute)type.GetCustomAttributes(typeof(TestInterfaceAttribute), true)[0];
            attribute.Value = value;
        }
    }

    internal class MyClass1 : ITest1Interface
    {
        public void TestMethod()
        {
            throw new NotImplementedException();
        }
    }

    internal class MyClass2 : ITest2Interface
    {
        public void TestMethod()
        {
            Console.WriteLine("TestMethod2");
        }

        public void TestMethod(int value)
        {
            throw new NotImplementedException();
        }

        public void TestAttributeChangedIfDone()
        {
            Type interfaceType = this.GetType().GetInterface(nameof(ITest2Interface));
            TestInterfaceAttribute attribute = (TestInterfaceAttribute)interfaceType.GetCustomAttributes(typeof(TestInterfaceAttribute), true)[0];
            Console.WriteLine("获取的attribute值为" + attribute.Value);
        }
    }

    internal class MyClass3 : ITest2Interface
    {
        public void TestMethod()
        {
            throw new NotImplementedException();
        }

        public void TestAttributeChangedIfDone()
        {
            Type interfaceType = this.GetType().GetInterface(nameof(ITest2Interface));
            TestInterfaceAttribute attribute = (TestInterfaceAttribute)interfaceType.GetCustomAttributes(typeof(TestInterfaceAttribute), true)[0];
            Console.WriteLine("获取的attribute值为"+attribute.Value);
        }
    }

    internal static class TestForAttributeUsing
    {
        public static void Test()
        {
            Console.WriteLine("==================================");
            Type type = typeof(MyClass1);
            if (type.GetInterface(nameof(IMainInterface)) != null)
            {
                Type interfaceType = type.GetInterface(nameof(IMainInterface));
                if (interfaceType.GetCustomAttributes(typeof(TestInterfaceAttribute), true).Length > 0)
                {
                    TestInterfaceAttribute attribute = (TestInterfaceAttribute)interfaceType.GetCustomAttributes(typeof(TestInterfaceAttribute), true)[0];
                    int value = attribute.Value;
                    if (value == 1)
                    {
                        Console.WriteLine("找到1");
                        var instance = Activator.CreateInstance(type) as ITest1Interface;
                        instance.TestMethod(0);

                        Console.WriteLine(attribute.Value);
                    }
                    else
                    {
                        Console.WriteLine("不能找到找到标签1");
                    }
                }
                else
                {
                    Console.WriteLine("不能找到标签");
                }
            }
            else
            {
                Console.WriteLine("不能找到接口");
            }
            Console.WriteLine("==================================");
        }

        public static void TestNew()
        {
            MyClass2 myClass2 = new MyClass2();
            MyClass3 myClass3 = new MyClass3();

            ITest2Interface test2Interface = myClass2;
            ITest2Interface test3Interface = myClass3;

            test2Interface.TestAttributeMethod(0);
            myClass2.TestAttributeChangedIfDone();
            myClass3.TestAttributeChangedIfDone();
        }
    }

    
}
