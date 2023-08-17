namespace TestFunction
{
    public class TestNotFoundException : Exception
    {
        public TestNotFoundException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
