using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Entities;

namespace MyJobLeads.DomainModel.Queries.Positions
{
    /// <summary>
    /// Query object that returns a position by it's ID value
    /// </summary>
    public class PositionByIdQuery
    {
        protected IServiceFactory _ServiceFactory;
        protected int _posId;

        public PositionByIdQuery(IServiceFactory iServiceFactory)
        {
            _ServiceFactory = iServiceFactory;
        }
        
        /// <summary>
        /// Specifies the id value of the position to retrieve 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public PositionByIdQuery WithPositionId(int id)
        {
            _posId = id;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public Position Execute()
        {
            var unitOfWork = _ServiceFactory.GetService<IUnitOfWork>();
            return unitOfWork.Positions.Fetch().Where(x => x.Id == _posId).SingleOrDefault();
        }
    }
}
