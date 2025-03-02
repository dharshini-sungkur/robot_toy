using System;
using System.IO;
using Xunit;
using ToyRobot;

namespace Tests
{
    // Tests for the Robot class to verify  robot functionality
    public class RobotTests
    {
        // Test valid placement of the robot
        [Fact]
        public void PlaceValid_Pass()
        {
            // Create a new robot
            var robot = new Robot();
            
            // Place the robot at a valid position
            bool result = robot.Place(0, 0, Direction.NORTH);
            
            // Assert - Verify placement was successful and position is correct
            Assert.True(result);
            Assert.Equal("0, 0, NORTH", robot.Report());
        }
        
        // Test invalid placement (outside table boundaries)
        [Fact]
        public void PlaceInvalid_Fail()
        {
            // Create a new robot
            var robot = new Robot();
            
            // Try to place the robot outside the table
            bool result = robot.Place(-1, 0, Direction.NORTH);
            
            // Assert - Verify placement failed
            Assert.False(result);
            Assert.Equal("Robot not placed on table", robot.Report());
        }
        
        // Test valid movement within table boundaries 
        [Fact]
        public void MoveValid()
        {
            // Create a robot and place it
            var robot = new Robot();
            robot.Place(0, 0, Direction.NORTH);
            
            // Move the robot
            bool result = robot.Move();
            
            // Assert - Verify movement succeeded and position updated
            Assert.True(result);
            Assert.Equal("0, 1, NORTH", robot.Report());
        }
        
        // Test that robot doesn't fall off the table edge
        [Fact]
        public void MoveInvalid()
        {
            // Create a robot at the edge facing outward
            var robot = new Robot();
            robot.Place(0, 0, Direction.SOUTH);
            
            // Redirect console output to capture "Move will cause robot to fall" message
            var originalOutput = Console.Out;
            using var writer = new StringWriter();
            Console.SetOut(writer);
            
            // Try to move (which would cause a fall)
            bool result = robot.Move();
            
            // Restore console output
            Console.SetOut(originalOutput);
            
            // Assert - Verify movement was prevented and warning was displayed
            Assert.False(result);
            Assert.Equal("0, 0, SOUTH", robot.Report());
            Assert.Contains("Move will cause robot to fall", writer.ToString());
        }
        
        // Test left rotation
        [Fact]
        public void Left_Pass()
        {
            // Create a robot facing NORTH
            var robot = new Robot();
            robot.Place(0, 0, Direction.NORTH);
            
            // Rotate left
            robot.Left();
            
            // Assert - Verify rotation to WEST
            Assert.Equal("0, 0, WEST", robot.Report());
        }
        
        // Test right rotation 
        [Fact]
        public void Right_Pass()
        {
            // Create a robot facing NORTH
            var robot = new Robot();
            robot.Place(0, 0, Direction.NORTH);
            
            // Rotate right
            robot.Right();
            
            // Assert - Verify rotation to EAST
            Assert.Equal("0, 0, EAST", robot.Report());
        }
        
        
        // Test operations on unplaced robot
        [Fact]
        public void UnplacedRobot()
        {
            // Create a robot but don't place it
            var robot = new Robot();
            
            // Redirect console output to capture "Robot not placed" messages
            var originalOutput = Console.Out;
            using var writer = new StringWriter();
            Console.SetOut(writer);
            
            // Attempt operations
            bool moveResult = robot.Move();
            bool leftResult = robot.Left();
            bool rightResult = robot.Right();
            
            // Restore console output
            Console.SetOut(originalOutput);
            
            // Assert - Verify operations fail and warnings are displayed
            Assert.False(moveResult);
            Assert.False(leftResult);
            Assert.False(rightResult);
            Assert.Equal("Robot not placed on table", robot.Report());
            Assert.Contains("Robot not placed", writer.ToString());
        }
        
        // Test multiple commands
        [Fact]
        public void MultipleCommands()
        {
            // Create a robot
            var robot = new Robot();
            
            // Execute the example sequence from requirements
            robot.Place(1, 2, Direction.EAST);
            robot.Move();
            robot.Move();
            robot.Left();
            robot.Move();
            
            // Assert - Verify final position and direction
            Assert.Equal("3, 3, NORTH", robot.Report());
        }
        
        // Test robot movement in each direction
        [Fact]
        public void MoveInEachDirection()
        {
            // Create a robot in the middle of the table
            var robot = new Robot();
            robot.Place(2, 2, Direction.NORTH);
            
            // Test each direction
            
            // Move North
            robot.Move();
            Assert.Equal("2, 3, NORTH", robot.Report());
            
            // Move East
            robot.Right();
            robot.Move();
            Assert.Equal("3, 3, EAST", robot.Report());
            
            // Move South
            robot.Right();
            robot.Move();
            Assert.Equal("3, 2, SOUTH", robot.Report());
            
            // Move West
            robot.Right();
            robot.Move();
            Assert.Equal("2, 2, WEST", robot.Report());
        }
    }



