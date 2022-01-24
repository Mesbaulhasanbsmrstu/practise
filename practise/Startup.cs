using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using practise.ErrorHandling;
using practise.Helper;
using practise.IRepository;
using practise.Model;
using practise.Repository;
using Serilog;
using System;
using System.Net;
using System.Text;

namespace practise
{
    public class Startup
    {
        private readonly IHostEnvironment _env;
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));
            services.AddHttpContextAccessor();
            services.AddCors();
            services.AddDataProtection();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddDbContext<practiseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Development")), ServiceLifetime.Transient);
            //Connection.PRACTISE= Configuration.GetConnectionString("Development");
            Connection.PRACTISE = Configuration["ConnectionStrings:Development"];
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title="My Api",
                    Description="This is a web API for practise operation",
                    TermsOfService =new Uri("https://www.euromoney.com/learning/blockchain-explained/how-transactions-get-into-the-blockchain"),
                    License=new OpenApiLicense()
                    {
                        Name="MIT"
                    },
                    Contact=new OpenApiContact()
                    {
                        Name="Mesba",
                        Email="mesbaulhasanbsmrstu@gmail.com",

                    }
                });

                // To Enable authorization using Swagger (JWT)
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter ‘Bearer’ [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                        });
                   swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                        {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                        {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                        }
                        },
                        new string[] {}
                        }
                        });
                                });
           // JwtConfiguration(services);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Audience:Iss"],
                    ValidAudience = Configuration["Audience:Iss"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Audience:Secret"]))
                };
            });
            //services.AddAutofac();
            services.AddMvc();
            // services.AddMediatR(typeof(Startup));
            //RegisterServices(services);

            /*services.AddControllers(opts =>
            {
                if (_env.IsDevelopment())
                {
                    // opts.Filters.Add<AllowAnonymousFilter>();
                    var authenticatedUserPolicy = new AuthorizationPolicyBuilder()
                   .RequireAuthenticatedUser()
                   .Build();
                    opts.Filters.Add(new AuthorizeFilter(authenticatedUserPolicy));
                }
                else
                {
                    var authenticatedUserPolicy = new AuthorizationPolicyBuilder()
                              .RequireAuthenticatedUser()
                              .Build();
                    opts.Filters.Add(new AuthorizeFilter(authenticatedUserPolicy));
                }
            
            });*/

            services.AddControllers(mvcOptions =>
              mvcOptions.EnableEndpointRouting = false);

            

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
            // services.AddTransient<IPerson, Person>();
            /* services.AddScoped<IPerson, Person>();
             services.AddScoped<ILogin, Login>();
             services.AddScoped<IProduct,Repository.Products>();
             services.AddScoped<HashService>();

             var builder = new ContainerBuilder();
             ConfigureContainer(builder);
             builder.RegisterType<Person>().As<IPerson>();*/
           // services.AddScoped<HashService>();
            //var builder = new ContainerBuilder();
           // builder.RegisterModule<Services>();
           // ConfigureContainer(builder);
           // builder.Populate(services);


            //var container = builder.Build();
            //return new AutofacServiceProvider(container);

        }
       public void ConfigureContainer(ContainerBuilder builder)
        {
            #region === Services ===
            builder.RegisterModule<Services>();
            /*builder.RegisterType<Person>().As<IPerson>();
            builder.RegisterType<Login>().As<ILogin>();
            builder.RegisterType<Repository.Products>().As<IProduct>();
            builder.RegisterType<HashService>();*/


            #endregion
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // Security Header

            var policyCollection = new HeaderPolicyCollection()
                .AddXssProtectionBlock()
                .AddContentTypeOptionsNoSniff()
                .AddExpectCTNoEnforceOrReport(0)
                .AddStrictTransportSecurityMaxAgeIncludeSubDomains(maxAgeInSeconds: 60 * 60 * 24 * 365) // maxage = one year in seconds
                .AddReferrerPolicyStrictOriginWhenCrossOrigin()
                .AddContentSecurityPolicy(builder =>
                {
                    builder.AddUpgradeInsecureRequests();
                    builder.AddDefaultSrc().Self();
                    builder.AddConnectSrc().From("*");
                    builder.AddFontSrc().From("*");
                    builder.AddFrameAncestors().From("*");
                    builder.AddFrameSource().From("*");
                    builder.AddWorkerSrc().From("*");
                    builder.AddMediaSrc().From("*");
                    builder.AddImgSrc().From("https://erp.ibos.io").Data();
                    builder.AddObjectSrc().From("*");
                    builder.AddScriptSrc().From("*").UnsafeInline().UnsafeEval();
                    builder.AddStyleSrc().From("*").UnsafeEval().UnsafeInline();
                })
                .RemoveServerHeader();

           app.UseSecurityHeaders(policyCollection);


            
            loggerFactory.AddSerilog();
          
            app.UseCors(Options => Options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
              }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();
           /* app.UseCors(x => x
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin => true) // allow any origin
              .AllowCredentials());*/
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://apirequest.io"));
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseCors(builder => builder.WithOrigins("https://apirequest.io/").WithMethods("GET", "POST").AllowAnyHeader());
            app.ConfigureCustomExceptionMiddleware();
            app.UseMvc();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API ");
            });

        }

        private void JwtConfiguration(IServiceCollection services)
        {
            //throw new NotImplementedException();
            var audienceConfig = Configuration.GetSection("Audience");
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Audience:Secret"]));
            // var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audienceConfig["Secret"]));
            // var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_env.IsProduction() ? Configuration.GetSection("REACT_APP_SECRET_NAME").Value.Trim() : audienceConfig["Secret"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = audienceConfig["Iss"],
                
                ValidAudience = audienceConfig["Aud"],

                IssuerSigningKey =signingKey
                
            };
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
           {
               x.RequireHttpsMetadata = false;
               x.TokenValidationParameters = tokenValidationParameters;
           });


        }
    }
}
