using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Identity;

public class IdentityContext : IdentityDbContext
{
	// Have to have this class or Identity Complains
	public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }
}
