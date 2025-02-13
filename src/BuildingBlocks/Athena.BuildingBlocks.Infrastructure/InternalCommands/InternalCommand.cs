﻿using System;

namespace Athena.BuildingBlocks.Infrastructure.InternalCommands
{
    public class InternalCommand
    {
        public Guid Id { get; set; }

        public DateTime EnqueueDate { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }

        public string Error { get; set; }

        public DateTime? ProcessedDate { get; set; }
    }
}