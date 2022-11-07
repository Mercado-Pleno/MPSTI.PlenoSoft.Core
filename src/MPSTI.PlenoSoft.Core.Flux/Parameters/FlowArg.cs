using MPSTI.PlenoSoft.Core.Extensions.Static;
using MPSTI.PlenoSoft.Core.Flux.Messages;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace MPSTI.PlenoSoft.Core.Flux.Parameters
{
	public class FlowArg
	{
		private readonly List<Message> _messages = new List<Message>();
		public dynamic Params { get; } = new ExpandoObject();

		public bool Status { get; private set; } = true;
		public IEnumerable<Message> Messages { get { return _messages.ToArray(); } }

		public FlowArg AddParam<T>(string name, T value)
		{
			var dicParams = Params as IDictionary<string, object>;
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

		public void AddTrack(string description)
		{
			AddMessage(new Message { Description = description, FullDescription = Status ? "OK" : "No", Type = MessageType.Track });
		}

		public void AddValidation(string description, string fullDescription = null, long? messageCode = null)
		{
			AddMessage(new Message(messageCode) { Description = description, FullDescription = fullDescription ?? description, Type = MessageType.Validation });
		}

		public void AddInformation(string description, string fullDescription = null, long? messageCode = null)
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

		public void MergeFrom(FlowArg flowArg)
		{
			flowArg.Messages.ForEach(m => AddMessage(m));
		}
	}
}