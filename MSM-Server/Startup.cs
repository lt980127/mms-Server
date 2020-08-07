using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mms_Server.Common;
using MSM_Server.Utility;

namespace MSM_Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<LogIActionFilterAttribute>();

            DapperHelper._connectionString = Configuration.GetSection("DbConfig:ConnectionString").Value;

            #region Jwt配置
            //将appsettings.json中的JwtSettings部分文件读取到JwtSettings中，这是给其他地方用的
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            //由于初始化的时候我们就需要用，所以使用Bind的方式读取配置
            //将配置绑定到JwtSettings实例中
            var jwtSettings = new JwtSettings();
            Configuration.Bind("JwtSettings", jwtSettings);

            //添加身份验证
            services.AddAuthentication(options =>
                {
                    ////认证middleware配置
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    //jwt token参数设置
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //Token颁发机构
                        ValidIssuer = jwtSettings.Issuer,
                        //颁发给谁
                        ValidAudience = jwtSettings.Audience,
                        //这里的key要进行加密
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey))
                    };
                });
            #endregion

            #region swagger

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("V1", new OpenApiInfo()
                {
                    Title = "test",
                    Version = "version-01",
                    Description = "会员管理系统"
                });
                //名称必须为“Bearer”，否者请求接口时Header不会带上Token
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可，注意两者之间有空格。",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
                        new List<string>()
                    }
                });
            });

            #endregion

            #region 跨域

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(s => { s.SwaggerEndpoint("/swagger/V1/swagger.json", "test1"); });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            //身份授权认证
            app.UseAuthentication();//需要放在 app.UseAuthorization 的前面

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
