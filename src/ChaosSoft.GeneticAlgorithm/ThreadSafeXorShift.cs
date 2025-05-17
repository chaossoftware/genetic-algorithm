using static System.Net.WebRequestMethods;

namespace ChaosSoft.GeneticAlgorithm;

/// <summary>
/// Implementation of thread safe Xorshift random number generator.
/// See <see href="https://en.wikipedia.org/wiki/Xorshift"/>.
/// </summary>
public sealed class ThreadSafeXorShift
{
    private uint _x = 123456789;
    private uint _y = 362436069;
    private uint _z = 521288629;
    private uint _w = 8867512;
    private readonly object _lockObject = new object();

    /// <summary>
    /// Initializes a new instance of the <see cref="ThreadSafeXorShift"/> class.
    /// </summary>
    public ThreadSafeXorShift()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThreadSafeXorShift"/> class with custom seeding parameters
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="w"></param>
    public ThreadSafeXorShift(uint x, uint y, uint z, uint w)
    {
        _x = x;
        _y = y;
        _z = z;
        _w = w;
    }

    /// <summary>
    /// Gets thread safe instance of the generator.
    /// </summary>
    public static ThreadSafeXorShift Instance { get; } = new ThreadSafeXorShift();

    /// <summary>
    /// Gets random <see cref="uint"/>.
    /// </summary>
    /// <returns>uint number</returns>
    public uint Next()
    {
        lock (_lockObject)
        {
            uint t = _x ^ (_x << 11);
            _x = _y;
            _y = _z;
            _z = _w;
            return _w = _w ^ (_w >> 19) ^ t ^ (t >> 8);
        }
    }

    /// <summary>
    /// Gets random <see cref="int"/> not greater than specified number.
    /// </summary>
    /// <param name="maxValue">upper bound</param>
    /// <returns>integer value</returns>
    public int NextInt(int maxValue) =>
        (int)(Next() % (uint)maxValue);

    /// <summary>
    /// Gets random <see cref="int"/> with value between specified numbers.
    /// </summary>
    /// <param name="minValue">lower bound</param>
    /// <param name="maxValue">upper bound</param>
    /// <returns>integer value</returns>
    public int NextIntRange(int minValue, int maxValue) =>
        minValue + NextInt(maxValue - minValue);

    /// <summary>
    /// Gets random <see cref="double"/> with value between 0 and 1.
    /// </summary>
    /// <returns>double value</returns>
    public double NextDouble() =>
        (float)Next() / uint.MaxValue;

    /// <summary>
    /// Gets random <see cref="double"/> with value between specified numbers.
    /// </summary>
    /// <param name="minValue">lower bound</param>
    /// <param name="maxValue">upper bound</param>
    /// <returns>double value</returns>
    public double NextDoubleRange(float minValue, float maxValue) =>
        minValue + (maxValue - minValue) * NextDouble();
}
