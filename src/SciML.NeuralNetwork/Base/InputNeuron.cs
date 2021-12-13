using System.Collections.Generic;

namespace SciML.NeuralNetwork.Base
{
    public class InputNeuron : INeuron<InputNeuron>
    {
        public InputNeuron()
        {
            Outputs = new List<Synapse>();
        }

        public Synapse Input { get; set; }

        public List<Synapse> Outputs { get; set; }

        public virtual object Clone() =>
            new InputNeuron();

        public virtual void Process() =>
            Outputs.ForEach(s => s.Signal = Input.Signal * s.Weight);
    }
}
