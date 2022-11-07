using MPSC.PlenoSoft.ControlFlux.Messages;
using MPSC.PlenoSoft.ControlFlux.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace MPSC.PlenoSoft.ControlFlux.Parameters
{
	public class FluxArg
	{
		private readonly List<Message> _messages = new List<Message>();
		public dynamic Params { get; } = new ExpandoObject();

		public Boolean Status { get; private set; } = true;
		public IEnumerable<Message> Messages { get { return _messages.ToArray(); } }

		public FluxArg AddParam<T>(String name, T value)
		{
			var dicParams = Params as IDictionary<String, Object>;
			if (!(dicParams?.TryAdd(name, value)).GetValueOrDefault(false))
			{
				if (dicParams.TryGetValue(name, out var existingValue))
				{
					if (existingValue is List<T> list)
						list.Add(value);
					else
						dicParams[name] = new List<T> { (T)existingValue, value };
				}
				else
					dicParams[name] = value;
			}
			return this;
		}

		public void AddTrack(String description)
		{
			AddMessage(new Message { Description = description, FullDescription = (Status ? "OK" : "No"), Type = MessageType.Track });
		}

		public void AddValidation(String description, String fullDescription = null, Int64? messageCode = null)
		{
			AddMessage(new Message(messageCode) { Description = description, FullDescription = fullDescription ?? description, Type = MessageType.Validation });
		}

		public void AddInformation(String description, String fullDescription = null, Int64? messageCode = null)
		{
			AddMessage(new Message(messageCode) { Description = description, FullDescription = fullDescription ?? description, Type = MessageType.Information });
		}

		public Exception AddException(Exception exception)
		{
			AddMessage(new Message { Description = exception.Message, FullDescription = exception.Message, Type = MessageType.Exception });
			return exception;
		}

		public Message AddMessage(Message message)
		{
			_messages.Add(message);
			Status &= !message.Type.In(MessageType.Exception, MessageType.Validation);
			return message;
		}

		public void MergeFrom(FluxArg fluxArg)
		{
			fluxArg.Messages.ForEach(m => AddMessage(m));
		}
	}
}