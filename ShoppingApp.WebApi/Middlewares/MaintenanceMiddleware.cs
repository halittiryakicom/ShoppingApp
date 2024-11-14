﻿using ShoppingApp.Business.Operations.Setting;

namespace ShoppingApp.WebApi.Middlewares
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;

        public MaintenanceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var settingService = context.RequestServices.GetRequiredService<ISettingService>();
            bool maintenanceMode = settingService.GetMainTenanceState();
            if (context.Request.Path.StartsWithSegments("/api/auth/login") || context.Request.Path.StartsWithSegments("/api/settings"))
            {
                await _next(context);
                return;
            }
            if (maintenanceMode)
            {
                await context.Response.WriteAsync("Şuanda hizmet verememekteyiz.");
            }
            else
            {
                await _next(context);
            }
        }
    }
}
