using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.ServiceSetup;

public static class IdentitySetup
{
    public static void AddAndSetupIdentity(this WebApplicationBuilder builder)
    {
        // Identity
        builder.Services.AddDbContext<IdentityDbContext>(opts =>
        {
            opts.UseSqlServer(builder.Configuration["ConnectionStrings:TodoApiIdentity"],
                opts => opts.MigrationsAssembly("TodoApi")
                );
        });

        builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts =>
        {
            opts.SignIn.RequireConfirmedAccount = false;
        }).AddEntityFrameworkStores<IdentityDbContext>()
        .AddDefaultTokenProviders();
    }
}
