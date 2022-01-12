using SciML.NeuralNetwork.Entities;
using SciML.NeuralNetwork.Evolution.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SciML.NeuralNetwork.Evolution
{
    /// <summary>
    /// Contains functionality for <see cref="EvolvingNetBase"/> serialization and deserialization using XML.
    /// </summary>
    public static class EvolvingNetLoader
    {
        /// <summary>
        /// Serializes neural network to xml file.
        /// </summary>
        /// <typeparam name="T">neural net type</typeparam>
        /// <param name="neuralNet">neural net instance</param>
        /// <param name="fileName">file name to serialize to</param>
        public static void ToFile<T>(T neuralNet, string fileName) where T : EvolvingNetBase
        {
            XElement root = new XElement(typeof(T).Name);

            XElement metadataElement = new XElement("Metadata");
            metadataElement.Value = neuralNet.Metadata;

            XElement inputLayerElement = new XElement("InputLayer");

            foreach (EvolvingNeuron neuron in neuralNet.InputLayer.Neurons)
            {
                inputLayerElement.Add(StoreNeuron(neuron));
            }

            XElement hiddenLayerElement = new XElement("HiddenLayer");

            foreach (EvolvingNeuron neuron in neuralNet.HiddenLayer.Neurons)
            {
                hiddenLayerElement.Add(StoreNeuron(neuron));
            }

            XElement outputLayerElement = new XElement("OutputLayer");

            foreach (EvolvingNeuron neuron in neuralNet.OutputLayer.Neurons)
            {
                outputLayerElement.Add(StoreNeuron(neuron));
            }

            XElement connections0Element = new XElement("Connections0");
            neuralNet.Connections[0].ForEach(s => connections0Element.Add(StoreSynapse(s)));

            XElement connections1Element = new XElement("Connections1");
            neuralNet.Connections[1].ForEach(s => connections1Element.Add(StoreSynapse(s)));

            root.Add(metadataElement);
            root.Add(inputLayerElement);
            root.Add(hiddenLayerElement);
            root.Add(outputLayerElement);
            root.Add(connections0Element);
            root.Add(connections1Element);

            new XDocument(root).Save(fileName);
        }

        /// <summary>
        /// Deserializes neural network from xml file.
        /// </summary>
        /// <typeparam name="T">neural net type</typeparam>
        /// <param name="fileName">file name to deserialize from</param>
        /// <returns>instance of neural net</returns>
        public static T FromFile<T>(string fileName) where T : EvolvingNetBase
        {
            T neuralNet = Activator.CreateInstance<T>();
            XElement root = XDocument.Load(fileName).Element(typeof(T).Name);

            if (root == null)
            {
                throw new ArgumentException($"You are trying to load brain not of type {typeof(T).Name}");
            }

            XElement metadataElement = root.Element("Metadata");
            neuralNet.Metadata = metadataElement?.Value;

            XElement[] inputLayerElements = root.Element("InputLayer").Elements().ToArray();

            for (int i = 0; i < inputLayerElements.Count(); i++)
            {
                EvolvingNeuron neuron = LoadNeuron(inputLayerElements[i]);
                neuron.Inputs.Add(new Synapse(i, i, 1));
                neuralNet.InputLayer.Neurons[i] = neuron;
            }

            XElement[] hiddenLayerElements = root.Element("HiddenLayer").Elements().ToArray();

            for (int i = 0; i < hiddenLayerElements.Count(); i++)
            {
                neuralNet.HiddenLayer.Neurons[i] = LoadNeuron(hiddenLayerElements[i]);
            }

            XElement[] outputLayerElements = root.Element("OutputLayer").Elements().ToArray();

            for (int i = 0; i < outputLayerElements.Count(); i++)
            {
                EvolvingNeuron neuron = LoadNeuron(outputLayerElements[i]);
                neuron.Outputs.Add(new Synapse(i, i, 1));
                neuralNet.OutputLayer.Neurons[i] = neuron;
            }

            root.Element("Connections0").Elements().ToList()
                .ForEach(s => neuralNet.Connections[0].Add(LoadSynapse(s)));

            root.Element("Connections1").Elements().ToList()
                .ForEach(s => neuralNet.Connections[1].Add(LoadSynapse(s)));

            foreach (Synapse synapse in neuralNet.Connections[0])
            {
                neuralNet.InputLayer.Neurons[synapse.InIndex].Outputs.Add(synapse);
                neuralNet.HiddenLayer.Neurons[synapse.OutIndex].Inputs.Add(synapse);
            }

            foreach (Synapse synapse in neuralNet.Connections[1])
            {
                neuralNet.HiddenLayer.Neurons[synapse.InIndex].Outputs.Add(synapse);
                neuralNet.OutputLayer.Neurons[synapse.OutIndex].Inputs.Add(synapse);
            }

            return neuralNet;
        }

        private static XElement StoreSynapse(Synapse synapse)
        {
            return new XElement("Synapse",
                new XAttribute("weight", synapse.Weight),
                new XAttribute("sourceIndex", synapse.InIndex),
                new XAttribute("destinationIndex", synapse.OutIndex));
        }

        private static Synapse LoadSynapse(XElement synapseXElement)
        {
            int indexSource = Convert.ToInt32(synapseXElement.Attribute("sourceIndex").Value);
            int indexDestination = Convert.ToInt32(synapseXElement.Attribute("destinationIndex").Value);
            double weight = Convert.ToDouble(synapseXElement.Attribute("weight").Value);

            return new Synapse(indexSource, indexDestination, weight);
        }

        private static XElement StoreNeuron(EvolvingNeuron neuron)
        {
            return new XElement("TresholdNeuron",
                new XAttribute("function", neuron.ActivationFunction.GetType().Name),
                new XAttribute("params", string.Join(" ", neuron.ActivationFunction.Parameters)));
        }

        private static EvolvingNeuron LoadNeuron(XElement brainXml)
        {
            EvolvingActivationFunctionBase function = EvolvingActivationFunctionBase.GetFunction(brainXml.Attribute("function").Value);
            string[] parameters = brainXml.Attribute("params").Value.Split(' ');
            List<double> parsedParams = new List<double>();

            function.Parameters.Clear();

            foreach (string parameter in parameters)
            {
                function.Parameters.Add(Convert.ToDouble(parameter.Trim()));
            }

            return new EvolvingNeuron(function);
        }
    }
}
