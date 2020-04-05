using Autofac;

namespace Dapper.Conventions.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ConfigureContainer();
            
            //init app 
            container.Resolve<IApplication>().Run();
        }

        public static IContainer ConfigureContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacModule());
            return builder.Build();
        }
    }
}
