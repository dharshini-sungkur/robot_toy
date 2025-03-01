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


    // main program class to run the toy robot - provides user interface
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Toy Robot");
            // create a new robot object
            Robot robot = new Robot();
            // create a new instructiom object
            Instruction instruction = new Instruction(robot);
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
                    string result = instruction.Instruction(input);
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
                string result = instruction.Instruction(input);
                if (!string.IsNullOrEmpty(result))
                    Console.WriteLine(result);
            }
        }
    }
}