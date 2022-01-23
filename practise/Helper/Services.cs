using Autofac;
using practise.IRepository;
using practise.Repository;

namespace practise.Helper
{
    public class Services : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<Person>().As<IPerson>();
            builder.RegisterType<Login>().As<ILogin>();
            builder.RegisterType<Repository.Products>().As<IProduct>();
            builder.RegisterType<HashService>();
        }
    }
}
