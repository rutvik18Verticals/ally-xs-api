using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Data.Updates.Models;

namespace Theta.XSPOC.Apex.Api.WellControl.Data.Services.Contracts.Mappers
{
    /// <summary>
    /// Maps a <seealso cref="UpdatePayload"/> to a <seealso cref="TransactionsModel"/>.
    /// </summary>
    public class TransactionPayloadMapper
    {

        /// <summary>
        /// Maps the <seealso cref="UpdatePayload"/> that came from the integration layer to the
        /// <seealso cref="TransactionsModel"/> that will be handled by the persistence layer.
        /// </summary>
        /// <param name="input">The input to map.</param>
        /// <returns>
        /// A new <seealso cref="TransactionsModel"/> for the provided <paramref name="input"/>.
        /// <c>null</c> is returned if any of the following are missing:
        /// <paramref name="input"/>
        /// <paramref name="input.transaction"/>.
        /// </returns>
        public static TransactionsModel Map(UpdatePayload input)
        {
            if (input == null)
            {
                return null;
            }
            var transactionData = input.Data as List<UpdateColumnValuePair>;

            var transactions = new TransactionsModel();

            foreach (var data in transactionData)
            {
                if (data.Column != null)
                {
                    switch (data.Column.ToLower())
                    {
                        case "transactionid":
                            transactions.TransactionID = int.Parse(data.Value);
                            break;
                        case "correlationid":
                            transactions.CorrelationId = data.Value;
                            break;
                        case "daterequest":
                            transactions.DateRequest = DateTime.Parse(data.Value);
                            break;
                        case "portid":
                            transactions.PortID = int.Parse(data.Value);
                            break;
                        case "task":
                            transactions.Task = data.Value;
                            break;
                        case "priority":
                            transactions.Priority = int.Parse(data.Value);
                            break;
                        case "source":
                            transactions.Source = data.Value;
                            break;
                        case "nodeid":
                            transactions.NodeID = data.Value;
                            break;
                        case "input":
                            transactions.Input = Convert.FromBase64String(data.Value);
                            break;
                        case "output":
                            transactions.Output = Convert.FromBase64String(data.Value);
                            break;
                        case "processed":
                            transactions.DateProcess = DateTime.Parse(data.Value);
                            break;
                        case "taskname":
                            transactions.Task = data.Value;
                            break;
                        case "transactionsource":
                            transactions.Source = data.Value;
                            break;
                        case "result":
                            transactions.Result = data.Value;
                            break;
                        case "tries":
                            transactions.Tries = int.Parse(data.Value);
                            break;
                        case "requested":
                            transactions.DateRequest = DateTime.Parse(data.Value);
                            break;
                        default:
                            break;
                    }// switch (data.Column)
                }// if (data.Column != null)
            }// foreach (var data in assetData)

            return transactions;
        }
    }
}