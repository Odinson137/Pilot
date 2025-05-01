using AutoMapper;
using MassTransit;
using Pilot.Capability.Consumers.Base;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.DTO.TransferServiceDto;
using Pilot.Contracts.Interfaces;
using Pilot.SqrsControllerLibrary.RabbitMqMessages;

namespace Pilot.Capability.Consumers.JobApplicationConsumer;

public class JobApplicationUpdatedConsumer(
    ILogger<JobApplicationUpdatedConsumer> logger,
    IJobApplication repository,
    IMessageService message,
    IBaseMassTransitService massTransitService,
    IValidatorService validate,
    IMapper mapper)
    : BaseUpdateConsumer<JobApplication, JobApplicationDto>(logger, repository, message, validate, mapper)
{
    public override async Task Consume(ConsumeContext<UpdateCommandMessage<JobApplicationDto>> context)
    {
        await base.Consume(context);

        var jobApplicationDto = context.Message.Value;
        switch (jobApplicationDto.Status)
        {
            case ApplicationStatus.Approved:
            {
                var companyAndPost =
                    await repository.GetJobApplicationCompanyAndPostIdsAsync(jobApplicationDto.CompanyPost.Id);
                if (companyAndPost == null) throw new Exception("Job Application Not Found");

                await massTransitService.Publish(new BaseCommandMessage<AppliedStatusDto>(new AppliedStatusDto
                    {
                        CompanyId = companyAndPost!.Item1,
                        UserId = context.Message.Value.UserId,
                        PostId = companyAndPost.Item2,
                    },
                    context.Message.UserId));
                break;
            }
            case ApplicationStatus.Reviewing:
            // {
            //     await massTransitService.Publish(new BaseCommandMessage<ReviewingStatusDto>(new ReviewingStatusDto
            //         {
            //             ApplicationCreatorUserId = context.Message.Value.UserId
            //         },
            //         context.Message.UserId));
            //     break;
            // }
            case ApplicationStatus.Pending:
            case ApplicationStatus.Canceled:
            case ApplicationStatus.Rejected:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}