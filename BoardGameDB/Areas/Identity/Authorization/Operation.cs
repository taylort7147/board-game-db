using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BoardGameDB.Areas.Identity.Authorization
{
    public static class Operation
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement { Name = OperationName.Create };
        public static OperationAuthorizationRequirement View =
            new OperationAuthorizationRequirement { Name = OperationName.View };
        public static OperationAuthorizationRequirement Edit =
            new OperationAuthorizationRequirement { Name = OperationName.Edit };
        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = OperationName.Delete };
    }

    public static class OperationName
    {
        public const string Create = "Create";
        public const string View = "View";
        public const string Edit = "Edit";
        public const string Delete = "Delete";
    }
}