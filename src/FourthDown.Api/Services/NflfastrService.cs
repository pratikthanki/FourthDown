using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FourthDown.Api.Controllers;
using FourthDown.Shared.Models;
using FourthDown.Api.Parameters;
using FourthDown.Shared.Repositories;
using FourthDown.Shared.Utilities;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace FourthDown.Api.Services
{
    public class NflfastrService : INflfastrService
    {
        private readonly ILogger<NflfastrController> _logger;
        private readonly ITracer _tracer;
        private readonly IPlayByPlayRepository _playByPlayRepository;

        public NflfastrService(
            IPlayByPlayRepository playByPlayRepository,
            ILogger<NflfastrController> logger,
            ITracer tracer)
        {
            _playByPlayRepository = playByPlayRepository;
            _logger = logger;
            _tracer = tracer;
        }

        public IEnumerable<TeamPlayByPlay> GetSummarisedStats(
            NflfastrQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Started method {nameof(GetSummarisedStats)}");

            var plays = _playByPlayRepository
                .GetPlayByPlaysAsync(
                    queryParameter.Season ?? StringParser.GetCurrentSeason(), queryParameter.Team, cancellationToken)
                .GroupBy(x => x.ToPlayKey())
                .Select(x => new TeamPlayByPlay(x.Key, x.ToList()));

            _logger.LogInformation($"Finished method {nameof(GetSummarisedStats)}");

            return plays;
        }
    }
}