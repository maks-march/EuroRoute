namespace Application.Common.Exceptions;

public class ForbiddenException(string name, Guid userId)
    : Exception($"Entity '{name}' not belong to user with Id {userId}");