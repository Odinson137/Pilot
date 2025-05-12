using MassTransit;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Commands;
using Pilot.SqrsControllerLibrary.Notifications;

namespace Pilot.Capability.Saga
{
    public class JobApplicationSaga : MassTransitStateMachine<JobApplicationSagaState>
    {
        public State AwaitingEmployeeCreation { get; private set; }
        // public State AwaitingNotification { get; private set; }

        public Event<UpdateCommand<JobApplicationDto>> JobApplicationUpdatedEvent { get; private set; }
        public Event<CreateCommand<CompanyUserDto>> EmployeeCreatedEvent { get; private set; }
        public Event<Fault<CreateCommand<CompanyUserDto>>> EmployeeCreationFailedEvent { get; private set; }
        public Event<MessageSentNotification> NotificationSentEvent { get; private set; }
        // public Event<NotificationFailed> NotificationFailedEvent { get; private set; }

        public JobApplicationSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => JobApplicationUpdatedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => EmployeeCreatedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => EmployeeCreationFailedEvent, x => x.CorrelateById(context => context.CorrelationId ?? Guid.NewGuid()));
            Event(() => NotificationSentEvent, x => x.CorrelateById(context => context.CorrelationId ?? Guid.NewGuid()));
            // Event(() => NotificationFailedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

            Initially(
                    When(JobApplicationUpdatedEvent)
                        .Then(context =>
                        {
                            context.Saga.JobApplicationId = context.Message.ValueDto.Id;
                            context.Saga.UserId = context.Message.ValueDto.UserId;
                            context.Saga.ChangerId = context.Message.UserId;
                            context.Saga.PostId = context.Message.ValueDto.CompanyPost.Id;
                            context.Saga.CurrentStatus = context.Message.ValueDto.Status;
                            context.Saga.PreviousStatus = ApplicationStatus.Reviewing; // Предположительно такой
                            context.Saga.CorrelationId = context.Message.CorrelationId;
                        })
                        .If(context => context.Message.ValueDto.Status == ApplicationStatus.Approved,
                            binder => binder
                                .Publish(context => new CreateCommand<CompanyUserDto>(new CompanyUserDto
                                {
                                    PostId = context.Saga.PostId,
                                    UserId = context.Saga.UserId,
                                }, context.Saga.ChangerId, context.Saga.CorrelationId)))
                        .TransitionTo(AwaitingEmployeeCreation)
                .Finalize());

            During(AwaitingEmployeeCreation,
                When(EmployeeCreationFailedEvent)
                    .Publish(context => new RevertStatusCommand(
                        context.Saga.JobApplicationId,
                        context.Saga.PreviousStatus,
                        context.Saga.CorrelationId))
                    .Publish(context => new SendNotificationCommand(
                        context.Saga.UserId,
                        context.Saga.PostId,
                        ApplicationStatus.Rejected, // Уведомление об отмене
                        context.Saga.CorrelationId))
                    .Publish(context => new DeleteCommand<CompanyUserDto>(
                        0, context.Saga.ChangerId, context.Saga.CorrelationId))
                    .Finalize());

            // During(AwaitingNotification,
            //     When(NotificationSentEvent)
            //         .Finalize(),
            //     When(NotificationFailedEvent)
            //         .Publish(context => new RevertStatusCommand(
            //             context.Saga.JobApplicationId,
            //             context.Saga.PreviousStatus,
            //             context.Saga.CorrelationId))
            //         .Finalize());

            SetCompletedWhenFinalized();
        }
    }

    public class JobApplicationSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public int JobApplicationId { get; set; }
        public int UserId { get; set; }
        public int ChangerId { get; set; }
        public int PostId { get; set; }
        public ApplicationStatus CurrentStatus { get; set; }
        public ApplicationStatus PreviousStatus { get; set; }
    }
}