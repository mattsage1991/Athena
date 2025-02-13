﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Athena.BuildingBlocks.Infrastructure;
using Athena.Stocks.Application.Configuration.Commands;
using Athena.Stocks.Application.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Athena.Stocks.Infrastructure.Configuration.Processing
{
    internal class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T>
        where T : ICommand
    {
        private readonly ICommandHandler<T> _decorated;
        private readonly IUnitOfWork _unitOfWork;
        private readonly StocksContext _stockContext;

        public UnitOfWorkCommandHandlerDecorator(
            ICommandHandler<T> decorated,
            IUnitOfWork unitOfWork,
            StocksContext stockContext)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
            _stockContext = stockContext;
        }

        public async Task<Unit> Handle(T command, CancellationToken cancellationToken)
        {
            await this._decorated.Handle(command, cancellationToken);

            if (command is InternalCommandBase)
            {
                var internalCommand =
                    await _stockContext.InternalCommands.FirstOrDefaultAsync(
                        x => x.Id == command.Id,
                        cancellationToken: cancellationToken);

                if (internalCommand != null)
                {
                    internalCommand.ProcessedDate = DateTime.UtcNow;
                }
            }

            await this._unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}