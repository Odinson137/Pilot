using Pilot.Contracts.Data.Enums;

namespace Pilot.SqrsControllerLibrary.Commands;

public record ApprovedJobApplicationCommand(int JobApplicationId, int CompanyId, int UserId, int PostId, int ChangerUserId, Guid CorrelationId);

public record RevertJobApplicationCommand(int CompanyId, int UserId, int PostId, int ChangerUserId);

public record SendNotificationCommand(int UserId, int PostId, ApplicationStatus Status, Guid CorrelationId);

public record RevertStatusCommand(int JobApplicationId, int ChangerUserId, int UserId, int PostId, Guid CorrelationId);