using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KBAPI.DataAccessLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TectonaDatabaseHandlerDLL;

namespace KBAPI
{
    public class Startup
    {
        //DatabaseCommon objcommon = new DatabaseCommon();
        KBCommon objcommon = new KBCommon();
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _hostingEnvironment = env;
            Configuration = configuration;
            objcommon.WriteLog("Startup", "log", "KB", "_hostingEnvironment : " + _hostingEnvironment, true);
            objcommon.WriteLog("Startup", "log", "KB", "Configuration : " + Configuration, true);

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            // caching response for middlewares
            services.AddResponseCaching();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            // for Linux
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                  Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                RequestPath = "/wwwroot"
            });

            if (OwnYITConstant.LINUX_ROOT_PATH == "")
                OwnYITConstant.LINUX_ROOT_PATH = env.ContentRootPath;

            if (OwnYITConstant.LINUX_WWW_PATH == "")
                OwnYITConstant.LINUX_WWW_PATH = env.WebRootPath;

            DBConfiguration db_conf;
            DBSettings settings;
            db_conf = new DBConfiguration("KBDM.xml");
            objcommon.WriteLog("Startup", "log", "KB", "db_conf : " + db_conf, true);
            settings = db_conf.GetDBSettings();
            objcommon.WriteLog("Startup", "log", "KB", "settings : " + settings, true);
            LocalConstant.poolKB = new DatabasePool(settings);
            objcommon.WriteLog("Startup", "log", "KB", "PoolKB : " + LocalConstant.poolKB, true);
            LocalConstant.poolKB.load();
            KBCommon objCom = new KBCommon();
            //objCom.SetConfig();
            //objcommon.WriteLog("Startup", "log", "PoolKB", "objCom : " + objCom, true);
            string OSType = objCom.readOSType();
            LocalConstant.NocdeskTicket = objCom.readDBConfig("NocdeskTicket", OSType, "WSURL.xml", "WSURL.xml");
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseResponseCaching();
        }

    }
}
