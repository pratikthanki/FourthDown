using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Controllers;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;
using FourthDown.Api.Repositories;
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

        public async Task<IEnumerable<TeamPlayByPlay>> GetSummarisedStats(
            NflfastrQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var plays = await _playByPlayRepository.GetPlayByPlaysAsync(
                queryParameter.Season, queryParameter.Team, cancellationToken);

            var playsByKey = plays
                .GroupBy(x => x.ToPlayKey())
                .ToDictionary(k => k.Key, x =>
                {
                    return new TeamPlayByPlay(x.Key, x.ToList());
                });

            var teamPlays = playsByKey.Select(x => x.Value).ToList();

            return teamPlays;
        }
    }
}