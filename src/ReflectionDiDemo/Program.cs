using System;

namespace ReflectionDiDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new SimpleDependencyInjection())
            {
                container.RegisterInterface<ISmallCar>(CreationRules.Static);
                container.RegisterInterface<IMediumCar>(CreationRules.Transient);
                container.RegisterType<ILargeCar, LargeCar>(CreationRules.Static);

                var car = container.Get<ILargeCar>();
                car.Name = "this is a large car";

                var carRef = container.Get<ILargeCar>();

                Console.WriteLine("value : " + carRef.Name);
            }
        }
    }
}
