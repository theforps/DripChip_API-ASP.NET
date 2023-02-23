namespace DripChip_API.Domain.Enums;

public enum StatusCode
{
    AccountNotFound = 100,
    AnimalNotFound = 101,
    TypeNotFound = 102,
    LocationNotFound = 103,
    LocationStoryNotFound = 104,
    
    AccountIsNotExists = 300,
    AccountExists = 301,
    
    AccountNotCreated = 400,
    AccountNotDeleted = 401,
    
    OK = 200,
    
    AuthorizationDataIsEmpty = 501,
    
    ServerError = 500,
}