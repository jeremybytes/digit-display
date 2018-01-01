using System.Collections.Generic;
using ObservationLoader;

namespace CSharp
{
    public interface IClassifier
    {
        void Train(IEnumerable<Observation> trainingSet);
        string Predict(int[] pixels);
    }
}
