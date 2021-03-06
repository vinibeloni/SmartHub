﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.Internal;
using MediatR;
using Serilog;
using SmartHub.Application.Common.Interfaces;
using SmartHub.Application.Common.Interfaces.Database;
using SmartHub.Application.Common.Models;
using SmartHub.Application.UseCases.PluginAdapter.Helper;
using SmartHub.Application.UseCases.PluginAdapter.Host;
using SmartHub.BasePlugin.Interfaces.DeviceTypes;
using SmartHub.Domain.Common.Enums;
using SmartHub.Domain.Entities;

namespace SmartHub.Application.UseCases.DeviceState.LightState
{
	public class DeviceLightStateHandler : IRequestHandler<DeviceLightStateQuery, Response<DeviceStateDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPluginHostService _pluginHostService;
		private readonly IHttpService _httpService;
		private string _query = "";
		private readonly ILogger _log = Log.ForContext(typeof(DeviceLightStateHandler));
		public DeviceLightStateHandler(IUnitOfWork unitOfWork, IPluginHostService pluginHostService, IHttpService httpService)
		{
			_unitOfWork = unitOfWork;
			_pluginHostService = pluginHostService;
			_httpService = httpService;
		}

		public async Task<Response<DeviceStateDto>> Handle(DeviceLightStateQuery request, CancellationToken cancellationToken)
		{
			if (request is null)
			{
				throw new ArgumentNullException(nameof(request));
			}
			var home = await _unitOfWork.HomeRepository.GetHome();
			if (home is null)
			{
				return Response.Fail<DeviceStateDto>("Error: There is no home created at the moment.");

			}

			var foundDevice = home.Groups.SelectMany(x => x.Devices).SingleOrDefault(d => d.Id == request.LightStateDto.DeviceId);
			if (foundDevice is null)
			{
				return Response.Fail<DeviceStateDto>($"Error: No device found by the given deviceId {request.LightStateDto.DeviceId}");
			}
			// var pluginObject = await _pluginHostService.Plugins.GetAndLoadByName(foundDevice.PluginName, home) as ILight;
			var pluginObject = await _pluginHostService.GetPluginByNameAsync<ILight>(foundDevice.PluginName);
			var connectionType = PluginHelper.CombineConnectionTypes(pluginObject);
			if ((connectionType & ConnectionTypes.Http) != 0 && foundDevice.PrimaryConnection == ConnectionTypes.Http)
			{
				_query = pluginObject.InstantiateQuery().SetToggleLight(request.LightStateDto.ToggleLight).Build();
			}
			else if ((connectionType & ConnectionTypes.Mqtt) != 0 && foundDevice.PrimaryConnection == ConnectionTypes.Mqtt)
			{
				// TODO: implement later when Mqtt is useable
				_log.Information("{connectionType}");
			}
			else
			{
				// TODO: implement later -> error path
				_log.Information("{connectionType}");

			}
			// var response = await _httpService.SendAsync(foundDevice.Ip.Ipv4, _query);
			return true ?
				Response.Ok<DeviceStateDto>($"{foundDevice.Name} changed light status", request.LightStateDto) :
				Response.Fail<DeviceStateDto>($"Error: Couldn't send new light status to {foundDevice.Name}");
		}
	}
}
