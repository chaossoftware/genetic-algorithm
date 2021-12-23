using SciML.NeuralNetwork.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SciML.NeuralNetwork.Evolution
{
    public static class EvolvingNetworkLoader
    {
        public static void ToFile<T>(T network, string fileName) where T : EvolvingNeuralNetBase
        {
            XElement root = new XElement(typeof(T).Name);

            XElement inputLayerElement = new XElement("InputLayer");

            foreach (TresholdNeuron neuron in network.InputLayer.Neurons)
            {
                inputLayerElement.Add(StoreNeuron(neuron));
            }

            XElement hiddenLayerElement = new XElement("HiddenLayer");

            foreach (TresholdNeuron neuron in network.HiddenLayer.Neurons)
            {
                hiddenLayerElement.Add(StoreNeuron(neuron));
            }

            XElement outputLayerElement = new XElement("OutputLayer");

            foreach (TresholdNeuron neuron in network.OutputLayer.Neurons)
            {
                outputLayerElement.Add(StoreNeuron(neuron));
            }

            XElement connections0Element = new XElement("Connections0");
            network.Connections[0].ForEach(s => connections0Element.Add(StoreSynapse(s)));

            XElement connections1Element = new XElement("Connections1");
            network.Connections[1].ForEach(s => connections1Element.Add(StoreSynapse(s)));

            root.Add(inputLayerElement);
            root.Add(hiddenLayerElement);
            root.Add(outputLayerElement);
            root.Add(connections0Element);
            root.Add(connections1Element);

            new XDocument(root).Save(fileName);
        }

        public static T FromFile<T>(string fileName) where T : EvolvingNeuralNetBase
        {
            T network = Activator.CreateInstance<T>();
            XElement root = XDocument.Load(fileName).Element(typeof(T).Name);

            if (root == null)
            {
                throw new ArgumentException($"You are trying to load brain not of type {typeof(T).Name}");
            }

            XElement[] inputLayerElements = root.Element("InputLayer").Elements().ToArray();

            for (int i = 0; i < inputLayerElements.Count(); i++)
            {
                TresholdNeuron neuron = LoadNeuron(inputLayerElements[i]);
                neuron.Inputs.Add(new Synapse(i, i, 1));
                network.InputLayer.Neurons[i] = neuron;
            }

            XElement[] hiddenLayerElements = root.Element("HiddenLayer").Elements().ToArray();

            for (int i = 0; i < hiddenLayerElements.Count(); i++)
            {
                network.HiddenLayer.Neurons[i] = LoadNeuron(hiddenLayerElements[i]);
            }

            XElement[] outputLayerElements = root.Element("OutputLayer").Elements().ToArray();

            for (int i = 0; i < outputLayerElements.Count(); i++)
            {
                TresholdNeuron neuron = LoadNeuron(outputLayerElements[i]);
                neuron.Outputs.Add(new Synapse(i, i, 1));
                network.OutputLayer.Neurons[i] = neuron;
            }

            root.Element("Connections0").Elements().ToList()
                .ForEach(s => network.Connections[0].Add(LoadSynapse(s)));

            root.Element("Connections1").Elements().ToList()
                .ForEach(s => network.Connections[1].Add(LoadSynapse(s)));

            foreach (Synapse synapse in network.Connections[0])
            {
                network.InputLayer.Neurons[synapse.InIndex].Outputs.Add(synapse);
                network.HiddenLayer.Neurons[synapse.OutIndex].Inputs.Add(synapse);
            }

            foreach (Synapse synapse in network.Connections[1])
            {
                network.HiddenLayer.Neurons[synapse.InIndex].Outputs.Add(synapse);
                network.OutputLayer.Neurons[synapse.OutIndex].Inputs.Add(synapse);
            }

            return network;
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

        private static XElement StoreNeuron(TresholdNeuron neuron)
        {
            return new XElement("TresholdNeuron",
                new XAttribute("function", neuron.TresholdFunction.GetType().Name),
                new XAttribute("params", string.Join(" ", neuron.GetTresholdParameters())));
        }

        private static TresholdNeuron LoadNeuron(XElement brainXml)
        {
            ThresholdFunction function = ThresholdFunction.GetFunction(brainXml.Attribute("function").Value);
            string[] parameters = brainXml.Attribute("params").Value.Split(' ');
            List<double> parsedParams = new List<double>();

            foreach (string parameter in parameters)
            {
                parsedParams.Add(Convert.ToDouble(parameter.Trim()));
            }

            return new TresholdNeuron(function, parsedParams);
        }

    }
}
