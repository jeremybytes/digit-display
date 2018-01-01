using System;
using System.Collections.Generic;
using ObservationLoader;

namespace CSharp
{
    public class BasicClassifier : IClassifier
    {
        private IEnumerable<Observation> data;
        private readonly IDistance distance;

        public BasicClassifier(IDistance distance)
        {
            this.distance = distance;
        }

        public void Train(IEnumerable<Observation> trainingSet)
        {
            data = trainingSet;
        }

        public string Predict(int[] pixels)
        {
            Observation currentBest = null;
            var shortest = Double.MaxValue;

            foreach(Observation obs in data)
            {
                var dist = distance.Between(obs.Pixels, pixels);
                if(dist < shortest)
                {
                    shortest = dist;
                    currentBest = obs;
                }
            }

            return currentBest.Label;
        }
    }
}
