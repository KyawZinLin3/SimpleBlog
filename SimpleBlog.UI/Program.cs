using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using SimpleBlog.UI.Models;
using SimpleBlog.UI.Services;

namespace SimpleBlog.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure authentication in MVC
            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login"; 
                    options.AccessDeniedPath = "/Auth/AccessDenied";
                });
         
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
            builder.Services.AddHttpClient("ApiClient",(serviceProvider, client) =>
            {
                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                var httpContext = httpContextAccessor.HttpContext;

                if (httpContext != null && httpContext.Request.Cookies.TryGetValue("AuthToken", out var token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
            });
            builder.Services.AddHttpContextAccessor();
            //builder.Services.AddHttpClient<ProductService>();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<TagService>();
            builder.Services.AddScoped<PostService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.Use(async (context, next) =>
            //{
            //    context.Response.OnStarting(() =>
            //    {
            //        if (context.Response.StatusCode == 401)
            //        {
            //            context.Response.Redirect("/Auth/Login");
            //        }
            //        else if (context.Response.StatusCode == 403)
            //        {
            //            context.Response.Redirect("/Auth/AccessDenied");
            //        }
            //        else if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
            //        {
            //            context.Response.Redirect("/Home/Error");
            //        }

            //        return Task.CompletedTask;
            //    });

            //    await next(); 
            //});

          


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("Middleware caught unauthorized");
                    context.Response.Redirect("/Auth/Login");
                }
                if (context.Response.StatusCode == 401)
                {
                    Console.WriteLine("Get Unauthorize let go");
                    context.Response.Redirect("/Auth/Login");
                }
                else if (context.Response.StatusCode == 403)
                {
                    context.Response.Redirect("/Auth/AccessDenied");
                }
                else if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                {
                    context.Response.Redirect("/Home/Error");
                }
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
