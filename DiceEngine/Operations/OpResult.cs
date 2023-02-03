namespace DiceEngine.Operations
{
    /// <summary>
    ///     
    /// </summary>
    public struct OpResult
    {
        /// <summary>
        ///     
        /// </summary>
        public double Output { get; }

        /// <summary>
        ///     
        /// </summary>
        public string Report { get; private set; }

        private OpResult(double output, string calculationReport)
        {
            Output = output;
            Report = calculationReport;
        }
        
        /// <summary>
        ///     
        /// </summary>
        /// <param name="addition"></param>
        /// <param name="front"></param>
        public void AppendReport(string addition, bool front)
        {
            if (front)
                Report = (addition + "\n" + Report);
            else
                Report += ("\n" + addition);
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="output"></param>
        /// <param name="report"></param>
        /// <returns></returns>
        public static OpResult Create(double output, string report)
        {
            return new(output, report);
        }
    }
}
