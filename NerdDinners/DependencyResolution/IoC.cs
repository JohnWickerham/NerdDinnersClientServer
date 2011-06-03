using NerdDinners.Services;
using StructureMap;
namespace NerdDinners {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                    });
                            x.For<INerdDinnerService>().Use<NerdDinnerService>();
                        });
            return ObjectFactory.Container;
        }
    }
}