    // Tests for the CommandsProcessor class to verify command parsing and execution
    public class CommandsProcessorTests
    {
        // Test valid PLACE command
        [Fact]
        public void ProcessPlaceCommand_ValidFormat()
        {
            // Create robot and command processor
            var robot = new Robot();
            var processor = new CommandsProcessor(robot);
            
            // Process a valid PLACE command
            string result = processor.CommandProcessor("PLACE 0,0,NORTH");
            
            // Assert - Verify command processed with no errors
            Assert.Equal(string.Empty, result);
            Assert.Equal("0, 0, NORTH", robot.Report());
        }
        
        // Test invalid PLACE command format
        [Fact]
        public void ProcessPlaceCommand_InvalidFormat()
        {
            // Create robot and command processor
            var robot = new Robot();
            var processor = new CommandsProcessor(robot);
            
            // Process an invalid PLACE command
            string result = processor.CommandProcessor("PLACE 0,NORTH");
            
            // Assert - Verify error is returned
            Assert.Contains("Invalid", result);
        }
        
        // Test PLACE command with invalid coordinates
        [Fact]
        public void ProcessPlaceCommand_InvalidCoordinates()
        {
            // Create robot and command processor
            var robot = new Robot();
            var processor = new CommandsProcessor(robot);
            
            // Process a PLACE command with out-of-bounds coordinates
            processor.CommandProcessor("PLACE 10,10,NORTH");
            
            // Assert - Verify robot was not placed
            Assert.Equal("Robot not placed on table", robot.Report());
        }
        
        // Test MOVE command after valid placement
        [Fact]
        public void ProcessMoveCommand_AfterValidPlacement()
        {
            // Create robot, place it, and set up processor
            var robot = new Robot();
            var processor = new CommandsProcessor(robot);
            processor.CommandProcessor("PLACE 0,0,NORTH");
            
            // Process MOVE command
            string result = processor.CommandProcessor("MOVE");
            
            // Assert - Verify move executed
            Assert.Equal(string.Empty, result);
            Assert.Equal("0, 1, NORTH", robot.Report());
        }
        
        // Test REPORT command properly returns robot position
        [Fact]
        public void ProcessReportCommand()
        {
            // Create robot, place it, and set up processor
            var robot = new Robot();
            var processor = new CommandsProcessor(robot);
            processor.CommandProcessor("PLACE 2,3,WEST");
            
            // Process REPORT command
            string result = processor.CommandProcessor("REPORT");
            
            // Assert - Verify report is returned correctly
            Assert.Equal("2, 3, WEST", result);
        }
        
        // Test multiple commands
        [Fact]
        public void ProcessCommandsSequence()
        {
            // Create robot and processor
            var robot = new Robot();
            var processor = new CommandsProcessor(robot);
            
            // Command 1
            processor.CommandProcessor("PLACE 0,0,NORTH");
            processor.CommandProcessor("MOVE");
            Assert.Equal("0, 1, NORTH", processor.CommandProcessor("REPORT"));
            
            // Command 2
            processor.CommandProcessor("PLACE 0,0,NORTH");
            processor.CommandProcessor("LEFT");
            Assert.Equal("0, 0, WEST", processor.CommandProcessor("REPORT"));
            
            // Command 3
            processor.CommandProcessor("PLACE 1,2,EAST");
            processor.CommandProcessor("MOVE");
            processor.CommandProcessor("MOVE");
            processor.CommandProcessor("LEFT");
            processor.CommandProcessor("MOVE");
            Assert.Equal("3, 3, NORTH", processor.CommandProcessor("REPORT"));
        }
        
        // Test commands before valid placement
        [Fact]
        public void CommandsBeforePlacement()
        {
            // Create robot and processor
            var robot = new Robot();
            var processor = new CommandsProcessor(robot);
            
            // Redirect console output to capture "Robot not placed" messages
            var originalOutput = Console.Out;
            using var writer = new StringWriter();
            Console.SetOut(writer);
            
            // Send commands before placement
            processor.CommandProcessor("MOVE");
            processor.CommandProcessor("LEFT");
            processor.CommandProcessor("RIGHT");
            string reportResult = processor.CommandProcessor("REPORT");
            
            // Restore console output
            Console.SetOut(originalOutput);
            
            // Assert - Verify robot is still not placed and proper messages were shown
            Assert.Equal("Robot not placed on table", reportResult);
            Assert.Contains("Robot not placed", writer.ToString());
            
            // Place robot and verify it works after placement
            processor.CommandProcessor("PLACE 2,2,NORTH");
            processor.CommandProcessor("MOVE");
            Assert.Equal("2, 3, NORTH", processor.CommandProcessor("REPORT"));
        }
        
        // Test unknown command handling
        [Fact]
        public void ProcessUnknownCommand()
        {
            // Create robot and processor
            var robot = new Robot();
            var processor = new CommandsProcessor(robot);
            
            // Process unknown command
            string result = processor.CommandProcessor("JUMP");
            
            // Assert - Verify error is returned
            Assert.Contains("Unknown command", result);
        }
    }

}