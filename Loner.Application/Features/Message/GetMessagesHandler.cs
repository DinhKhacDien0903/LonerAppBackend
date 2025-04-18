using Loner.Application.DTOs;
using static Loner.Application.DTOs.Message;

namespace Loner.Application.Features.Message
{
    public class GetMessagesHandler : IRequestHandler<GetMessagesRequest, Result<GetMessagesResponse>>
    {
        private readonly IUnitOfWork _uow;
        public GetMessagesHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<GetMessagesResponse>> Handle(GetMessagesRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.PaginationRequest.MatchId))
                return Result<GetMessagesResponse>.Failure("Unauthorized. Need MatchId paramater!"); 
            if (string.IsNullOrEmpty(request.PaginationRequest.UserId))
                return Result<GetMessagesResponse>.Failure("Unauthorized. Need UserId paramater!");

            var messages = await _uow.MessageRepository.GetMessagesPaginatedByMatchIdAsync(request.PaginationRequest.MatchId,
                request.PaginationRequest.ValidPageNumber, request.PaginationRequest.ValidPageSize);

            List<MessageDetailDto> results = new();
            results = [.. messages.Select(x => new MessageDetailDto
            {
                Id = x.Id,
                IsCurrentUserSend = x.SenderId.Equals(request.PaginationRequest.UserId),
                IsImage = x.IsImage,
                Content = x.Content,
                SendTime = x.CreatedAt
                //TODO: set IsRead.
            })];

            return Result<GetMessagesResponse>.Success(new GetMessagesResponse
            (
                new PaginatedResponse<MessageDetailDto>
                {
                    TotalItems = await _uow.MessageRepository.GetTotalRecordByMatchIdAsync(request.PaginationRequest.MatchId),
                    PageSize = request.PaginationRequest.ValidPageSize,
                    PageNumber = request.PaginationRequest.ValidPageNumber,
                    Items = results
                }
            ));
        }
    }
}