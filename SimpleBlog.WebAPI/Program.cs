
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SimpleBlog.WebAPI.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SimpleBlog.WebAPI.Services;
using SimpleBlog.WebAPI.Entites;
using SimpleBlog.WebAPI.Repositories.Base;
using SimpleBlog.WebAPI.Middlewares;
using SimpleBlog.WebAPI.Repositories.PostRepo;

namespace SimpleBlog.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IRepository<Tag>, Repository<Tag>>();
            builder.Services.AddScoped<IRepository<Media>, Repository<Media>>();
            builder.Services.AddScoped<IRepository<Post>, Repository<Post>>();
            builder.Services.AddScoped<IPostRepository,PostRepository>();

            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<TagService>();
            builder.Services.AddScoped<MediaService>();
            builder.Services.AddScoped<PostService>();

         

            builder.Services.AddDbContext<SimpleBlogDbContext>(options =>
                                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<SimpleBlogDbContext>()
                            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // Ensures JWT is used everywhere
            })
                            .AddJwtBearer(options =>
                            {
                                options.RequireHttpsMetadata = true;
                                options.SaveToken = true;
                                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                                {
                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                                    ValidateIssuer = false,
                                    ValidateAudience = false,
                                    //ValidIssuer = builder.Configuration["Jwt:Issuer"],
                                    //ValidAudience = builder.Configuration["Jwt:Audience"],
                                    ClockSkew = TimeSpan.Zero,
                                    ValidateLifetime = true
                                };
                            });
            builder.Services.AddAuthorization();


            var SimpleBlogMVCOrigins = "_SimpleBlogMVCOrigins";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: SimpleBlogMVCOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("")
                                            .AllowAnyMethod()
                                            .AllowAnyHeader();
                                  });
            });

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

           
            app.UseHttpsRedirection();
            app.UseCors(SimpleBlogMVCOrigins);
            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllers();

            app.Run();
        }
    }
}
