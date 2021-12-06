using System.Collections.Generic;

namespace SciAI.NeuralNetwork.Base
{
    public class OutputNeuron : INeuron<OutputNeuron>
    {
        public OutputNeuron()
        {
            Inputs = new List<Synapse>();
        }

        public List<Synapse> Inputs { get; set; }

        public Synapse Output { get; set; }

        public virtual object Clone() =>
            new OutputNeuron();

        public virtual void Process()
        {
            Output.Signal = 0;

            Inputs.ForEach(s => Output.Signal += s.Signal);
        }
    }
}
