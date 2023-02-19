namespace DripChip_API.Domain.Enums;

public enum StatusCode
{
    AccountNotFound = 100,
    AnimalNotFound = 101,
    TypeNotFound = 102,
    LocationNotFound = 103,
    LocationStoryNotFound = 104,
    
    AccountExists = 301,
    
    OK = 200,
    
    ServerError = 500,
}