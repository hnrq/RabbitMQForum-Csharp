namespace MessagingApi.Models
{
    public class Counter
    {
        private int _value = 0;

        public int Value { get => _value; }

        public void Increment()
        {
            _value++;
        }
    }
}