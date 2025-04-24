using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.WellControl.Integration.Models;
using Theta.XSPOC.Apex.Api.WellControl.Logging;
using Theta.XSPOC.Apex.Kernel.Integration;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.WellControl.BackgroundServices
{
    /// <summary>
    /// The background worker service that hosts the <seealso cref="IConsumeMessage"/> that listens 
    /// for card messages.
    /// </summary>
    public class ProcessWellControlWorker : BackgroundService
    {

        #region Private Members

        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly IConsumeMessage<Responsibility> _processDataUpdate;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an instance of the <seealso cref="ProcessWellControlWorker"/>.
        /// </summary>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/>.</param>
        /// <param name="consumers">The <seealso cref="IEnumerable{IConsumeMessage}"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerFactory"/> is null
        /// OR
        /// <paramref name="consumers"/> is null
        /// OR
        /// <paramref name="consumers"/> does not contain a suitable implementation of
        /// <seealso cref="IConsumeMessage{Responsibility}"/>.
        /// </exception>
        public ProcessWellControlWorker(IThetaLoggerFactory loggerFactory,
            IEnumerable<IConsumeMessage<Responsibility>> consumers)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

            if (consumers == null)
            {
                throw new ArgumentNullException(nameof(consumers));
            }

            _processDataUpdate = consumers.FirstOrDefault(x => x.Responsibility == Responsibility.ConsumeTransationUpdateFromMicroservices)
                                 ?? throw new ArgumentNullException(nameof(consumers) + " did not contain " +
                                                                    Responsibility.ConsumeTransationUpdateFromMicroservices);
        }

        #endregion

        #region Protected Overrides

        /// <summary>
        /// Initialize the consumer. If this fails it will retry the initialization. If the <paramref name="stoppingToken"/>
        /// is set, initialization attempts will be stopped.
        /// </summary>
        /// <param name="stoppingToken">The <seealso cref="CancellationToken"/>.</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            var logger = _loggerFactory.Create(LoggingModel.WellControl);
            var initialized = false;

            while (initialized == false && stoppingToken.IsCancellationRequested == false)
            {
                try
                {
                    initialized = await _processDataUpdate.InitAsync(stoppingToken);
                }
                catch (Exception e)
                {
                    initialized = false;
                    logger.Write(Level.Error, e);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        #endregion

        #region Override Public Methods

        /// <summary>
        /// Stops the back ground worker service, including cleaning up objects that needs to be disposed of.
        /// </summary>
        /// <param name="stoppingToken">The <seealso cref="CancellationToken"/>.</param>
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            var logger = _loggerFactory.Create(LoggingModel.WellControl);

            _processDataUpdate?.Dispose();

            logger.Write(Level.Trace, "Update well control worker stopped");

            await base.StopAsync(stoppingToken);
        }

        #endregion

    }
}
