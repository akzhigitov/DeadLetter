namespace Common
{
    public class IntegrationMessage
    {
        public IntegrationMessage(int number)
        {
            Number = number;
        }

        public int Number { get; private set; }
    }
}
