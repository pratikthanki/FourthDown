using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using OpenTracing;
using OpenTracing.Tag;

namespace FourthDown.Api.Extensions
{
    public static class OpenTracingExtensions
    {
        public static IScope InitializeTrace(this ITracer tracer, string actionName) =>
            tracer.BuildSpan(actionName)
                .StartActive(true);

        public static void SetTags(this ISpan span, HttpContext httpContext) =>
            span.SetTag(Tags.SpanKind, Tags.SpanKindClient)
                .SetTag(Tags.HttpMethod, HttpMethod.Get.ToString())
                .SetTag(Tags.HttpUrl, httpContext.Request.GetDisplayUrl())
                .SetTag(Tags.PeerHostIpv4, httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString())
                .SetTag(Tags.PeerHostIpv6, httpContext.Connection.RemoteIpAddress.MapToIPv6().ToString());

        public static IScope BuildTrace(this ITracer tracer, string actionName) =>
            tracer.BuildSpan(actionName).StartActive(true);

        public static void LogStart(this IScope scope, string method) => scope.Span.Log($"Start{method}");

        public static void LogEnd(this IScope scope, string method) => scope.Span.Log($"End{method}");

        public static void LogEvent(this IScope scope, params (string, object)[] args)
        {
            foreach (var arg in args)
                scope.Span.Log($"{arg.Item1}: {arg.Item2}");
        }
    }
}
