# Test Documentation for Toy Robot Simulator
This document provides detailed information about the test suite implemented in `tests.cs`. 
The tests validate that the Toy Robot Simulator meets the required specifications and behaves correctly under various conditions.

# Test Structure
The test suite is organized into two main classes:
1. `RobotTests` - Tests for the  `Robot` class functionality
2. `CommandsProcessorTests` - Tests for the `CommandsProcessor` class that handles command parsing and execution

XUnit is used as the testing framework and include standard assertions to validate expected behaviors.

# RobotTests Class
## PlaceValid_Pass
Validates that a robot can be placed at a valid position on the table.
1. Create a new robot
2. Place the robot at position (0,0) facing NORTH
3. Verify the placement was successful (returns true)
4. Verify the robot reports its position correctly as "0, 0, NORTH"

## PlaceInvalid_Fail
Ensures that placing a robot outside the table boundaries fails.
1. Create a new robot
2. Attempt to place the robot at an invalid position (-1,0) facing NORTH
3. Verify the placement fails (returns false)
4. Verify the robot reports "Robot not placed on table"

## MoveValid
Validates that a robot can move one unit forward when the move is valid.
1. Create a new robot
2. Place the robot at position (0,0) facing NORTH
3. Move the robot forward
4. Verify the move was successful (returns true)
5. Verify the robot is now at position "0, 1, NORTH"

## MoveInvalid
Ensures that the robot cannot fall off the table edge.
1. Create a new robot
2. Place the robot at position (0,0) facing SOUTH
3. Redirect console output to capture warning messages
4. Attempt to move the robot (which would cause it to fall)
5. Verify the move fails (returns false)
6. Verify the robot remains at "0, 0, SOUTH"
7. Verify the console output contains "Move will cause robot to fall"

## Left_Pass
Validates that the robot can rotate 90° anticlockwise.
1. Create a new robot
2. Place the robot at position (0,0) facing NORTH
3. Rotate the robot left
4. Verify the robot is now facing WEST ("0, 0, WEST")

## Right_Pass
Validates that the robot can rotate 90° clockwise.
1. Create a new robot
2. Place the robot at position (0,0) facing NORTH
3. Rotate the robot right
4. Verify the robot is now facing EAST ("0, 0, EAST")

## UnplacedRobot
Ensures that operations on an unplaced robot are rejected.
1. Create a new robot (without placing it)
2. Redirect console output to capture warning messages
3. Attempt to move, rotate left, and rotate right
4. Verify all operations fail (return false)
5. Verify the robot reports "Robot not placed on table"
6. Verify the console output contains "Robot not placed" messages

## MultipleCommands
Validates that the robot behaves correctly when given a sequence of commands.
1. Create a new robot
2. Execute a sequence of commands (PLACE 1,2,EAST; MOVE; MOVE; LEFT; MOVE)
3. Verify the final position and direction is "3, 3, NORTH"

## MoveInEachDirection
Ensures the robot moves correctly in all four directions.
1. Create a new robot
2. Place the robot at position (2,2) facing NORTH
3. Move north and verify position
4. Rotate right, move east, and verify position
5. Rotate right, move south, and verify position
6. Rotate right, move west, and verify position

# CommandsProcessorTests Class
## ProcessPlaceCommand_ValidFormat
Validates that the command processor handles valid PLACE commands.
1. Create a robot and command processor
2. Process a "PLACE 0,0,NORTH" command
3. Verify the command returns empty string (success)
4. Verify the robot is at "0, 0, NORTH"

## ProcessPlaceCommand_InvalidFormat
Ensures that malformed PLACE commands are rejected.
1. Create a robot and command processor
2. Process an invalid "PLACE 0,NORTH" command
3. Verify the result contains "Invalid"

## ProcessPlaceCommand_InvalidCoordinates
Validates that PLACE commands with out-of-bounds coordinates are rejected.
1. Create a robot and command processor
2. Process a "PLACE 10,10,NORTH" command (outside table boundaries)
3. Verify the robot reports "Robot not placed on table"

## ProcessMoveCommand_AfterValidPlacement
Validates MOVE command execution after a valid placement.
1. Create a robot and command processor
2. Place the robot at (0,0) facing NORTH
3. Process a "MOVE" command
4. Verify the command returns empty string (success)
5. Verify the robot is at "0, 1, NORTH"

## ProcessReportCommand
Ensures the REPORT command returns the robot's position correctly.
1. Create a robot and command processor
2. Place the robot at (2,3) facing WEST
3. Process a "REPORT" command
4. Verify the result is "2, 3, WEST"

## ProcessCommandsSequence
Validates the robot follows sequences of commands correctly.
1. Create a robot and command processor
2. Process three separate command sequences from the requirements
3. Verify each sequence results in the expected position and direction

## CommandsBeforePlacement
Ensures commands before a valid placement are ignored.
1. Create a robot and command processor
2. Redirect console output to capture messages
3. Process MOVE, LEFT, RIGHT, REPORT commands before placing the robot
4. Verify REPORT returns "Robot not placed on table"
5. Verify console contains "Robot not placed" messages
6. Place the robot, move it, and verify it works after placement

## ProcessUnknownCommand
Validates that unknown commands are rejected with an error.
1. Create a robot and command processor
2. Process an unknown "JUMP" command
3. Verify the result contains "Unknown command"


# Running the Tests
To run the tests, use the .NET CLI:
```
cd Tests
dotnet test
```