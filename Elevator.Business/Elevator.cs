using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Enums = Elevator.Models.Enums;

namespace Elevator.Business
{
    public class Elevator
    {
        private const int SecondsToWaitAtFloor = 1; // Set by requirement #5
        private const int SecondsToGoToTheNextFloor = 3; // Set by requirement #5
        private const double DefaultPersonWeight = 136.7; // Since we don't know the weights of the people getting in the elevator, we set a default weight

        private readonly ILogger _logger;
        private List<Models.ElevatorCommand> _queue;
        private List<Models.ElevatorCommand> _backlog;

        #region Properties
        private Models.ElevatorSensor _sensor { get; set; }
        public Models.ElevatorSensor Sensor { get => _sensor; }

        private readonly int _numberOfFloors;
        public int NumberOfFloors { get => _numberOfFloors; }

        private readonly int _carryingCapacity;
        public int CarryingCapacity { get => _carryingCapacity; }
        #endregion

        public Elevator(int numberOfFloors, int carryingCapacity)
        {
            // add sensor data to logger
            _logger = Log.ForContext<Elevator>().ForContext("SensorData", _sensor);

            _numberOfFloors = numberOfFloors;
            _carryingCapacity = carryingCapacity;
            Initialize();
        }

        #region Public Methods
        /// <summary>
        /// Goes the specified floor.
        /// This action mimics the button press from inside the elevator.
        /// </summary>
        /// <param name="floor">The floor.</param>
        public void Go(int floor)
        {
            _logger.Information("Button {Button} pressed from inside", floor);

            #region Validations
            Models.ElevatorException exception = null;
            if (floor > _numberOfFloors)
                exception = new Models.ElevatorException("ERR05", string.Format(Messages.Errors.ERR05, floor, _numberOfFloors));
            else if (floor < 1)
                exception = new Models.ElevatorException("ERR06", string.Format(Messages.Errors.ERR06, floor));

            if (exception != null)
            {
                _logger.Error("Elevator exception", exception);
                throw exception;
            }
            #endregion

            PressButton(floor, Enums.PanelLocationEnum.Inside);

            if (_sensor.State == Enums.StateEnum.Stopped && _sensor.CurrentFloor == floor)
            { // when current floor's button pressed from inside while stopping, just open the doors
                ArrivedToFloor();
            }
        }

        /// <summary>
        /// Calls the elevator to specified floor.
        /// This action mimics the button press from outside of the elevator.
        /// </summary>
        /// <param name="floor">The floor.</param>
        /// <param name="direction">The direction.</param>
        public void Call(int floor, Enums.DirectionEnum direction)
        {
            _logger.Information("Elevator called from #{Floor} to go {Direction}", floor, direction);

            #region Validations
            Models.ElevatorException exception = null;
            if (floor > _numberOfFloors)
                exception = new Models.ElevatorException("ERR01", string.Format(Messages.Errors.ERR01, floor, _numberOfFloors));
            else if (floor < 1)
                exception = new Models.ElevatorException("ERR02", string.Format(Messages.Errors.ERR02, floor));
            else if (floor == _numberOfFloors && direction == Enums.DirectionEnum.Up)
                exception = new Models.ElevatorException("ERR03", Messages.Errors.ERR03);
            else if (floor == 1 && direction == Enums.DirectionEnum.Down)
                exception = new Models.ElevatorException("ERR04", Messages.Errors.ERR04);

            if (exception != null)
            {
                _logger.Error("Elevator exception", exception);
                throw exception;
            }
            #endregion

            PressButton(floor, Enums.PanelLocationEnum.Outside, direction);

            if (_sensor.State == Enums.StateEnum.Stopped && _sensor.CurrentFloor == floor)
            { // when elevator called in the same floor it is stopping, just open the doors
                ArrivedToFloor();
            }
        }

        /// <summary>
        /// Waits until elevator finishes it's duty.
        /// </summary>
        public async Task Shutdown()
        {
            _logger.Information("Shutdown requested");

            while (true)
            {
                if (_queue.Any())
                    Thread.Sleep(1000);
                else
                    break;
            }
        }
        #endregion

        /// <summary>
        /// Initializes elevator instance.
        /// </summary>
        private void Initialize()
        {
            _sensor = new Models.ElevatorSensor();
            _queue = new List<Models.ElevatorCommand>();
            _backlog = new List<Models.ElevatorCommand>();
        }

        /// <summary>
        /// Presses the elevator button (inside or outside).
        /// </summary>
        /// <param name="floor">The floor.</param>
        /// <param name="panel">The panel.</param>
        /// <param name="direction">The direction.</param>
        private void PressButton(int floor, Enums.PanelLocationEnum panel, Enums.DirectionEnum? direction = null)
        {
            if (_sensor.State == Enums.StateEnum.Moving)
            { // To satisfy requirement #4, if elevator is moving we wait until next stop to add this request to queue
                _backlog.Add(new Models.ElevatorCommand(floor, panel, direction));
                return;
            }
            else
            { // add request to the queue
                _queue.Add(new Models.ElevatorCommand(floor, panel, direction));
            }

            CheckQueue();
        }

