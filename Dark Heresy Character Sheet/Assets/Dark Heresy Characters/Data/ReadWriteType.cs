namespace JollyRoger.Data
{ 
    /// <summary>
    /// Enum determining how a file is written to / read from file.
    /// </summary>
    public enum ReadWriteType
    {
        /// <summary> Standard JSON file, mostly for explorting to other applications. </summary>
        Json,
        /// <summary> Basic binary file. Smaller than a JSON file. </summary>
        BasicSerialization,
        /// <summary> 
        /// Super optimized serialization that results in the smallest file size. 
        /// Only a few KB on average. Requires carful management of data.
        /// </summary>
        AdvancedSerialization
    }
}