using SciML.NeuralNetwork.Base;
using System;
using System.Collections.Generic;

namespace SciML.NeuralNetwork.Evolution
{
    public class TresholdNeuron : INeuron<TresholdNeuron>
    {
        private List<double> parameters;

        public TresholdNeuron(ThresholdFunction function, List<double> parameters)
        {
            Inputs = new List<Synapse>();
            Outputs = new List<Synapse>();
            SetFunctionAndParams(function, parameters);
        }

        public TresholdNeuron()
        {
            Inputs = new List<Synapse>();
            Outputs = new List<Synapse>();
        }

        public List<Synapse> Inputs { get; set; }

        public List<Synapse> Outputs { get; set; }

        public ThresholdFunction TresholdFunction { get; protected set; }

        public void SetFunctionAndParams(ThresholdFunction function, List<double> parameters)
        {
            if (parameters.Count != function.DefaultParameters.Count)
            {
                throw new ArgumentException(
                    $"Function needs {function.DefaultParameters.Count} parameters. But params count is {parameters.Count}");
            }

            TresholdFunction = function;
            this.parameters = parameters;
        }

        public List<double> GetTresholdParameters()
        {
            List<double> ret = new List<double>(parameters.Count);

            foreach (double d in parameters)
            {
                ret.Add(d);
            }

            return ret;
        }

        public virtual void Process()
        {
            double arg = 0;

            foreach (Synapse synapse in Inputs)
            {
                arg += synapse.Signal;
            }

            arg = TresholdFunction.Calculate(arg, parameters);
            foreach (Synapse synapse in Outputs)
            {
                synapse.Signal = synapse.Weight * arg;
            }
        }

        public virtual object Clone()
        {
            List<double> cloneParams = new List<double>(parameters.Count);

            foreach (double d in parameters)
            {
                cloneParams.Add(d);
            }

            TresholdNeuron clone = new TresholdNeuron(TresholdFunction, cloneParams);
            return clone;
        }
    }
}
