using System;
using System.Collections.Generic;
using System.Text;

namespace SuaveKeys.Clients.Models.Luis
{
    public class LuisPredictionResponse
    {
        public string Query { get; set; }
        public LuisPrediction Prediction { get; set; }
    }
}
