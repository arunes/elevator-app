namespace Elevator.Models
{
    /// <summary>
    /// Contains the information about the elevator commands.
    /// Commands can be given inside or outide of the elevator.
    /// </summary>
    public class ElevatorCommand
    {
        /// <summary>
        /// Gets or sets the target floor.
        /// </summary>
        /// <value>
        /// The floor.
        /// </value>
        public int Floor { get; set; }

        /// <summary>
        /// Gets or sets the location of the panel that indicates if call made from inside or outside.
        /// </summary>
        /// <value>
        /// The location of the panel.
        /// </value>
        public Enums.PanelLocationEnum PanelLocation { get; set; }

        /// <summary>
        /// Gets or sets the direction of the command if command made by outside buttons.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public Enums.DirectionEnum? Direction { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevatorCommand" /> class.
        /// </summary>
        /// <param name="floor">The floor.</param>
        /// <param name="panelLocation">The location of the panel.</param>
        /// <param name="direction">The direction.</param>
        public ElevatorCommand(int floor, Enums.PanelLocationEnum panelLocation, Enums.DirectionEnum? direction = null)
        {
            Floor = floor;
            PanelLocation = panelLocation;
            Direction = direction;
        }
    }
}