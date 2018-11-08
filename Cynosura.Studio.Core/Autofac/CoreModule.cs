using System;
using System.Linq;
using Autofac;
using Cynosura.Studio.Core.Infrastructure;
using FluentValidation;
using MediatR;

namespace Cynosura.Studio.Core.Autofac
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
            RegisterAllRequestHandlers(builder);
            builder.RegisterGeneric(typeof(RequestValidationBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            RegisterAllValidators(builder);
        }

        private void RegisterAllRequestHandlers(ContainerBuilder builder)
        {
            var handlerTypes = new [] { typeof(IRequestHandler<>), typeof(IRequestHandler<,>) };
            var handlers = typeof(CoreModule).Assembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Any(i => handlerTypes.Any(ht => ht.Name == i.Name)))
                .ToList();

            foreach (var handler in handlers)
            {
                builder.RegisterType(handler).AsImplementedInterfaces();
            }
        }

        private void RegisterAllValidators(ContainerBuilder builder)
        {
            var validatorTypes = new[] { typeof(IValidator<>) };
            var validators = typeof(CoreModule).Assembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Any(i => validatorTypes.Any(ht => ht.Name == i.Name)))
                .ToList();

            foreach (var validator in validators)
            {
                builder.RegisterType(validator).AsImplementedInterfaces();
            }
        }
    }
}
