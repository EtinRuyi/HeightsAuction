﻿using Microsoft.Extensions.Configuration;

namespace HeightsAuction.Common.Utilities
{
    public class ConfigurationHelper
    {
        private static IConfiguration _configuration;
        public static void InstantiateConfiguration(IConfiguration configuration) => _configuration = configuration;

        public static IConfiguration GetConfigurationInstance() => _configuration;
    }
}
