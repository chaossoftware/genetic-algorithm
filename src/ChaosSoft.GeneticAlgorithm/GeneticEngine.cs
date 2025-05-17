using System;

namespace ChaosSoft.GeneticAlgorithm;

/// <summary>
/// Genetic engine has defined initial population, fitness function and evolution params.<br/>
/// The engine evolves from initial population using chromosomes mutation and crossover via
/// selecting best individuals based on fitness function results.
/// </summary>
/// <typeparam name="C">chromosome type</typeparam>
/// <typeparam name="T">fitness measure type</typeparam>
public class GeneticEngine<C, T> where C : IChromosome<C> where T : IComparable<T>
{
    internal readonly IFitness<C, T> _fitnessFunction;
    private readonly ChromosomeComparer<C, T> _chromosomeComparer;
    private readonly int _originalPopulationSize;

    private bool terminate = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneticEngine{C, T}"/> class 
    /// based on initial population and fitness function.
    /// </summary>
    /// <param name="population">initial population for evolution process</param>
    /// <param name="fitnessFunction">function to calculate chromosomes fitness</param>
    public GeneticEngine(Population<C> population, IFitness<C, T> fitnessFunction)
    {
        Population = population;
        _originalPopulationSize = population.Size;
        _fitnessFunction = fitnessFunction;
        _chromosomeComparer = new ChromosomeComparer<C, T>(_fitnessFunction);
    }

    /// <summary>
    /// Delegate for <see cref="OnIterationFinish"/> event.
    /// </summary>
    /// <param name="engineInstance"></param>
    public delegate void GeneticIterationEvent(GeneticEngine<C, T> engineInstance);

    /// <summary>
    /// Event called after each evolution step execution.
    /// </summary>
    public event GeneticIterationEvent OnIterationFinish;

    /// <summary>
    /// Event called after each evolution step execution.
    /// </summary>
    public event GeneticIterationEvent OnFitnessEvaluate;

    /// <summary>
    /// Gets or sets evolution iteration.
    /// </summary>
    public int Generation { get; private set; } = 0;

    /// <summary>
    /// Gets or sets evolving population
    /// </summary>
    public Population<C> Population { get; protected set; }

    /// <summary>
    /// Gets the best chromosome in population (with index 0)
    /// </summary>
    public C BestChromosome => Population.GetChromosomeByIndex(0);

    /// <summary>
    /// Gets the worst chromosome in population (with largest index)
    /// </summary>
    public C WorstChromosome => Population.GetChromosomeByIndex(Population.Size - 1);

    /// <summary>
    /// Gets number of parental chromosomes, which survive (and move to new population)
    /// </summary>
    public int ParentChromosomesSurviveCount { get; set; } = int.MaxValue;

    /// <summary>
    /// Performs population evolution step:<br/> 
    ///  - sorts population by fitness score<br/> 
    ///  - trims population to original size<br/>
    ///  - generates new population based on survived parent chromosomes (without mutation) 
    ///  and mutation and crossover of rest of chromosomes.
    /// </summary>
    public virtual void Evolve()
    {
        FormNewPopulation();

        OnFitnessEvaluate?.Invoke(this);

        Population.SortByFitness(_chromosomeComparer);
        Population.Trim(_originalPopulationSize);

        _chromosomeComparer.ClearCache();

        Generation++;

        OnIterationFinish?.Invoke(this);
    }

    /// <summary>
    /// Performs specified number of evolution steps (<see cref="Evolve(int)"/>) 
    /// and calling <see cref="OnIterationFinish"/> after each iteration. 
    /// The operation could be terminated.
    /// </summary>
    /// <param name="iterations">number of iterations to perform</param>
    public void Evolve(int iterations)
    {
        terminate = false;

        for (int i = 0; i < iterations; i++)
        {
            if (terminate)
            {
                break;
            }

            Evolve();
        }
    }

    /// <summary>
    /// Simulate extinction event by killing of specified part of population ignoring chromosome fitness.
    /// </summary>
    /// <param name="extinctionRatio">percent of chromosomes to kill (from 0 to 1)</param>
    public void CallMassExtinction(double extinctionRatio)
    {
        int chromosomesToKill = (int)(Population.Size * extinctionRatio);

        for (int i = 0; i < chromosomesToKill; i++)
        {
            Population.RemoveChromosome(Population.GetRandomChromosome());
        }
    }

    /// <summary>
    /// Simulate extinction event by killing of 90% of population ignoring chromosome fitness.
    /// </summary>
    public void CallMassExtinction() => CallMassExtinction(0.9);

    /// <summary>
    /// Gets fitness score of specified chromosome.
    /// </summary>
    /// <param name="chromosome">chromosome instance</param>
    /// <returns>fitness score in form of fitness measure type</returns>
    public T GetFitnessOf(C chromosome) => _chromosomeComparer.Fit(chromosome);

    /// <summary>
    /// Terminates evolution process.
    /// </summary>
    public void Terminate() => terminate = true;

    private void FormNewPopulation()
    {
        // Generates new population based on existing one
        Population<C> newPopulation = new Population<C>();

        int survivedParents = Math.Min(Population.Size, ParentChromosomesSurviveCount);
        
        for (int i = 0; i < survivedParents; i++)
        {
            newPopulation.AddChromosome(Population.GetChromosomeByIndex(i));
        }

        for (int i = 0; i < Population.Size; i++)
        {
            C chromosome = Population.GetChromosomeByIndex(i);

            C mutated = chromosome.Mutate();
            newPopulation.AddChromosome(mutated);

            C otherChromosome;

            do
            {
                otherChromosome = Population.GetRandomChromosome();
            }
            while (otherChromosome.Equals(chromosome));

            foreach (C crossoveredChromosome in chromosome.Crossover(otherChromosome))
            {
                newPopulation.AddChromosome(crossoveredChromosome);
            }
        }

        Population = newPopulation;
    }
}
