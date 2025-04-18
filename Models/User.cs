﻿namespace BreadcrumbsAPI.Models;

public class User : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public virtual ICollection<GroupUserRelationship>? GroupUserRelationships { get; set; }
}
