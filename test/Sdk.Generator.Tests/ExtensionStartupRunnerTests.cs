﻿using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Core;
using Microsoft.Azure.Functions.Worker.Sdk.Generators;
using Xunit;
namespace Sdk.Generator.Tests
{
    public class ExtensionStartupRunnerTests
    {
        const string expectedGeneratedFileName = $"ExtensionStartupRunner.g.cs";
        [Fact]
        public async Task StartupCodeGetsGenerated()
        {
            string inputCode = @"
public class Foo
{
}";

            string expectedOutput = @"// <auto-generated/>
using System;
using Microsoft.Azure.Functions.Worker;
namespace MyUnitTestNamespace
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WorkerExtensionStartupAttribute : Attribute
    {
    }
    [WorkerExtensionStartup]
    internal class WorkerExtensionStartupRunner
    {
        public void RunStartupForExtensions(IFunctionsWorkerApplicationBuilder builder)
        {
            new Worker.Extensions.Sample.MySampleExtensionStartup().Configure(builder);
        }
    }
}
";
            var extensionAssemblies = new[]
            {
                    typeof(Worker.Extensions.Sample.MySampleExtensionStartup).Assembly,
            };

            await TestHelpers.RunTestAsync<ExtensionStartupRunnerGenerator>(
                extensionAssemblies,
                inputCode,
                expectedGeneratedFileName,
                expectedOutput);
        }
    }
}
