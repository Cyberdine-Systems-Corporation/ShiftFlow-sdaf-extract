using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShiftFlow.Infrastructure.Identity;

namespace ShiftFlow.Infrastructure.Persistence;

public sealed class ShiftFlowDbContext(DbContextOptions<ShiftFlowDbContext> options)
    : IdentityDbContext<ApplicationUser>(options);
