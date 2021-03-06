﻿using System.Net;
using System.Net.Http;
using System.Web.Http;
using SlimMessageBus;
using TrainingIoT.RemoteControl.App.Comm;
using TrainingIoT.RemoteControl.App.Comm.Http;
using TrainingIoT.RemoteControl.App.Messages;

namespace TrainingIoT.RemoteControl.App.Controllers
{
    [RoutePrefix("api/Device")]
    public class DeviceController : ApiController
    {
        private readonly IFeatureCommandQueue _featureCommandQueue;

        public DeviceController(IFeatureCommandQueue featureCommandQueue)
        {
            _featureCommandQueue = featureCommandQueue;
        }

        [HttpPost]
        [Route("Register")]
        public HttpResponseMessage Register(DeviceDescriptionEvent e)
        {
            MessageBus.Current.Publish(e);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("{deviceId}")]
        public HttpResponseMessage PopCommand(string deviceId)
        {
            var command = _featureCommandQueue.PopCommand(deviceId);
            if (command == null)
            {
                // OK, but no payload in response
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.OK, command);
        }

        [HttpPost]
        [Route("Sensor")]
        public HttpResponseMessage Sensor(SensorFeatureEvent e)
        {
            MessageBus.Current.Publish(e);
            return Request.CreateResponse(HttpStatusCode.OK);

        }
    }
}
