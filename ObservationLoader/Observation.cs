namespace ObservationLoader
{
    public class Observation
    {
        public string Label { get; }
        public int[] Pixels { get; }

        public Observation(string label, int[] pixels)
        {
            Label = label;
            Pixels = pixels;
        }
    }
}
