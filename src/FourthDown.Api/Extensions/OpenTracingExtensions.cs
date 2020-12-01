using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using OpenTracing;
using OpenTracing.Tag;

namespace FourthDown.Api.Extensions
{
    public static class OpenTracingExtensions
    {
        public static IScope InitializeTrace(this ITracer tracer, HttpContext httpContext, string action) =>
            tracer.BuildSpan(action)
                .WithTag(Tags.SpanKind, Tags.SpanKindClient)
                .WithTag(Tags.HttpMethod, HttpMethod.Get.ToString())
                .WithTag(Tags.HttpUrl, httpContext.Request.GetDisplayUrl())
                .WithTag(Tags.PeerHostIpv4, httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString())
                .WithTag(Tags.PeerHostIpv6, httpContext.Connection.RemoteIpAddress.MapToIPv6().ToString())
                .StartActive(true);

        public static void LogStart(this IScope scope, string method) => scope.Span.Log($"Start{method}");

        public static void LogEnd(this IScope scope, string method) => scope.Span.Log($"End{method}");
    }
}