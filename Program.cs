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
        private int? _x;             // x-coordinate of robot
        private int? _y;             // y-coordinate of robot
        private Direction? _f;       // direction robot is facing
        
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

        // Check if the robot has been placed on the table
        // return True if the robot is placed, otherwise false
        private bool IsPlaced()
        {
            return _x.HasValue && _y.HasValue && _f.HasValue;
        }


        // Validate if a position is within the table boundaries
        // parameter name="x" X-coordinate to check
        // parameter name="y" Y-coordinate to check
        // returns True if position is valid, otherwise false
        private bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < _tableSizeX && y >= 0 && y < _tableSizeY;
        }

        // Moves the robot one unit forward in the direction it is facing
        // returns True if move was successful, false if not placed or would fall off table
        public bool Move()
        {
            // Cannot move if not placed
            if (!IsPlaced())
            {
                Console.WriteLine("Robot not placed");
                return false;
            }    

            // Calculate new position based on current direction
            int newX = _x.Value;
            int newY = _y.Value;

            switch (_f)
            {
                case Direction.NORTH:
                    newY += 1;
                    break;
                case Direction.EAST:
                    newX += 1;
                    break;
                case Direction.SOUTH:
                    newY -= 1;
                    break;
                case Direction.WEST:
                    newX -= 1;
                    break;
            }

            // Only update position if new position is valid
            if (IsValidPosition(newX, newY))
            {
                _x = newX;
                _y = newY;
                return true;
            }

            // Return false if move will cause robot to fall
            Console.WriteLine("Move will cause robot to fall");
            return false;
        }


        // Rotate the robot 90 degrees counterclockwise
        // return True if rotation was successful, false if robot not placed
        public bool Left()
        {
            if (!IsPlaced())
            {
                Console.WriteLine("Robot not placed");
                return false;
            }

            // Update direction using switch expression
            _f = _f.Value switch
            {
                Direction.NORTH => Direction.WEST,
                Direction.EAST => Direction.NORTH,
                Direction.SOUTH => Direction.EAST,
                Direction.WEST => Direction.SOUTH,
                _ => _f
            };

            return true;
        }

        // Rotate the robot 90 degrees clockwise
        // returns True if rotation was successful, false if robot not placed
        public bool Right()
        {
            if (!IsPlaced())
            {
                Console.WriteLine("Robot not placed");
                return false;
            }

            // Update direction using switch expression
            _f = _f.Value switch
            {
                Direction.NORTH => Direction.EAST,
                Direction.EAST => Direction.SOUTH,
                Direction.SOUTH => Direction.WEST,
                Direction.WEST => Direction.NORTH,
                _ => _f
            };

            return true;
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
                // place robot
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

                    // Execute placement of robot
                    _robot.Place(x, y, facing);
                    return string.Empty;

                // move robot by one step
                case "MOVE":
                    _robot.Move();
                    return string.Empty;

                // rotate robot 90 degrees to the left
                case "LEFT":
                    _robot.Left();
                    return string.Empty;

                // rotate robot 90 degrees to the right
                case "RIGHT":
                    _robot.Right();
                    return string.Empty;

                // cannot recognise command
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
            Console.WriteLine("\nToy Robot");
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