using System.Collections.Generic;
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

        public async Task<IEnumerable<PlayByPlay>> GetAllPlaysAsync(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            return await _playByPlayRepository.GetPlayByPlaysAsync(queryParameter, cancellationToken);
        }
    }
}