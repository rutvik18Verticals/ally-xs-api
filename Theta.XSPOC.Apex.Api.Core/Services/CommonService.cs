using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Models;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Implementation of ICommonService.
    /// </summary>
    public class CommonService : ICommonService
    {

        #region Private Dependencies

        private readonly ILocalePhrases _localePhrases;
        private readonly IStates _states;
        private readonly ISystemParameter _systemParameterStore;
        private readonly IMemoryCache _memoryCache;

        private enum PhraseIDs
        {
            Startup = 141, // Startup
            Current = 142, // Current
            Alarm = 149, // Alarm
            Base = 150, // Base
            Failure = 151, // Failure
            POSD = 921, // PO/SD
            SMal = 1621, // S-Mal
            PumpoffReference = 6747, // Pump off Reference
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new <seealso cref="CommonService"/>.
        /// </summary>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <param name="localePhrases">The <seealso cref="ILocalePhrases"/>.</param>
        /// <param name="states">The <seealso cref="IStates"/> service.</param>
        /// <param name="systemParameterStore">The <see cref="ISystemParameter"/>.</param>
        /// <param name="memoryCache">The <see cref="IMemoryCache"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerFactory"/> is null or
        /// <paramref name="localePhrases"/> is null or
        /// <paramref name="states"/> is null or
        /// <paramref name="systemParameterStore"/> is null or
        /// <paramref name="memoryCache"/> is null.
        /// </exception>
        public CommonService(IThetaLoggerFactory loggerFactory, ILocalePhrases localePhrases,
            IStates states, ISystemParameter systemParameterStore, IMemoryCache memoryCache)
        {
            _ = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _localePhrases = localePhrases ?? throw new ArgumentNullException(nameof(localePhrases));
            _states = states ?? throw new ArgumentNullException(nameof(states));
            _systemParameterStore = systemParameterStore ?? throw new ArgumentNullException(nameof(systemParameterStore));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        #endregion

        #region ICommonService Implementation

        /// <summary>
        /// Get the Card type name by card type, cause id and poc type.
        /// </summary>
        /// <param name="cardType">The card type.</param>
        /// <param name="causeId">The cause id.</param>
        /// <param name="pocType">The poc type.</param>
        /// <param name="phraseCache">The phrases cache dictionary.</param>
        /// <param name="causeIdPhraseCache">The cause id phrase cache dictionary.</param>
        /// <param name="correlationId"></param>
        /// <returns>The card type name.</returns>
        public string GetCardTypeName(string cardType, int? causeId, short pocType, ref IDictionary<int, string> phraseCache,
            ref IDictionary<int, string> causeIdPhraseCache, string correlationId)
        {
            if (phraseCache == null)
            {
                throw new ArgumentNullException(nameof(phraseCache));
            }

            if (causeIdPhraseCache == null)
            {
                throw new ArgumentNullException(nameof(causeIdPhraseCache));
            }

            var cardTypeName = string.Empty;
            if (cardType == null)
            {
                return cardTypeName;
            }

            causeId ??= 0;

            switch (cardType)
            {
                case "N":
                    cardTypeName = GetPhrase(ref phraseCache, (int)PhraseIDs.Startup, correlationId);
                    break;
                case "P":
                    cardTypeName = GetPhrase(ref phraseCache, (int)PhraseIDs.Current, correlationId);
                    break;
                case "S":
                case "M":
                    if (causeId.HasValue && causeId.Value > 0)
                    {
                        if (pocType == 17)
                        {
                            cardTypeName = GetCauseIdPhrase(ref causeIdPhraseCache, causeId.Value, correlationId);
                        }
                        else
                        {
                            cardTypeName = GetPhrase(ref phraseCache, causeId.Value + 1500, correlationId);
                        }
                    }

                    if (string.IsNullOrEmpty(cardTypeName) || cardTypeName == "<Missing>")
                    {
                        if (cardType == "S")
                        {
                            cardTypeName = GetPhrase(ref phraseCache, (int)PhraseIDs.POSD, correlationId);
                        }
                        else if (cardType == "M")
                        {
                            cardTypeName = GetPhrase(ref phraseCache, (int)PhraseIDs.SMal, correlationId);
                        }
                    }

                    break;
                case "V":
                case "Q":
                    cardTypeName = "SDB-4";
                    break;
                case "W":
                case "R":
                    cardTypeName = "SDB-3";
                    break;
                case "X":
                case "H":
                    cardTypeName = "SDB-2";
                    break;
                case "Y":
                case "T":
                    cardTypeName = "SDB-1";
                    break;
                case "Z":
                case "U":
                    cardTypeName = "SDB";
                    break;
                case "1":
                    cardTypeName = "POB-4";
                    break;
                case "2":
                    cardTypeName = "POB-3";
                    break;
                case "3":
                    cardTypeName = "POB-2";
                    break;
                case "4":
                    cardTypeName = "POB-1";
                    break;
                case "5":
                    cardTypeName = "POB";
                    break;
                case "K":
                    cardTypeName = pocType == (short)Models.DeviceType.RPC_Rockwell_OptiLift
                        ? GetPhrase(ref phraseCache, (int)PhraseIDs.PumpoffReference, correlationId)
                        : "Std";
                    break;
                case "A":
                    cardTypeName = "StNMPT";
                    break;
                case "F":
                    cardTypeName = GetPhrase(ref phraseCache, (int)PhraseIDs.Failure, correlationId); // Failure
                    break;
                case "B":
                    cardTypeName = GetPhrase(ref phraseCache, (int)PhraseIDs.Base, correlationId); // Base
                    break;
                case "L":
                    cardTypeName = GetPhrase(ref phraseCache, (int)PhraseIDs.Alarm, correlationId); // Alarm
                    break;
                case "J":
                    cardTypeName = "SD Buff";
                    break;
                case "O":
                    cardTypeName = "S-Oper";
                    break;
                case "D":
                    cardTypeName = "Dyn-C";
                    break;
                case "E":
                    cardTypeName = "Dyn-U";
                    break;
                default:
                    cardTypeName = "Unknown";
                    break;
            }

            return cardTypeName;
        }

        /// <summary>
        /// Gets the system parameter. If key is in cache, return value from cache; otherwise, query database.
        /// </summary>
        /// <param name="systemParameterKey">The system parameter key.</param>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns></returns>
        public string GetSystemParameter(string systemParameterKey, string cacheKey, string correlationId)
        {
            if (_memoryCache.TryGetValue(cacheKey, out var cacheValue))
            {
                return cacheValue?.ToString();
            }

            var systemParameterValue = _systemParameterStore.Get(systemParameterKey, correlationId);

            _memoryCache.Set(cacheKey, systemParameterValue, TimeSpan.FromHours(24));

            return systemParameterValue;
        }

        /// <summary>
        /// Gets the system parameter NextGen Significant Digits.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>">
        /// <returns>The system parameter next gen significant digits.</returns>
        public int GetSystemParameterNextGenSignificantDigits(string correlationId)
        {
            var digits = GetSystemParameter("NextGen.SignificantDigits", $"{MemoryCacheValueType.SystemParameterNextGenSignificantDigits}", correlationId);

            return string.IsNullOrWhiteSpace((digits)) || int.TryParse(digits, out var parsedDigits) == false ? 3 : parsedDigits;
        }

        #endregion

        private string GetPhrase(ref IDictionary<int, string> phrases, int phraseId, string correlationId)
        {
            if (phrases.TryGetValue(phraseId, out var phrase))
            {
                return phrase;
            }

            phrase = _localePhrases.Get(phraseId, correlationId);

            phrases.TryAdd(phraseId, phrase);

            return phrase;
        }

        private string GetCauseIdPhrase(ref IDictionary<int, string> causeIdPhrases, int causeId, string correlationId)
        {
            if (causeIdPhrases.TryGetValue(causeId, out var phrase))
            {
                return phrase;
            }

            phrase = _states.GetCardTypeNamePocType17CardTypeMS(causeId, correlationId);

            causeIdPhrases.TryAdd(causeId, phrase);

            return phrase;
        }

    }
}
