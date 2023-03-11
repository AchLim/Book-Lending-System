// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Book_Lending_System.Models;

/// <summary>
/// Represents the link between a user and a role.
/// </summary>
/// <typeparam name="TKey">The type of the primary key used for users and roles.</typeparam>
public class AccountRole : IdentityUserRole<string>
{

    [DeleteBehavior(DeleteBehavior.ClientCascade)]
    public virtual Account Account { get; set; } = default!;

    [DeleteBehavior(DeleteBehavior.ClientCascade)]
    public virtual Role Role { get; set; } = default!;

}