namespace SangoUtils_Server.Test
{
    public class Test1
    {
        private Action<int>? OnSquare1;

        private void Square(int num)
        {
            Console.WriteLine(num * num);
        }

        public void AddTestListener()
        {
            OnSquare1 = Square;
        }

        public void InvokeTest()
        {
            OnSquare1?.Invoke(42);
        }
    }

    public class Test2
    {
        private Func<int,int>? OnSquare2;

        private int Square(int num)
        {
            return num * num;
        }

        public void AddTestListener()
        {
            OnSquare2 = Square;
        }

        public void InvokeTest()
        {
            int? res = OnSquare2?.Invoke(42);
            Console.WriteLine(res);
        }
    }
}
