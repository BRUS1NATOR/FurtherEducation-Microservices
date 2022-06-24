using FurtherEducation.Common.Commands;
using FurtherEducation.Common.CQRS.Commands;
using FurtherEducation.Common.CQRS.Queries;
using FurtherEducation.Common.Helpers;
using MassTransit.Mediator;
using System.Threading.Tasks;

namespace FurtherEducation.Common.Mediator
{
    public class EduMediator : IEduMediator
    {
        IMediator _mediator;

        public EduMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CommandResponse> SendCommand(ICommand command)
        {
            var response = await _mediator.CreateRequestClient<ICommand>().GetResponse<CommandResponse>(command);
            MediatorHelper.IsMessageConsumptionFailed(response.Message?.Message);

            return response.Message;
        }

        public async Task<QueryResponse<Response>> GetBaseQuery<Response>(IQuery query) where Response : class
        {
            var response = await _mediator.CreateRequestClient<IQuery>().GetResponse<QueryResponse<Response>>(query);
            return response.Message;
        }

        public async Task<Response?> GetQuery<Response>(IQuery query) where Response : class
        {
            var response = await GetBaseQuery<Response>(query);

            MediatorHelper.IsMessageConsumptionFailed(response.Message);

            return response.Data;
        }

        public async Task<Response?> GetQueryNotNull<Response>(IQuery request) where Response : class
        {
            var response = await GetBaseQuery<Response>(request);

            MediatorHelper.IsMessageConsumptionFailedOrNotFound(response.Message);

            return response.Data;
        }
    }
}
