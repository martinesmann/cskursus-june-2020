using System;
using System.Collections.Generic;
using System.Text;

namespace ReflectionDiDemo
{
    public interface ISmallCar
    {
        string Name { get; set; }
    }

    public class SmallCar : ISmallCar
    {
        public string Name { get; set; }
    }


    public interface IMediumCar
    {
        string Name { get; set; }
    }


    public class MediumCar : IMediumCar
    {
        public string Name { get; set; }
    }


    public interface ILargeCar
    {
        string Name { get; set; }
    }

    public class LargeCar : ILargeCar
    {
        public string Name { get; set; }
    }
}
