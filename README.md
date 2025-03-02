# robot_toy
ICWA Robot Toy

# Toy Robot Simulator
A simple application that simulates a toy robot moving on a 5x5 tabletop with no obstructions.

This application is a simulation of a toy robot moving on a square tabletop. The robot is free to roam around the surface of the table, but must be prevented from falling to destruction, while allowing further valid commands

# Commands
The robot can accept the following commands:
- `PLACE X,Y,FACING`: Places the robot at position (X,Y) facing one of the four directions (NORTH, SOUTH, EAST, WEST).
- `MOVE`: Moves the robot one unit forward in the direction it is currently facing.
- `LEFT`: Rotates the robot 90 degrees counterclockwise without changing its position
- `RIGHT`: Rotates the robot 90 degrees clockwise without changing its position
- `REPORT`: Outputs the current position (coordinates) and direction of the robot.
- `EXIT`: Exits the application


# Constraints
- The origin (0,0) is the southwest corner of the table.
- The robot must be placed on the table before any other commands are executed.
- The robot must not fall off the table.


# Implementation

# Project Structure
- `Program.cs`: The main entry point for the console application and all classes for the application
- `Tests`: A separate project containing unit tests.
- `tests.cs`: Contains unit tests for the application.

# Building the Application
```bash
dotnet build
```
# Running the Application
```bash
dotnet run --project ToyRobot
```
# Running Tests
```bash
cd Tests
dotnet test
```

# Example
Toy Robot
Place the robot on the table (use PLACE X,Y,FACING) or type EXIT to quit
> PLACE 0,0,NORTH
Robot placed successfully!
Commands: MOVE | LEFT | RIGHT | REPORT | EXIT
> MOVE
> REPORT
0, 1, NORTH
> LEFT
> MOVE
> LEFT
> MOVE
> EXIT

# Design Decisions
1. **Two-Phase Approach**: The application forces the user to place the robot first before allowing other commands.
2. **Safety Constraints**: The robot is prevented from falling off the table with appropriate feedback.
3. **Nullable State**: The robot's position and direction are nullable, allowing the application to track whether the robot has been placed.
4. **Command Processing**: A dedicated command processor handles parsing and executing commands.
5. **User Feedback**: The application provides clear feedback on command success or failure.
6. **Immutable Table Size**: The table dimensions are set at construction time and cannot be changed afterward, ensuring a consistent simulation space.
7. **Testing**: Unit tests cover required functionalities, edge cases, and example scenarios.