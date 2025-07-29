namespace MyFw.DS
{
    public interface IPercent
    {
        int Value { get; }
        float Per { get; }
    }

    public class Percent : IPercent
    {
        public int Value { get; private set; }
        public float Per { get; private set; }

        public Percent(int value)
        {
            this.Value = value;
            this.Per = this.Value / 100f;
        }
    }
}