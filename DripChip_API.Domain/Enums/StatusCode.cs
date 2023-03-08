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
    LocationAlreadyExist = 302,
    LocationRelated = 303,
    TypeAlreadyExist = 304,
    TypeRelated = 305,
    
    AccountNotCreated = 400,
    AccountNotDeleted = 401,
    
    AnimalLeft = 701,
    
    OK = 200,
    
    AuthorizationDataIsEmpty = 501,
    
    ServerError = 500,
    
    Invalid = 600,
    NotFound = 601,
    Conflict = 602
}