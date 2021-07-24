namespace Elevator.Models
{
    /// <summary>
    /// Contains the current state of the elevator.
    /// </summary>
    public class ElevatorSensor
    {
        /// <summary>
        /// Gets or sets the direction.
        /// Default direction is Up
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public Enums.DirectionEnum Direction { get; set; } = Enums.DirectionEnum.Up;

        /// <summary>
        /// Gets or sets the current floor.
        /// Default current floor is 1
        /// </summary>
        /// <value>
        /// The current floor.
        /// </value>
        public int CurrentFloor { get; set; } = 1;

        /// <summary>
        /// Gets or sets the next floor.
        /// </summary>
        /// <value>
        /// The next floor.
        /// </value>
        public int? NextFloor { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// Default state is Stopped
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public Enums.StateEnum State { get; set; } = Enums.StateEnum.Stopped;

        /// <summary>
        /// Gets or sets the current weight.
        /// Default weight is 0
        /// </summary>
        /// <value>
        /// The current weight.
        /// </value>
        public double CurrentWeight { get; set; } = 0;
    }
}