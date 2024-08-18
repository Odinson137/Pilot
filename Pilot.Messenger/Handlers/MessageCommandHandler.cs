using AutoMapper;
using MediatR;
using Pilot.Messenger.Interfaces;

namespace Pilot.Messenger.Handlers;

// public class MessageCommandHandler :
//     IRequestHandler<?>
// {
//     private readonly IMessageRepository _messageRepository;
//     private readonly ILogger<MessageCommandHandler> _logger;
//     private readonly IMapper _mapper;
//     
//     public MessageCommandHandler(ILogger<MessageCommandHandler> logger, IMapper mapper, IMessageRepository messageRepository)
//     {
//         _logger = logger;
//         _mapper = mapper;
//         _messageRepository = messageRepository;
//     }
//
//     public async Task Handle(MessageUserAddCommand request, CancellationToken token)
//     {
//
//         throw new Exception();
//     }
// }