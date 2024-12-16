using AutoMapper;
using Pilot.BackgroundJob.Data;
using Pilot.BackgroundJob.Interface;
using Pilot.BackgroundJob.Models;
using Pilot.Contracts.Base;

namespace Pilot.BackgroundJob.Repository;

public class ChatReminderRepository(DataContext context, IMapper mapper) : BaseRepository<ChatReminder>(context, mapper), IChatReminder
{

}