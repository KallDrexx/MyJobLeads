using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using Castle.Windsor;

namespace MyJobLeads.Infrastructure.Providers
{
    public class WindsorValidatorFactory : ValidatorFactoryBase
    {
        protected IWindsorContainer _container;

        public WindsorValidatorFactory(IWindsorContainer container)
        {
            _container = container;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return _container.Resolve(validatorType) as IValidator;
        }
    }
}