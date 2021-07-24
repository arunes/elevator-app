using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Threading;

namespace Elevator.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var logFile = $"logs-{DateTime.Now:yyyyMMddHHmm}.txt";
            SetupLogging(logFile);

            var elevator = SetupElevator();
            DrawScreen(elevator);

            var currentPosition = Console.GetCursorPosition();
            while (true)
            {
                Console.SetCursorPosition(currentPosition.Left, currentPosition.Top);
                Console.Write(new string(' ', Console.WindowWidth)); // clear line
                Console.SetCursorPosition(currentPosition.Left, currentPosition.Top);

                Console.Write("Enter a command: ");
                var command = Console.ReadLine();

                try
                {
                    var refreshScreen = true;

                    if (command == "Q")
                    { // shutdown requested
                        Console.Clear();
                        Console.WriteLine("Shutting down, please wait until all commands processes.");
                        elevator.Shutdown().Wait();
                        break;
                    }
                    else if (int.TryParse(command, out int insideFloor))
                    { // button pressed from inside
                        elevator.Go(insideFloor);
                    }
                    else if ((command.EndsWith("U") || command.EndsWith("D")) && int.TryParse(command.Remove(command.Length - 1, 1), out int callFloor))
                    { // button pressed from outside
                        var direction = command.EndsWith("U") ? Models.Enums.DirectionEnum.Up : Models.Enums.DirectionEnum.Down;
                        elevator.Call(callFloor, direction);
                    }
                    else if (!string.IsNullOrEmpty(command))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong command, please refer README.md for reference.");
                        Console.ForegroundColor = ConsoleColor.White;
                        refreshScreen = false;
                    }

                    if (refreshScreen)
                        DrawScreen(elevator);
                }
                catch (Models.ElevatorException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error Code: {0}, {1}", ex.ErrorCode, ex.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            Console.WriteLine("Elevator.App finished. Elevator logs saved to {0}", Path.Combine(Environment.CurrentDirectory, logFile));
            Console.WriteLine("Press a key to exit.");
            Console.ReadKey();
        }

        private static void SetupLogging(string logFile)
        {
            // setup serilog 
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(logFile)
                .CreateLogger();
        }

        private static Business.Elevator SetupElevator()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Welcome to Elevator App");
            Console.WriteLine("Please answer questions to start the app.");
            Console.WriteLine("Leave empty if you want to use default values");
            Console.WriteLine(new string('-', 50));

            var numberOfFloors = 10;
            var carryingCapacity = 1000;

            while (true)
            {
                Console.WriteLine("How many floors that elevator can move? (Between 2-15)");
                Console.Write("Enter value (Default 10): ");
                var inpNumberOfFloors = Console.ReadLine();

                // get the default
                if (string.IsNullOrEmpty(inpNumberOfFloors))
                    break;

                if (int.TryParse(inpNumberOfFloors, out numberOfFloors))
                {
                    if (numberOfFloors >= 2 && numberOfFloors <= 15)
                        break;
                }

                numberOfFloors = 10; // set back to default

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You entered a invalid value, please enter a number between 2 and 15.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("What is the carrying capacity of the elevator (in lbs)? (Minimum 150)");
                Console.Write("Enter value (Default 1000): ");
                var inpCarryingCapacity = Console.ReadLine();

                // get the default
                if (string.IsNullOrEmpty(inpCarryingCapacity))
                    break;

                if (int.TryParse(inpCarryingCapacity, out carryingCapacity))
                {
                    if (carryingCapacity >= 150)
                        break;
                }

                carryingCapacity = 1000; // set back to default

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You entered a invalid value, please enter a number at least 150.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }

            return new Business.Elevator(numberOfFloors, carryingCapacity);
        }

        /// <summary>
        /// Draws the console screen.
        /// </summary>
        /// <param name="elevator">The elevator.</param>
        private static void DrawScreen(Business.Elevator elevator)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Elevator is ready to travel!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Please refer the README.md for command references.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Building has {0} floors and elevator has {1} lbs carrying capacity.", elevator.NumberOfFloors, elevator.CarryingCapacity);
            Console.WriteLine(new string('-', 50));

            for (int i = elevator.NumberOfFloors; i > 0; i--)
            {
                Console.ForegroundColor = ConsoleColor.Blue;

                var floorContent = "   ";
                if (elevator.Sensor.CurrentFloor == i)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    var carContent = elevator.Sensor.CurrentWeight > 0 ? "*" : " ";
                    floorContent = $"[{carContent}]";
                }

                Console.WriteLine("Floor {0,2} |{1}|", i, floorContent);
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(new string('-', 50));
            Console.Write("Weight: {0}, State: {1}", elevator.Sensor.CurrentWeight, elevator.Sensor.State);
            if (elevator.Sensor.State == Models.Enums.StateEnum.Moving)
                Console.Write(" {0}", elevator.Sensor.Direction.ToString());

            Console.WriteLine();
            Console.WriteLine(new string('-', 50));
        }
    }
}
