﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpConfigurationInitializer.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
// </copyright>
// <summary>
//   Initialize the HttpConfiguration for OWIN
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sample.PolicyRecordingBot.FrontEnd.Http
{
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;
    using Microsoft.Graph.Communications.Common.Telemetry;
    using Owin;
    using Sample.PolicyRecordingBot.FrontEnd.Middleware;

    /// <summary>
    /// Initialize the HttpConfiguration for OWIN.
    /// </summary>
    public class HttpConfigurationInitializer
    {
        /// <summary>
        /// Configuration settings like Authentication, Routes for OWIN.
        /// </summary>
        /// <param name="app">Builder to configure.</param>
        /// <param name="logger">Graph logger.</param>
        public void ConfigureSettings(IAppBuilder app, IGraphLogger logger)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();
            httpConfig.MapHttpAttributeRoutes();
            httpConfig.MessageHandlers.Add(new LoggingMessageHandler(isIncomingMessageHandler: true, logger: logger, urlIgnorers: new[] { "/logs" }));

            httpConfig.Services.Add(typeof(IExceptionLogger), new ExceptionLogger(logger));

            // TODO: Provide serializer settings hooks
            // httpConfig.Formatters.JsonFormatter.SerializerSettings = RealTimeMediaSerializer.GetSerializerSettings();
            httpConfig.EnsureInitialized();

            // Add the middleware to log service start/stop and response time to logs 
            app.Use(typeof(ResponseTimeMiddleware));

            // Use the HTTP configuration initialized above
            app.UseWebApi(httpConfig);
        }
    }
}