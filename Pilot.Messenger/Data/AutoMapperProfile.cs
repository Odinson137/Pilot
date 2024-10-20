using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Data;

public class AutoMapperProfile : BaseMappingProfile
{
    public AutoMapperProfile()
    {
        BaseMap();
        
        CreateMap<BaseModel, BaseDto>();
        CreateMap<BaseDto, BaseModel>();

        Mapping<InfoMessage, InfoMessageDto>();
        Mapping<Message, MessageDto>();
        Mapping<Chat, ChatDto>();
        Mapping<ChatMember, ChatMemberDto>();
    }
}