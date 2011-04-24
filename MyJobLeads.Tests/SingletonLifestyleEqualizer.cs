using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.ModelBuilder;
using Castle.MicroKernel;
using Castle.Core;

namespace MyJobLeads.Tests
{
    /// <summary>
    /// Used to set the lifestyle of all components created by Windsor to Singleton instead of PerWebRequest (for testing purposes only)
    /// </summary>
    public class SingletonLifestyleEqualizer : IContributeComponentModelConstruction
    {
        public void ProcessModel(IKernel kernel, ComponentModel model)
        {
            model.LifestyleType = LifestyleType.Singleton;
        }
    }
}
