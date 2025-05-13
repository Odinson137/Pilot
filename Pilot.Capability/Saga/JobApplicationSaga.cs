// using MassTransit;
// using Pilot.Capability.Interface;
// using Pilot.Capability.Models;
// using Pilot.Contracts.Base;
// using Pilot.Contracts.Data.Enums;
// using Pilot.Contracts.DTO.ModelDto;
// using Pilot.Contracts.Services;
// using Pilot.SqrsControllerLibrary.Commands;
// using Pilot.SqrsControllerLibrary.Notifications;
//
// namespace Pilot.Capability.Saga;
//
// public class JobApplicationSaga : MassTransitStateMachine<JobApplicationSagaState>
// {
//     private readonly ILogger<JobApplicationSaga> _logger;
//     private readonly IJobApplication _application;
//
//     public State AwaitingEmployeeCreation { get; private set; }
//     public Schedule<JobApplicationSagaState, TimeoutExpired> Timeout { get; private set; }
//
//     public Event<UpdateCommand<JobApplicationDto>> JobApplicationUpdatedEvent { get; private set; }
//     public Event<CreateCommand<CompanyUserDto>> EmployeeCreatedEvent { get; private set; }
//     public Event<Fault<CreateCommand<CompanyUserDto>>> EmployeeCreationFailedEvent { get; private set; }
//
//     public JobApplicationSaga(ILogger<JobApplicationSaga> logger, IJobApplication application)
//     {
//         _logger = logger;
//         _application = application;
//
//         InstanceState(x => x.CurrentState);
//
//         Event(() => JobApplicationUpdatedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
//         Event(() => EmployeeCreatedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
//         Event(() => EmployeeCreationFailedEvent, x => x.CorrelateById(context => context.Message.Message.CorrelationId));
//
//         Schedule(() => Timeout, x => x.TimeoutId, s =>
//         {
//             s.Delay = TimeSpan.FromMinutes(5);
//             s.Received = r => r.CorrelateById(context => context.Message.CorrelationId);
//         });
//
//         Initially(
//             When(JobApplicationUpdatedEvent)
//                 .ThenAsync(async context =>
//                 {
//                     _logger.LogInformation("Processing JobApplicationUpdatedEvent for ID {JobApplicationId}", context.Message.ValueDto.Id);
//                     context.Saga.JobApplicationId = context.Message.ValueDto.Id;
//                     context.Saga.UserId = context.Message.ValueDto.UserId;
//                     context.Saga.ChangerId = context.Message.UserId;
//                     context.Saga.PostId = context.Message.ValueDto.CompanyPost.Id;
//                     var companyAndPost = await _application.GetJobApplicationCompanyAndPostIdsAsync(context.Saga.PostId);
//                     if (companyAndPost == null) throw new Exception("Company not found for PostId");
//                     context.Saga.CompanyId = companyAndPost.Item1;
//                     context.Saga.CurrentStatus = context.Message.ValueDto.Status;
//                     context.Saga.PreviousStatus = ApplicationStatus.Reviewing;
//                     context.Saga.CorrelationId = context.Message.CorrelationId;
//                 })
//                 .If(context => context.Saga.CurrentStatus == ApplicationStatus.Approved,
//                     binder => binder
//                         .Schedule(Timeout, context => new TimeoutExpired { CorrelationId = context.Saga.CorrelationId })
//                         .Publish(context => new CreateCommand<CompanyUserDto>(
//                             new CompanyUserDto
//                             {
//                                 UserId = context.Saga.UserId,
//                                 Company = new BaseDto { Id = context.Saga.CompanyId },
//                                 PostId = context.Saga.PostId
//                             }, context.Saga.ChangerId, context.Saga.CorrelationId)))
//                 .TransitionTo(AwaitingEmployeeCreation)
//                 .Finalize());
//
//         During(AwaitingEmployeeCreation,
//             When(EmployeeCreatedEvent)
//                 .Unschedule(Timeout)
//                 .Publish(context => new MessageSentNotification
//                 {
//                     Message = new InfoMessageDto
//                     {
//                         MessagePriority = MessageInfo.Success | MessageInfo.Create,
//                         EntityType = PilotEnumExtensions.GetModelEnumValue<JobApplication>(),
//                         EntityId = context.Message.ValueDto.Id
//                     },
//                     UserId = context.Saga.ChangerId
//                 })
//                 .Finalize(),
//             When(EmployeeCreationFailedEvent)
//                 .Unschedule(Timeout)
//                 .Publish(context => new RevertStatusCommand(
//                     context.Saga.JobApplicationId,
//                     context.Saga.PreviousStatus,
//                     context.Saga.CorrelationId))
//                 .Publish(context => new MessageSentNotification
//                 {
//                     Message = new InfoMessageDto
//                     {
//                         MessagePriority = MessageInfo.Error,
//                         EntityType = PilotEnumExtensions.GetModelEnumValue<JobApplication>(),
//                         EntityId = context.Saga.JobApplicationId,
//                         // Message = "Operation canceled due to failure in creating employee"
//                     },
//                     UserId = context.Saga.ChangerId
//                 })
//                 .Finalize(),
//             When(Timeout.Received)
//                 .Publish(context => new RevertStatusCommand(
//                     context.Saga.JobApplicationId,
//                     context.Saga.PreviousStatus,
//                     context.Saga.CorrelationId))
//                 .Publish(context => new MessageSentNotification
//                 {
//                     Message = new InfoMessageDto
//                     {
//                         MessagePriority = MessageInfo.Error,
//                         EntityType = PilotEnumExtensions.GetModelEnumValue<JobApplication>(),
//                         EntityId = context.Saga.JobApplicationId,
//                         // Message = "Operation canceled due to timeout"
//                     },
//                     UserId = context.Saga.ChangerId
//                 })
//                 .Finalize());
//
//         SetCompletedWhenFinalized();
//     }
// }
//
// public class JobApplicationSagaState : SagaStateMachineInstance
// {
//     public Guid CorrelationId { get; set; }
//     public string CurrentState { get; set; } = null!;
//     public int JobApplicationId { get; set; }
//     public int UserId { get; set; }
//     public int ChangerId { get; set; }
//     public int PostId { get; set; }
//     public int CompanyId { get; set; }
//     public ApplicationStatus CurrentStatus { get; set; }
//     public ApplicationStatus PreviousStatus { get; set; }
//     public Guid? TimeoutId { get; set; }
// }
//
// public class TimeoutExpired
// {
//     public Guid CorrelationId { get; set; }
// }