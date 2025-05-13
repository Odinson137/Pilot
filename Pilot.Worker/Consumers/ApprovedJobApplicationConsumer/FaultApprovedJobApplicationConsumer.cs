using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pilot.SqrsControllerLibrary.Commands;
using Pilot.SqrsControllerLibrary.Interfaces;
using Pilot.Worker.Interface;

namespace Pilot.Worker.Consumers.ApprovedJobApplicationConsumer;

public class FaultApprovedJobApplicationConsumer(
    ILogger<FaultApprovedJobApplicationConsumer> logger,
    ICompanyUser companyUser,
    IUnitOfWork unitOfWork,
    IMediator mediator) : IConsumer<Fault<ApprovedJobApplicationCommand>>
{
    public async Task Consume(ConsumeContext<Fault<ApprovedJobApplicationCommand>> context)
    {
        logger.LogInformation($"{nameof(FaultApprovedJobApplicationConsumer)} handle consume");

        var model = context.Message.Message;
        var companyUserExist = await companyUser.DbSet.FirstOrDefaultAsync(c =>
            c.UserId == model.UserId && c.PostId == model.PostId && c.DeleteAt != null);

        if (companyUserExist == null) return;

        companyUser.Delete(companyUserExist);
        await unitOfWork.SaveChangesAsync(context.CancellationToken);

        await mediator.Publish(
            new RevertStatusCommand(model.JobApplicationId, model.ChangerUserId, model.UserId, model.PostId,
                context.CorrelationId!.Value), context.CancellationToken);
    }
}