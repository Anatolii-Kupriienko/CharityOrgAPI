using System.ComponentModel.DataAnnotations;

public record UpsertSupportDirectionRequest
(
    int? Id,
    string Name,
    string Description,
    string About);
