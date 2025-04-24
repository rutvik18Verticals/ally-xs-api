using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Common
{
    /// <summary>
    /// A base controller providing common functionality for all controllers.
    /// </summary>
    public abstract class BaseController : ControllerBase
    {

        #region Protected Abstract Properties

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected abstract IThetaLogger ControllerLogger { get; }

        #endregion

        #region Protected Properties

        /// <summary>
        /// The logger factory.
        /// </summary>
        protected IThetaLoggerFactory LoggerFactory { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        protected BaseController(IThetaLoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region Protected Classes

        /// <summary>
        /// Class containing common query param names.
        /// </summary>
        protected static class QueryParams
        {
            /// <summary>
            /// The AssetId from the Api parameter.
            /// </summary>
            public const string AssetId = "assetId";

            /// <summary>
            /// The testDate from the Api parameter.
            /// </summary>
            public const string TestDate = "testDate";

            /// <summary>
            /// The cardDate from the Api parameter.
            /// </summary>
            public const string CardDate = "cardDate";

            /// <summary>
            /// The groupName from the Api parameter.
            /// </summary>
            public const string GroupName = "groupName";

            /// <summary>
            /// The addresses from the Api parameter.
            /// </summary>
            public const string Addresses = "addresses";

            /// <summary>
            /// The type from the Api parameter.
            /// </summary>
            public const string Type = "type";

            /// <summary>
            /// The surveyDate from the Api parameter.
            /// </summary>
            public const string SurveyDate = "surveyDate";

            /// <summary>
            /// The itemid from the Api parameter.
            /// </summary>
            public const string ItemId = "itemId";

            /// <summary>
            /// The start date from the Api parameter.
            /// </summary>
            public const string StartDate = "startDate";

            /// <summary>
            /// The enddate from the Api parameter.
            /// </summary>
            public const string EndDate = "endDate";

            /// <summary>
            /// The analysisTypeId from the Api parameter.
            /// </summary>
            public const string AnalysisTypeId = "analysisTypeId";

            /// <summary>
            /// The id from the Api parameter.
            /// </summary>
            public const string Id = "id";

            /// <summary>
            /// The analysisResultId from the Api parameter.
            /// </summary>
            public const string AnalysisResultId = "analysisResultId";

            /// <summary>
            /// The viewId from the Api parameter.
            /// </summary>
            public const string ViewId = "viewId";

            /// <summary>
            /// The userId from the Api parameter.
            /// </summary>
            public const string UserId = "userId";

            /// <summary>
            /// The control type query param key.
            /// </summary>
            public const string ControlType = "controlType";

            /// <summary>
            /// The web socket id query param key.
            /// </summary>
            public const string SocketId = "socketId";

            /// <summary>
            /// The customer id query param key.
            /// </summary>
            public const string CustomerId = "customerId";

            /// <summary>
            /// The customer id query param key.
            /// </summary>
            public const string POCType = "pocType";

            /// <summary>
            /// The address query param key.
            /// </summary>
            public const string Address = "address";

            /// <summary>
            /// The param standard type query param key.
            /// </summary>
            public const string ParamStandardType = "paramStandardType";

            /// <summary>
            /// The property query param key.
            /// </summary>
            public const string Property = "property";

            /// <summary>
            /// The chart1Type query param key.
            /// </summary>
            public const string Chart1Type = "chart1Type";

            /// <summary>
            /// The chart1ItemId query param key.
            /// </summary>
            public const string Chart1ItemId = "chart1ItemId";

            /// <summary>
            /// The chart2Type query param key.
            /// </summary>
            public const string Chart2Type = "chart2Type";

            /// <summary>
            /// The chart2ItemId query param key.
            /// </summary>
            public const string Chart2ItemId = "chart2ItemId";

            /// <summary>
            /// The chart3Type query param key.
            /// </summary>
            public const string Chart3Type = "chart3Type";

            /// <summary>
            /// The chart3ItemId query param key.
            /// </summary>
            public const string Chart3ItemId = "chart3ItemId";

            /// <summary>
            /// The chart4Type query param key.
            /// </summary>
            public const string Chart4Type = "chart4Type";

            /// <summary>
            /// The chart4ItemId query param key.
            /// </summary>
            public const string Chart4ItemId = "chart4ItemId";

            /// <summary>
            /// The query param key indicating overlay chart or not.
            /// </summary>
            public const string IsOverlay = "isOverlay";

            /// <summary>
            /// The tags query param key.
            /// </summary>
            public const string Tags = "tags";

            /// <summary>
            /// The sampling query param key.
            /// </summary>
            public const string Sampling = "sampling";

            /// <summary>
            /// The customer ids query param key.
            /// </summary>
            public const string CustomerIds = "customerId";

            /// <summary>
            /// The api token.
            /// </summary>
            public const string TokenKey = "APITokenKey";

            /// <summary>
            /// The api token.
            /// </summary>
            public const string TokenValue = "APITokenValue";

        }

        /// <summary>
        /// Class containing common Body param names.
        /// </summary>
        protected static class BodyParams
        {
            /// The AssetId body param key.
            public const string AssetId = "assetId";

            /// The AddressValues body param key.
            public const string AddressValues = "addressValues";

        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Validates that the <paramref name="queryParams"/> list is valid and
        /// contains the provided <paramref name="queryParamNames"/>. 
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="defaultInvalidStatusCodeResult">
        /// A status code result to be returned from the calling controller when validation of query params fails.
        /// </param>
        /// <param name="queryParams">
        /// The query param dictionary containing key-pair values where the query param id and its value.
        /// </param>
        /// <param name="queryParamNames">
        /// The query param ids to search for in <paramref name="queryParams"/>.
        /// </param>
        /// <returns>True if query params are valid, false otherwise.</returns>
        protected bool ValidateQueryParams(string correlationId,
            out StatusCodeResult defaultInvalidStatusCodeResult,
            IDictionary<string, string> queryParams,
            params string[] queryParamNames)
        {
            defaultInvalidStatusCodeResult = BadRequest();

            if (queryParamNames == null)
            {
                ControllerLogger.WriteCId(Level.Error, "Missing query params to validate", correlationId);

                return false;
            }

            if (queryParams == null || queryParams.Count == 0)
            {
                ControllerLogger.WriteCId(Level.Error, "Missing query params", correlationId);

                return false;
            }

            foreach (var queryParamName in queryParamNames)
            {
                if (!queryParams.TryGetValue(queryParamName, out var queryParamValue)
                    || string.IsNullOrEmpty(queryParamValue))
                {
                    ControllerLogger.WriteCId(Level.Error, $"Missing query param value for {queryParamName}", correlationId);

                    return false;
                }

                ControllerLogger.WriteCId(Level.Debug, $"Found query param {queryParamName} with value {queryParamValue}",
                    correlationId);
            }

            return true;
        }

        /// <summary>
        /// Validates that the <paramref name="queryParams"/> list is valid and
        /// contains the provided <paramref name="queryParamName"/>. 
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="defaultInvalidStatusCodeResult">
        /// A status code result to be returned from the calling controller when validation of query params fails.
        /// </param>
        /// <param name="queryParams">
        /// The query param string array containing values.
        /// </param>
        /// <param name="queryParamName">
        /// The query param ids to search for in <paramref name="queryParams"/>.
        /// </param>
        /// <returns>True if query params are valid, false otherwise.</returns>
        protected bool ValidateQueryParams(string correlationId,
            out StatusCodeResult defaultInvalidStatusCodeResult,
            string[] queryParams,
            params string[] queryParamName)
        {
            defaultInvalidStatusCodeResult = BadRequest();

            if (queryParamName == null)
            {
                ControllerLogger.WriteCId(Level.Error, "Missing query params to validate", correlationId);

                return false;
            }

            if (queryParams == null || queryParams.Length == 0)
            {
                ControllerLogger.WriteCId(Level.Error, "Missing query params", correlationId);

                return false;
            }

            ControllerLogger.WriteCId(Level.Debug, $"Found query param {string.Join(",", queryParams)}", correlationId);

            return true;
        }

        /// <summary>
        /// Validates that the <paramref name="bodyParams"/> list is valid and
        /// contains the provided <paramref name="bodyParamNames"/>. 
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="defaultInvalidStatusCodeResult">
        /// A status code result to be returned from the calling controller when validation of body params fails.
        /// </param>
        /// <param name="bodyParams">
        /// The query param dictionary containing key-pair values where the body param id and its value.
        /// </param>
        /// <param name="bodyParamNames">
        /// The query param ids to search for in <paramref name="bodyParams"/>.
        /// </param>
        /// <returns>True if body params are valid, false otherwise.</returns>
        protected bool ValidateBodyParams(string correlationId,
            out StatusCodeResult defaultInvalidStatusCodeResult,
            IDictionary<string, string> bodyParams,
            params string[] bodyParamNames)
        {
            defaultInvalidStatusCodeResult = BadRequest();

            if (bodyParamNames == null)
            {
                ControllerLogger.WriteCId(Level.Error, "Missing body params to validate", correlationId);

                return false;
            }

            if (bodyParams == null || bodyParams.Count == 0)
            {
                ControllerLogger.WriteCId(Level.Error, "Missing body params", correlationId);

                return false;
            }

            foreach (var bodyParamName in bodyParamNames)
            {
                if (!bodyParams.TryGetValue(bodyParamName, out var bodyParamValue)
                    || string.IsNullOrEmpty(bodyParamValue))
                {
                    ControllerLogger.WriteCId(Level.Error, $"Missing body param value for {bodyParamName}", correlationId);

                    return false;
                }

                ControllerLogger.WriteCId(Level.Debug, $"Found body param {bodyParamName} with value {bodyParamValue}",
                                       correlationId);
            }

            return true;
        }

        /// <summary>
        /// Validates a given service result.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="defaultInvalidStatusCodeResult">
        /// A status code result to be returned from the calling controller when validation of service result fails.
        /// </param>
        /// <param name="serviceResult">The service result to be validated.</param>
        /// <returns>True if the service result is valid, false otherwise.</returns>
        protected bool ValidateServiceResult(string correlationId,
            out StatusCodeResult defaultInvalidStatusCodeResult, CoreOutputBase serviceResult)
        {
            defaultInvalidStatusCodeResult = new StatusCodeResult(500);

            if (serviceResult?.Result == null)
            {
                // null, we didn't properly handle the error, return 500.
                ControllerLogger.WriteCId(Level.Error, "Service result is null", correlationId);

                return false;
            }

            if (serviceResult.Result.Status == false)
            {
                // status is false, we properly handled the error and logged it, return 200.
                ControllerLogger.WriteCId(Level.Error, "Service result status is false", correlationId);

                defaultInvalidStatusCodeResult = Ok();

                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates a given asset id, determining if it is parseable as a guid value.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="defaultInvalidStatusCodeResult">
        /// A status code result to be returned from the calling controller when validation of service result fails.
        /// </param>
        /// <param name="parsedAssetId">The original asset id, parsed as a guid type.</param>
        /// <param name="assetIdString">The asset id to validate and have parsed as a guid.</param>
        /// <returns>True if the asset id is valid and parseable as a guid value, false otherwise.</returns>
        protected bool ValidateAssetId(string correlationId, out StatusCodeResult defaultInvalidStatusCodeResult, out Guid parsedAssetId,
            string assetIdString)
        {
            defaultInvalidStatusCodeResult = BadRequest();
            parsedAssetId = Guid.Empty;

            if (string.IsNullOrEmpty(assetIdString))
            {
                ControllerLogger.WriteCId(Level.Error, "Missing asset id", correlationId);

                return false;
            }

            var isGuid = Guid.TryParse(assetIdString, out parsedAssetId);
            if (isGuid)
            {
                parsedAssetId = new Guid(assetIdString);

                if (parsedAssetId == Guid.Empty)
                {
                    ControllerLogger.WriteCId(Level.Error, $"Invalid asset id {parsedAssetId}", correlationId);

                    return false;
                }
            }
            else
            {
                ControllerLogger.WriteCId(Level.Error, "Invalid asset id", correlationId);

                return false;
            }

            ControllerLogger.WriteCId(Level.Debug, $"Asset id {parsedAssetId}", correlationId);

            return true;
        }

        /// <summary>
        /// Validates a given asset id, determining if it is parseable as a guid value.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="defaultInvalidStatusCodeResult">
        /// A status code result to be returned from the calling controller when validation of service result fails.
        /// </param>
        /// <param name="parsedGroupName">The parsed GroupName.</param>
        /// <param name="groupNameString">The group Name String.</param>
        /// <returns>True if the asset id is valid and parseable as a guid value, false otherwise.</returns>
        protected bool ValidateGroupName(string correlationId, out StatusCodeResult defaultInvalidStatusCodeResult, out string parsedGroupName, string groupNameString)
        {
            defaultInvalidStatusCodeResult = BadRequest();
            parsedGroupName = groupNameString;

            if (string.IsNullOrEmpty(groupNameString))
            {
                ControllerLogger.WriteCId(Level.Error, "Missing group name", correlationId);

                return false;
            }

            ControllerLogger.WriteCId(Level.Debug, $"Group name {parsedGroupName}", correlationId);

            return true;
        }

        /// <summary>
        /// Validates a given tag id, determining if it is not empty.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="defaultInvalidStatusCodeResult">
        /// A status code result to be returned from the calling controller when validation of service result fails.
        /// </param>
        /// <param name="parsedTags">The parsed Tags.</param>
        /// <param name="tagsString">The group Name String.</param>
        /// <returns>True if the asset id is valid and parseable as a guid value, false otherwise.</returns>
        protected bool ValidateTags(string correlationId, out StatusCodeResult defaultInvalidStatusCodeResult, out string parsedTags, string tagsString)
        {
            defaultInvalidStatusCodeResult = BadRequest();
            parsedTags = tagsString;

            if (string.IsNullOrEmpty(tagsString))
            {
                ControllerLogger.WriteCId(Level.Error, "Missing tags", correlationId);

                return false;
            }

            ControllerLogger.WriteCId(Level.Debug, $"Tags {parsedTags}", correlationId);

            return true;
        }

        /// <summary>
        /// Validates a given asset pe, determining if it is parseable as a guid value.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="defaultInvalidStatusCodeResult">
        /// A status code result to be returned from the calling controller when validation of service result fails.
        /// </param>
        /// <param name="type">The original asset type.</param>
        /// <param name="assetType">The asset id to validate and have parsed as a guid.</param>
        /// <returns>True if the asset type is valid and false otherwise.</returns>
        protected bool ValidateAssetType(string correlationId, out StatusCodeResult defaultInvalidStatusCodeResult, out string type, string assetType)
        {
            defaultInvalidStatusCodeResult = BadRequest();
            type = assetType;

            if (type == "asset" || type == "group")
            {
                ControllerLogger.WriteCId(Level.Debug, $"Type {type}", correlationId);

                return true;
            }

            ControllerLogger.WriteCId(Level.Error, "Invalid type", correlationId);

            return false;
        }

        /// <summary>
        /// Validates an array, check if the comma delimted string is parseable to an array.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="defaultInvalidStatusCodeResult" >
        /// A status code result <seealso cref="StatusCodeResult"/> to be returned from the calling controller
        /// when validation of service result fails.
        /// </param>
        /// <param name="addresses">The parsed string array.</param>
        /// <param name="addressesString">The comma delimited string of addresses.</param>
        /// <returns>True if the addressesString is valid and parseable to an array, false otherwise.</returns>
        protected bool ValidateAddressesArray(string correlationId, out StatusCodeResult defaultInvalidStatusCodeResult,
            out string[] addresses, string addressesString)
        {
            defaultInvalidStatusCodeResult = BadRequest();
            addresses = Array.Empty<string>();

            if (string.IsNullOrEmpty(addressesString))
            {
                ControllerLogger.WriteCId(Level.Error, "Missing addresses", correlationId);

                return false;
            }

            addresses = addressesString.Split(",");

            if (addresses.Length <= 0)
            {
                addresses = Array.Empty<string>();

                ControllerLogger.WriteCId(Level.Error, "Missing comma separated addresses", correlationId);

                return false;
            }

            ControllerLogger.WriteCId(Level.Debug, $"Addresses {addressesString}", correlationId);

            return true;
        }

        /// <summary>
        /// Validates a given user, determining if it is authorized.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="defaultInvalidStatusCodeResult">
        /// A status code result to be returned from the calling controller when validation of user fails.
        /// </param>
        /// <param name="user">The authorized user.</param>
        /// <returns>True if the user is authorized, false otherwise.</returns>
        protected bool ValidateUserAuthorized(string correlationId, out StatusCodeResult defaultInvalidStatusCodeResult, out string user)
        {
            defaultInvalidStatusCodeResult = BadRequest();

            var userData = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            if (userData == null)
            {
                defaultInvalidStatusCodeResult = Unauthorized();

                user = null;

                ControllerLogger.WriteCId(Level.Error, "Missing user data", correlationId);

                return false;
            }

            var userName = userData.Value?.ToString();

            if (string.IsNullOrEmpty(userName))
            {
                defaultInvalidStatusCodeResult = Unauthorized();

                user = null;

                ControllerLogger.WriteCId(Level.Error, "Missing user name", correlationId);

                return false;
            }

            user = userName;
            
            ControllerLogger.WriteCId(Level.Debug, $"User name {user}", correlationId);

            return true;
        }

        /// <summary>
        /// Gets the correlation id from the header. If the correlation id is not found, a new one is generated.
        /// </summary>
        /// <param name="correlationId">The correlation Id.</param>
        /// <returns>True if the header is valid, false otherwise.</returns>
        protected bool GetOrCreateCorrelationId(out string correlationId)
        {
            if (HttpContext != null && HttpContext.Request.Headers.TryGetValue(CustomHeaders.XSPOC_UI_TRACKING_ID, out var headerValue))
            {
                correlationId = headerValue;

                ControllerLogger.WriteCId(Level.Info, "Retrieved correlation id from header", correlationId);
                ControllerLogger.WriteCId(Level.Info, $"Trace Identifier {HttpContext.TraceIdentifier}", correlationId);

                return true;
            }

            correlationId = Guid.NewGuid().ToString();

            ControllerLogger.WriteCId(Level.Info, "Missing correlation id from header, generated one", correlationId);
            ControllerLogger.WriteCId(Level.Info, $"Trace Identifier {HttpContext?.TraceIdentifier}", correlationId);

            return false;
        }

        #endregion

    }
}