# Elevator App
This application simulates the operation of a simple elevator. For requirements visit click [here](REQUIREMENTS.md).

**Approach**
- Application is built with [.NET 5](https://dotnet.microsoft.com/download/dotnet/5.0)
- The N-Layer architecture was used to design the application.
- TDD has been practiced to build the core business logic of the elevator.

**Assumptions**

About carrying weight: 
- Default person weight is set to `136.7 pounds (Average adult human weight)`.
- every time elevator doors open for outside calls; `the number of people to get in = number of times that button pressed.`
- every time elevator doors open for inside call; `the number of people to get out = number of times that button pressed.`

## Projects
- **Elevator.App** - Main executable to run the app
- **Elevator.Business** - Business logic for the app
- **Elevator.Business.Tests** - Unit tests for business logic
- **Elevator.Messages** - Application wide user messages
- **Elevator.Models** - Application models, enums, pocos

# External Libraries
- [Serilog](https://serilog.net/) - Used for logging.
- [NUnit](https://nunit.org/) - Used for unit testing.


# How to Run?
> Application requires [.NET 5](https://dotnet.microsoft.com/download/dotnet/5.0)

**Debug Mode**

Open the solution file with Visual Studio and debug SMAElevator.App project.

**Executable**

Download the latest version from [Releases](https://github.com/arunes/elevator-app/releases) page.

- Windows
Run `dist/Elevator.App.exe` to run the application on windows.

- Linux
Run `dotnet dist/Elevator.App.dll`

**Commands**

The application will work on user commands given through the console window.
- Go to the floor (Press from inside)
  `7` will go to the 7th floor
- Call the elevator (Press from outside)
  Floor number followed by direction `U` for up, `D` for down.
  `5U` will call the elevator to the 5th floor to go up
  `4D` will call the elevator to the 4th floor to go down
- Get elevator status 
  `↵` Enter key
- Shutdown the elevator
  `Q`
