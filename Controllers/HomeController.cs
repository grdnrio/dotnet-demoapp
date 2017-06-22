﻿using System;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using DotnetDemoApp.ViewModels;
using System.Linq;

namespace DotnetDemoApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Demo App - System Runtime Information";
            HomeAboutViewModel model = new HomeAboutViewModel();

            model.container = (System.IO.File.Exists(".insidedocker")) ? "Docker container" : "regular process";
            model.host = Environment.MachineName;
            model.os = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            model.procs = Environment.ProcessorCount.ToString();
            model.arch = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString();
            model.framework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
            model.mem = (System.Diagnostics.Process.GetCurrentProcess().VirtualMemorySize64 / (1024*1024*1024)).ToString();

            var httpConnectionFeature = this.ControllerContext.HttpContext.Features.Get<IHttpConnectionFeature>();
            model.ip = httpConnectionFeature?.LocalIpAddress.ToString();

            // Harvest a few "interesting" environmental vars which may or may not be present
            var env = Environment.GetEnvironmentVariables().GetEnumerator();
            string[] interesting_envs = {"PROCESSOR_IDENTIFIER", "MACHTYPE", "DOTNET_VERSION", "HOSTTYPE", "PROCESSOR_ARCHITECTURE"};
            while (env.MoveNext()) {
                if(interesting_envs.Any(s=>env.Key.ToString().Contains(s))) {
                    model.envs.Add(env.Key.ToString(), env.Value.ToString());
                }
            } 
            
            return View(model);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
