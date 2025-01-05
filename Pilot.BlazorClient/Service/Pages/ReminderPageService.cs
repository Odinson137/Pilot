using AutoMapper;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BlazorClient.Service.Pages;

public class ReminderPageService(IGateWayApiService apiService, IMapper mapper) 
    : BaseModelService<ChatReminderDto, ChatReminderViewModel>(apiService, mapper), IReminderPageService
{
   
}