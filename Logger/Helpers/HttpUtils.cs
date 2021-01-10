﻿using System;
using System.Linq;
using System.Net.Http;

namespace Logger.Helpers
{
    public class HttpUtils
    {
        public string GetCorrelationId(HttpRequestMessage request)
        {
            string correlationId;
            if (!request.Headers.Contains("X-CorrelationId"))
            {
                correlationId = Guid.NewGuid().ToString();
                request.Headers.Add("X-CorrelationId", correlationId);
            }
            else
            {
                correlationId = request.Headers.GetValues("X-CorrelationId").Aggregate((x, y) => $"{x},{y}");
            }

            return correlationId;
        }
    }
}
