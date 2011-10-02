using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.Providers;
using MyJobLeads.DomainModel.Entities;
using MyJobLeads.DomainModel.ProcessParams.Security;
using MyJobLeads.DomainModel.ViewModels.Authorizations;

namespace MyJobLeads.DomainModel.Queries.Positions
{
    /// <summary>
    /// Query object that returns a position by it's ID value
    /// </summary>
    public class PositionByIdQuery
    {
        protected IServiceFactory _ServiceFactory;
        protected IProcess<PositionAuthorizationParams, AuthorizationResultViewModel> _positionAuthProcess;
        protected int _posId, _userId;

        public PositionByIdQuery(IServiceFactory iServiceFactory, IProcess<PositionAuthorizationParams, AuthorizationResultViewModel> positionAuthProcess)
        {
            _ServiceFactory = iServiceFactory;
            _positionAuthProcess = positionAuthProcess;
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
        /// Specifies the id value of the user calling the query
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public PositionByIdQuery RequestedByUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        /// <summary>
        /// Executes the query
        /// </summary>
        /// <returns></returns>
        public Position Execute()
        {
            // Make sure the user is authorized for the position
            if (!_positionAuthProcess.Execute(new PositionAuthorizationParams { RequestingUserId = _userId, PositionId = _posId }).UserAuthorized)
                return null;

            var unitOfWork = _ServiceFactory.GetService<IUnitOfWork>();
            return unitOfWork.Positions.Fetch().Where(x => x.Id == _posId).SingleOrDefault();
        }
    }
}
