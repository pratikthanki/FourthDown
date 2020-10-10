using System.Collections.Generic;
using FourthDown.API.Models;
using FourthDown.API.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FourthDown.API.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly JsonPlayByPlayService PlayByPlayService;
        public IEnumerable<PlayByPlay> Plays { get; private set; }

        public IndexModel(
            ILogger<IndexModel> logger, 
            JsonPlayByPlayService playByPlayService)
        {
            _logger = logger;
            PlayByPlayService = playByPlayService;
        }

        public void OnGet()
        {
            Plays = PlayByPlayService.GetPlayByPlays();
        }
    }
}