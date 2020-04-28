﻿using Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.AreaNote.Shipping;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.IdentityProvider;
using Com.Danliris.Service.Packing.Inventory.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Packing.Inventory.Test.Controllers
{
    public class ShippingAreaNoteControllerTest
    {
        private ShippingAreaNoteController GetController(IShippingAreaNoteService service, IIdentityProvider identityProvider)
        {
            var claimPrincipal = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            claimPrincipal.Setup(claim => claim.Claims).Returns(claims);

            var controller = new ShippingAreaNoteController(service, identityProvider)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = claimPrincipal.Object
                    }
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");

            return controller;
        }

        private int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        [Fact]
        public void Should_Success_GetShippingAreaNote()
        {
            //v
            var serviceMock = new Mock<IShippingAreaNoteService>();
            serviceMock.Setup(s => s.GetReport(It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new List<IndexViewModel>());
            var service = serviceMock.Object;

            var identityProviderMock = new Mock<IIdentityProvider>();
            var identityProvider = identityProviderMock.Object;

            var controller = GetController(service, identityProvider);
            //controller.ModelState.IsValid == false;
            var response = controller.GetShippingAreaNote(null, null, null);

            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public void Should_Exception_GetShippingAreaNote()
        {
            //v
            var serviceMock = new Mock<IShippingAreaNoteService>();
            serviceMock.Setup(s => s.GetReport(It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Throws(new Exception());
            var service = serviceMock.Object;

            var identityProviderMock = new Mock<IIdentityProvider>();
            var identityProvider = identityProviderMock.Object;

            var controller = GetController(service, identityProvider);
            //controller.ModelState.IsValid == false;
            var response = controller.GetShippingAreaNote(null, null, null);

            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        //[Fact]
        //public void Should_Success_GetShippingAreaNoteExcel()
        //{
        //    //v
        //    var serviceMock = new Mock<IShippingAreaNoteService>();
        //    serviceMock.Setup(s => s.GenerateExcel(It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
        //        .Returns(new MemoryStream());
        //    var service = serviceMock.Object;

        //    var identityProviderMock = new Mock<IIdentityProvider>();
        //    var identityProvider = identityProviderMock.Object;

        //    var controller = GetController(service, identityProvider);
        //    //controller.ModelState.IsValid == false;
        //    var response = controller.GetShippingAreaNoteExcel(null, null, null);

        //    Assert.NotNull(response);
        //}

        //[Fact]
        //public void Should_Exception_GetShippingAreaNoteExcel()
        //{
        //    //v
        //    var serviceMock = new Mock<IShippingAreaNoteService>();
        //    serviceMock.Setup(s => s.GenerateExcel(It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
        //        .Throws(new Exception());
        //    var service = serviceMock.Object;

        //    var identityProviderMock = new Mock<IIdentityProvider>();
        //    var identityProvider = identityProviderMock.Object;

        //    var controller = GetController(service, identityProvider);
        //    //controller.ModelState.IsValid == false;
        //    var response = controller.GetAvalAreaNoteExcel(null, null, null);

        //    Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        //}
    }
}