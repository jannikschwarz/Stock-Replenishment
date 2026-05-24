
namespace Replenishment.Domain.Enums;

public enum RequestStatus
{
    Draft,
    Submitted,
    Approved,
    Rejected,
    Fulfilled
}

public enum RequestPriority
{
    LOW,
    MEDIUM,
    Urgent
}

public enum Building
{
    HallA,
    HallB,
    HallC,
    AssemblyHO1,
    AssemblyHO2,
}