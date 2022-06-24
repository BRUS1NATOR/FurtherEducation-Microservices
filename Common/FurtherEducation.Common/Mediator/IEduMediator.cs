using FurtherEducation.Common.Commands;
using FurtherEducation.Common.CQRS.Commands;
using FurtherEducation.Common.CQRS.Queries;
using System.Threading.Tasks;

namespace FurtherEducation.Common.Mediator
{
    public interface IEduMediator
    {
        public Task<CommandResponse> SendCommand(ICommand command);
        public Task<QueryResponse<Response>> GetBaseQuery<Response>(IQuery request) where Response : class;
        public Task<Response?> GetQuery<Response>(IQuery request) where Response : class;
        public Task<Response?> GetQueryNotNull<Response>(IQuery request) where Response : class;
    }
}