using System;

namespace SciML.NeuralNetwork.Entities;

/// <summary>
/// Interface for neurons.
/// </summary>
/// <typeparam name="N">neuron type</typeparam>
public interface INeuron<N> : ICloneable where N : INeuron<N>
{
    /// <summary>
    /// Processes signal through neuron.
    /// </summary>
    void Process();
}
