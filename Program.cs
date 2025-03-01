using System;
using System.Collections.Generic;

namespace ToyRobot
{
    // enumeration for the facing directions of the robot
    public enum Direction
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }


    // Class for a toy robot
    public class Robot
    {
        // Robot position and direction
        private int _x;             // x-coordinate of robot
        private int _y;             // y-coordinate of robot
        private Direction _f;       // direction robot is facing
        
        // Table dimensions
        private readonly int _tableSizeX; // width of table (X-axis)
        private readonly int _tableSizeY; // height of table (Y-axis)

        // Initialise a new instance of the Robot class with a specified table size
        // parameter name="tableSizeX" Width of the table (default is 5)
        // parameter name="tableSizeY" Height of the table (default is 5)
        public Robot(int tableSizeX = 5, int tableSizeY = 5)
        {
            _tableSizeX = tableSizeX;
            _tableSizeY = tableSizeY;
        }
        // Place the robot at the specified position and direction
        // parameter name="x" X-coordinate to place the robot
        // parameter name="y" Y-coordinate to place the robot
        // parameter name="f" Direction for the robot to face
        // returns True if placement was successful, false if position is invalid
        public bool Place(int x, int y, Direction f)
        {
            if (IsValidPosition(x, y))
            {
                _x = x;
                _y = y;
                _f = f;
                return true;
            }
            return false;
        }

        // Validate if a position is within the table boundaries
        // parameter name="x" X-coordinate to check
        // parameter name="y" Y-coordinate to check
        // returns True if position is valid, otherwise false
        private bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < _tableSizeX && y >= 0 && y < _tableSizeY;
        }
    }



    // Process text commands and direct them to the robot
    public class CommandsProcessor
    {
        private readonly Robot _robot;

        // Initialise a new instance of the CommandProcessor class
        // parameter name="robot" Robot instance to control
        public CommandsProcessor(Robot robot)
        {
            _robot = robot ?? throw new ArgumentNullException(nameof(robot));
        }

        // Processes a text command and executes it on the robot
        // parameter name="command" Text command to process
        // returns Result message or empty string for commands that don't produce output
        public string CommandProcessor(string command)
        {
            // Handle empty commands
            if (string.IsNullOrWhiteSpace(command))
                return string.Empty;

            // Split the command into action and parameters
            string[] parts = command.Trim().Split(' ', 2);
            string action = parts[0].ToUpper();

            // Process based on command type
            switch (action)
            {
                case "PLACE":
                    // Validate PLACE command format
                    if (parts.Length != 2)
                        return "Invalid PLACE command";

                    string[] parameters = parts[1].Split(',');
                    if (parameters.Length != 3)
                        return "Invalid PLACE parameters";

                    // Parse and validate parameters
                    if (!int.TryParse(parameters[0], out int x) || 
                        !int.TryParse(parameters[1], out int y) || 
                        !Enum.TryParse(parameters[2], true, out Direction facing))
                        return "Invalid PLACE parameters format";

                    // Execute placement
                    _robot.Place(x, y, facing);
                    return string.Empty;


                default:
                    return "Unknown command";
            }
        }
    }
    


    // main program class to run the toy robot - provides user interface
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Toy Robot");
            // create a new robot object
            Robot robot = new Robot();
            // create a new command object
            CommandsProcessor command = new CommandsProcessor(robot);
            bool robotPlaced = false;

            // Only allow PLACE command
            Console.WriteLine("Place the robot on the table (use PLACE X,Y,FACING) or type EXIT to quit");
            
            while (!robotPlaced)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                
                // Check for exit command
                if (input?.ToUpper() == "EXIT")
                    return;
                
                // Check if command starts with PLACE
                if (input?.StartsWith("PLACE", StringComparison.OrdinalIgnoreCase) == true)
                {
                    string result = command.CommandProcessor(input);
                    if (!string.IsNullOrEmpty(result))
                    {
                        // Display error message if placement failed
                        Console.WriteLine(result);
                    }
                    else
                    {
                        // placement succeeded
                        robotPlaced = true;
                        Console.WriteLine("Robot placed successfully!");
                    }
                }
                else
                {
                    // Ask user to place robot first
                    Console.WriteLine("Use the PLACE command first to place the robot on the table");
                }
            }
            
            // Allow all commands
            Console.WriteLine("Commands: PLACE X,Y,FACING | MOVE | LEFT | RIGHT | REPORT | EXIT");
            
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                
                // Check for exit command
                if (input?.ToUpper() == "EXIT")
                    break;
                
                // Process any valid command
                string result = command.CommandProcessor(input);
                if (!string.IsNullOrEmpty(result))
                    Console.WriteLine(result);
            }
        }
    }
}