using OpenTracing;
using OpenTracing.Tag;

namespace FourthDown.Api.Extensions
{
    public static class OpenTracingExtensions
    {
        public static IScope InitializeTrace(this ITracer tracer, string action) =>
            tracer.BuildSpan(action)
                .WithTag(Tags.SpanKind, Tags.SpanKindClient)
                .WithTag(Tags.HttpMethod, "GET")
                .StartActive();

        public static void LogStart(this IScope scope, string method) => scope.Span.Log($"Start{method}");

        public static void LogEnd(this IScope scope, string method) => scope.Span.Log($"End{method}");
    }
}