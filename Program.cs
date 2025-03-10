using System;

namespace PackageExpress
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create service implementations
            IConsoleService consoleService = new ConsoleService();
            IShippingCalculator calculator = new ShippingCalculator();
            
            // Create and run the application
            ShippingApplication app = new ShippingApplication(consoleService, calculator);
            app.Run();
            
            // Keep console window open
            Console.ReadLine();
        }
    }
    
    #region Interfaces
    
    /// <summary>
    /// Interface for console input/output operations
    /// </summary>
    public interface IConsoleService
    {
        void WriteLine(string message);
        string ReadLine();
        float ReadFloat(string prompt);
    }
    
    /// <summary>
    /// Interface for shipping calculations
    /// </summary>
    public interface IShippingCalculator
    {
        float CalculateShippingCost(float width, float height, float length, float weight);
        bool IsPackageTooHeavy(float weight);
        bool IsPackageTooBig(float width, float height, float length);
    }
    
    #endregion
    
    #region Implementations
    
    /// <summary>
    /// Console service implementation
    /// </summary>
    public class ConsoleService : IConsoleService
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
        
        public string ReadLine()
        {
            return Console.ReadLine();
        }
        
        public float ReadFloat(string prompt)
        {
            bool isValid = false;
            float result = 0;
            
            while (!isValid)
            {
                WriteLine(prompt);
                string input = ReadLine();
                
                isValid = float.TryParse(input, out result);
                
                if (!isValid)
                {
                    WriteLine("Invalid input. Please enter a numeric value.");
                }
            }
            
            return result;
        }
    }
    
    /// <summary>
    /// Shipping calculator implementation
    /// </summary>
    public class ShippingCalculator : IShippingCalculator
    {
        // Constants
        private const float MAX_WEIGHT = 50.0f;
        private const float MAX_DIMENSION_SUM = 50.0f;
        private const float DIVISOR = 100.0f;
        
        public float CalculateShippingCost(float width, float height, float length, float weight)
        {
            return (width * height * length * weight) / DIVISOR;
        }
        
        public bool IsPackageTooHeavy(float weight)
        {
            return weight > MAX_WEIGHT;
        }
        
        public bool IsPackageTooBig(float width, float height, float length)
        {
            return (width + height + length) > MAX_DIMENSION_SUM;
        }
    }
    
    #endregion
    
    /// <summary>
    /// Main application class
    /// </summary>
    public class ShippingApplication
    {
        private readonly IConsoleService _consoleService;
        private readonly IShippingCalculator _calculator;
        
        public ShippingApplication(IConsoleService consoleService, IShippingCalculator calculator)
        {
            _consoleService = consoleService;
            _calculator = calculator;
        }
        
        public void Run()
        {
            // Display welcome message
            _consoleService.WriteLine("Welcome to Package Express. Please follow the instructions below.");
            
            // Get package weight
            float weight = _consoleService.ReadFloat("Please enter the package weight:");
            
            // Check if package is too heavy
            if (_calculator.IsPackageTooHeavy(weight))
            {
                _consoleService.WriteLine("Package too heavy to be shipped via Package Express. Have a good day.");
                return;
            }
            
            // Get package dimensions
            float width = _consoleService.ReadFloat("Please enter the package width:");
            float height = _consoleService.ReadFloat("Please enter the package height:");
            float length = _consoleService.ReadFloat("Please enter the package length:");
            
            // Check if package is too big
            if (_calculator.IsPackageTooBig(width, height, length))
            {
                _consoleService.WriteLine("Package too big to be shipped via Package Express.");
                return;
            }
            
            // Calculate shipping cost
            float cost = _calculator.CalculateShippingCost(width, height, length, weight);
            
            // Display the quote
            _consoleService.WriteLine($"Your estimated total for shipping this package is: ${cost:F2}");
            _consoleService.WriteLine("Thank you!");
        }
    }
