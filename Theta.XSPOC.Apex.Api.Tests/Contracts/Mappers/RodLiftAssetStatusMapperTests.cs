using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Theta.XSPOC.Apex.Api.Contracts;
using Theta.XSPOC.Apex.Api.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.Core.Models;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs.AssetStatus;
using OverlayFields = Theta.XSPOC.Apex.Api.Core.Models.OverlayFields;

namespace Theta.XSPOC.Apex.Api.Tests.Contracts.Mappers
{
    [TestClass]
    public class RodLiftAssetStatusMapperTests
    {

        #region Test Methods

        [TestMethod]
        public void RodLiftAssetStatusDataContractMapDataIsNullTest()
        {
            var correlationId = Guid.NewGuid().ToString();
            var result = AssetStatusMapper.Map(correlationId, null);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DateCreated);
            Assert.AreEqual(correlationId, result.Id);
        }

        [TestMethod]
        public void RodLiftAssetStatusDataContractMapOutputValuesAreNullTest()
        {
            var correlationId = Guid.NewGuid().ToString();

            var result = AssetStatusMapper.Map(correlationId, new RodLiftAssetStatusDataOutput()
            {
                StatusRegisters = null,
                Alarms = null,
                Exceptions = null,
                ImageOverlayItems = null,
                LastWellTest = null,
                RodStrings = null,
                WellStatusOverview = null,
            });

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Values);
            Assert.IsNotNull(result.DateCreated);
            Assert.IsNull(result.Values.StatusRegisters);
            Assert.IsNull(result.Values.Alarms);
            Assert.IsNull(result.Values.Exceptions);
            Assert.IsNull(result.Values.ImageOverlayItems);
            Assert.IsNull(result.Values.LastWellTest);
            Assert.IsNull(result.Values.RodStrings);
            Assert.IsNull(result.Values.WellStatusOverview);
            Assert.AreEqual(correlationId, result.Id);
        }

        [TestMethod]
        public void RodLiftAssetStatusDataContractMapOutputListItemsHaveNullValuesTest()
        {
            var correlationId = Guid.NewGuid().ToString();

            var result = AssetStatusMapper.Map(correlationId, new RodLiftAssetStatusDataOutput()
            {
                StatusRegisters = new List<PropertyValueOutput>()
                {
                    null,
                },
                Alarms = new List<PropertyValueOutput>()
                {
                    null,
                },
                Exceptions = new List<PropertyValueOutput>()
                {
                    null,
                },
                ImageOverlayItems = new List<OverlayStatusDataOutput>()
                {
                    null,
                },
                LastWellTest = new List<PropertyValueOutput>()
                {
                    null,
                },
                RodStrings = new List<PropertyValueOutput>()
                {
                    null,
                },
                WellStatusOverview = new List<PropertyValueOutput>()
                {
                    null,
                },
            });

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DateCreated);
            Assert.AreEqual(correlationId, result.Id);
            Assert.IsNotNull(result.Values);
            Assert.IsNotNull(result.Values.StatusRegisters);
            Assert.IsNotNull(result.Values.Alarms);
            Assert.IsNotNull(result.Values.Exceptions);
            Assert.IsNotNull(result.Values.ImageOverlayItems);
            Assert.IsNotNull(result.Values.LastWellTest);
            Assert.IsNotNull(result.Values.RodStrings);
            Assert.IsNotNull(result.Values.WellStatusOverview);

            Assert.AreEqual(1, result.Values.StatusRegisters.Count);
            Assert.AreEqual(1, result.Values.Alarms.Count);
            Assert.AreEqual(1, result.Values.Exceptions.Count);
            Assert.AreEqual(1, result.Values.ImageOverlayItems.Count);
            Assert.AreEqual(1, result.Values.LastWellTest.Count);
            Assert.AreEqual(1, result.Values.RodStrings.Count);
            Assert.AreEqual(1, result.Values.WellStatusOverview.Count);

            Assert.IsNull(result.Values.StatusRegisters[0]);
            Assert.IsNull(result.Values.Alarms[0]);
            Assert.IsNull(result.Values.Exceptions[0]);
            Assert.IsNull(result.Values.ImageOverlayItems[0]);
            Assert.IsNull(result.Values.LastWellTest[0]);
            Assert.IsNull(result.Values.RodStrings[0]);
            Assert.IsNull(result.Values.WellStatusOverview[0]);
        }

        [TestMethod]
        public void RodLiftAssetStatusDataContractMapTest()
        {
            var correlationId = Guid.NewGuid().ToString();

            var result = AssetStatusMapper.Map(correlationId, new RodLiftAssetStatusDataOutput()
            {
                StatusRegisters = new List<PropertyValueOutput>()
                {
                    new PropertyValueOutput()
                    {
                        DisplayState = DisplayState.Error,
                        Value = "Status Value",
                        IsVisible = true,
                        Label = "Status Label",
                    },
                },
                Alarms = new List<PropertyValueOutput>()
                {
                    new PropertyValueOutput()
                    {
                        DisplayState = DisplayState.Warning,
                        Value = "Alarm Value",
                        IsVisible = true,
                        Label = "Alarm Label",
                    },
                },
                Exceptions = new List<PropertyValueOutput>()
                {
                    new PropertyValueOutput()
                    {
                        DisplayState = DisplayState.Ok,
                        Value = "Exception Value",
                        IsVisible = true,
                        Label = "Exception Label",
                    },
                },
                ImageOverlayItems = new List<OverlayStatusDataOutput>()
                {
                    new OverlayStatusDataOutput()
                    {
                        Value = "Image Overlay Value",
                        Label = "Image Overlay Label",
                        IsVisible = true,
                        DisplayState = DisplayState.Emphasis,
                        OverlayField = OverlayFields.ApiDesignation,
                    }
                },
                LastWellTest = new List<PropertyValueOutput>()
                {
                    new PropertyValueOutput()
                    {
                        DisplayState = DisplayState.Normal,
                        Value = "Last Well Test Value",
                        IsVisible = true,
                        Label = "Last Well Test Label",
                    },
                },
                RodStrings = new List<PropertyValueOutput>()
                {
                    new PropertyValueOutput()
                    {
                        DisplayState = DisplayState.Normal,
                        Value = "Rod String Value",
                        IsVisible = true,
                        Label = "Rod String Label",
                    },
                },
                WellStatusOverview = new List<PropertyValueOutput>()
                {
                    new PropertyValueOutput()
                    {
                        DisplayState = DisplayState.Normal,
                        Value = "Well Status Overview Value",
                        IsVisible = true,
                        Label = "Well Status Overview Label",
                    },
                },
            });

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DateCreated);
            Assert.AreEqual(correlationId, result.Id);
            Assert.IsNotNull(result.Values);

            Assert.AreEqual(1, result.Values.StatusRegisters.Count);
            Assert.AreEqual(1, result.Values.Alarms.Count);
            Assert.AreEqual(1, result.Values.Exceptions.Count);
            Assert.AreEqual(1, result.Values.LastWellTest.Count);
            Assert.AreEqual(1, result.Values.WellStatusOverview.Count);
            Assert.AreEqual(1, result.Values.RodStrings.Count);
            Assert.AreEqual(1, result.Values.ImageOverlayItems.Count);

            Assert.IsNotNull(result.Values.StatusRegisters[0]);
            Assert.AreEqual("Status Label", result.Values.StatusRegisters[0].Label);
            Assert.AreEqual("Status Value", result.Values.StatusRegisters[0].Value);
            Assert.IsTrue(result.Values.StatusRegisters[0].IsVisible);
            Assert.AreEqual(DisplayStatusState.Error, result.Values.StatusRegisters[0].DisplayState);

            Assert.IsNotNull(result.Values.Alarms[0]);
            Assert.AreEqual("Alarm Label", result.Values.Alarms[0].Label);
            Assert.AreEqual("Alarm Value", result.Values.Alarms[0].Value);
            Assert.IsTrue(result.Values.Alarms[0].IsVisible);
            Assert.AreEqual(DisplayStatusState.Warning, result.Values.Alarms[0].DisplayState);

            Assert.IsNotNull(result.Values.Exceptions[0]);
            Assert.AreEqual("Exception Label", result.Values.Exceptions[0].Label);
            Assert.AreEqual("Exception Value", result.Values.Exceptions[0].Value);
            Assert.IsTrue(result.Values.Exceptions[0].IsVisible);
            Assert.AreEqual(DisplayStatusState.Ok, result.Values.Exceptions[0].DisplayState);

            Assert.IsNotNull(result.Values.ImageOverlayItems[0]);
            Assert.AreEqual("Image Overlay Label", result.Values.ImageOverlayItems[0].Label);
            Assert.AreEqual("Image Overlay Value", result.Values.ImageOverlayItems[0].Value);
            Assert.IsTrue(result.Values.ImageOverlayItems[0].IsVisible);
            Assert.AreEqual(DisplayStatusState.Emphasis, result.Values.ImageOverlayItems[0].DisplayState);
            Assert.AreEqual(Api.Contracts.OverlayFields.ApiDesignation,
                result.Values.ImageOverlayItems[0].OverlayField);

            Assert.IsNotNull(result.Values.LastWellTest[0]);
            Assert.AreEqual("Last Well Test Label", result.Values.LastWellTest[0].Label);
            Assert.AreEqual("Last Well Test Value", result.Values.LastWellTest[0].Value);
            Assert.IsTrue(result.Values.LastWellTest[0].IsVisible);
            Assert.AreEqual(DisplayStatusState.Normal, result.Values.LastWellTest[0].DisplayState);

            Assert.IsNotNull(result.Values.RodStrings[0]);
            Assert.AreEqual("Rod String Label", result.Values.RodStrings[0].Label);
            Assert.AreEqual("Rod String Value", result.Values.RodStrings[0].Value);
            Assert.IsTrue(result.Values.RodStrings[0].IsVisible);
            Assert.AreEqual(DisplayStatusState.Normal, result.Values.RodStrings[0].DisplayState);

            Assert.IsNotNull(result.Values.WellStatusOverview[0]);
            Assert.AreEqual("Well Status Overview Label", result.Values.WellStatusOverview[0].Label);
            Assert.AreEqual("Well Status Overview Value", result.Values.WellStatusOverview[0].Value);
            Assert.IsTrue(result.Values.WellStatusOverview[0].IsVisible);
            Assert.AreEqual(DisplayStatusState.Normal, result.Values.WellStatusOverview[0].DisplayState);
        }

        #endregion

    }
}