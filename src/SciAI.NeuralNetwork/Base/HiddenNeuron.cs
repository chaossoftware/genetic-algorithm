using SciAI.NeuralNetwork.Activation;
using System.Collections.Generic;
using System.Linq;

namespace SciAI.NeuralNetwork.Base
{
    public class HiddenNeuron : INeuron<HiddenNeuron>
    {
        private ActivationFunction activationFunction;
        private ActivationFunctionType activationFunctionType;

        public HiddenNeuron(ActivationFunctionType activationFunctionType)
        {
            Inputs = new List<Synapse>();
            Outputs = new List<Synapse>();
            this.activationFunctionType = activationFunctionType;
            activationFunction = ActivationFunction.Get(activationFunctionType);
        }

        public List<Synapse> Inputs { get; set; }

        public List<Synapse> Outputs { get; set; }

        public virtual object Clone() =>
            new HiddenNeuron(activationFunctionType);

        public virtual void Process()
        {
            double arg = 0;

            arg = Inputs.Sum(s => s.Signal);

            Outputs.ForEach(s => s.Signal = s.Weight * activationFunction.Phi(arg));
        }
    }
}