        /// <summary>
        /// Gets the next floor by looking at the queue.
        /// </summary>
        /// <returns></returns>
        private int? GetNextFloor()
        {
            // Queue is empty, nothing to do here
            if (!_queue.Any())
                return null;

            // Let's get next upper floor
            int? nextUp = _queue.OrderBy(q => q.Floor)
                    .Where(q => q.Floor > _sensor.CurrentFloor)
                    .Select(q => (int?)q.Floor)
                    .FirstOrDefault();

            // Let's get next lower floor
            int? nextDown = _queue.OrderByDescending(q => q.Floor)
                    .Where(q => q.Floor < _sensor.CurrentFloor)
                    .Select(q => (int?)q.Floor)
                    .FirstOrDefault();

            // The direction we are going has a priority for choosing where to go next
            // Satisfies the requirement #1
            if (_sensor.Direction == Enums.DirectionEnum.Up)
            {
                return nextUp ?? nextDown;
            }
            else
            {
                return nextDown ?? nextUp;
            }
        }

        /// <summary>
        /// Checks the queue and processes if anything needs to be processed.
        /// </summary>
        private void CheckQueue()
        {
            // Elevator is already on the move
            if (_sensor.State == Enums.StateEnum.Moving)
                return;

            var next = GetNextFloor();
            if (next.HasValue)
            {
                _sensor.NextFloor = next.Value;
                _sensor.Direction = _sensor.NextFloor > _sensor.CurrentFloor ? Enums.DirectionEnum.Up : Enums.DirectionEnum.Down;

                // Start moving the elevator if it's not already moving
                if (_sensor.State == Enums.StateEnum.Stopped)
                {
                    _sensor.State = Enums.StateEnum.Moving;
                    Task.Run(() => Move());
                }
            }
        }

        /// <summary>
        /// Triggers when elevator arrives to any floor.
        /// </summary>
        private void ArrivedToFloor()
        {
            _sensor.State = Enums.StateEnum.Stopped;
            _sensor.NextFloor = null;

            // Calculate how many people are getting in and out to find the current weight
            // Satisfies the Bonus Enhancement
            var gettingIn = _queue.Where(q => q.Floor == _sensor.CurrentFloor && q.PanelLocation == Enums.PanelLocationEnum.Outside);
            var gettingOut = _queue.Where(q => q.Floor == _sensor.CurrentFloor && q.PanelLocation == Enums.PanelLocationEnum.Inside);

            var gettingInWeight = gettingIn.Count() * DefaultPersonWeight;
            var gettingOutWeight = gettingOut.Count() * DefaultPersonWeight;

            if ((_sensor.CurrentWeight + gettingInWeight - gettingOutWeight) > _carryingCapacity)
            { // We can't take everyone to car, lets see how many we can take
                var availableWeight = _carryingCapacity - _sensor.CurrentWeight;
                var canTake = (int)Math.Floor(availableWeight / DefaultPersonWeight);

                var canGetIn = gettingIn.Take(canTake);
                var cannotGetIn = gettingIn.Except(canGetIn);

                // add commands to backlog, if they can't make it this time
                _backlog.AddRange(cannotGetIn);

                gettingInWeight = canGetIn.Count() * DefaultPersonWeight;
            }

            _sensor.CurrentWeight += (gettingInWeight - gettingOutWeight);

            // Since button can be pressed even no one is in the car, weight can go negative. Reset to zero
            if (_sensor.CurrentWeight < 0) _sensor.CurrentWeight = 0;

            // Remove all processed commands from the queue
            _queue.RemoveAll(q => q.Floor == _sensor.CurrentFloor);

            // Let's check the queue
            CheckQueue();
        }

        /// <summary>
        /// Moves elevator to current sensor direction.
        /// </summary>
        private void Move()
        {
            // Takes some time to go to the next floor. Satisfies the requirement #5
            Thread.Sleep(SecondsToGoToTheNextFloor * 1000);

            if (_backlog.Any())
            { // If any request is on the backlog, lets add them to the queue
                _queue.AddRange(_backlog);
                _backlog.Clear();
            }

            if (_sensor.Direction == Enums.DirectionEnum.Up)
                _sensor.CurrentFloor++;
            else
                _sensor.CurrentFloor--;

            // if this is not our target floor skip this
            var skipTheFloor = _sensor.NextFloor != _sensor.CurrentFloor;
            if (skipTheFloor)
            {
                _logger.Information("Passing the floor #{Floor}, going to #{TargetFloor}. Current carrying weight is: {CurrentWeight}",
                    _sensor.CurrentFloor, _sensor.NextFloor, _sensor.CurrentWeight);

                // Wait some time on the floor. Satisfies the requirement #5
                // NOTE: Would be better if we only make elevator wait when door opens
                Thread.Sleep(SecondsToWaitAtFloor * 1000);

                Move();
            }
            else
            {
                _logger.Information("Elevator stopped at the floor #{Floor}", _sensor.CurrentFloor);
                ArrivedToFloor();

                // Wait some time on the floor. Satisfies the requirement #5
                Thread.Sleep(SecondsToWaitAtFloor * 1000);
            }
        }
    }
}
