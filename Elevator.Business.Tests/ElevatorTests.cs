using NUnit.Framework;

namespace Elevator.Business.Tests
{
    public class ElevatorTests
    {
        private int _numberOfFloors;
        private int _carryingCapacity;

        [SetUp]
        public void Setup()
        {
            _numberOfFloors = 10;
            _carryingCapacity = 1000;
        }

        [Test]
        public void Should_Be_On_First_Floor_When_Init()
        {
            var elevator = new Elevator(_numberOfFloors, _carryingCapacity);

            Assert.That(elevator.Sensor.CurrentFloor, Is.EqualTo(1));
        }

        [Test]
        public void Should_Have_No_Weight_When_Init()
        {
            var elevator = new Elevator(_numberOfFloors, _carryingCapacity);

            Assert.That(elevator.Sensor.CurrentWeight, Is.EqualTo(0));
        }

        [Test]
        public void Should_Move_When_Pressed_Outside_While_Stopped_Other_Floor()
        {
            var elevator = new Elevator(_numberOfFloors, _carryingCapacity);
            elevator.Call(2, Models.Enums.DirectionEnum.Down);

            Assert.That(elevator.Sensor.State, Is.EqualTo(Models.Enums.StateEnum.Moving));
        }

        [Test]
        public void Should_Not_Move_When_Pressed_Outside_While_Stopped_Same_Floor()
        {
            var elevator = new Elevator(_numberOfFloors, _carryingCapacity);
            elevator.Call(elevator.Sensor.CurrentFloor, Models.Enums.DirectionEnum.Up);

            Assert.That(elevator.Sensor.State, Is.EqualTo(Models.Enums.StateEnum.Stopped));
        }

        [Test]
        public void Should_Move_When_Pressed_Inside_While_Stopped_Other_Floor()
        {
            var elevator = new Elevator(_numberOfFloors, _carryingCapacity);
            elevator.Go(2);

            Assert.That(elevator.Sensor.State, Is.EqualTo(Models.Enums.StateEnum.Moving));
        }

        [Test]
        public void Should_Not_Move_When_Pressed_Inside_While_Stopped_Same_Floor()
        {
            var elevator = new Elevator(_numberOfFloors, _carryingCapacity);
            elevator.Go(elevator.Sensor.CurrentFloor);

            Assert.That(elevator.Sensor.State, Is.EqualTo(Models.Enums.StateEnum.Stopped));
        }

        [Test]
        public void Should_Have_Weight_When_Called_On_Same_Floor_While_Stopped()
        {
            var elevator = new Elevator(_numberOfFloors, _carryingCapacity);

            elevator.Call(elevator.Sensor.CurrentFloor, Models.Enums.DirectionEnum.Up);
            Assert.That(elevator.Sensor.CurrentWeight, Is.GreaterThan(0));
        }

        [Test]
        public void Should_Not_Have_Weight_When_Goes_On_Same_Floor_While_Stopped()
        {
            var elevator = new Elevator(_numberOfFloors, _carryingCapacity);

            elevator.Go(elevator.Sensor.CurrentFloor);
            Assert.That(elevator.Sensor.CurrentWeight, Is.EqualTo(0));
        }

        [Test]
        public void Should_Not_Increase_Weight_When_Called_On_Same_Floor_While_Moving()
        {
            var elevator = new Elevator(_numberOfFloors, _carryingCapacity);
            elevator.Call(_numberOfFloors, Models.Enums.DirectionEnum.Down);

            var expectedWeight = elevator.Sensor.CurrentWeight;
            elevator.Call(1, Models.Enums.DirectionEnum.Up);

            Assert.That(elevator.Sensor.CurrentWeight, Is.EqualTo(expectedWeight));
        }

        [Test]
        public void Should_Not_Increase_Weight_When_Called_On_Different_Floor_While_Stopped()
        {
            var elevator = new Elevator(_numberOfFloors, _carryingCapacity);
            var expectedWeight = elevator.Sensor.CurrentWeight;
            elevator.Call(_numberOfFloors, Models.Enums.DirectionEnum.Down);

            Assert.That(elevator.Sensor.CurrentWeight, Is.EqualTo(expectedWeight));
        }

        [Test]
        public void Should_Throw_Exception_When_Called_From_Non_Existing_Floor()
        {
            var elevator = new Elevator(_numberOfFloors, _carryingCapacity);
            var nonExistingFloor = _numberOfFloors + 1;

            var exception = Assert.Throws<Models.ElevatorException>(() => elevator.Call(nonExistingFloor, Models.Enums.DirectionEnum.Up));
            Assert.That(exception.ErrorCode, Is.EqualTo("ERR01"));
        }

        [Test]
        public void Should_Throw_Exception_When_Called_From_Zero_or_Negative_Floors()
        {
            var elevator = new Elevator(_numberOfFloors, _carryingCapacity);
            var nonExistingFloor = 0;

            var exception = Assert.Throws<Models.ElevatorException>(() => elevator.Call(nonExistingFloor, Models.Enums.DirectionEnum.Up));
            Assert.That(exception.ErrorCode, Is.EqualTo("ERR02"));
        }

        [Test]
        public void Should_Throw_Exception_When_Called_From_Top_Floor_And_Up_Direction()
        {
            var elevator = new Elevator(_numberOfFloors, _carryingCapacity);

            var exception = Assert.Throws<Models.ElevatorException>(() => elevator.Call(_numberOfFloors, Models.Enums.DirectionEnum.Up));
            Assert.That(exception.ErrorCode, Is.EqualTo("ERR03"));
        }

        [Test]
        public void Should_Throw_Exception_When_Called_From_Main_Floor_And_Down_Direction()
        {
            var elevator = new Elevator(_numberOfFloors, _carryingCapacity);
            
            var exception = Assert.Throws<Models.ElevatorException>(() => elevator.Call(1, Models.Enums.DirectionEnum.Down));
            Assert.That(exception.ErrorCode, Is.EqualTo("ERR04"));
        }

        [Test]
        public void Should_Throw_Exception_When_Goes_Non_Existing_Floor()
        {
            var elevator = new Elevator(_numberOfFloors, _carryingCapacity);
            var nonExistingFloor = _numberOfFloors + 1;

            var exception = Assert.Throws<Models.ElevatorException>(() => elevator.Go(nonExistingFloor));
            Assert.That(exception.ErrorCode, Is.EqualTo("ERR05"));
        }

        [Test]
        public void Should_Throw_Exception_When_Goes_Zero_or_Negative_Floors()
        {
            var elevator = new Elevator(_numberOfFloors, _carryingCapacity);
            var nonExistingFloor = 0;

            var exception = Assert.Throws<Models.ElevatorException>(() => elevator.Go(nonExistingFloor));
            Assert.That(exception.ErrorCode, Is.EqualTo("ERR06"));
        }
    }
}